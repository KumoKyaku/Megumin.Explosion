using System;
using System.Collections.Generic;
using System.Text;

namespace Megumin
{
    public interface ICalculate
    {
        void Calculate();
    }

    public interface ICalculate<out T>
    {
        T Calculate(bool force = false);
    }
}
