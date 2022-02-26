// See https://aka.ms/new-console-template for more information
using Megumin;

namespace MyApp
{
    internal class TestEvent
    {
        public WeakEvent weakEvent = new WeakEvent();
        private TestClass? test = null;

        internal void Test()
        {
            InitClass();
            weakEvent.OnInvoke += test.WeakEvent_OnInvoke;
            weakEvent.Invoke();
            GC.Collect();

            test = null;
            GC.Collect();
            GC.Collect(2);
            GC.Collect(2, GCCollectionMode.Forced);
            weakEvent.Invoke();

            //weakEvent.OnInvoke -= test.WeakEvent_OnInvoke;
            GC.Collect();
            weakEvent.Invoke();
        }

        private void InitClass()
        {
            test = new TestClass();
        }

        private void WeakEvent_OnInvoke()
        {
            Console.WriteLine($"事件触发");
        }
    }

    internal class TestClass
    {
        public void WeakEvent_OnInvoke()
        {
            Console.WriteLine($"事件触发");
        }
    }
}



