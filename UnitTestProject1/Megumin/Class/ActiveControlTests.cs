using Microsoft.VisualStudio.TestTools.UnitTesting;
using Megumin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Megumin.Tests
{
    [TestClass()]
    public class ActiveControlTests
    {
        [TestMethod()]
        public void ActiveControlRentAutoReturn()
        {
            ActiveControl control = new ActiveControl(new object(), false);
            Assert.AreEqual(false, control);

            var c1 = new object();
            control.Control(c1, false);
            Assert.AreEqual(false, control);

            var o1 = new object();
            control.Control(o1, true);
            Assert.AreEqual(true, control);

            var o2 = new object();
            control.Control(o2, true);
            Assert.AreEqual(true, control);

            control.Cancel(o1);
            Assert.AreEqual(true, control);

            control.Cancel(o2);
            Assert.AreEqual(false, control);

            control.CancelAll();
            Assert.AreEqual(false, control);
        }
    }
}