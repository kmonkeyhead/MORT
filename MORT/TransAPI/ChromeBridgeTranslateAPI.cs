using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MORT.Service.ChromeBridge;

namespace MORT.TransAPI
{
    /// <summary>
    /// 크롬 번역기. 로컬 서버가 MORT 안에 있으므로 HTTP를 거치지 않고 허브를 바로 부른다.
    /// 실제 번역은 크롬 창의 브릿지 페이지가 내장 AI로 수행한다.
    /// </summary>
    public class ChromeBridgeTranslateAPI
    {
        private readonly ChromeBridgeService _service;

        private string _transCode = "";
        private string _resultCode = "ko";
        private string _mode = ChromeBridgeEngineMode.Auto;

        public ChromeBridgeTranslateAPI(ChromeBridgeService service)
        {
            _service = service;
        }

        public void Init(string transCode, string resultCode, string mode)
        {
            //MORT가 넘기는 값은 구글 언어 코드라 크롬이 아는 형태로 한 번 정리한다.
            _transCode = ChromeBridgeLanguage.Normalize(transCode);
            _resultCode = ChromeBridgeLanguage.Normalize(resultCode);

            if (_resultCode == ChromeBridgeLanguage.Auto)
            {
                _resultCode = "ko";
            }

            _mode = ChromeBridgeEngineMode.Normalize(mode);

            //설정을 적용한 시점에 크롬 창에도 알린다. 첫 번역을 기다리지 않고 맞는 언어 쌍을 준비하게 한다.
            _service.SetConfig(_transCode, _resultCode, _mode);
        }

        /// <summary>
        /// 오버레이처럼 OCR 영역이 여럿이면 MORT는 구분 토큰으로 이어 붙여 한 번에 넘기고,
        /// 돌아온 결과를 같은 토큰으로 다시 쪼개 영역별로 나눠 넣는다(Util.GetSpliteByToken).
        ///
        /// 그런데 크롬 내장 AI는 그 토큰을 지켜 주지 않는다. Translator는 줄바꿈을 공백으로 합쳐서
        /// "//////\r\n" 이 "////// " 가 되어 버리고, LLM은 마커째로 지워 버리기도 한다.
        /// 그대로 두면 영역이 하나도 안 나뉘고 첫 영역에 전부 몰린다.
        ///
        /// 그래서 이어 붙인 채로 넘기지 않고 여기서 토큰으로 쪼개 한 덩이씩 번역한 뒤 같은 모양으로 다시 잇는다.
        /// 덩이마다 왕복이 생기지만 영역이 정확히 나뉘고, 덩이별로 번역해서 결과도 서로 안 섞인다.
        /// </summary>
        public async Task<string> GetResultAsync(string original, CancellationToken token)
        {
            string splitToken = Util.GetSpliteToken(SettingManager.TransType.chromeBridge);

            if (string.IsNullOrEmpty(original) || !original.Contains(splitToken))
            {
                return await TranslateBlockAsync(original, token);
            }

            string[] blocks = original.Split(new string[] { splitToken }, StringSplitOptions.RemoveEmptyEntries);
            StringBuilder builder = new StringBuilder();

            foreach (string block in blocks)
            {
                string trimmed = block.TrimEnd('\r', '\n');

                if (string.IsNullOrWhiteSpace(trimmed))
                {
                    continue;
                }

                string translated = await TranslateBlockAsync(trimmed, token);

                if (IsError)
                {
                    //한 덩이라도 실패하면 사유를 그대로 올린다. 다른 번역기와 같은 처리다.
                    return translated;
                }

                builder.Append(splitToken).Append(translated).Append(Environment.NewLine);
            }

            return builder.ToString();
        }

        private async Task<string> TranslateBlockAsync(string text, CancellationToken token)
        {
            ChromeBridgeResult result = await _service.TranslateAsync(
                text, _transCode, _resultCode, _mode, AdvencedOptionManager.ChromeBridgeTimeout, token);

            if (!result.IsSuccess)
            {
                IsError = true;
                return result.Error;
            }

            IsError = false;
            return result.Text;
        }

        /// <summary>가장 최근 번역이 실패였는지. ref 인자를 쓰는 다른 API와 형태가 달라서 이렇게 둔다.</summary>
        public bool IsError { get; private set; }
    }
}
