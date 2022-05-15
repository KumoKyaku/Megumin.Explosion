using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

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

            var res3 = "sdfasd(1)".FileNameAddOne();
            Assert.AreEqual("sdfasd(2)", res3);

            var res4 = "sadfasf (654)".FileNameAddOne();
            Assert.AreEqual("sadfasf (655)", res4);
        }

        [TestMethod()]
        public void SortAaBbCcTest()
        {
            List<string> list = new List<string>()
            {
                "AaBBcc",
                "aabbCC",
                "AAbBcc",
                "BaCC"
            };

            var a1 = "A".CompareAaBbCc("a");
            Assert.IsTrue(a1 < 0);

            var a2 = "B".CompareAaBbCc("a");
            Assert.IsTrue(a2 > 0);

            var a3 = "a".CompareAaBbCc("A");
            Assert.IsTrue(a3 > 0);

            var a4 = "a".CompareAaBbCc("B");
            Assert.IsTrue(a4 < 0);

            var a5 = "b".CompareAaBbCc("A");
            Assert.IsTrue(a5 > 0);

            var a6 = "Ab".CompareAaBbCc("AbA");
            Assert.IsTrue(a6 < 0);


            var res = list[0].CompareAaBbCc(list[1]);
            Assert.IsTrue(res < 0);

            var res2 = list[1].CompareAaBbCc(list[2]);
            Assert.IsTrue(res2 > 0);

            var res3 = list[0].CompareAaBbCc(list[2]);
            Assert.IsTrue(res3 > 0);

            list.Sort((a, b) => a.CompareAaBbCc(b));
            Assert.AreEqual(list[0], "AAbBcc");
            Assert.AreEqual(list[1], "AaBBcc");
            Assert.AreEqual(list[2], "aabbCC");
            Assert.AreEqual(list[3], "BaCC");
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
            orignal.TryPerseHexColor(out var r, out var g, out var b, out var a);
            Assert.AreEqual(255, r);
            Assert.AreEqual(255, g);
            Assert.AreEqual(0, b);
            Assert.AreEqual(255, a);
        }
    }
}