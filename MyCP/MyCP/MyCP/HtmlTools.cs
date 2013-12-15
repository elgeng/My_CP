using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace MyCP
{
    class HtmlTools
    {
        /// <summary>
        ///  获取网址HTML
        /// </summary>
        /// <param name="URL">网址 </param>
        /// <returns> </returns>
        public static string GetHtml(string URL)
        {
            WebRequest wrt;
            wrt = WebRequest.Create(URL);
            wrt.Credentials = CredentialCache.DefaultCredentials;
            WebResponse wrp;
            wrp = wrt.GetResponse();
            string reader = new StreamReader(wrp.GetResponseStream(), Encoding.GetEncoding("gb2312")).ReadToEnd();
            try
            {
                wrt.GetResponse().Close();
            }
            catch (WebException ex)
            {
                throw ex;
            }
            return reader;
        }

        /// <summary>
        /// 获取网站cookie
        /// </summary>
        /// <param name="URL">网址 </param>
        /// <param name="cookie">cookie </param>
        /// <returns> </returns>
        public static string GetHtml(string URL, out string cookie)
        {
            WebRequest wrt;
            wrt = WebRequest.Create(URL);
            wrt.Credentials = CredentialCache.DefaultCredentials;
            WebResponse wrp;

            wrp = wrt.GetResponse();


            string html = new StreamReader(wrp.GetResponseStream(), Encoding.GetEncoding("gb2312")).ReadToEnd();

            try
            {
                wrt.GetResponse().Close();
            }
            catch (WebException ex)
            {
                throw ex;
            }

            cookie = wrp.Headers.Get("Set-Cookie");
            return html;
        }

        public static string GetHtml(string URL, string postData, string cookie, out string header, string server)
        {
            return GetHtml(server, URL, postData, cookie, out header);
        }

        public static string GetHtml(string server, string URL, string postData, string cookie, out string header)
        {
            byte[] byteRequest = Encoding.GetEncoding("gb2312").GetBytes(postData);
            return GetHtml(server, URL, byteRequest, cookie, out header);
        }

        public static string GetHtml(string server, string URL, byte[] byteRequest, string cookie, out string header)
        {
            byte[] bytes = GetHtmlByBytes(server, URL, byteRequest, cookie, out header);
            Stream getStream = new MemoryStream(bytes);
            StreamReader streamReader = new StreamReader(getStream, Encoding.GetEncoding("gb2312"));
            string getString = streamReader.ReadToEnd();
            streamReader.Close();
            getStream.Close();
            return getString;
        }


        /// <summary>
        /// Post模式浏览
        /// </summary>
        /// <param name="server">服务器地址 </param>
        /// <param name="URL">网址 </param>
        /// <param name="byteRequest">流 </param>
        /// <param name="cookie">cookie </param>
        /// <param name="header">句柄 </param>
        /// <returns> </returns>
        public static byte[] GetHtmlByBytes(string server, string URL, byte[] byteRequest, string cookie, out string header)
        {
            long contentLength;
            HttpWebRequest httpWebRequest;
            HttpWebResponse webResponse;
            Stream getStream;

            httpWebRequest = (HttpWebRequest)HttpWebRequest.Create(URL);
            CookieContainer co = new CookieContainer();
            co.SetCookies(new Uri(server), cookie);

            httpWebRequest.CookieContainer = co;

            httpWebRequest.ContentType = "application/x-www-form-urlencoded";
            httpWebRequest.Accept =
                "image/gif, image/x-xbitmap, image/jpeg, image/pjpeg, application/x-shockwave-flash, application/vnd.ms-excel, application/vnd.ms-powerpoint, application/msword, */*";
            httpWebRequest.Referer = server;
            httpWebRequest.UserAgent =
                "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.1; SV1; Maxthon; .NET CLR 1.1.4322)";
            httpWebRequest.Method = "Post";
            httpWebRequest.ContentLength = byteRequest.Length;
            Stream stream;
            stream = httpWebRequest.GetRequestStream();
            stream.Write(byteRequest, 0, byteRequest.Length);
            stream.Close();
            webResponse = (HttpWebResponse)httpWebRequest.GetResponse();
            header = webResponse.Headers.ToString();
            getStream = webResponse.GetResponseStream();
            contentLength = webResponse.ContentLength;

            byte[] outBytes = new byte[contentLength];
            outBytes = ReadFully(getStream);
            getStream.Close();
            return outBytes;
        }

        public static byte[] ReadFully(Stream stream)
        {
            byte[] buffer = new byte[128];
            using (MemoryStream ms = new MemoryStream())
            {
                while (true)
                {
                    int read = stream.Read(buffer, 0, buffer.Length);
                    if (read <= 0)
                        return ms.ToArray();
                    ms.Write(buffer, 0, read);
                }
            }
        }

        /// <summary>
        /// Get模式
        /// </summary>
        /// <param name="URL">网址 </param>
        /// <param name="cookie">cookies </param>
        /// <param name="header">句柄 </param>
        /// <param name="server">服务器 </param>
        /// <param name="val">服务器 </param>
        /// <returns> </returns>
        public static string GetHtml(string URL, string cookie, out string header, string server)
        {
            return GetHtml(URL, cookie, out header, server, "");
        }

        /// <summary>
        /// Get模式浏览
        /// </summary>
        /// <param name="URL">Get网址 </param>
        /// <param name="cookie">cookie </param>
        /// <param name="header">句柄 </param>
        /// <param name="server">服务器地址 </param>
        /// <param name="val"> </param>
        /// <returns> </returns>
        public static string GetHtml(string URL, string cookie, out string header, string server, string val)
        {
            HttpWebRequest httpWebRequest;
            HttpWebResponse webResponse;
            Stream getStream;
            StreamReader streamReader;
            string getString = "";
            httpWebRequest = (HttpWebRequest)HttpWebRequest.Create(URL);
            httpWebRequest.Accept = "*/*";
            httpWebRequest.Referer = server;
            CookieContainer co = new CookieContainer();
            co.SetCookies(new Uri(server), cookie);
            httpWebRequest.CookieContainer = co;
            httpWebRequest.UserAgent =
                "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.1; SV1; Maxthon; .NET CLR 1.1.4322)";
            httpWebRequest.Method = "GET";
            webResponse = (HttpWebResponse)httpWebRequest.GetResponse();
            header = webResponse.Headers.ToString();
            getStream = webResponse.GetResponseStream();
            streamReader = new StreamReader(getStream, Encoding.GetEncoding("gb2312"));
            getString = streamReader.ReadToEnd();

            streamReader.Close();
            getStream.Close();
            return getString;
        }

        /// <summary>
        /// 返回验证码图片流
        /// </summary>
        /// <param name="server">服务器地址 </param>
        /// <param name="URL">验证码网址 </param>
        /// <param name="cookie">cookie </param>
        /// <returns> </returns>
        public static Stream GetStreamByBytes(string server, string URL, string cookie)
        {
            Stream stream = GetCode(server, URL, cookie);
            return stream;
        }

        /// <summary>
        /// //获取验证码
        /// </summary>
        /// <param name="server">服务器地址 </param>
        /// <param name="url">验证码网址 </param>
        /// <param name="cookie">cookie </param>
        /// <returns> </returns>
        public static Stream GetCode(string server, string url, string cookie)
        {

            HttpWebRequest httpWebRequest;

            httpWebRequest = (HttpWebRequest)HttpWebRequest.Create(url);
            CookieContainer co = new CookieContainer();
            co.SetCookies(new Uri(server), cookie);
            httpWebRequest.CookieContainer = co;

            HttpWebResponse response = (HttpWebResponse)httpWebRequest.GetResponse();
            Stream stream = response.GetResponseStream();
            return stream;

        }

        /// <summary>
        /// 获取html
        /// </summary>
        /// <param name="server"> </param>
        /// <param name="url"> </param>
        /// <param name="cookie"> </param>
        /// <returns> </returns>

        public static string GetUser(string server, string url, string cookie)
        {
            string getString = "";
            try
            {
                HttpWebRequest httpWebRequest;
                StreamReader streamReader;


                httpWebRequest = (HttpWebRequest)HttpWebRequest.Create(url);
                CookieContainer co = new CookieContainer();
                co.SetCookies(new Uri(server), cookie);
                httpWebRequest.CookieContainer = co;

                HttpWebResponse response = (HttpWebResponse)httpWebRequest.GetResponse();

                Stream stream = response.GetResponseStream();


                streamReader = new StreamReader(stream, Encoding.GetEncoding("gb2312"));
                getString = streamReader.ReadToEnd();


                try
                {
                    httpWebRequest.GetResponse().Close();
                }
                catch (WebException ex)
                {
                    throw ex;
                }
                streamReader.Close();
                stream.Close();

            }
            catch
            {
            }
            return getString;

        }

    }

}
