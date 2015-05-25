using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpAdvancedProgramming.Linq
{
    /// <summary>
    /// String类的扩展方法
    /// </summary>
    public static class StringExtension
    {
        /// <summary>
        /// 参数this string 
        /// 第一个参数带this
        /// 表示扩展的是String
        /// </summary>
        /// <param name="s"></param>
        public static void Foo(this string s)
        {
            Console.WriteLine(string.Format("Invoke Extension Method Foo param is {0}", s));
        }

    }
}
