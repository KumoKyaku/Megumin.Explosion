using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tests
{
    [TestClass()]
    public class StructExtension_28FDB7156FD24F39B5EA39D95892E328Tests
    {
        [TestMethod()]
        public void ToBinaryStringTest()
        {
            int a = 0b0000_1111_0000_1111_0000_1111_0000_1111;
            string s = a.ToBinaryString(true);
            Assert.AreEqual("0000_1111_0000_1111_0000_1111_0000_1111", s);
            string s2 = a.ToBinaryString();
            Assert.AreEqual("00001111000011110000111100001111", s2);
        }

        [TestMethod()]
        public void SnapRoundTest()
        {
            int a = 6;
            int b = 5;

            a = 1;
            a.SnapRound(b);
            Assert.AreEqual(0, a);

            a = 2;
            a.SnapRound(b);
            Assert.AreEqual(0, a);

            a = 3;
            a.SnapRound(b);
            Assert.AreEqual(5, a);

            a = 6;
            a.SnapRound(b);
            Assert.AreEqual(5, a);

            a = 9;
            a.SnapRound(b);
            Assert.AreEqual(10, a);

            a = -1;
            a.SnapRound(b);
            Assert.AreEqual(0, a);

            a = -2;
            a.SnapRound(b);
            Assert.AreEqual(0, a);

            a = -3;
            a.SnapRound(b);
            Assert.AreEqual(-5, a);

            a = -5;
            a.SnapRound(b);
            Assert.AreEqual(-5, a);

            a = -6;
            a.SnapRound(b);
            Assert.AreEqual(-5, a);

            a = -9;
            a.SnapRound(b);
            Assert.AreEqual(-10, a);
        }

        [TestMethod()]
        public void SnapCeilTest()
        {
            int a = 6;
            int b = 5;
            a.SnapCeil(b);
            Assert.AreEqual(10, a);

            a = 1;
            a.SnapCeil(b);
            Assert.AreEqual(5, a);

            a = -1;
            a.SnapCeil(b);
            Assert.AreEqual(0, a);

            a = -6;
            a.SnapCeil(b);
            Assert.AreEqual(-5, a);

            a = 9;
            a.SnapFloor(b);
            Assert.AreEqual(5, a);

            a = 1;
            a.SnapFloor(b);
            Assert.AreEqual(0, a);

            a = -1;
            a.SnapFloor(b);
            Assert.AreEqual(-5, a);

            a = -6;
            a.SnapFloor(b);
            Assert.AreEqual(-10, a);

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