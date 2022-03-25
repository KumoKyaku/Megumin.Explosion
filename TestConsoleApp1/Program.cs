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
                    Thread.Sleep(500);
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

                    switch (key.Key)
                    {
                        case ConsoleKey.F1:
                            TestSwitch();
                            break;
                        case ConsoleKey.F2:
                            TestSwitch3();
                            break;
                        case ConsoleKey.F4:
                            TestSwitch2();
                            break;
                    }
                }
            });

            threadb.Start();
            Thread.Sleep(1000 * 60);
        }

        static int TestCount = 1;
        private static async void TestSwitch2()
        {
            var id = TestCount++;
            Console.WriteLine($"    切换线程{nameof(TestSwitch2)}测试[{id}]-Pre.       CurrentThreadID:{Thread.CurrentThread.ManagedThreadId} -- {DateTimeOffset.Now.Millisecond}");
            await DefaultThreadSwitcher.Switch2();
            Console.WriteLine($"        切换线程{nameof(TestSwitch2)}测试[{id}]-Post.  CurrentThreadID:{Thread.CurrentThread.ManagedThreadId} -- {DateTimeOffset.Now.Millisecond}");
        }

        private static async void TestSwitch3()
        {
            var id = TestCount++;
            Console.WriteLine($"    切换线程{nameof(TestSwitch3)}测试[{id}]-Pre.       CurrentThreadID:{Thread.CurrentThread.ManagedThreadId} -- {DateTimeOffset.Now.Millisecond}");
            await DefaultThreadSwitcher.Switch3MaxWait();
            Console.WriteLine($"        切换线程{nameof(TestSwitch3)}测试[{id}]-Post.  CurrentThreadID:{Thread.CurrentThread.ManagedThreadId} -- {DateTimeOffset.Now.Millisecond}");
        }

        private static async void TestSwitch()
        {
            var id = TestCount++;
            Console.WriteLine($"    切换线程{nameof(TestSwitch)}测试[{id}]-Pre.       CurrentThreadID:{Thread.CurrentThread.ManagedThreadId} -- {DateTimeOffset.Now.Millisecond}");
            await DefaultThreadSwitcher.Switch();
            Console.WriteLine($"        切换线程{nameof(TestSwitch)}测试[{id}]-Post.  CurrentThreadID:{Thread.CurrentThread.ManagedThreadId} -- {DateTimeOffset.Now.Millisecond}");
        }
    }
}
