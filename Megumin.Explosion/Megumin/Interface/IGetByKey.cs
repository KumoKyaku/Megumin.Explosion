using System;
using System.Collections.Generic;
using System.Text;

namespace Megumin
{
    public interface IGetByKey<in Key, in Value>
    {
        bool TryGet<T, V>(T key, out V value) where T : Key where V : Value;
    }
}
