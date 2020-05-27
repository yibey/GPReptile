using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace GPReptile.Service
{

    public class HttpHelper
    {
        private HttpClient _httpClient;
        private string _baseIPAddress;

        /// <param name="ipaddress">请求的基础IP，例如：http://192.168.0.33:8080/ </param>
        public HttpHelper(string ipaddress = "")
        {
            this._baseIPAddress = ipaddress;
            _httpClient = new HttpClient { BaseAddress = new Uri(_baseIPAddress) };

        }

        /// <summary>
        /// 创建带用户信息的请求客户端
        /// </summary>
        /// <param name="userName">用户账号</param>
        /// <param name="pwd">用户密码，当WebApi端不要求密码验证时，可传空串</param>
        /// <param name="uriString">The URI string.</param>
        public HttpHelper(string userName, string pwd = "", string uriString = "")
            : this(uriString)
        {
            if (!string.IsNullOrEmpty(userName))
            {
                _httpClient.DefaultRequestHeaders.Authorization = CreateBasicCredentials(userName, pwd);
            }
        }

        /// <summary>
        /// Get请求数据
        ///   /// <para>最终以url参数的方式提交</para>
        /// <para>yubaolee 2016-3-3 重构与post同样异步调用</para>
        /// </summary>
        /// <param name="parameters">参数字典,可为空</param>
        /// <param name="requestUri">例如/api/Files/UploadFile</param>
        /// <returns></returns>
        public string Get(Dictionary<string, string> parameters, string requestUri)
        {
            if (parameters != null)
            {
                var strParam = string.Join("&", parameters.Select(o => o.Key + "=" + o.Value));
                requestUri = string.Concat(ConcatURL(requestUri), '?', strParam);
            }
            else
            {
                requestUri = ConcatURL(requestUri);
            }

            var header = _httpClient.DefaultRequestHeaders;


            var result = _httpClient.GetStringAsync(requestUri);
            return result.Result;
        }





        public string GetWithHeader(Dictionary<string, string> parameters, string requestUri, Dictionary<string, string> newheaders)
        {
            if (parameters != null)
            {
                var strParam = string.Join("&", parameters.Select(o => o.Key + "=" + o.Value));
                requestUri = string.Concat(ConcatURL(requestUri), '?', strParam);
            }
            else
            {
                requestUri = ConcatURL(requestUri);
            }

            var headers = _httpClient.DefaultRequestHeaders;
            foreach (var item in newheaders)
            {
                if (headers.Contains(item.Key))
                {
                    headers.Remove(item.Key);
                }

                headers.Add(item.Key, item.Value);
            }
            var result = _httpClient.GetStringAsync(requestUri);
            return result.Result;
        }




        /// <summary>
        /// 以json的方式Post数据 返回string类型
        /// <para>最终以json的方式放置在http体中</para>
        /// </summary>
        /// <param name="entity">实体</param>
        /// <param name="requestUri">例如/api/Files/UploadFile</param>
        /// <returns></returns>
        public string PostString(string str, string requestUri)
        {
            //string request = string.Empty;
            HttpContent httpContent = new StringContent(str);
            httpContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            return Post(requestUri, httpContent);
        }


        public string PostStringWithHeader(string str, string requestUri, Dictionary<string, string> newheaders)
        {
            //string request = string.Empty;
            HttpContent httpContent = new StringContent(str);
            httpContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            var headers = _httpClient.DefaultRequestHeaders;

            foreach (var item in newheaders)
            {
                if (headers.Contains(item.Key))
                {
                    headers.Remove(item.Key);
                }

                headers.Add(item.Key, item.Value);
            }
            return Post(requestUri, httpContent);
        }




        /// <summary>
        /// Post Dic数据
        /// <para>最终以formurlencode的方式放置在http体中</para>
        /// <para>李玉宝于2016-07-15 15:28:41</para>
        /// </summary>
        /// <returns>System.String.</returns>

        public string PostDicForm(Dictionary<string, string> temp, string requestUri)
        {
            HttpContent httpContent = new FormUrlEncodedContent(temp);
            ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(CheckValidationResult);
            httpContent.Headers.ContentType = new MediaTypeHeaderValue("application/x-www-form-urlencoded");
            return Post(requestUri, httpContent);
        }

        /// <summary>
        /// Post Dic数据
        /// <para>最终以formurlencode的方式放置在http体中 解决 FormUrlEncodedContent uri太长的错误，需要使用手动拼接stringcontent</para>
        /// <para></para>
        /// </summary>
        /// <returns>System.String.</returns>

        public string PostDic(Dictionary<string, string> temp, string requestUri)
        {

            ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(CheckValidationResult);
            var encodedItems = temp.Select(i => WebUtility.UrlEncode(i.Key) + "=" + WebUtility.UrlEncode(i.Value));
            var encodedContent = new StringContent(String.Join("&", encodedItems), null, "application/x-www-form-urlencoded");

            // Post away!
            //var response = _httpClient.PostAsync(url, encodedContent);
            //HttpContent httpContent = new FormUrlEncodedContent(temp);
            //ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(CheckValidationResult);
            //httpContent.Headers.ContentType = new MediaTypeHeaderValue("application/x-www-form-urlencoded");
            return Post(requestUri, encodedContent);
        }


        public string PostDicJson(Dictionary<string, string> temp, string requestUri)
        {
            HttpContent httpContent = new FormUrlEncodedContent(temp);
            httpContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            return Post(requestUri, httpContent);
        }

        private static readonly string DefaultUserAgent = "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/71.0.3578.98 Safari/537.36";

        private static bool CheckValidationResult(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors errors)
        {
            return true; //总是接受     
        }




        public string CreateGetHttpResponse()
        {
            HttpWebRequest request = null;
            //HTTPSQ请求  
            UTF8Encoding encoding = new System.Text.UTF8Encoding();
            ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(CheckValidationResult);
            request = WebRequest.Create(_baseIPAddress) as HttpWebRequest;
            request.ProtocolVersion = HttpVersion.Version10;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11;
            request.Method = "GET";
            request.ContentType = "application/x-www-form-urlencoded";
            request.UserAgent = DefaultUserAgent;

            HttpWebResponse response;

            try
            {
                //获得响应流
                response = (HttpWebResponse)request.GetResponse();

                Stream s = response.GetResponseStream();

                StreamReader readStream = new StreamReader(s, Encoding.GetEncoding("GBK"));
                string SourceCode = readStream.ReadToEnd();
                response.Close();
                readStream.Close();
                return SourceCode;
            }
            catch (WebException ex)
            {
                response = ex.Response as HttpWebResponse;

                return null;
            }

        }


        public string CreatePostHttpResponse(IDictionary<string, string> parameters)
        {
            HttpWebRequest request = null;
            //HTTPSQ请求  
            UTF8Encoding encoding = new System.Text.UTF8Encoding();
            ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(CheckValidationResult);
            request = WebRequest.Create(_baseIPAddress) as HttpWebRequest;
            request.ProtocolVersion = HttpVersion.Version10;
            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";
            // request.ContentType = "application/json";
            request.UserAgent = DefaultUserAgent;
            //如果需要POST数据     
            if (!(parameters == null || parameters.Count == 0))
            {
                StringBuilder buffer = new StringBuilder();
                int i = 0;
                foreach (string key in parameters.Keys)
                {
                    if (i > 0)
                    {
                        buffer.AppendFormat("&{0}={1}", key, parameters[key]);
                    }
                    else
                    {
                        buffer.AppendFormat("{0}={1}", key, parameters[key]);
                    }
                    i++;
                }
                byte[] data = encoding.GetBytes(buffer.ToString());
                using (Stream stream = request.GetRequestStream())
                {
                    stream.Write(data, 0, data.Length);
                }
            }

            HttpWebResponse response;

            try
            {
                //获得响应流
                response = (HttpWebResponse)request.GetResponse();
            }
            catch (WebException ex)
            {
                response = ex.Response as HttpWebResponse;
            }

            Stream s = response.GetResponseStream();

            StreamReader readStream = new StreamReader(s, Encoding.UTF8);
            string SourceCode = readStream.ReadToEnd();
            response.Close();
            readStream.Close();
            return SourceCode;
        }


        public string CreatePostHttpResponseForPC(IDictionary<string, string> headers, IDictionary<string, string> parameters)
        {
            HttpWebRequest request = null;
            //HTTPSQ请求  
            UTF8Encoding encoding = new System.Text.UTF8Encoding();
            ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(CheckValidationResult);
            request = WebRequest.Create(_baseIPAddress) as HttpWebRequest;
            request.ProtocolVersion = HttpVersion.Version10;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11;
            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";
            // request.ContentType = "application/json";
            request.UserAgent = DefaultUserAgent;
            //request.Headers.Add("X-CSRF-TOKEN", "bc0cc533-60cc-484a-952d-0b4c1a95672c");
            //request.Referer = "https://www.asianacargo.com/tracking/viewTraceAirWaybill.do";

            //request.Headers.Add("Origin", "https://www.asianacargo.com");
            //request.Headers.Add("Cookie", "JSESSIONID=HP21d2Dq5FoSlG4Fyw4slWwHb0-Sl1CG6jGtj7HE41e5f4aN_R1p!-435435446!117330181");
            //request.Host = "www.asianacargo.com";


            if (!(headers == null || headers.Count == 0))
            {

                foreach (string key in headers.Keys)
                {
                    request.Headers.Add(key, headers[key]);
                }

            }


            //如果需要POST数据     
            if (!(parameters == null || parameters.Count == 0))
            {
                StringBuilder buffer = new StringBuilder();
                int i = 0;
                foreach (string key in parameters.Keys)
                {
                    if (i > 0)
                    {
                        buffer.AppendFormat("&{0}={1}", key, parameters[key]);
                    }
                    else
                    {
                        buffer.AppendFormat("{0}={1}", key, parameters[key]);
                    }
                    i++;
                }
                byte[] data = encoding.GetBytes(buffer.ToString());
                using (Stream stream = request.GetRequestStream())
                {
                    stream.Write(data, 0, data.Length);
                }
            }

            HttpWebResponse response;

            try
            {
                //获得响应流
                response = (HttpWebResponse)request.GetResponse();
                Stream s = response.GetResponseStream();

                StreamReader readStream = new StreamReader(s, Encoding.UTF8);
                string SourceCode = readStream.ReadToEnd();
                response.Close();
                readStream.Close();
                return SourceCode;
            }
            catch (WebException ex)
            {
                response = ex.Response as HttpWebResponse; return null;
            }

        }



        public string CreatePostHttpResponse1(IDictionary<string, string> parameters)
        {
            HttpWebRequest request = null;
            //HTTPSQ请求  
            UTF8Encoding encoding = new System.Text.UTF8Encoding();
            ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(CheckValidationResult);
            request = WebRequest.Create(_baseIPAddress) as HttpWebRequest;
            request.ProtocolVersion = HttpVersion.Version10;
            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";
            // request.ContentType = "application/json";
            request.UserAgent = DefaultUserAgent;
            //如果需要POST数据     
            if (!(parameters == null || parameters.Count == 0))
            {
                StringBuilder buffer = new StringBuilder();
                int i = 0;
                foreach (string key in parameters.Keys)
                {
                    if (i > 0)
                    {
                        buffer.AppendFormat("&{0}={1}", key, parameters[key]);
                    }
                    else
                    {
                        buffer.AppendFormat("{0}={1}", key, parameters[key]);
                    }
                    i++;
                }
                byte[] data = encoding.GetBytes(buffer.ToString());
                using (Stream stream = request.GetRequestStream())
                {
                    stream.Write(data, 0, data.Length);
                }
            }

            HttpWebResponse response;

            try
            {
                //获得响应流
                response = (HttpWebResponse)request.GetResponse();
            }
            catch (WebException ex)
            {
                response = ex.Response as HttpWebResponse;
            }

            Stream s = response.GetResponseStream();

            StreamReader readStream = new StreamReader(s, Encoding.UTF8);
            string SourceCode = readStream.ReadToEnd();
            response.Close();
            readStream.Close();
            return SourceCode;
        }


        public string PostByte(byte[] bytes, string requestUrl)
        {
            HttpContent content = new ByteArrayContent(bytes);
            content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
            return Post(requestUrl, content);
        }

        private string Post(string requestUrl, HttpContent content)
        {
            var result = _httpClient.PostAsync(ConcatURL(requestUrl), content);
            return result.Result.Content.ReadAsStringAsync().Result;
        }

        /// <summary>
        /// 把请求的URL相对路径组合成绝对路径
        /// <para>李玉宝于2016-07-21 9:54:07</para>
        /// </summary>
        private string ConcatURL(string requestUrl)
        {
            return new Uri(_httpClient.BaseAddress, requestUrl).OriginalString;
        }

        private AuthenticationHeaderValue CreateBasicCredentials(string userName, string password)
        {
            string toEncode = userName + ":" + password;
            // The current HTTP specification says characters here are ISO-8859-1.
            // However, the draft specification for the next version of HTTP indicates this encoding is infrequently
            // used in practice and defines behavior only for ASCII.
            Encoding encoding = Encoding.GetEncoding("utf-8");
            byte[] toBase64 = encoding.GetBytes(toEncode);
            string parameter = Convert.ToBase64String(toBase64);

            return new AuthenticationHeaderValue("Basic", parameter);
        }

    }

}
