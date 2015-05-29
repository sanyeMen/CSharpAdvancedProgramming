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
            ThreadSync tt = new ThreadSync();
            tt.Test();

            //Console.WriteLine("CSharpAdvancedProgramming：" + AppDomain.CurrentDomain.FriendlyName);

            //AppDomain domainA = AppDomain.CreateDomain("AppDomainA");
            //domainA.ExecuteAssembly("AppDomainA.exe");
            //AppDomain.Unload(domainA);

            Console.ReadKey();
        }
    }
}
