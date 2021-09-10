using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System.Net.Tests
{
    [TestClass()]
    public class UnitTest1
    {
        [TestMethod()]
        public void GetIPGetIP()
        {
            var ip = IPAddressExtension_A6F086FB3EE3403BB5033720C34DA414.GetIP();
            ip.ToString();
        }
    }
}



