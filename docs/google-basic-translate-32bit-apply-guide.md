# Google Basic 정상 품질 번역 경로 32비트 적용 가이드

## 목적

64비트 MORT에 반영한 Google Basic 번역 경로를 별도 32비트 코드베이스에도 적용한다.

기존 32비트 구현은 `client=gtx` 요청이 HTTP 429를 반환하면 같은 호출 안에서 곧바로 저품질 모드로 전환한다. 수정 후 호출 순서는 다음과 같아야 한다.

1. Google 웹 번역의 `MkEWBc batchexecute` 정상 품질 경로
2. 기존 `translate.googleapis.com + client=gtx` 정상 품질 보조 경로
3. 앞의 두 경로가 모두 실패했을 때만 `clients5.google.com + dict-chrome-ex` 저품질 경로

저품질 모드에 들어가면 10분 동안 저품질 경로를 사용한 뒤 정상 품질 경로를 다시 확인한다.

## 대상 코드베이스와 호환성

- 32비트 루트: `D:\project\MORT\32bit\MORT`
- 대상 프레임워크: .NET Framework 4.7.2
- RestSharp: 106.12.0
- Newtonsoft.Json: 13.0.3
- 주요 대상 파일:
  - `MORT/TransAPI/GoogleBasicTranslateAPI.cs`
  - `MORT/Manager/TransManager.cs`

64비트는 .NET 9, RestSharp 114, `System.Text.Json`을 사용한다. 32비트에는 다음 코드를 그대로 복사하면 안 된다.

- `System.Text.Json.JsonDocument`
- `RestResponse`
- `new RestRequest("", Method.Post)`
- `TimeSpan` 형식의 `request.Timeout`

32비트에서는 각각 Newtonsoft.Json, `IRestResponse`, `new RestRequest(Method.POST)`, 밀리초 정수 Timeout을 사용한다.

## 1. GoogleBasicTranslateAPI의 using 추가

`GoogleBasicTranslateAPI.cs` 상단에 다음 using을 추가한다.

```csharp
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
```

기존 `RestSharp.Serialization.Json.JsonDeserializer`는 새 구현에서 사용하지 않으므로 `GetResult` 안의 `deserial` 지역 변수도 제거한다.

## 2. MkEWBc 요청 메서드 추가

기존 `GetResult` 앞에 `TryGetBatchExecuteResult`를 추가한다. 32비트 RestSharp 문법을 사용해야 한다.

```csharp
private bool TryGetBatchExecuteResult(
    string original,
    string transCode,
    string resultCode,
    out string result)
{
    result = "";

    try
    {
        string rpcArgument = JsonConvert.SerializeObject(new object[]
        {
            new object[] { original, transCode, resultCode, true },
            new object[] { null }
        });

        string rpcRequest = JsonConvert.SerializeObject(new object[]
        {
            new object[]
            {
                new object[] { "MkEWBc", rpcArgument, null, "generic" }
            }
        });

        var client = new RestClient(
            "https://translate.google.com/_/TranslateWebserverUi/data/batchexecute?rpcids=MkEWBc&rt=c");

        var request = new RestRequest(Method.POST);
        request.AddHeader(
            "content-type",
            "application/x-www-form-urlencoded;charset=UTF-8");
        request.AddHeader("origin", "https://translate.google.com");
        request.AddHeader("referer", "https://translate.google.com/");
        request.AddParameter(
            "application/x-www-form-urlencoded",
            "f.req=" + Uri.EscapeDataString(rpcRequest),
            ParameterType.RequestBody);
        request.Timeout = 3000;

        IRestResponse response = client.Execute(request);

        Util.ShowLog(
            $"Google Batch Result Status : Success = {response.IsSuccessful} StatusCode : {response.StatusCode}");

        if (!response.IsSuccessful || string.IsNullOrEmpty(response.Content))
        {
            return false;
        }

        return TryParseBatchExecuteResponse(response.Content, out result);
    }
    catch (Exception e)
    {
        Util.ShowLog("Google Batch Error : " + e);
        return false;
    }
}
```

중요: `AddParameter`의 첫 번째 인수에는 `;charset=UTF-8`을 넣지 않는다. RestSharp 114에서는 이 값이 `FormatException`을 발생시켰고, 106에서도 순수 미디어 타입을 사용하는 편이 안전하다. charset은 헤더에만 둔다.

## 3. batchexecute 응답 파서 추가

응답 전체는 JSON 하나가 아니다. 보안 프리픽스와 길이 줄을 건너뛰고 `MkEWBc` 프레임을 찾아 외부 JSON과 내부 JSON을 차례로 파싱해야 한다.

```csharp
private bool TryParseBatchExecuteResponse(string content, out string result)
{
    result = "";

    try
    {
        string[] lines = content.Split('\n');
        foreach (string line in lines)
        {
            string frame = line.Trim();
            if (!frame.StartsWith("[") || !frame.Contains("\"MkEWBc\""))
            {
                continue;
            }

            JArray outer = JArray.Parse(frame);
            string innerJson = (string)outer[0][2];
            if (string.IsNullOrEmpty(innerJson))
            {
                continue;
            }

            JArray inner = JArray.Parse(innerJson);
            JArray translations = inner[1][0][0][5] as JArray;
            if (translations == null)
            {
                continue;
            }

            foreach (JToken item in translations)
            {
                JArray part = item as JArray;
                if (part != null
                    && part.Count > 0
                    && part[0].Type == JTokenType.String)
                {
                    result += part[0].Value<string>() ?? string.Empty;
                }
            }

            return !string.IsNullOrEmpty(result);
        }
    }
    catch (Exception e)
    {
        Util.ShowLog("Google Batch Parse Error : " + e);
    }

    return false;
}
```

번역 조각을 결합할 때 임의의 공백을 추가하지 않는다. Google 응답에 필요한 공백과 줄바꿈이 이미 들어 있으며, 이 방식으로 MORT의 `//////` 영역 구분자도 보존된다.

## 4. gtx와 저품질 공통 호출 메서드 추가

`TryGetGoogleApiResult`를 추가하고 `lowQuality` 값으로 URL과 응답 구조를 나눈다.

```csharp
private bool TryGetGoogleApiResult(
    string original,
    string transCode,
    string resultCode,
    bool lowQuality,
    out string result,
    out bool isRateLimited)
{
    result = "";
    isRateLimited = false;

    string encodedOriginal = Uri.EscapeDataString(original);
    string url = lowQuality
        ? $"https://clients5.google.com/translate_a/t?client=dict-chrome-ex&sl={transCode}&tl={resultCode}&q={encodedOriginal}"
        : $"https://translate.googleapis.com/translate_a/single?client=gtx&sl={transCode}&tl={resultCode}&dt=t&q={encodedOriginal}";

    try
    {
        var client = new RestClient(url);
        var request = new RestRequest(Method.GET);
        request.AddHeader("content-type", "application/x-www-form-urlencoded");
        request.AddHeader("cache-control", "no-cache");
        request.AddHeader("charset", "UTF-8");
        request.Timeout = 2000;

        IRestResponse response = client.Execute(request);

        if ((int)response.StatusCode == 429)
        {
            isRateLimited = true;
            return false;
        }

        if (!response.IsSuccessful || string.IsNullOrEmpty(response.Content))
        {
            return false;
        }

        JArray root = JArray.Parse(response.Content);

        if (lowQuality)
        {
            foreach (JToken item in root)
            {
                if (item.Type == JTokenType.String)
                {
                    result += item.Value<string>() ?? string.Empty;
                }
            }
        }
        else
        {
            JArray translations = root[0] as JArray;
            if (translations != null)
            {
                foreach (JToken item in translations)
                {
                    JArray part = item as JArray;
                    if (part != null
                        && part.Count > 0
                        && part[0].Type == JTokenType.String)
                    {
                        result += (part[0].Value<string>() ?? string.Empty) + " ";
                    }
                }
            }
        }

        return !string.IsNullOrEmpty(result);
    }
    catch (Exception e)
    {
        Util.ShowLog("Google Translate Error : " + e);
        return false;
    }
}
```

## 5. GetResult를 폴백 조정 메서드로 변경

기존 재귀식 429 처리와 `clientType` 선택을 제거하고 다음 흐름으로 바꾼다.

```csharp
private string GetResult(
    string original,
    ref bool isError,
    string transCode,
    string resultCode)
{
    if (string.IsNullOrWhiteSpace(original))
    {
        Util.ShowLog("Empty");
        return "";
    }

    Util.ShowLog("Original : " + original);

    if (!_lowQuailtyMode)
    {
        string batchResult;
        if (TryGetBatchExecuteResult(
            original, transCode, resultCode, out batchResult))
        {
            isError = false;
            return batchResult;
        }

        string gtxResult;
        bool ignoredRateLimit;
        if (TryGetGoogleApiResult(
            original, transCode, resultCode, false,
            out gtxResult, out ignoredRateLimit))
        {
            isError = false;
            return gtxResult;
        }

        _dtNextCheck = DateTime.Now.AddMinutes(10);
        _lowQuailtyMode = true;
        UpdateCondition();
    }

    string lowQualityResult;
    bool isRateLimited;
    if (TryGetGoogleApiResult(
        original, transCode, resultCode, true,
        out lowQualityResult, out isRateLimited))
    {
        isError = false;
        return lowQualityResult;
    }

    isError = true;
    if (isRateLimited)
    {
        return "시간당 사용할 수 있는 쿼리 모두 소모 - 다른 번역 방법을 선택하거나, 잠시 뒤에 다시 사용해 주세요";
    }

    return "처리하는 도중 오류가 발생했습니다";
}
```

`DoTrans`의 만료 검사와 `[저품질]` 표시 로직은 유지한다. 만료 시간이 지나면 `_lowQuailtyMode`가 false가 되어 다음 호출에서 `batchexecute`부터 다시 시도한다.

## 6. 캐시 정책과 변경 금지 범위

- `TransManager.GetFormerResult`의 기존 캐시 조회 및 반환 로직을 변경하지 말 것
- `TransManager.AddFormerResult` 호출을 조건부로 막거나 `[저품질]` 캐시를 삭제하지 말 것
- 이번 작업은 Google Basic의 요청 경로와 폴백 순서만 변경할 것
- `SettingManager.TransType` enum 순서 변경 금지
- 설정 직렬화 키 변경 금지
- `Resources/localize.csv` 직접 수정 금지
- Google Basic 이외 번역 제공자 구조 리팩터링 금지
- 32비트 전체를 `System.Text.Json`이나 새 RestSharp 문법으로 통일하지 말 것

## 7. 빌드 및 검증

Visual Studio Developer PowerShell에서 32비트 솔루션을 x86으로 빌드한다.

```powershell
msbuild D:\project\MORT\32bit\MORT\MORT.sln /t:Build /p:Configuration=Debug /p:Platform=x86
```

다음 테스트를 수행한다.

1. 영어 → 한국어 Google Basic 번역
2. 일본어 → 한국어 번역
3. 경유 번역 활성화 후 영어 → 일본어 → 한국어
4. `First area\r\n//////\r\nSecond area` 입력에서 구분자 보존
5. 기존 gtx가 429인 환경에서도 결과 앞에 `[저품질]`이 붙지 않는지 확인
6. `Google Batch Result Status : Success = True StatusCode : OK` 로그 확인
7. batchexecute를 의도적으로 실패시켜 gtx, 그다음 저품질 순으로 내려가는지 확인

64비트에서 확인한 기준 결과는 다음과 같다.

- 기존 gtx: HTTP 429
- MkEWBc batchexecute: HTTP 200
- 다국어·경유·연속 요청 14/14 성공
- 빌드된 MORT.dll의 실제 RestSharp 요청 결과: `적군이 지휘소를 점령했습니다.`
- `[저품질]` 접두사 없음

## 완료 조건

- x86 빌드 오류 0개
- 정상 네트워크 상태에서 첫 번역이 `MkEWBc`로 성공
- gtx 429만으로 저품질 모드에 진입하지 않음
- 두 정상 품질 경로가 모두 실패한 경우에만 `[저품질]` 표시
- `TransManager`의 기존 캐시 조회·저장 동작이 유지됨

## 새 작업에 전달할 요청문

아래 문장을 32비트 저장소 작업에 그대로 전달할 수 있다.

> `D:\project\MORT\32bit\MORT`에 `docs/google-basic-translate-32bit-apply-guide.md`의 내용을 적용해 주세요. 32비트는 .NET Framework 4.7.2, RestSharp 106.12.0, Newtonsoft.Json 13.0.3이므로 64비트의 System.Text.Json/RestSharp 114 코드를 그대로 복사하지 마세요. `MkEWBc batchexecute → gtx → dict-chrome-ex 저품질` 순서만 GoogleBasicTranslateAPI에 반영하고 x86 Debug 빌드와 실제 Google Basic 번역 호출까지 검증해 주세요. TransManager의 기존 캐시 조회·저장 로직, 설정 enum, 직렬화 키, localize.csv는 변경하지 마세요.
