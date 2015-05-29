using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CSharpAdvancedProgramming.Threadtest
{
    public class ParallelTest
    {

        public void Test()
        {
            ParallelTest6();
        }


        private void ParallelTest1()
        {
            ParallelLoopResult result = Parallel.For(0, 10, new Action<int>((i) =>
                {
                    Console.WriteLine(i + "----" + "task: " + Task.CurrentId + "----" + "thread: " + System.Threading.Thread.CurrentThread.ManagedThreadId);
                }));
            Console.WriteLine(result.IsCompleted);
        }

        private void ParallelTest2()
        {
            ParallelLoopResult result = Parallel.For(0, 10, new Action<int, ParallelLoopState>((int i, ParallelLoopState pls) =>
            {
                Console.WriteLine(i + "----" + "task: " + Task.CurrentId + "----" + "thread: " + System.Threading.Thread.CurrentThread.ManagedThreadId);
                if (i > 8)
                    pls.Break();
            }));
            Console.WriteLine(result.IsCompleted);
        }


        private void ParallelTest3()
        {
            Parallel.For<string>(0, 20, () =>
            {
                Console.WriteLine("init thread {0}, task {1}", System.Threading.Thread.CurrentThread.ManagedThreadId, Task.CurrentId);

                return string.Format("t{0}", System.Threading.Thread.CurrentThread.ManagedThreadId);
            }, (i, pls, str1) =>
            {
                Console.WriteLine("body i {0} str1 {1} thread {2} task {3}", i, str1, System.Threading.Thread.CurrentThread.ManagedThreadId, Task.CurrentId);
                System.Threading.Thread.Sleep(10);
                return string.Format("i {0}", i);
            }, (str1) => 
            {
                Console.WriteLine("finally {0}", str1);
            });
        }

        private void ParallelTest4()
        {
            int[] intArr = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10};

            Parallel.ForEach<int>(intArr, (i) => {
                Console.WriteLine(i);
            });
        }

        /// <summary>
        /// 取消FOR方法
        /// </summary>
        private void ParallelTest5()
        {
            CancellationTokenSource cts = new CancellationTokenSource();
            cts.Token.Register(() => {
                Console.WriteLine("* * * token canceled");
            });

            new Task(() => 
            {
                Thread.Sleep(500);
                cts.Cancel(false);
            }).Start();

            try
            {
                ParallelLoopResult result = Parallel.For(0, 100, new ParallelOptions() { CancellationToken = cts.Token }, (x) => {
                    Console.WriteLine("loop {0} started", x);
                    int sum = 0;
                    for (int i = 0; i < 100; i++)
                    {
                        Thread.Sleep(2);
                        sum += i;
                    }
                    Console.WriteLine("loop {0} finished", x);
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        /// <summary>
        /// 取消任务
        /// </summary>
        private void ParallelTest6()
        {
            CancellationTokenSource cts = new CancellationTokenSource();
            cts.Token.Register(() =>
            {
                Console.WriteLine("* * * token canceled");
            });

            new Task(() =>
            {
                Thread.Sleep(500);
                cts.Cancel(false);
            }).Start();

            TaskFactory factory = new TaskFactory(cts.Token);
            Task t1 = factory.StartNew(new Action<object>((f) => {
                Console.WriteLine("in task");

                for (int i = 0; i < 20; i++)
                {
                    Thread.Sleep(100);

                    CancellationToken ct = (f as TaskFactory).CancellationToken;
                    if (ct.IsCancellationRequested)
                    {
                        Console.WriteLine("canceling was requested, canceling from within the task");
                        ct.ThrowIfCancellationRequested();
                        break;
                    }
                    Console.WriteLine("in loop");
                }
                Console.WriteLine("task finished without cancellation");
            }), factory, cts.Token);

            try
            {
                t1.Wait();
            }
            catch (Exception ex)
            {
                Console.WriteLine("exception: {0}, {1}", ex.GetType().Name, ex.Message);
                if (ex.InnerException != null)
                {
                    Console.WriteLine("inner exception: {0}, {1}", ex.InnerException.GetType().Name, ex.InnerException.Message);
                }
            }

            Console.WriteLine("status of the task: {0}", t1.Status);
        }

    }
}
