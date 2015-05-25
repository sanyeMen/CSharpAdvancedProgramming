using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpAdvancedProgramming.Linq
{
    public class LinqTest
    {

        public void Test()
        {
            SearchTest3();
        }

        private void LingQuery()
        {
            var query = from r in Formula1.GetChampions() where r.Country == "Italy" orderby r.Wins descending select r;

            foreach(Racer r in query)
            {
                Console.WriteLine("{0:A}", r);
            }
        }

        /// <summary>
        /// linq查询后，进行迭代
        /// 数据源插入新数据
        /// 再次迭代
        /// </summary>
        private void SearchTest1()
        {
            var names = new List<string> { "Nino", "Alberto", "Juan", "Mike", "Phill"};

            var query = from n in names where n.StartsWith("J") orderby n select n;

            Console.WriteLine("第一次迭代");
            foreach (var q in query)
            {
                Console.WriteLine(q);
            }

            Console.WriteLine();

            names.Add("Jone");
            names.Add("Jim");
            names.Add("Jack");
            names.Add("Denny");

            Console.WriteLine("第二次迭代");

            foreach (var q in query)
            {
                Console.WriteLine(q);
            }
        }

        private void SearchTest2()
        {
            var names = new List<string> { "Nino", "Alberto", "Juan", "Mike", "Phill" };

            var query = (from n in names where n.StartsWith("J") orderby n select n).ToList();

            Console.WriteLine("第一次迭代");
            foreach (var q in query)
            {
                Console.WriteLine(q);
            }

            Console.WriteLine();

            names.Add("Jone");
            names.Add("Jim");
            names.Add("Jack");
            names.Add("Denny");

            Console.WriteLine("第二次迭代");

            foreach (var q in query)
            {
                Console.WriteLine(q);
            }
        }

        private void SearchTest3()
        { 
            var query = from r in Formula1.GetChampions() where r.Wins > 1 && (r.Country == "UK" || r.Country == "USA") orderby r.LastName descending select r;
            foreach (var r in query)
            {
                Console.WriteLine("{0:A}", r);
            }
            //使用扩展方法重写查询

            Console.WriteLine("使用扩展查询结果：");

            var extensionQuery = Formula1.GetChampions().Where((r) => r.Wins > 1 && (r.Country=="UK" || r.Country == "USA")).OrderByDescending((r)=>r.LastName).Select(r => r);
            foreach (var r in extensionQuery)
            {
                Console.WriteLine("{0:A}", r);
            }
        }
    }
}
