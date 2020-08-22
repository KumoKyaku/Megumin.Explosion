using System;
using System.Collections.Generic;
using System.Text;

namespace Megumin
{
    /// <summary>
    /// 可采样的
    /// </summary>
    public interface ISamplable
    {
        void Sample();
    }

    /// <summary>
    /// 可采样的
    /// </summary>
    public interface ISamplable<out T>
    {
        T Sample();
    }

    /// <summary>
    /// 可估值的
    /// </summary>
    public interface IEvaluable
    {
        /// <summary>
        /// 求解
        /// </summary>
        void Evaluate();
    }

    /// <summary>
    /// 可计算的/可求值的
    /// </summary>
    public interface ICalculable
    {
        void Calculate();
    }

    /// <summary>
    /// 可计算的/可求值的
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface ICalculable<out T>
    {
        T Calculate(bool force = false);
    }
}
