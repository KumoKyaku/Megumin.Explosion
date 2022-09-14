using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
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
        /// 尝试Ping指定IP是否能够Ping通
        /// </summary>
        /// <param name="address">指定IP</param>
        /// <param name="timeout">Ping类默认超时是5000 https://source.dot.net/#System.Net.Ping/System/Net/NetworkInformation/Ping.cs,16</param>
        /// <returns>true 是 false 否</returns>
        public static async ValueTask<bool> IsPingIP(IPAddress address, int timeout = 1000)
        {
            try
            {
                //创建Ping对象
                Ping ping = new Ping();
                //接受Ping返回值
                PingReply reply = await ping.SendPingAsync(address, timeout);
                if (reply.Status == IPStatus.Success)
                {
                    //Ping通
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch
            {
                //Ping失败
                return false;
            }
        }

        /// <summary>
        /// 取得局域网IP
        /// </summary>
        /// <returns></returns>
        public static IPAddress GetLANIP(AddressFamily addressFamily = AddressFamily.InterNetwork)
        {
            var list = Dns.GetHostEntry(Dns.GetHostName()).AddressList;
            foreach (var item in list)
            {
                if (item.AddressFamily == addressFamily)
                {
                    return item;
                }
            }
            return null;
        }

        /// <summary>
        /// 取得网关IP
        /// </summary>
        /// <returns></returns>
        public static async ValueTask<IPAddress> GetGateway(AddressFamily addressFamily = AddressFamily.InterNetwork)
        {
            //网关地址
            IPAddress gateway = null;
            //获取所有网卡
            NetworkInterface[] nics = NetworkInterface.GetAllNetworkInterfaces();
            //遍历数组
            foreach (var netWork in nics)
            {
                //单个网卡的IP对象
                IPInterfaceProperties ip = netWork.GetIPProperties();
                //获取该IP对象的网关
                GatewayIPAddressInformationCollection gateways = ip.GatewayAddresses;
                foreach (var gateWay in gateways)
                {
                    if (gateWay.Address.AddressFamily != addressFamily)
                    {
                        continue;
                    }

                    //如果能够Ping通网关
                    if (await IsPingIP(gateWay.Address))
                    {
                        //得到网关地址
                        gateway = gateWay.Address;
                        //跳出循环
                        break;
                    }
                }
                //如果已经得到网关地址
                if (gateway != null)
                {
                    //跳出循环
                    break;
                }
            }

            //返回网关地址
            return gateway;
        }

        /// <summary>
        /// 取得广域网IP
        /// </summary>
        /// <returns></returns>
        public static async ValueTask<IPAddress> GetWANIP(AddressFamily addressFamily = AddressFamily.InterNetwork)
        {
            Uri uri = new Uri("http://ip.42.pl/raw");
            System.Net.HttpWebRequest req = (System.Net.HttpWebRequest)System.Net.WebRequest.Create(uri);
            req.Method = "get";
            using (Stream s = (await req.GetResponseAsync()).GetResponseStream())
            {
                using (StreamReader reader = new StreamReader(s))
                {
                    var ipstring = reader.ReadToEnd();
                    Match m = Regex.Match(reader.ReadToEnd(), @"""query"":""(\S+)""");
                    if (m.Success)
                    {
                        ipstring = m.Groups[1].Value;
                    }
                    IPAddress.TryParse(ipstring, out var iPAddress);
                    return iPAddress;
                }
            }
        }

        /// <summary>
        /// 取得IP
        /// <para>true取得局域网IP，flase取得外网IP，默认值为flase</para>
        /// </summary>
        /// <param name="IsLAN">true取得局域网IP，flase取得外网IP，默认值为flase</param>
        /// <param name="addressFamily"></param>
        /// <returns></returns>
        public static IPAddress GetIP(bool IsLAN = false, AddressFamily addressFamily = AddressFamily.InterNetwork)
        {
            if (IsLAN)
            {
                return GetLANIP(addressFamily);
            }
            else
            {
                return GetWANIP(addressFamily).Result;
            }
        }

        public static async ValueTask<IPAddress> GetIPAsync(bool IsLAN = false, AddressFamily addressFamily = AddressFamily.InterNetwork)
        {
            if (IsLAN)
            {
                return GetLANIP(addressFamily);
            }
            else
            {
                return await GetWANIP(addressFamily);
            }
        }
    }
}
