using ORM.Models;
using ORM.Translators;
using ORM.LinqToSql;

using System;
using System.Data;
using System.Data.SqlClient;
using System.Linq.Expressions;

namespace ORM
{
    class Program
    {
        private static void SelectCustomers()
        {            
            using (var sqlConnection = new SqlConnection(@"Server=DESKTOP-1CNU397\SQLEXPRESS; Database=customer;Integrated Security=True;"))
            {
                var command = new SqlCommand();

                command.CommandText = "SELECT * FROM Customers";
                command.CommandType = CommandType.Text;
                command.Connection = sqlConnection;

                sqlConnection.Open();

                var reader = command.ExecuteReader();
                if (reader.HasRows)
                {
                    while(reader.Read())
                    {
                        Console.WriteLine(reader.GetGuid(0) +" "+ reader.GetString(1));
                    }
                }


                sqlConnection.Close();
            }            
        }

        private static void InspectExpressionTree(Expression expression)
        {
            var queryTranslator = new QueryTranslator();
            var info = queryTranslator.Visit(expression);
        }

        private static void ShowSelectRequest()
        {
            var dbProvider = new DBProvider(string.Empty);
            var customers = dbProvider.GetTable<Customer>();
            var result = customers.Select(c => c.FirstName).ToString();

            Console.WriteLine(result);
        }

        static void Main(string[] args)
        {
            ShowSelectRequest();


            /*
            // SelectCustomers();
            using (var context = new CustomDbContext())
            {
                // Returns one specific field.
                var customerFirstNames = context.Customers
                    .Select(c => c.FirstName)
                    .Execute();

                foreach(var firstName in customerFirstNames)
                {
                    Console.WriteLine(firstName);
                }
                
                // Returns anynamous type.
                var customerInformations = context.Customers
                    .Select(c => new
                    {
                        c.FirstName,
                        c.LastName
                    })
                    .Execute();

                foreach(var info in customerInformations)
                {
                    Console.WriteLine(info.FirstName +" "+ info.LastName);
                }
            }
            */

            Console.ReadLine();
        }
    }
}
