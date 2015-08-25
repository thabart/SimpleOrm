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

        private static void ExecuteSelectQueryWithConditionAndDisplayResult()
        {
            using (var context = new CustomDbContext())
            {
                var result = context.Customers.Where(c => c.FirstName == "Deviiittt").ToString();
                Console.WriteLine(string.Empty);
            }
        }

        static void Main(string[] args)
        {
            // ExecuteQueryAndDisplayResult();
            ExecuteSelectQueryWithConditionAndDisplayResult();

            Console.ReadLine();
        }
    }
}
