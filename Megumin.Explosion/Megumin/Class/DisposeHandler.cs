using System;
using System.Collections.Generic;
using System.Text;

namespace Megumin
{
    /// <summary>
    /// 观察者模式注册时需要返回IDisposable，这里时将一个回调包装成一个IDisposable
    /// <seealso cref="IObservable{T}.Subscribe(IObserver{T})"/>
    /// </summary>
    /// <remarks>
    /// https://learn.microsoft.com/zh-cn/dotnet/csharp/fundamentals/types/anonymous-types
    /// <para/>
    /// C# 不允许匿名类实现接口，所以必须声明一个类型。
    /// </remarks>
    public class DisposeHandler : IDisposable
    {
        public event Action OnDispose;

        public DisposeHandler(Action dispose = null)
        {
            OnDispose = dispose;
        }

        public void Dispose()
        {
            OnDispose?.Invoke();
        }

        public static implicit operator DisposeHandler(Action action)
        {
            return new DisposeHandler(action);
        }
    }
}




