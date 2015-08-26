using ORM.LinqToSql;

using System;

namespace SampleClient
{
    class Program
    {
        private static void ExecuteQueryAndDisplayResult()
        {
            using (var context = new CustomDbContext())
            {
                var result = context.Customers
                    .Select(c => new
                    {
                        c.FirstName,
                        c.Id
                    });

                foreach(var record in result)
                {
                    Console.WriteLine(record.Id +" "+ record.FirstName);
                }
            }
        }

        private static void ExecuteWhereQueryWithConditionsAndDisplayResult()
        {
            using (var context = new CustomDbContext())
            {
                var result = context.Customers.Where(c => c.FirstName == "Thierry");
                foreach (var record in result)
                {
                    Console.WriteLine(record.FirstName);
                }

                Console.WriteLine(result.ToString());
            }
        }

        private static void ExecuteSelectWhereQueriesAndDisplayResult()
        {
            using (var context = new CustomDbContext())
            {
                var result = context.Customers.Where(c => c.FirstName == "Thierry" || c.FirstName == "Loki").Select(c => c.FirstName);
                foreach (var record in result)
                {
                    Console.WriteLine(record.FirstName);
                }

                Console.WriteLine(result.ToString());
            }
        }

        static void Main(string[] args)
        {
            // ExecuteQueryAndDisplayResult();
            // ExecuteWhereQueryWithConditionsAndDisplayResult();
            ExecuteSelectWhereQueriesAndDisplayResult();

            Console.ReadLine();
        }
    }
}
