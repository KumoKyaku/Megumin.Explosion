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
    public class UnitTest1
    {
        [TestMethod()]
        public void RentAutoReturnRentAutoReturn()
        {
            using (var handle = ListPool<IVisibleable>.RentAutoReturn())
            {
                var list = handle.List;
                list.Add(null);
            }
        }
    }
}