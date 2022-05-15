using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;

namespace System.Collections.Generic.Tests
{
    [TestClass()]
    public class IEnumerableExtensionD36DB03F995A41EA836C18482535C530Tests
    {
        [TestMethod()]
        public void GetRemoverTest()
        {
            List<string> list = new List<string>() { "a","b"};

            var remover = list.GetForeachRemover();
            foreach (var item in list)
            {
                if (item == "a")
                {
                    remover.DelayRemove(item);
                }
            }
            remover.RemoveNow();
        }
    }
}