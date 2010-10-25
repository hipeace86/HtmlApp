using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Xml;

namespace Jfw.HtmlKit
{
    public class ProxyPool
    {
        private static ProxyPool fpool = null;
        private XmlDocument XmlDoc = new XmlDocument();
        private static int Count = 0;
        private static XmlNodeList ProxyList = null;
        private static Random randObj = new Random();

        protected ProxyPool()
        {
            XmlDoc.Load("proxy.xml");
            ProxyList = XmlDoc.SelectNodes("proxys/proxy");
            Count = ProxyList.Count;
        }
        public static WebProxy GetProxy
        {
            get
            {
                if (fpool == null)
                {
                    fpool = new ProxyPool();
                }
                XmlNode node = ProxyList[randObj.Next(0, Count)];
                return new WebProxy(node.Attributes["host"].Value.Trim(), int.Parse(node.Attributes["port"].Value.Trim()));
            }
        }

    }
}
