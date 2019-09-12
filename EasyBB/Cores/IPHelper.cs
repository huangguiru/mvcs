using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;

namespace EasyBB.Cores
{
    public class IPHelper
    {
        /// <summary>
        /// 获取访问客户端的IPV4地址
        /// </summary>
        /// <returns></returns>
        public static string GetClientIPv4Address()
        {
            string ipv4 = String.Empty;
            foreach (IPAddress ip in Dns.GetHostAddresses(GetClientIP()))
            {
                if (ip.AddressFamily.ToString() == "InterNetwork")
                {
                    ipv4 = ip.ToString();
                    break;
                }
            }

            if (ipv4 != String.Empty)
            {
                return ipv4;
            }
            // 利用 Dns.GetHostEntry 方法，由获取的 IPv6 位址反查 DNS 纪录，
            // 再逐一判断何者为 IPv4 协议，即可转为 IPv4 位址。
            foreach (IPAddress ip in Dns.GetHostEntry(GetClientIP()).AddressList)
            //foreach (IPAddress ip in Dns.GetHostAddresses(Dns.GetHostName()))
            {
                if (ip.AddressFamily.ToString() == "InterNetwork")
                {
                    ipv4 = ip.ToString();
                    break;
                }
            }

            return ipv4;
        }
        public static string GetClientIP()
        {
            if (null == HttpContext.Current.Request.ServerVariables["HTTP_VIA"])
            {
                return HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
            }
            else
            {
                return HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
            }
        }

        /// <summary>
        /// 淘宝api
        /// </summary>
        /// <param name="strIP"></param>
        /// <returns></returns>
        public static string GetIPCitys(string strIP)
        {
            try
            {
                string Url = "http://ip.taobao.com/service/getIpInfo.php?ip=" + strIP + "";

                System.Net.WebRequest wReq = System.Net.WebRequest.Create(Url);
                wReq.Timeout = 2000;
                System.Net.WebResponse wResp = wReq.GetResponse();
                System.IO.Stream respStream = wResp.GetResponseStream();
                using (System.IO.StreamReader reader = new System.IO.StreamReader(respStream))
                {
                    string jsonText = reader.ReadToEnd();
                    JObject ja = (JObject)JsonConvert.DeserializeObject(jsonText);
                    if (ja["code"].ToString() == "0")
                    {
                        string c = ja["data"]["city"].ToString();
                        int ci = c.IndexOf('市');
                        if (ci != -1)
                        {
                            c = c.Remove(ci, 1);
                        }
                        return c;
                    }
                    else
                    {
                        return "未知";
                    }
                }
            }
            catch (Exception)
            {
                return ("未知");
            }
        }
    }
}