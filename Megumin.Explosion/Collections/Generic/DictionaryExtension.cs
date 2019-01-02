using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace System.Collections.Generic
{
    /// <summary>
    /// 字典扩展
    /// </summary>
    public static class DictionaryExtension_5191D922C5B740EBB5B4C72E5DA5C11C
    {
        public static void RemoveAll<K, V>(this IDictionary<K, V> source, Func<KeyValuePair<K, V>, bool> predicate)
        {
            if (predicate == null || source == null)
            {
                return;
            }

            unsafe
            {
                var rl = stackalloc IntPtr[source.Count];
                int index = 0;
                lock (source)
                {
                    try
                    {
                        foreach (var item in source)
                        {
                            if (predicate(item))
                            {
                                rl[index] = (IntPtr)GCHandle.Alloc(item.Key);
                            }
                            else
                            {
                                rl[index] = IntPtr.Zero;
                            }
                            index++;
                        }

                        for (int i = 0; i < index; i++)
                        {
                            IntPtr intPtr = rl[i];
                            if (intPtr != IntPtr.Zero)
                            {
                                GCHandle handle = (GCHandle)intPtr;
                                source.Remove((K)handle.Target);
                            }
                        }
                    }
                    finally
                    {
                        for (int i = 0; i < index; i++)
                        {
                            IntPtr intPtr = rl[i];
                            if (intPtr != IntPtr.Zero)
                            {
                                GCHandle handle = (GCHandle)intPtr;
                                if (handle.IsAllocated)
                                {
                                    handle.Free();
                                }
                            }
                        }
                    }
                    
                }
            }
        }

        public static void RemoveAll<V>(this IDictionary<int, V> source, Func<KeyValuePair<int, V>, bool> predicate)
        {
            if (predicate == null || source == null)
            {
                return;
            }

            unsafe
            {
                var rList = stackalloc int[source.Count];
                int index = 0;

                lock (source)
                {
                    foreach (var item in source)
                    {
                        if (predicate(item))
                        {
                            rList[index] = item.Key;
                            index++;
                        }
                    }

                    for (int i = 0; i < index; i++)
                    {
                        source.Remove(rList[i]);
                    }
                }
            }
        }

        public static void RemoveAll<V>(this IDictionary<long, V> source, Func<KeyValuePair<long, V>, bool> predicate)
        {
            if (predicate == null || source == null)
            {
                return;
            }

            unsafe
            {
                var rList = stackalloc long[source.Count];
                int index = 0;

                lock (source)
                {
                    foreach (var item in source)
                    {
                        if (predicate(item))
                        {
                            rList[index] = item.Key;
                            index++;
                        }
                    }

                    for (int i = 0; i < index; i++)
                    {
                        source.Remove(rList[i]);
                    }
                }
            }
        }
    }
}
