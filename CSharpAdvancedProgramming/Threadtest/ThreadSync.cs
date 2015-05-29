using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CSharpAdvancedProgramming.Threadtest
{
    public class ThreadSync
    {
        public void Test()
        {
            BackGroundWorkTest();
        }

        private void SemaphoreTest1()
        {
            int threadCount = 6;
            int semaphoreCount = 4;

            Semaphore semaphore = new Semaphore(semaphoreCount, semaphoreCount);

            Thread[] threads = new Thread[threadCount];
            for (int i = 0; i < threadCount; i++)
            {
                threads[i] = new Thread(ThreadMain);
                threads[i].Start(semaphore);
            }

            for (int i = 0; i < threadCount; i++)
            {
                threads[i].Join();
            }

            Console.WriteLine("all threads finished");
        }

        private void ThreadMain(object o)
        {
            Semaphore semaphore = o as Semaphore;
            Trace.Assert(semaphore != null, "o must be a Semaphore type");

            bool isCompleted = false;
            while (!isCompleted)
            {
                if (semaphore.WaitOne(600))
                {
                    try
                    {
                        Console.WriteLine("Thread {0} locks the semaphore", Thread.CurrentThread.ManagedThreadId);
                        Thread.Sleep(2000);
                    }
                    finally
                    {
                        semaphore.Release();
                        Console.WriteLine("Thread {0} releases the semaphore", Thread.CurrentThread.ManagedThreadId);
                        isCompleted = true;
                    }
                }
                else
                {
                    Console.WriteLine("Timeout for thread {0}, wait again", Thread.CurrentThread.ManagedThreadId);
                }
            }
        }
        
        private void EventTest1()
        {
            int taskCount = 4;
            ManualResetEventSlim[] mEvents = new ManualResetEventSlim[taskCount];
            WaitHandle[] waitHandles = new WaitHandle[taskCount];
            Caculator[] calcs = new Caculator[taskCount];

            TaskFactory factory = new TaskFactory();
            for (int i = 0; i < taskCount; i++)
            {
                mEvents[i] = new ManualResetEventSlim(false);
                waitHandles[i] = mEvents[i].WaitHandle;
                calcs[i] = new Caculator(mEvents[0]);
                factory.StartNew(calcs[i].Caculation, Tuple.Create(i + 1, i + 3));
            }

            for (int i = 0; i < taskCount; i++)
            {
                int index = WaitHandle.WaitAny(waitHandles);
                if (index == WaitHandle.WaitTimeout)
                {
                    Console.WriteLine("Timeout!");
                }
                else
                {
                    mEvents[index].Reset();
                    Console.WriteLine("finished task for {0}, result: {1}", index, calcs[index].Result);
                }
            }
        }

        private void BarrierTest1()
        {
            int numberTasks = 2;
            int partitionSize = 1000000;

            List<string> data = new List<string>(FillData(partitionSize * numberTasks));

            Barrier barrier = new Barrier(numberTasks + 1);
            TaskFactory taskFactory = new TaskFactory();
            Task<int[]>[] tasks = new Task<int[]>[numberTasks];
            for(int i=0; i<numberTasks; i++)
            {
                tasks[i] = taskFactory.StartNew<int[]>(CalculationInTask, Tuple.Create(i, partitionSize, barrier, data));
            }

            barrier.SignalAndWait();

            var resultCollection = tasks[0].Result.Zip(tasks[1].Result, (c1, c2) =>
            {
                return c1 + c2;
            });

            char ch = 'a';
            int sum = 0;
            foreach (var x in resultCollection)
            {
                Console.WriteLine("{0}, count: {1}", ch++, x);
                sum += x;
            }

            Console.WriteLine("BarrierTest1 finished {0}", sum);
            Console.WriteLine("remaining {0}", barrier.ParticipantsRemaining);
        }

        private int[] CalculationInTask(object p)
        {
            var p1 = p as Tuple<int, int, Barrier, List<string>>;
            Barrier barrier = p1.Item3;
            List<string> data = p1.Item4;

            int start = p1.Item1 * p1.Item2;
            int end = start + p1.Item2;

            Console.WriteLine("Task {0}: partition from {1} to {2}", Task.CurrentId, start, end);

            int[] charCount = new int[26];
            for (int j = start; j < end; j++)
            {
                char c = data[j][0];
                charCount[c - 97]++;
            }
            Console.WriteLine("Calculation completed from task {0}. {1} times a, {2} times z", Task.CurrentId, charCount[0], charCount[25]);
            barrier.RemoveParticipant();
            Console.WriteLine("Task {0} removed from barrier, remaining participants {1}", Task.CurrentId, barrier.ParticipantsRemaining);

            return charCount;
        }

        private IEnumerable<string> FillData(int size)
        {
            List<string> data = new List<string>(size);
            Random r = new Random();
            for (int i = 0; i < size; i++)
            {
                data.Add(GetString(r));
            }
            return data;
        }

        private string GetString(Random r)
        {
            StringBuilder sb = new StringBuilder(6);
            for (int i = 0; i < 6; i++)
            {
                sb.Append((char)(r.Next(26) + 97));
            }
            return sb.ToString();
        }

        private void ReadWriterLockTest1()
        {
            TaskFactory taskFactory = new TaskFactory(TaskCreationOptions.LongRunning, TaskContinuationOptions.None);

            Task[] tasks = new Task[6];

            tasks[0] = taskFactory.StartNew(WriterMethod, 1);
            tasks[1] = taskFactory.StartNew(ReaderMethod, 1);
            tasks[2] = taskFactory.StartNew(ReaderMethod, 2);
            tasks[3] = taskFactory.StartNew(WriterMethod, 2);
            tasks[4] = taskFactory.StartNew(ReaderMethod, 3);
            tasks[5] = taskFactory.StartNew(ReaderMethod, 4);

            for (int i = 0; i < 6; i++)
            {
                tasks[i].Wait();
            }
        }

        private List<int> items = new List<int>() { 0, 1, 2, 3, 4, 5 };
        private static ReaderWriterLockSlim rwl = new ReaderWriterLockSlim(LockRecursionPolicy.SupportsRecursion);

        private void ReaderMethod(object reader)
        {
            try
            {
                rwl.EnterReadLock();
                for (int i = 0; i < items.Count; i++)
                {
                    Console.WriteLine("reader {0}, loop: {1}, item: {2}", reader, i, items[i]);
                    Thread.Sleep(40);
                }
            }
            finally
            {
                rwl.ExitReadLock();
            }
        }

        private void WriterMethod(object writer)
        {
            try
            {
                while (!rwl.TryEnterWriteLock(50))
                {
                    Console.WriteLine("Writer {0} waiting for the write lock", writer);
                    Console.WriteLine("current reader count: {0}", rwl.CurrentReadCount);
                }
                Console.WriteLine("Writer {0} acquired the lock", writer);

                for (int i = 0; i < items.Count; i++)
                {
                    items[i]++;
                    Thread.Sleep(50);
                }
                Console.WriteLine("Writer {0} finished", writer);
            }
            finally
            {
                rwl.ExitWriteLock();
            }
        }

        private void BackGroundWorkTest()
        {
            BackgroundWorker bgw = new BackgroundWorker();
            bgw.WorkerSupportsCancellation = true;

            bgw.DoWork += bgw_DoWork;
            bgw.RunWorkerCompleted += bgw_RunWorkerCompleted;
            bgw.RunWorkerAsync(5);
            bgw.CancelAsync();
        }

        void bgw_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            Console.WriteLine("complete");
        }

        void bgw_DoWork(object sender, DoWorkEventArgs e)
        {
            BackgroundWorker bgw = sender as BackgroundWorker;
            if (bgw != null)
            {
                bgw.ReportProgress(10);

                if (bgw.CancellationPending)
                {
                    Console.WriteLine("cancel");
                    return;
                }
            }

            Thread.Sleep(1000);
            Console.WriteLine(e.Argument);
        }
    }

    public class Caculator
    {
        private ManualResetEventSlim mEvent;

        public int Result { get; private set; }

        public Caculator(ManualResetEventSlim ev)
        {
            this.mEvent = ev;
        }

        public void Caculation(object obj)
        {
            Tuple<int, int> data = (Tuple<int, int>)obj;
            Console.WriteLine("Task {0} starts caculation", Task.CurrentId);
            Thread.Sleep(new Random().Next(3000));

            Result = data.Item1 + data.Item2;

            Console.WriteLine("Task {0} is ready", Task.CurrentId);

            mEvent.Set();
        }
    }
}
