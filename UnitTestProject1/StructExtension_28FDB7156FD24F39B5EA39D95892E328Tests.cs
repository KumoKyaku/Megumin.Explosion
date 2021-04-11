using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System.Tests
{
    [TestClass()]
    public class StructExtension_28FDB7156FD24F39B5EA39D95892E328Tests
    {
        [TestMethod()]
        public void SnapCeilTest()
        {
            int a = 6;
            int b = 5;
            a.SnapCeil(b);
            Assert.AreEqual(10, a);

            a = 9;
            a.SnapFloor(b);
            Assert.AreEqual(5, a);


            double da = 6.2f;
            double db = 1.5f;
            da.SnapCeil(db);
            Assert.AreEqual(7.5f, da);

            da = 6.5f;
            da.SnapFloor(db);
            Assert.AreEqual(6, da);
        }
    }
}