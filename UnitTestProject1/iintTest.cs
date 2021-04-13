using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Megumin;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTestProject1
{
    [TestClass]
    public class iintTest
    {
        [TestMethod]
        public void Test()
        {
            int x = 4;
            iint pi = iint.PositiveInfinity;
            iint ni = iint.NegativeInfinity;

            Assert.IsTrue(x + pi == iint.PositiveInfinity);
            Assert.IsTrue(pi + 1 == iint.PositiveInfinity);
            Assert.IsTrue(pi + (-ni) == iint.PositiveInfinity);
            Assert.IsTrue((int)((iint)5) == 5);
        }
    }
}
