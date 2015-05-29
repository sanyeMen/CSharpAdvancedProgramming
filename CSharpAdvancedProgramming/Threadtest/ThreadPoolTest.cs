using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CSharpAdvancedProgramming.Threadtest
{
    public class ThreadPoolTest
    {
        public void Test()
        {
            PoolTest1();
        }


        private void PoolTest1()
        {
            int workThread;
            int complateThread;
            ThreadPool.GetMaxThreads(out workThread, out complateThread);

            for (int i = 0; i < 5; i++)
            {
                ThreadPool.QueueUserWorkItem((obj) =>
                {
                    for (int j = 0; j < 10; j++)
                    {
                        Console.WriteLine(Thread.CurrentThread.ManagedThreadId);
                        Thread.Sleep(500);
                    }
                });   
            }
        }
    }
}
