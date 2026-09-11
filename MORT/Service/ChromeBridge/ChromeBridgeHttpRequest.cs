using System;
using System.Collections.Generic;
using System.Globalization;

namespace MORT.Service.ChromeBridge
{
    /// <summary>
    /// 요청의 첫 줄과 헤더만 담는다. 본문은 Leftover 뒤에 이어서 읽는다.
    /// 로컬 전용 서버라 완전한 HTTP 구현 대신 이 정도만 다룬다.
    /// </summary>
    internal sealed class ChromeBridgeHttpRequest
    {
        public string Method { get; init; } = "";
        public string Path { get; init; } = "";
        public string Query { get; init; } = "";
        public Dictionary<string, string> Headers { get; init; } = new Dictionary<string, string>();
        public byte[] Leftover { get; init; } = Array.Empty<byte>();

        public string? GetHeader(string name)
        {
            string? value;

            if(Headers.TryGetValue(name, out value))
            {
                return value;
            }

            return null;
        }

        public int ContentLength
        {
            get
            {
                string? raw = GetHeader("content-length");

                if(string.IsNullOrEmpty(raw))
                {
                    return 0;
                }

                int length;

                if(int.TryParse(raw.Trim(), NumberStyles.Integer, CultureInfo.InvariantCulture, out length) && length > 0)
                {
                    return length;
                }

                return 0;
            }
        }

        public bool IsWebSocketUpgrade
        {
            get
            {
                string? upgrade = GetHeader("upgrade");

                if(string.IsNullOrEmpty(upgrade))
                {
                    return false;
                }

                if(upgrade.IndexOf("websocket", StringComparison.OrdinalIgnoreCase) < 0)
                {
                    return false;
                }

                return !string.IsNullOrEmpty(GetHeader("sec-websocket-key"));
            }
        }

        public static ChromeBridgeHttpRequest? Parse(string headText, byte[] leftover)
        {
            string[] lines = headText.Split(new string[] { "\r\n" }, StringSplitOptions.None);

            if(lines.Length == 0)
            {
                return null;
            }

            string[] requestLine = lines[0].Split(' ');

            if(requestLine.Length < 2)
            {
                return null;
            }

            string target = requestLine[1];
            string path = target;
            string query = "";
            int mark = target.IndexOf('?');

            if(mark >= 0)
            {
                path = target.Substring(0, mark);
                query = target.Substring(mark + 1);
            }

            Dictionary<string, string> headers = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

            for(int i = 1; i < lines.Length; i++)
            {
                string line = lines[i];

                if(string.IsNullOrEmpty(line))
                {
                    continue;
                }

                int colon = line.IndexOf(':');

                if(colon <= 0)
                {
                    continue;
                }

                string name = line.Substring(0, colon).Trim();
                string value = line.Substring(colon + 1).Trim();
                headers[name] = value;
            }

            return new ChromeBridgeHttpRequest
            {
                Method = requestLine[0].ToUpperInvariant(),
                Path = path,
                Query = query,
                Headers = headers,
                Leftover = leftover,
            };
        }
    }
}
