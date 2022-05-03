using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Megumin.SyntaxTest
{
    public interface ITestInterface<T>
    {
        public bool Foo(T v)
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
            t.Foo(1);
        }

        public void Test2(TestStruct test)
        {
            //调用方法Foo 会不会导致装箱?
            //根据指令文档猜测: 会不会装箱取决于方法的具体实现位置. 如果方法(Foo) 是在结构体实现的,不会装箱/是接口默认实现的,会装箱.
            //会. https://docs.microsoft.com/zh-cn/dotnet/api/system.reflection.emit.opcodes.constrained?view=net-5.0
            Test(test);
        }

        [Button]
        public void Test() => Test2(default);
    }
}

