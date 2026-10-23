using System.IO;
using System.Net.Http;

namespace MORT.Service.Http
{
    /// <summary>
    /// 버전 확인, 설정 검색처럼 작은 텍스트 파일을 받아오는 곳에서 쓰던 WebClient를 대신한다.
    /// 호출하는 쪽이 전부 동기 흐름이라 HttpClient.Send로 동기 호출을 유지한다.
    /// HttpClient는 소켓 고갈을 막기 위해 하나만 만들어 재사용한다.
    /// </summary>
    public static class HttpTextDownloader
    {
        private static readonly HttpClient httpClient = new HttpClient();

        //WebClient.OpenRead 대체. 응답 본문을 메모리로 다 받은 뒤 돌려주므로 호출 측은 스트림만 닫으면 된다.
        //실패(네트워크 오류, 2xx 아님)는 WebClient와 마찬가지로 예외로 올린다.
        public static Stream OpenRead(string url)
        {
            using (HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, url))
            using (HttpResponseMessage response = httpClient.Send(request))
            {
                response.EnsureSuccessStatusCode();

                MemoryStream buffer = new MemoryStream();
                using (Stream stream = response.Content.ReadAsStream())
                {
                    stream.CopyTo(buffer);
                }

                buffer.Position = 0;
                return buffer;
            }
        }

        //WebClient.DownloadString 대체
        public static string DownloadString(string url)
        {
            using (StreamReader reader = new StreamReader(OpenRead(url)))
            {
                return reader.ReadToEnd();
            }
        }
    }
}
