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
            new TestEvent().Test();
            TestThreadSwitcher();
            Console.ReadLine();
        }

        private static void TestThreadSwitcher()
        {
            Thread threada = new Thread(() =>
            {
                while (true)
                {
                    Console.WriteLine($"DefaultThreadSwitcher.Tick----------CurrentThreadID:{Thread.CurrentThread.ManagedThreadId} -- {DateTimeOffset.Now.Millisecond}");
                    DefaultThreadSwitcher.Tick();
                    Thread.Sleep(1);
                }
            });

            threada.Start();
            Thread threadb = new Thread(() =>
            {
                Console.WriteLine($"Press F1 Test ThreadSwitcher.Switch.        CurrentThreadID:{Thread.CurrentThread.ManagedThreadId} -- {DateTimeOffset.Now.Millisecond}");
                Console.WriteLine($"Press F2 Test ThreadSwitcher.Switch3MaxWait. CurrentThreadID:{Thread.CurrentThread.ManagedThreadId} -- {DateTimeOffset.Now.Millisecond}");
                while (true)
                {
                    var key = Console.ReadKey();

                    if (key.Key == ConsoleKey.F1)
                    {
                        TestSwitch();
                    }
                    else if (key.Key == ConsoleKey.F2)
                    {
                        TestSwitch3();
                    }
                }
            });

            threadb.Start();
            Thread.Sleep(1000 * 60);
        }

        static int TestCount = 1;
        private static async void TestSwitch()
        {
            var id = TestCount++;
            Console.WriteLine($"    切换线程Switch测试[{id}]-Pre.       CurrentThreadID:{Thread.CurrentThread.ManagedThreadId} -- {DateTimeOffset.Now.Millisecond}");
            await DefaultThreadSwitcher.Switch();
            Console.WriteLine($"        切换线程Switch测试[{id}]-Post.  CurrentThreadID:{Thread.CurrentThread.ManagedThreadId} -- {DateTimeOffset.Now.Millisecond}");
        }

        private static async void TestSwitch3()
        {
            var id = TestCount++;
            Console.WriteLine($"    切换线程Switch3MaxWait测试[{id}]-Pre.       CurrentThreadID:{Thread.CurrentThread.ManagedThreadId} -- {DateTimeOffset.Now.Millisecond}");
            await DefaultThreadSwitcher.Switch3MaxWait();
            Console.WriteLine($"        切换线程Switch3MaxWait测试[{id}]-Post.  CurrentThreadID:{Thread.CurrentThread.ManagedThreadId} -- {DateTimeOffset.Now.Millisecond}");
        }
    }
}
