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
                        c.FirstName,
                        c.Id
                    });

                foreach(var record in result)
                {
                    Console.WriteLine(record.Id +" "+ record.FirstName);
                }
            }
        }

        static void Main(string[] args)
        {
            ExecuteQueryAndDisplayResult();

            Console.ReadLine();
        }
    }
}
