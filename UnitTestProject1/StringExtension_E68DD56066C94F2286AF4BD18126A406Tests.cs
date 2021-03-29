using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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