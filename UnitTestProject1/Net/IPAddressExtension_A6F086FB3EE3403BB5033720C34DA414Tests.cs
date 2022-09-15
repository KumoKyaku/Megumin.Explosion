using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Net;
using System;
using System.Collections.Generic;
using System.Text;

namespace System.Net.Tests
{
    [TestClass()]
    public class IPAddressExtension_A6F086FB3EE3403BB5033720C34DA414Tests
    {
        [TestMethod()]
        public void GetIPGetIP()
        {
            var ip = IPAddressExtension_A6F086FB3EE3403BB5033720C34DA414.GetIP();
            ip?.ToString();
        }

        [TestMethod()]
        public void GetIPAsyncTest()
        {
            var ip = IPAddressExtension_A6F086FB3EE3403BB5033720C34DA414.GetIPAsync().Result;
            ip?.ToString();
        }

        [TestMethod()]
        public void GetGatewayTest()
        {
            var ip = IPAddressExtension_A6F086FB3EE3403BB5033720C34DA414.GetGateway().Result;
            Assert.AreEqual("192.168.1.1", ip?.ToString());
        }

        [TestMethod()]
        public void GetLANInformationTest()
        {
            var ip = IPAddressExtension_A6F086FB3EE3403BB5033720C34DA414.GetLANInformation();
            ip?.ToString();
        }
    }
}



