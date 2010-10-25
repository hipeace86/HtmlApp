using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Net;
using System.IO;
using HtmlAgilityPack;
using MySql.Data.MySqlClient;

namespace Jfw.HtmlKit
{
    public class PatentDetailHtml : AbstractHtml
    {
        private Thread runThread = null;
        static readonly string conn = "server=127.0.0.1;user=root;password=;database=tmp;pooling=true;Maximum Pool Size=200;Connect Timeout=60;";
        public PatentDetailHtml(Task task)
            : base(task)
        { }
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
            _task.Status = TaskStatus.Pause;
        }

        public override List<string> GetId()
        {
            List<string> ids = new List<string>();
            using (MySqlDataReader dr = MySqlHelper.ExecuteReader(conn, "select fi_pnum from p2008"))
            {
                while (dr.Read())
                {
                    ids.Add(dr["fi_pnum"].ToString());
                }
                return ids;
            }
        }

        public override void CreateHtmlThread()
        {
            for (int i = 0; i < _task.ThreadCount; i++)
            {
                ThreadPatentDetail thread = new ThreadPatentDetail(_task, paramsQueue);
                thread.StartLoad();
            }
        }
        internal class ThreadPatentDetail
        {
            private Thread thread = null;
            Queue<string> paramsQueue;
            Task _task;
            static readonly string conn = "server=127.0.0.1;user=root;password=;database=tmp;pooling=true;Maximum Pool Size=200;Connect Timeout=60;";

            public ThreadPatentDetail(Task task, Queue<string> queue)
            {
                _task = task;
                paramsQueue = queue;
            }
            public void StartLoad()
            {
                if (thread == null)
                {
                    thread = new Thread(new ThreadStart(Run));
                    thread.Start();
                }
                Console.WriteLine(string.Format("Thread In StartLoad:{0}", Thread.CurrentThread.ManagedThreadId));
            }
            private void Run()
            {
                WebProxy proxy = ProxyPool.GetProxy;
                while (_task.Status == TaskStatus.Run && paramsQueue.Count > 0)
                {
                    string v = paramsQueue.Dequeue().ToString();
                    cj(v, ref proxy);
                    Console.WriteLine(string.Format("Param is:{0}", v));
                }
                if (_task.Status != TaskStatus.Pause)
                {
                    _task.Status = TaskStatus.Completed;
                    Console.WriteLine(_task.Status.ToString());
                }
                Console.WriteLine(_task.ErrorCount);
            }
            private void cj(string param, ref WebProxy proxy)
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
                    //WebProxy proxy = new WebProxy("111.1.32.79", 80);
                    httpRequest.UserAgent = "Mozilla/4.0 (compatible; MSIE 7.0; Windows NT 5.2; .NET CLR 1.1.4322; .NET CLR 2.0.50727)";
                    httpRequest.Accept = "*/*";
                    httpRequest.Headers.Add("Accept-Language", "zh-cn");
                    httpRequest.KeepAlive = true;
                    httpRequest.Timeout = 10000;
                    httpRequest.Method = "GET";
                    httpRequest.Proxy = proxy;
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
                            MySqlHelper.ExecuteNonQuery(conn, string.Format("update p2008 set fi_flag='1' where fi_pnum='{0}'",param), new MySqlParameter[] { });
                        }
                       
                    }
                    catch (WebException we)
                    {
                        Console.WriteLine(we.Message);
                        proxy = ProxyPool.GetProxy;
                        cj(url, ref proxy);
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
