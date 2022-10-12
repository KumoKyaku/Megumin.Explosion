using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace Megumin
{
    public class UnityTraceListener : TraceListener
    {
        public override void Write(string message)
        {
            Debug.Log(message);
        }

        public override void Write(object o, string category)
        {
            if (category.Contains("Warning", StringComparison.OrdinalIgnoreCase))
            {
                Debug.LogWarning(o);
            }
            else
            {
                base.Write(o, category);
            }
        }

        public override void Write(string message, string category)
        {
            if (category.Contains("Warning", StringComparison.OrdinalIgnoreCase))
            {
                Debug.LogWarning(message);
            }
            else
            {
                base.Write(message, category);
            }
        }

        public override void WriteLine(string message)
        {
            Debug.Log(message);
        }

        public override void WriteLine(object o, string category)
        {
            if (category.Contains("Warning", StringComparison.OrdinalIgnoreCase))
            {
                Debug.LogWarning(o);
            }
            else
            {
                base.WriteLine(o, category);
            }
        }

        public override void WriteLine(string message, string category)
        {
            if (category.Contains("Warning", StringComparison.OrdinalIgnoreCase))
            {
                Debug.LogWarning(message);
            }
            else
            {
                base.WriteLine(message, category);
            }
        }

        public override void Fail(string message, string detailMessage)
        {
            Debug.LogError($"{message}\n{detailMessage}");
        }

        public override void Fail(string message)
        {
            Debug.LogError(message);
        }
    }
}


