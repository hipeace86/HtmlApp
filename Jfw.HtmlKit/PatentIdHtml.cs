using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Net;
using System.Text.RegularExpressions;
using System.IO;
using MySql.Data.MySqlClient;

namespace Jfw.HtmlKit
{
    public class PatentIdHtml : AbstractHtml
    {
        private Thread runThread = null;
        public PatentIdHtml(Task task)
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
            _task.Status = TaskStatus.Pause;
        }

        public override List<string> GetId()
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public override void CreateHtmlThread()
        {
            for (int i = 0; i < _task.ThreadCount; i++)
            {
                ThreadPatent thread = new ThreadPatent(_task, paramsQueue);
                thread.StartLoad();
            }
        }

        internal class ThreadPatent
        {
            private Thread thread = null;
            Queue<string> paramsQueue;
            Task _task;
            static readonly string conn = "server=127.0.0.1;user=root;password=;database=tmp;pooling=true;Maximum Pool Size=200;Connect Timeout=60;";
            public ThreadPatent(Task task, Queue<string> queue)
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
                Console.WriteLine(_task.ErrorCount);
            }
            private void cj(string param, ref WebProxy proxy)
            {
                bool flag = true;
                for (int i = 0; i < 5 && flag; i++)
                {
                    string url = string.Format("http://www.soopat.com/Home/Result?SearchWord=SQRQ%3A(200808)%20&PatentIndex={0}", param);
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
                        Regex regex = new Regex("/Patent/\\d+");
                        MatchCollection Matches = regex.Matches(sResponse);
                        if (Matches.Count > 0)
                        {
                            flag = false;
                            foreach (Match m in Matches)
                            {
                                try
                                {
                                    MySqlHelper.ExecuteNonQuery(conn, string.Format("insert into p2008 values('{0}','0')", m.Value.ToString()), new MySqlParameter[] { });
                                    Console.WriteLine(string.Format("{0}--采集成功", m.Value.ToString()));
                                }
                                catch (Exception e)
                                {
                                    Console.WriteLine(e.ToString());
                                }
                                _task.ErrorCount++;
                            }
                            Console.WriteLine(string.Format("Use Proxy Host:{0}  Port:{1}", proxy.Address.Host, proxy.Address.Port.ToString()));
                        }
                        else
                        {
                            proxy = ProxyPool.GetProxy;
                            cj(url, ref proxy);
                            Console.WriteLine(string.Format("Change Proxy Host:{0}  Port:{1}", proxy.Address.Host, proxy.Address.Port.ToString()));
                            break;
                        }
                        //return sResponse;
                    }
                    catch (WebException we)
                    {
                        //if (we.Message == "操作超时")
                        //{
                        //    proxy = ProxyControler.Instance.RandmProxy;
                        //    return GetHtmlFromUrl(url, _cookieContainer, ref proxy);
                        //}
                        Console.WriteLine(we.Message);
                        proxy = ProxyPool.GetProxy;
                        cj(url, ref proxy);
                        break;
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.ToString());
                        //return string.Empty;
                    }
                    //return string.Empty;
                }
            }
        }
    }
}
