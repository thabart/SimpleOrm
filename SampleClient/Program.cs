using ORM.LinqToSql;

using System;
using System.Data;

namespace SampleClient
{
    class Program
    {
        private static void ExecuteQueryAndDisplaySqlScript()
        {
            using (var context = new CustomDbContext())
            {
                var result = context.Customers
                    .Select(c => new
                    {
                        c.FirstName
                    })
                    .ToString();

                Console.WriteLine(result);
            }
        }

        private static void ExecuteQueryAndDisplayResult()
        {
            using (var context = new CustomDbContext())
            {
                var result = context.Customers
                    .Select(c => new
                    {
                        c.FirstName
                    }).GetEnumerator();
            }
        }

        static void Main(string[] args)
        {
            ExecuteQueryAndDisplayResult();

            Console.ReadLine();
        }
    }
}
