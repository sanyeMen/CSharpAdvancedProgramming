using CSharpAdvancedProgramming.Threadtest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpAdvancedProgramming
{
    class Program
    {
        static void Main(string[] args)
        {
            //ThreadSync tt = new ThreadSync();
            //tt.Test();

            Prints("111", 222, 12.3);
            //Console.WriteLine("CSharpAdvancedProgramming：" + AppDomain.CurrentDomain.FriendlyName);

            //AppDomain domainA = AppDomain.CreateDomain("AppDomainA");
            //domainA.ExecuteAssembly("AppDomainA.exe");
            //AppDomain.Unload(domainA);

            Console.ReadKey();
        }

        /// <summary>
        /// params关键字的用法
        /// </summary>
        /// <param name="param"></param>
        private static void Prints(params object[] param)
        {
            foreach (var obj in param)
            {
                Console.WriteLine(obj);
            }
        }
    }
}
