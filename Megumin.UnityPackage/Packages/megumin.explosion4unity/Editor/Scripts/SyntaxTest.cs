using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Megumin.Syntax
{
    public interface ITestInterface<T>
    {
        public bool Fun(T v)
        {
            return default(bool);
        }
    }

    public interface ITestInterface2 : ITestInterface<int>
    {

    }

    public class SyntaxTest
    {
        /// <summary>
        /// 默认接口实现与泛型
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="t"></param>
        [UnityEngine.Scripting.Preserve]
        public void Test<T>(T t)
            where T : ITestInterface2
        {
            t.Fun(1);
        }
    }
}

