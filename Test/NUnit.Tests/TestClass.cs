using Megumin;
using NUnit.Framework;
using System;
using System.Buffers;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NUnit.Tests
{
    [TestFixture]
    public class TestClass
    {
        [Test]
        public void TestUnmanagedMemoryManager()
        {
            TestM<byte>();
            TestM<short>();
            TestM<int>();
            TestM<long>();
        }

        void TestM<T>()
        {
            const int length = 100;
            UnmanagedMemoryManager<T> manager = new UnmanagedMemoryManager<T>(length);
            var span = manager.GetSpan();
            Assert.AreEqual(length, manager.Length);
            Assert.AreEqual(length, span.Length);

            dynamic value = default;

            if (typeof(T) == typeof(byte))
            {
                value = byte.MaxValue;
            }
            if (typeof(T) == typeof(short))
            {
                value = short.MaxValue;
            }
            if (typeof(T) == typeof(int))
            {
                value = int.MaxValue;
            }

            if (typeof(T) == typeof(long))
            {
                value = long.MaxValue;
            }

            span[0] = value;
            var span2 = manager.GetSpan();
            Assert.AreEqual(span2[0], span[0]);

            if (manager is IDisposable disposable)
            {
                disposable.Dispose();


                Assert.Catch<ObjectDisposedException>(() =>
                {
                    var span3 = manager.GetSpan();
                });

                Assert.AreEqual(-1, manager.Length);
            }
        }

        [Test]
        public void TestThreshold()
        {
            Threshold<int> threshold = new Threshold<int>(3,5);
            Assert.AreEqual(true, threshold.Contain(4));
            Assert.AreEqual(false, threshold.Contain(2));
            Assert.AreEqual(false, threshold.Contain(6));

            Assert.AreEqual(true, 6 > threshold);
            Assert.AreEqual(true, 2 < threshold);
        }

        [Test]
        public void TestClamp()
        {
            int a = 5;
            a.Clamp(3, 6);
            Assert.AreEqual(5, a);
            a.Clamp(6, 3);
            Assert.AreEqual(5, a);
            a.Clamp(6, 6);
            Assert.AreEqual(6, a);
            a.Clamp(3, 3);
            Assert.AreEqual(3, a);

            a.Clamp(3, 6);
            Assert.AreEqual(3, a);

            a.Clamp(2, 6);
            Assert.AreEqual(3, a);
        }
    }
}
