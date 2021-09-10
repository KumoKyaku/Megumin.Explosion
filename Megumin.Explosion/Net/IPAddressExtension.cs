using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace System.Net
{
    /// <summary>
    /// IPAddressExtension
    /// </summary>
    public static class IPAddressExtension_A6F086FB3EE3403BB5033720C34DA414
    {
        /// <summary>
        /// 取得IP的各个部分
        /// </summary>
        /// <param name="address"></param>
        /// <returns></returns>
        public static int[] GetIntPart(this IPAddress address)
        {
            var stringlist = address.GetStringParts();
            int[] res = new int[stringlist.Length];
            for (int i = 0; i < stringlist.Length; i++)
            {
                res[i] = int.Parse(stringlist[i]);
            }
            return res;
        }

        /// <summary>
        /// 取得IP的各个部分
        /// </summary>
        /// <param name="address"></param>
        /// <returns></returns>
        public static string[] GetStringParts(this IPAddress address)
        {
            string[] res;
            if (address.AddressFamily == Sockets.AddressFamily.InterNetwork)
            {
                //IPV4
                res = address.ToString().Split('.');
            }
            else
            {
                res = address.ToString().Split(':');
            }
            return res;
        }

        /// <summary>
        /// 是不是A类地址
        /// </summary>
        /// <param name="address"></param>
        /// <returns></returns>
        public static bool IsA(this IPAddress address)
        {
            var parts = address.GetIntPart();
            return parts[0] >= 1 && parts[0] <= 126;
        }

        /// <summary>
        /// 是不是B类地址
        /// </summary>
        /// <param name="address"></param>
        /// <returns></returns>
        public static bool IsB(this IPAddress address)
        {
            var parts = address.GetIntPart();
            return parts[0] >= 128 && parts[0] <= 191;
        }

        /// <summary>
        /// 是不是C类地址
        /// </summary>
        /// <param name="address"></param>
        /// <returns></returns>
        public static bool IsC(this IPAddress address)
        {
            var parts = address.GetIntPart();
            return parts[0] >= 192 && parts[0] <= 223;
        }

        /// <summary>
        /// 是不是D类地址（多点广播）
        /// </summary>
        /// <param name="address"></param>
        /// <returns></returns>
        public static bool IsD(this IPAddress address)
        {
            var parts = address.GetIntPart();
            return parts[0] >= 224 && parts[0] <= 239;
        }

        /// <summary>
        /// 是不是E类地址（广播）
        /// </summary>
        /// <param name="address"></param>
        /// <returns></returns>
        public static bool IsE(this IPAddress address)
        {
            var parts = address.GetIntPart();
            return parts[0] >= 240 && parts[0] <= 255;
        }

        /// <summary>
        /// 判断地址是不是局域网地址
        /// </summary>
        /// <param name="address"></param>
        /// <returns></returns>
        public static bool IsLocalAddress(this IPAddress address)
        {
            var parts = address.GetIntPart();
            if (address.IsA())
            {
                if (parts[0] == 10)
                {
                    return true;
                }
            }

            if (address.IsB())
            {
                if (parts[0] == 172)
                {
                    if (parts[1] >= 16 && parts[1] <= 31)
                    {
                        return true;
                    }
                }
            }

            if (address.IsC())
            {
                if (parts[0] == 192 && parts[1] == 168)
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// 取得IP
        /// <para>true取得局域网IP，flase取得外网IP，默认值为flase</para>
        /// </summary>
        /// <param name="IsLAN">true取得局域网IP，flase取得外网IP，默认值为flase</param>
        /// <returns></returns>
        public static IPAddress GetIP(bool IsLAN = false)
        {
            if (IsLAN)
            {
                return GetLANIP();
            }
            else
            {
                return GetGloablIPAsync().Result;
            }
        }

        public static IPAddress GetLANIP()
        {
            var list = Dns.GetHostEntry(Dns.GetHostName()).AddressList;
            foreach (var item in list)
            {
                if (item.AddressFamily == Sockets.AddressFamily.InterNetwork && item.IsLocalAddress())
                {
                    return item;
                }
            }
            return IPAddress.Loopback;
        }

        public static async ValueTask<IPAddress> GetIPAsync(bool IsLAN = false)
        {
            if (IsLAN)
            {
                return GetLANIP();
            }
            else
            {
                return await GetGloablIPAsync();
            }
        }

        public static async ValueTask<IPAddress> GetGloablIPAsync()
        {
            Uri uri = new Uri("http://ip-api.com/json");
            System.Net.HttpWebRequest req = (System.Net.HttpWebRequest)System.Net.WebRequest.Create(uri);
            req.Method = "get";
            using (Stream s = (await req.GetResponseAsync()).GetResponseStream())
            {
                using (StreamReader reader = new StreamReader(s))
                {
                    char[] ch = { '[', ']' };
                    string str = reader.ReadToEnd();
                    Match m = Regex.Match(str, @"""query"":""(\S+)""");
                    var ipstring = m.Value.Trim(ch);
                    if (m.Success)
                    {
                        ipstring = m.Groups[1].Value;
                    }

                    return IPAddress.Parse(ipstring);
                }
            }
        }
    }
}
