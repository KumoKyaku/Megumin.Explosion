// See https://aka.ms/new-console-template for more information
using Megumin;
using System;
using System.Threading.Tasks;

namespace MyApp // Note: actual namespace depends on the project name.
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            Thread threada = new Thread(() =>
            {
                while (true)
                {
                    Console.WriteLine($"DefaultThreadSwitcher.Tick----------CurrentThreadID:{Thread.CurrentThread.ManagedThreadId} -- {DateTimeOffset.Now.Millisecond}");
                    DefaultThreadSwitcher.Tick();
                    Thread.Sleep(1000);
                }
            });

            threada.Start();
            Thread threadb = new Thread(() =>
            {
                Console.WriteLine($"Press F1 Test ThreadSwitcher.Switch2.        CurrentThreadID:{Thread.CurrentThread.ManagedThreadId} -- {DateTimeOffset.Now.Millisecond}");
                Console.WriteLine($"Press F2 Test ThreadSwitcher.Switch3MaxWait. CurrentThreadID:{Thread.CurrentThread.ManagedThreadId} -- {DateTimeOffset.Now.Millisecond}");
                while (true)
                {
                    var key = Console.ReadKey();

                    if (key.Key == ConsoleKey.F1)
                    {
                        TestSwitch2();
                    }
                    else if (key.Key == ConsoleKey.F2)
                    {
                        TestSwitch3();
                    }
                }
            });

            threadb.Start();
            Thread.Sleep(1000 * 60);
            Console.ReadLine();
        }

        private static async void TestSwitch2()
        {
            Console.WriteLine($"    切换线程Switch2测试Pre.       CurrentThreadID:{Thread.CurrentThread.ManagedThreadId} -- {DateTimeOffset.Now.Millisecond}");
            await DefaultThreadSwitcher.Switch2();
            Console.WriteLine($"        切换线程Switch2测试Post.  CurrentThreadID:{Thread.CurrentThread.ManagedThreadId} -- {DateTimeOffset.Now.Millisecond}");
        }

        private static async void TestSwitch3()
        {
            Console.WriteLine($"    切换线程Switch3MaxWait测试Pre.       CurrentThreadID:{Thread.CurrentThread.ManagedThreadId} -- {DateTimeOffset.Now.Millisecond}");
            await DefaultThreadSwitcher.Switch3MaxWait();
            Console.WriteLine($"        切换线程Switch3MaxWait测试Post.  CurrentThreadID:{Thread.CurrentThread.ManagedThreadId} -- {DateTimeOffset.Now.Millisecond}");
        }
    }
}
