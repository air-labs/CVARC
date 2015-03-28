using System;
using System.IO;
using System.Net;
using System.Text;
using Newtonsoft.Json;

namespace NetworkCompetitionsPlayer
{
    abstract class HttpClient
    {
        protected string SendRequestInternal(string url, MethodType method, byte[] content = null)
        {
            var request = (HttpWebRequest) WebRequest.Create(url);
            request.ContentType = "text/json;charset=UTF-8";
            request.Method = method.ToString();
            request.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;
            request.Headers.Add("Accept-Encoding", "gzip");
            request.ContentLength = method == MethodType.POST && content != null ? content.Length : 0;
            request.CookieContainer = new CookieContainer();
            request.CookieContainer.Add(new Uri(url), new Cookie("rtsToken", "ubwVq61ptjfPzoq03FYxyw=="));
            
            if (content != null)
                using (var stream = request.GetRequestStream())
                    stream.Write(content, 0, content.Length);
            using (var response = (HttpWebResponse) request.GetResponse())//в случае эксепшена проверить что правильная кукка стоит
            {
                return new StreamReader(response.GetResponseStream()).ReadToEnd();
            }
        }
    }

    class JsonHttpClient : HttpClient
    {
        public TResult SendRequest<TResult, TSend>(string url, TSend sendData)
        {
            var content = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(sendData));
            var response = SendRequestInternal(url, MethodType.POST, content);
            return JsonConvert.DeserializeObject<TResult>(response);
        }

        public void SendRequest<TSend>(string url, TSend sendData)
        {
            var content = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(sendData));
            SendRequestInternal(url, MethodType.POST, content);
        }

        public TResult SendRequest<TResult>(string url)
        {
            var response = SendRequestInternal(url, MethodType.POST);
            return JsonConvert.DeserializeObject<TResult>(response);
        }
    }
}
