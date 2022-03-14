using System;
using System.Collections.Generic;
using System.Text;

namespace Megumin
{
    public interface IExceptionCallbackable
    {
        void OnException(Exception ex, object sender, object orignal);
    }
}
