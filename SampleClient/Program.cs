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
                var firstRecord = new Customer
                {
                    Id = Guid.NewGuid(),
                    FirstName = "temp 3",
                    LastName = "temp 3"
                };

                var sql = context.Customers.Add(firstRecord);
                var secondRecord = new Customer
                {
                    Id = Guid.NewGuid(),
                    FirstName = "temp 4",
                    LastName = "temp 4"
                };

                var secondSql = context.Customers.Add(secondRecord);
                Console.WriteLine(sql);
                Console.WriteLine(secondSql);
                context.SaveChanges();

                var thirdRecord = new Customer
                {
                    Id = Guid.NewGuid(),
                    FirstName = "temp5",
                    LastName = "temp5"
                };
                var thirdSql = context.Customers.Add(thirdRecord);
                Console.WriteLine(thirdSql);
                context.SaveChanges();
            }
        }

        static void Main(string[] args)
        {
            ExecuteQueryAndDisplayResult();
            // ExecuteWhereQueryWithConditionsAndDisplayResult();
            // ExecuteSelectWhereQueriesAndDisplayResult();
            // ExecuteInsertQueryAndDisplayResult();
            
            Console.ReadLine();
        }
    }
}
