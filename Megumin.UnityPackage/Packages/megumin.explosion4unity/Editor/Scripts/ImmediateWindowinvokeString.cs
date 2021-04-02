#if InvokeString

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.ImmediateWindow.UI;
using UnityEngine;

namespace Megumin
{
    public static class ImmediateWindowinvokeString
    {

        ///// <summary>
        ///// 第二次调用打不开窗口
        ///// </summary>
        //static Action<string> InvokeMethod { get; } = CreateInvokeMethod();
        //static Action<string> CreateInvokeMethod()
        //{
        //    ImmediateWindow.ShowPackageManagerWindow();
        //    dynamic immediate = ImmediateWindow.CurrentWindow;

        //    object console = typeof(ImmediateWindow).GetProperty("Console", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance).GetValue(immediate);

        //    object output = (console.GetType()).GetProperty("ConsoleOutput", System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance).GetValue(console);
        //    var clear = (output.GetType()).GetMethod("ClearLog");
        //    clear.Invoke(output, null);

        //    UnityEngine.UIElements.TextField textField = (console.GetType()).GetProperty("ConsoleInput", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance).GetValue(console) as UnityEngine.UIElements.TextField;

        //    var invoke = (console.GetType()).GetMethod("CodeEvaluate");

        //    Action<string> action = (str) =>
        //    {
        //        textField.value = str;
        //        invoke.Invoke(console, null);
        //    };

        //    return action;
        //}

        //public static void ExcuteString(string cmd)
        //{
        //    InvokeMethod?.Invoke(cmd);
        //}

        static void InvokeString(string cmd)
        {
            ImmediateWindow.ShowPackageManagerWindow();
            dynamic immediate = ImmediateWindow.CurrentWindow;

            object console = typeof(ImmediateWindow).GetProperty("Console", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance).GetValue(immediate);

            object output = (console.GetType()).GetProperty("ConsoleOutput", System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance).GetValue(console);
            var clear = (output.GetType()).GetMethod("ClearLog");
            clear.Invoke(output, null);

            UnityEngine.UIElements.TextField textField = (console.GetType()).GetProperty("ConsoleInput", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance).GetValue(console) as UnityEngine.UIElements.TextField;

            var invoke = (console.GetType()).GetMethod("CodeEvaluate");
            textField.value = cmd;
            invoke.Invoke(console, null);
        }

        private static System.Object cache;


        public static void ImmediateShow(this System.Object @object)
        {
            cache = @object;
            string cmd = "Megumin.ImmediateWindowinvokeString.ShowCache();";
            InvokeString(cmd);
        }

        public static System.Object ShowCache()
        {
            return cache;
        }

        public static string Test()
        {
            return "TEST";
        }
    }
}

#endif  



