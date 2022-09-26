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
        /// <para/> LAN，全称Local Area Network，中文名叫做局域网。
        /// <para/> WAN，全称Wide Area Network，中文名叫做广域网。
        /// <para/> WLAN，全称Wireless LAN, 无线局域网。
        /// </summary>
        /// <param name="address"></param>
        /// <returns></returns>
        public static bool IsLAN(this IPAddress address)
        {
            if (address == null)
            {
                return false;
            }

            if (address.AddressFamily == AddressFamily.InterNetwork)
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
            }
            else if (address.AddressFamily == AddressFamily.InterNetworkV6)
            {
                //开头只要不是F开头，都是公网IP
                //电信为240e开头的（240e::/20）；移动为2409开头的（2409:8000::/20）；联通为2408开头的（2408:8000::/20）。
                //https://post.smzdm.com/p/ag4oe8o7/
                return address.ToString().StartsWith("f", StringComparison.OrdinalIgnoreCase);
            }

            return false;
        }

        /// <summary>
        /// 判断地址是不是局域网地址
        /// </summary>
        /// <param name="address"></param>
        /// <returns></returns>
        [Obsolete("Use IsLAN")]
        public static bool IsLocalAddress(this IPAddress address)
        {
            return IsLAN(address);
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
                if (item.AddressFamily == addressFamily && item.IsLAN())
                {
                    return item;
                }
            }
            return null;
        }

        public static UnicastIPAddressInformation GetLANInformation(AddressFamily addressFamily = AddressFamily.InterNetwork)
        {
            var ipinfo = GetUnicastIPAddressInformation(NetworkInterfaceType.Ethernet, addressFamily);
            if (ipinfo != null)
            {
                return ipinfo;
            }

            ipinfo = GetUnicastIPAddressInformation(NetworkInterfaceType.Wireless80211, addressFamily);
            if (ipinfo != null)
            {
                return ipinfo;
            }

            ipinfo = GetUnicastIPAddressInformation(NetworkInterfaceType.Wman, addressFamily);
            if (ipinfo != null)
            {
                return ipinfo;
            }

            ipinfo = GetUnicastIPAddressInformation(NetworkInterfaceType.Wwanpp, addressFamily);
            if (ipinfo != null)
            {
                return ipinfo;
            }

            ipinfo = GetUnicastIPAddressInformation(NetworkInterfaceType.Wwanpp2, addressFamily);
            if (ipinfo != null)
            {
                return ipinfo;
            }

            return null;
        }

        public static UnicastIPAddressInformation GetUnicastIPAddressInformation(
            NetworkInterfaceType interfaceType, AddressFamily addressFamily = AddressFamily.InterNetwork)
        {
            var netf = GetUpNetworkInterface(interfaceType);
            return GetFirstIP(netf, addressFamily);
        }

        public static NetworkInterface[] GetUpNetworkInterface(NetworkInterfaceType interfaceType)
        {
            var networkInterfaces = NetworkInterface.GetAllNetworkInterfaces();
            var result = from i in networkInterfaces
                         where i.NetworkInterfaceType == interfaceType
                         where i.OperationalStatus == OperationalStatus.Up
                         select i;
            return result.ToArray();
        }

        public static UnicastIPAddressInformation GetFirstIP(NetworkInterface[] networkInterfaces, AddressFamily addressFamily = AddressFamily.InterNetwork)
        {
            foreach (var item in networkInterfaces)
            {
                IPInterfaceProperties iPInterfaceProperties = item.GetIPProperties();
                foreach (var ip in iPInterfaceProperties.UnicastAddresses)
                {
                    if (ip.Address.AddressFamily == addressFamily)
                    {
                        return ip;
                    }
                }
            }
            return null;
        }

        /// <summary>
        /// 取得网关IP
        /// </summary>
        /// <returns></returns>
        public static async ValueTask<IPAddress> GetGateway(AddressFamily addressFamily = AddressFamily.InterNetwork, bool pingCheck = true)
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

                    if (pingCheck)
                    {
                        //如果能够Ping通网关
                        if (await IsPingIP(gateWay.Address))
                        {
                            //得到网关地址
                            gateway = gateWay.Address;
                            //跳出循环
                            break;
                        }
                    }
                    else
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
