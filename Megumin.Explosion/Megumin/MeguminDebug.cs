using System;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace Megumin
{
    public static class MeguminDebug
    {
        public static bool Enable { get; set; } = true;
        public static bool EnableDefaultConsole { get; set; } = true;

        public static bool HookUnity()
        {
            EnableDefaultConsole = false;

#if NETSTANDARD1_6
            throw new NotSupportedException();
#elif NETSTANDARD2_0

            Assembly assembly = AppDomain.CurrentDomain.GetAssemblies().FirstOrDefault(ass => ass.GetName().Name == "UnityEngine.CoreModule");
            if (assembly == null)
            {
                return false;
            }

            var type = assembly.GetType("UnityEngine.Debug");

            if (type == null)
            {
                return false;
            }

            Action<object> CreateDelegate(string name)
            {
                MethodInfo method = type.GetMethod(name,
                BindingFlags.Static | BindingFlags.Public,
                null,
                CallingConventions.Any,
                new Type[] { typeof(object) },
                null);

                return (Action<object>)Delegate.CreateDelegate(typeof(Action<object>), method);
            }

            UnityLog = CreateDelegate("Log");
            UnityLogError = CreateDelegate("LogError");
            UnityLogWarning = CreateDelegate("LogWarning");

            return true;
#endif
        }
        public static void UnHookUnity()
        {
            UnityLog = null;
            UnityLogWarning = null;
            UnityLogError = null;
        }


        static Action<object> UnityLog;
        static Action<object> UnityLogError;
        static Action<object> UnityLogWarning;

        private static ILogger logger;
        static readonly Consolelog consolelog = new Consolelog();

        public static ILogger Logger
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                if (logger == null)
                {
                    if (EnableDefaultConsole)
                    {
                        return consolelog;
                    }
                }
                return logger;
            }
            set => logger = value;
        }

        public static void Log(object message, string moduleName = null)
        {
            if (Enable)
            {
                Logger?.Log(message, moduleName);
                UnityLog?.Invoke(message);
            }
        }

        public static void LogError(object message, string moduleName = null)
        {
            if (Enable)
            {
                Logger?.LogError(message, moduleName);
                UnityLogError?.Invoke(message);
            }
        }

        public static void LogWarning(object message, string moduleName = null)
        {
            if (Enable)
            {
                Logger?.LogWarning(message, moduleName);
                UnityLogWarning?.Invoke(message);
            }
        }

        class Consolelog : ILogger
        {
            public void Log(object message, string moduleName)
            {
                Console.WriteLine(message.ToString());
            }

            public void LogError(object message, string moduleName)
            {
                Console.WriteLine(message.ToString());
            }

            public void LogWarning(object message, string moduleName)
            {
                Console.WriteLine(message.ToString());
            }
        }
    }

    public interface ILogger
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        void Log(object message, string moduleName = null);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        void LogError(object message, string moduleName = null);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        void LogWarning(object message, string moduleName = null);
    }
}
