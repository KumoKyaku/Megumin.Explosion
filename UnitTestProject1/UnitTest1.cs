using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Megumin;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTestProject1
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestThreshold()
        {
            Threshold<int> threshold = new Threshold<int>(3, 5);
            Assert.AreEqual(true, threshold.Contain(4));
            Assert.AreEqual(false, threshold.Contain(2));
            Assert.AreEqual(false, threshold.Contain(6));

            Assert.AreEqual(true, 6 > threshold);
            Assert.AreEqual(true, 2 < threshold);
        }

        [TestMethod]
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

        [TestMethod]
        public void TestRemoveAllInt()
        {
            Dictionary<int, int> test = new Dictionary<int, int>();
            test.Add(1, 1);
            test.Add(2, 2);
            test.Add(3, 2);
            test.Add(4, 3);

            Func<KeyValuePair<int, int>, bool> predicate = (kv) =>
              {
                  return kv.Value >= 2;
              };

            test.RemoveAll(predicate);
            Assert.AreEqual(false, test.Any(predicate));
        }

        [TestMethod]
        public void TestRemoveAllString()
        {
            Dictionary<string, int> test = new Dictionary<string, int>();
            test.Add("1", 1);
            test.Add("2", 2);
            test.Add("3", 2);
            test.Add("4", 3);

            Func<KeyValuePair<string, int>, bool> predicate = (kv) =>
              {
                  return kv.Value >= 2;
              };

            test.RemoveAll(predicate);
            Assert.AreEqual(false, test.Any(predicate));
        }

        [TestMethod]
        public void TestWaitAsync()
        {
            Wait().Wait();
        }

        private static async Task Wait()
        {
            var c = await Task.Delay(100).WaitAsync(150);
            Assert.AreEqual(true, c);
            var c2 = await Task.Delay(200).WaitAsync(150);
            Assert.AreEqual(false, c2);
        }

        [TestMethod]
        public void TestXYZ()
        {
            XYZ<int> xyz = new XYZ<int>() { X = 1, Y = 2, Z = 3 };
            var (x, y, z) = xyz;
            Assert.AreEqual(1, x);
            Assert.AreEqual(2, y);
            Assert.AreEqual(3, z);
        }
    }
}
