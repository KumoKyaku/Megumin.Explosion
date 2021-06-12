using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tests
{
    [TestClass()]
    public class StringExtension_E68DD56066C94F2286AF4BD18126A406Tests
    {
        [TestMethod()]
        public void GetEndNumberTest()
        {
            var str = "test123";
            str.GetEndNumber(out var res);
            Assert.AreEqual(123, res);

            "set12sfa".GetEndNumber(out var res2);
            Assert.AreEqual(0, res2);

            "sdfasd(1)".GetEndNumber(out var res3);
            Assert.AreEqual(1, res3);

            "sadfasf (654)".GetEndNumber(out var res4);
            Assert.AreEqual(654, res4);
        }

        [TestMethod()]
        public void FileNameAddOneTest()
        {
            var str = "test123";
            var res = str.FileNameAddOne();
            Assert.AreEqual("test124", res);

            var res2 = "set12sfa".FileNameAddOne();
            Assert.AreEqual("set12sfa (1)", res2);

            var res3 =  "sdfasd(1)".FileNameAddOne();
            Assert.AreEqual("sdfasd(2)", res3);

            var res4 = "sadfasf (654)".FileNameAddOne();
            Assert.AreEqual("sadfasf (655)", res4);
        }
    }
}

namespace System.Tests
{
    [TestClass()]
    public class StringExtension_E68DD56066C94F2286AF4BD18126A406Tests
    {
        [TestMethod()]
        public void TryPerseHexColorTest()
        {
            string orignal = "FFff00";
            orignal.TryPerseHexColor(out var r,out var g,out var b,out var a);
            Assert.AreEqual(255,r);
            Assert.AreEqual(255, g);
            Assert.AreEqual(0, b);
            Assert.AreEqual(255, a);
        }
    }
}