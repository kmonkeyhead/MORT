using RestSharp;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;

namespace MORT
{
    public interface IGoogleBasicTranslateAPIContract
    {
        void UpdateCondition(string condition);
    }

    class GoogleBasicTranslateAPI
    {
        public static GoogleBasicTranslateAPI instance;

        private string _transCode;
        private string _resultCode;

        private bool _isAllowExecutive;

        private DateTime _dtNextCheck = DateTime.MinValue;
        private bool _lowQuailtyMode;
        private IGoogleBasicTranslateAPIContract _contract;

        public void InitContract(IGoogleBasicTranslateAPIContract contract)
        {
            _contract = contract;
        }

        public void UpdateCondition()
        {
            _contract.UpdateCondition(_lowQuailtyMode ? "Basic_LowQuality": "Basic_HighQuality");
        }

        public void SetTransCode(string transCode, string resultCode)
        {
            this._transCode = transCode;
            this._resultCode = resultCode;

            if (transCode != "ja")
            {
                _isAllowExecutive = true;
            }
            else
            {
                _isAllowExecutive = false;
            }
        }

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

        public string DoTrans(string original, ref bool isError)
        {
            string result = "";

            //저품질 모드인지 체크한다.
            if(_lowQuailtyMode && DateTime.Now > _dtNextCheck)
            {
                _lowQuailtyMode = false;
                UpdateCondition();
            }

            if(_isAllowExecutive && AdvencedOptionManager.IsExecutive)
            {
                original = GetResult(original, ref isError, _transCode, "ja");
                result = original;

                if(!isError)
                {
                    result = GetResult(original, ref isError, "ja", _resultCode);
                }
            }
            else
            {
                result = GetResult(original, ref isError, _transCode, _resultCode);
            }

            if(_lowQuailtyMode)
            {
                result = "[저품질]" + result;
            }

            return result;
        }
    }
}
