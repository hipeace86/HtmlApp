﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.IO;
using System.Web;

namespace Jfw.HtmlKit
{
    public class So5iPatent
    {
        static CookieContainer __Cookie = null;
        private static CookieContainer InitRequest()
        {
            __Cookie = new CookieContainer();
            string url = "http://so.5ipatent.com/SearchResult.aspx?queryString=ad:200808*&patentClass=7&isCN=0";
            WebRequest request = WebRequest.Create(url);
            HttpWebRequest httpRequest = request as HttpWebRequest;
            if (httpRequest == null)
            {
                throw new ApplicationException(
               string.Format("Invalid url string: {0}", url)
               );
            }
            //有些网站加入了限制,只有先从首页或验证页面访问才能访问,一般都记录到cookie中
            //这里就是将验证后的cookie容器赋给采集的client
            //WebProxy proxy = new WebProxy("111.1.32.79", 80);
            httpRequest.UserAgent = "Mozilla/4.0 (compatible; MSIE 7.0; Windows NT 5.2; .NET CLR 1.1.4322; .NET CLR 2.0.50727)";
            httpRequest.Accept = "*/*";
            httpRequest.Headers.Add("Accept-Language", "zh-cn");
            httpRequest.KeepAlive = true;
            httpRequest.Timeout = 10000;
            httpRequest.Method = "GET";
            httpRequest.CookieContainer = __Cookie;
            HttpWebResponse response = (HttpWebResponse)httpRequest.GetResponse();
            response.Close();
            return __Cookie;

        }
        public static string GetPageHtml(string postData)
        {
            if (__Cookie == null)
            {
                InitRequest();
            }
            string url = "http://so.5ipatent.com/SearchResult.aspx?queryString=ad:200808*&patentClass=7&isCN=0";
            WebRequest request = WebRequest.Create(url);
            HttpWebRequest httpRequest = request as HttpWebRequest;
            if (httpRequest == null)
            {
                throw new ApplicationException(
               string.Format("Invalid url string: {0}", url)
               );
            }
            //有些网站加入了限制,只有先从首页或验证页面访问才能访问,一般都记录到cookie中
            //这里就是将验证后的cookie容器赋给采集的client
            //WebProxy proxy = new WebProxy("111.1.32.79", 80);
            httpRequest.UserAgent = "Mozilla/4.0 (compatible; MSIE 7.0; Windows NT 5.2; .NET CLR 1.1.4322; .NET CLR 2.0.50727)";
            httpRequest.Accept = "*/*";
            httpRequest.Headers.Add("Accept-Language", "zh-cn");
            httpRequest.KeepAlive = true;
            httpRequest.Timeout = 10000;
            httpRequest.Referer = url;
            httpRequest.Method = "POST";
            httpRequest.CookieContainer = __Cookie;
            byte[] PostData = Encoding.UTF8.GetBytes(HttpUtility.HtmlEncode(postData));
            httpRequest.ContentLength = PostData.Length;
            Stream requestStream = httpRequest.GetRequestStream();
            requestStream.Write(PostData, 0, PostData.Length);
            requestStream.Close();

            Stream responseStream;
            try
            {
                responseStream = httpRequest.GetResponse().GetResponseStream();
                
                #region 读取服务器返回信息
                string stringResponse = string.Empty;
                using (StreamReader responseReader =
                    new StreamReader(responseStream, Encoding.UTF8))
                {
                    stringResponse = responseReader.ReadToEnd();
                }
                responseStream.Close();
                #endregion

                return stringResponse;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
            return string.Empty;
        }
    }
}
