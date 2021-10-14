using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Megumin.SyntaxTest
{
    public interface ITestInterface<T>
    {
        public bool Fun(T v)
        {
            return default(bool);
        }
    }

    public interface ITestInterface2 : ITestInterface<int> { }

    public struct TestStruct : ITestInterface2 { }

    public class SyntaxTest : MonoBehaviour
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

        public void Test2(TestStruct test)
        {
            //会不会导致装箱?
            //会. https://docs.microsoft.com/zh-cn/dotnet/api/system.reflection.emit.opcodes.constrained?view=net-5.0
            Test(test);
        }

        [EditorButton]
        public void Test3() => Test2(default);
    }
}

