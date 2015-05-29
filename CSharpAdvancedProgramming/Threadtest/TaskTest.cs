using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpAdvancedProgramming.Threadtest
{
    public class TaskTest
    {
        public void Test()
        {
            TaskTest2();
        }

        private void TaskTest1()
        {
            Task task = new Task(new Action(() => {
                Console.WriteLine("1111");
            }), TaskCreationOptions.LongRunning);
            task.Start();
        }

        private void TaskTest2()
        {
            Task<Tuple<int, string>> task = new Task<Tuple<int, string>>(TaskWithResult, Tuple.Create<int, int>(8, 3));
            task.Start();
            Console.WriteLine(task.Result);
            task.Wait();
            Console.WriteLine(task.Result.Item1 + "-----" + task.Result.Item2);
        }

        private Tuple<int, string> TaskWithResult(object obj)
        {
            Tuple<int, int> div = (Tuple<int, int>)obj;

            return Tuple.Create<int, string>(div.Item1 / div.Item2, (div.Item1 / div.Item2).ToString());
        }

    }
}
