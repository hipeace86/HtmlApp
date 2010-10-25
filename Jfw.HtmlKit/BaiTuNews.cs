using System;
using System.Collections.Generic;
using System.Text;
using Jfw.HtmlKit;
using System.Threading;
using System.Net;
using HtmlAgilityPack;
using System.IO;

namespace Jfw.HtmlKit
{
    public class BaiTuNews : AbstractHtml
    {
        private Thread runThread = null;
        public BaiTuNews(Task task)
            : base(task)
        {

        }
        public override void Start()
        {
            if (runThread == null)
            {
                runThread = new Thread(new ThreadStart(Run));
                runThread.Start();
                _task.Status = TaskStatus.Run;
            }
        }

        public override void Stop()
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public override List<string> GetId()
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public override void CreateHtmlThread()
        {
            for (int i = 0; i < _task.ThreadCount; i++)
            {
                HtmlThread thread = new HtmlThread(_task, paramsQueue);
                thread.StartLoad();
            }
        }

        internal class HtmlThread
        {
            public HtmlThread(Task task, Queue<string> queue)
            {
                _task = task;
                paramsQueue = queue;
            }
            private Thread thread = null;
            Queue<string> paramsQueue;
            Task _task;
            public void StartLoad()
            {
                if (thread == null)
                {
                    thread = new Thread(new ThreadStart(Run));
                    thread.Start();
                }
            }
            private void Run()
            {
                while (_task.Status == TaskStatus.Run && paramsQueue.Count > 0)
                {
                    string v = paramsQueue.Dequeue().ToString();
                    cj(v);
                    Console.WriteLine(string.Format("Param is:{0}", v));
                }
                if (_task.Status != TaskStatus.Pause)
                {
                    _task.Status = TaskStatus.Completed;
                    Console.WriteLine(_task.Status.ToString());
                }
                Console.WriteLine(_task.ErrorCount);
            }
            private void cj(string param)
            {
                bool flag = true;
                for (int i = 0; i < 5 && flag; i++)
                {
                    string url = string.Format("http://www.soopat.com/{0}", param);
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
                    httpRequest.UserAgent = "Mozilla/4.0 (compatible; MSIE 7.0; Windows NT 5.2; .NET CLR 1.1.4322; .NET CLR 2.0.50727)";
                    httpRequest.Accept = "*/*";
                    httpRequest.Headers.Add("Accept-Language", "zh-cn");
                    httpRequest.KeepAlive = true;
                    httpRequest.Timeout = 10000;
                    httpRequest.Method = "GET";
                    try
                    {
                        HttpWebResponse response = (HttpWebResponse)httpRequest.GetResponse();

                        string sResponse = string.Empty;
                        if (response.StatusCode == HttpStatusCode.OK)
                        {
                            using (StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.GetEncoding("utf-8")))
                            {
                                sResponse = reader.ReadToEnd();
                                reader.Close();
                            }
                        }
                        response.Close();

                        HtmlDocument html = new HtmlDocument();
                        html.LoadHtml(sResponse);
                        if (html.DocumentNode.SelectSingleNode("/html/body/div[7]/div/div/div/div/span") != null)
                        {
                            Console.WriteLine(html.DocumentNode.SelectSingleNode("/html/body/div[7]/div/div/div/div/span").InnerHtml);
                            Console.WriteLine(html.DocumentNode.SelectSingleNode("/html/body/div[7]/div/div/div/div/span[2]").InnerHtml);
                            flag = false;
                        }

                    }
                    catch (WebException we)
                    {
                        Console.WriteLine(we.Message);
                        break;
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.ToString());
                        throw e;
                    }
                }
            }
        }
    }
}
