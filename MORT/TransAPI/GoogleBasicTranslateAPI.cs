using RestSharp;
using System;
using System.Collections.Generic;
using System.Text.Json;

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

        private bool TryGetBatchExecuteResult(string original, string transCode, string resultCode, out string result)
        {
            result = "";

            try
            {
                string rpcArgument = JsonSerializer.Serialize(new object[]
                {
                    new object[] { original, transCode, resultCode, true },
                    new object[] { null }
                });

                string rpcRequest = JsonSerializer.Serialize(new object[]
                {
                    new object[]
                    {
                        new object[] { "MkEWBc", rpcArgument, null, "generic" }
                    }
                });

                var client = new RestClient("https://translate.google.com/_/TranslateWebserverUi/data/batchexecute?rpcids=MkEWBc&rt=c");
                var request = new RestRequest("", Method.Post);

                request.AddHeader("content-type", "application/x-www-form-urlencoded;charset=UTF-8");
                request.AddHeader("origin", "https://translate.google.com");
                request.AddHeader("referer", "https://translate.google.com/");
                request.AddParameter("application/x-www-form-urlencoded", "f.req=" + Uri.EscapeDataString(rpcRequest), ParameterType.RequestBody);
                request.Timeout = TimeSpan.FromMilliseconds(3000);

                RestResponse response = client.Execute(request);

                Util.ShowLog($"Google Batch Result Status : Success = {response.IsSuccessful} StatusCode : {response.StatusCode}");

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

                    using JsonDocument outerDocument = JsonDocument.Parse(frame);
                    string innerJson = outerDocument.RootElement[0][2].GetString();
                    if (string.IsNullOrEmpty(innerJson))
                    {
                        continue;
                    }

                    using JsonDocument innerDocument = JsonDocument.Parse(innerJson);
                    JsonElement translations = innerDocument.RootElement[1][0][0][5];

                    foreach (JsonElement item in translations.EnumerateArray())
                    {
                        if (item.ValueKind == JsonValueKind.Array
                            && item.GetArrayLength() > 0
                            && item[0].ValueKind == JsonValueKind.String)
                        {
                            result += item[0].GetString() ?? string.Empty;
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

        private bool TryGetGoogleApiResult(string original, string transCode, string resultCode, bool lowQuality, out string result, out bool isRateLimited)
        {
            result = "";
            isRateLimited = false;

            string encodedOriginal = Uri.EscapeDataString(original);
            string url;

            if (lowQuality)
            {
                url = $"https://clients5.google.com/translate_a/t?client=dict-chrome-ex&sl={transCode}&tl={resultCode}&q={encodedOriginal}";
            }
            else
            {
                url = $"https://translate.googleapis.com/translate_a/single?client=gtx&sl={transCode}&tl={resultCode}&dt=t&q={encodedOriginal}";
            }

            try
            {
                var client = new RestClient(url);
                var request = new RestRequest("", Method.Get);

                request.AddHeader("content-type", "application/x-www-form-urlencoded");
                request.AddHeader("cache-control", "no-cache");
                request.AddHeader("charset", "UTF-8");
                request.Timeout = TimeSpan.FromMilliseconds(2000);

                RestResponse response = client.Execute(request);

                Util.ShowLog($"Google {(lowQuality ? "Low Quality" : "GTX")} Result Status : Success = {response.IsSuccessful} StatusCode : {response.StatusCode}");

                if ((int)response.StatusCode == 429)
                {
                    isRateLimited = true;
                    return false;
                }

                if (!response.IsSuccessful || string.IsNullOrEmpty(response.Content))
                {
                    return false;
                }

                Util.ShowLog(response.Content);
                using JsonDocument document = JsonDocument.Parse(response.Content);

                if (lowQuality)
                {
                    foreach (JsonElement item in document.RootElement.EnumerateArray())
                    {
                        if (item.ValueKind == JsonValueKind.String)
                        {
                            result += item.GetString() ?? string.Empty;
                        }
                    }
                }
                else
                {
                    JsonElement translations = document.RootElement[0];
                    foreach (JsonElement item in translations.EnumerateArray())
                    {
                        if (item.ValueKind == JsonValueKind.Array
                            && item.GetArrayLength() > 0
                            && item[0].ValueKind == JsonValueKind.String)
                        {
                            result += (item[0].GetString() ?? string.Empty) + " ";
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

        private string GetResult(string original, ref bool isError, string transCode , string resultCode )
        {
            if (string.IsNullOrWhiteSpace(original))
            {
                Util.ShowLog("Empty");
                return "";
            }

            Util.ShowLog("Original : " + original);

            if (!_lowQuailtyMode)
            {
                if (TryGetBatchExecuteResult(original, transCode, resultCode, out string batchResult))
                {
                    isError = false;
                    return batchResult;
                }

                if (TryGetGoogleApiResult(original, transCode, resultCode, false, out string gtxResult, out _))
                {
                    isError = false;
                    return gtxResult;
                }

                _dtNextCheck = DateTime.Now.AddMinutes(10);
                _lowQuailtyMode = true;
                UpdateCondition();
            }

            if (TryGetGoogleApiResult(original, transCode, resultCode, true, out string lowQualityResult, out bool isRateLimited))
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

            if(_lowQuailtyMode && Form1.IsDebugDisplayLowQuality)
            {
                result = "[저품질]" + result;
            }

            return result;
        }
    }
}
