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
            using (var handle = ListPool<IVisibleable>.Shared.Rent(out var _))
            {
                var list = handle.Element;
                list.Add(null);
            }

            using (ListPool<IVisibleable>.Shared.Rent(out var list))
            {

            }

#if NET5_0_OR_GREATER
            using var _ = ListPool<int>.Shared.Rent(out var test);
            using var __ = ListPool<int>.Shared.Rent(out var test2);
#endif

            //CListP<int>.Pool = new PoolCore<List<int>>()
            //{
            //    Name = "CListP"
            //};

            //ConcurrentPool<List<int>>.Pool = new PoolCore<List<int>>()
            //{
            //    Name = "ConcurrentPool INT"
            //};

            //CListP<object>.Pool = new PoolCore<List<object>>()
            //{
            //    Name = "CListPObject"
            //};

            //CListP2<object>.Pool = new PoolCore<object>()
            //{
            //    Name = "CListPObject2"
            //};

            //var a = CListP<int>.Rent();
            //var b = CListP<object>.Rent();
            //var c = CListP2<object>.Rent();
        }
    }
}