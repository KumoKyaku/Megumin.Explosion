﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
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
            AnyTrueControl control = new AnyTrueControl();
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

        [TestMethod()]
        public void MultipleControlEnumRentAutoReturn()
        {
            MultipleValueEnum<object, KeypadSudoku> control = new MultipleValueEnum<object, KeypadSudoku>(new object(), KeypadSudoku.Up);
            var c1 = new object();
            control.Add(c1, KeypadSudoku.Left);
            Assert.AreEqual(KeypadSudoku.Left | KeypadSudoku.Up, control);

            var c2 = new object();
            control.Add(c2, KeypadSudoku.Right);
            Assert.AreEqual(KeypadSudoku.Left | KeypadSudoku.Right | KeypadSudoku.Up, control);
        }
    }
}


