using ORM.LinqToSql;
using SampleClient.Models;
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

        private static void ExecuteInsertQueryAndDisplayResult()
        {
            using (var context = new CustomDbContext())
            {
                var record = new Customer
                {
                    Id = Guid.NewGuid(),
                    FirstName = "temp",
                    LastName = "temp"
                };

                context.Customers.Add(record);
            }
        }

        static void Main(string[] args)
        {
            // ExecuteQueryAndDisplayResult();
            // ExecuteWhereQueryWithConditionsAndDisplayResult();
            // ExecuteSelectWhereQueriesAndDisplayResult();
            ExecuteInsertQueryAndDisplayResult();
            Console.ReadLine();
        }
    }
}
