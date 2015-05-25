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
            ParallelLinq();
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
        /// <summary>
        /// linq查询后，进行迭代（调用了.ToList()方法，此时产生了新的对象）
        /// 数据源插入新数据
        /// 再次迭代
        /// </summary>
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

        /// <summary>
        /// linq语法和扩展查询
        /// </summary>
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

        /// <summary>
        /// 使用索引筛选
        /// </summary>
        private void SearchTest4()
        {
            var racers = Formula1.GetChampions().Where((r, index) => r.LastName.StartsWith("A") && index % 2 != 0);

            foreach (var r in racers)
            {
                Console.WriteLine("{0:A}", r);
            }
        }

        /// <summary>
        /// 使用类型筛选
        /// </summary>
        private void SearchTest5()
        {
            var data = new object[]{ "obne", 2, 3, "four", "five", 6};

            var query = data.OfType<string>();

            foreach (var s in query)
            {
                Console.WriteLine(s);
            }
        }

        /// <summary>
        /// 使用复合的from子句
        /// </summary>
        private void SearchTest6()
        {
            /*此时查询到的是r.FirstName + " " + r.LastName字符串集合*/
            var query = from r in Formula1.GetChampions() from c in r.Cars where c == "Ferrari" orderby r.LastName select r.FirstName + " " + r.LastName;
            foreach (string s in query)
            {
                Console.WriteLine(s);
            }

            Console.WriteLine("使用扩展方法重写：");

            var extensionQuery = Formula1.GetChampions().SelectMany(r => r.Cars, (r, c) => new { Racer = r, Car = c }).Where(r => r.Car == "Ferrari").OrderByDescending(r => r.Racer.LastName).Select(r => r.Racer.FirstName + " " + r.Racer.LastName);
            foreach (string s in extensionQuery)
            {
                Console.WriteLine(s);
            }
        }

        /// <summary>
        /// 分组
        /// </summary>
        private void SearchTest7()
        {
            /*查询结果返回一个新对象集合*/
            var countries = from r in Formula1.GetChampions() group r by r.Country into g orderby g.Count() descending, g.Key ascending where g.Count() >= 2 select new { Country = g.Key, Count = g.Count() };
            foreach (var item in countries)
            {
                Console.WriteLine("{0, -10} {1}", item.Country, item.Count);
            }

            Console.WriteLine("使用扩展方法重写查询：");

            var extensionQuery = Formula1.GetChampions().GroupBy(r => r.Country).OrderByDescending(g => g.Count()).ThenBy(g => g.Key).Where(g => g.Count() >= 2).Select(g => new { Country = g.Key, Count = g.Count() });
            foreach (var item in extensionQuery)
            {
                Console.WriteLine("{0, -10} {1}", item.Country, item.Count);
            }
        }

        /// <summary>
        /// 对嵌套的对象进行分组
        /// </summary>
        private void SearchTest8()
        {
            var countries = from r in Formula1.GetChampions() group r by r.Country into g orderby g.Count() descending, g.Key ascending where g.Count() >= 2 select new { Country = g.Key, Count = g.Count(), Racers = from r1 in g orderby r1.LastName select r1.FirstName + " " + r1.LastName };
            foreach (var item in countries)
            {
                Console.WriteLine("{0, -10} {1}", item.Country, item.Count);
                foreach (var name in item.Racers)
                {
                    Console.WriteLine(name);
                }
            }
        }

        /// <summary>
        /// 集合操作--Intersect取交集
        /// </summary>
        private void SearchTest9()
        {
            Func<string, IEnumerable<Racer>> racersByCar = car => from r in Formula1.GetChampions() from c in r.Cars where c == car orderby r.LastName select r;

            foreach(var racer in racersByCar("Ferrari").Intersect(racersByCar("McLaren")))
            {
                Console.WriteLine(racer);
            }
        }

        /// <summary>
        /// 合并
        /// </summary>
        private void SearchTest10()
        {
            var racerNames = from r in Formula1.GetChampions() where r.Country == "Italy" orderby r.Wins descending select new { Name = r.FirstName + " " + r.LastName };

            var racerNamesAndStarts = from r in Formula1.GetChampions() where r.Country == "Italy" orderby r.Wins descending select new { LastName = r.LastName, Starts = r.Starts };

            var racers = racerNames.Zip(racerNamesAndStarts, (first, second) => first.Name + ", starts: " + second.Starts);

            foreach (var r in racers)
            {
                Console.WriteLine(r);
            }
        }

        /// <summary>
        /// 分区，分页
        /// </summary>
        private void SearchTest11()
        {
            int pageSize = 5;
            int numberPages = (int)Math.Ceiling(Formula1.GetChampions().Count() / (double) pageSize);

            for (int page = 0; page < numberPages; page++)
            {
                Console.WriteLine("Page {0}", page);

                var racers = (from r in Formula1.GetChampions() orderby r.LastName select r.FirstName + " " + r.LastName).Skip(page * pageSize).Take(pageSize);

                foreach (var name in racers)
                {
                    Console.WriteLine(name);
                }

                Console.WriteLine();
            }
        }

        /// <summary>
        /// 转换
        /// </summary>
        private void SearchTest12()
        {
            var racers = (from r in Formula1.GetChampions() from c in r.Cars select new { Car = c, Racer = r }).ToLookup(cr => cr.Car, cr => cr.Racer);
            if (racers.Contains("Williams"))
            {
                foreach (var williamsRacer in racers["Williams"])
                {
                    Console.WriteLine(williamsRacer);
                }
            }
        }

        /// <summary>
        /// 转换
        /// </summary>
        private void SearchTest13()
        {
            var list = new System.Collections.ArrayList(Formula1.GetChampions() as System.Collections.ICollection);

            var query = from r in list.Cast<Racer>() where r.Country == "USA" orderby r.Wins descending select r;

            foreach (var racer in query)
            {
                Console.WriteLine("{0:A}", racer);
            }
        }

        /// <summary>
        /// 并行Linq
        /// </summary>
        private void ParallelLinq()
        {
            const int arraySize = 100000000;
            var data = new int[arraySize];

            var r = new Random();

            for (int i = 0; i < arraySize; i++)
            {
                data[i] = r.Next(40);
            }

            var sum = (from x in data.AsParallel() where x < 20 select x).Sum();
        }
    }
}
