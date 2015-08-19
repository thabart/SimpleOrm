using ORM.Models;
using ORM.LinqToSql;

using System;
using System.Data;
using System.Data.SqlClient;

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

        private static void ShowSelectRequestWithAnonymousType()
        {
            var dbProvider = new DBProvider(string.Empty);
            var customers = dbProvider.GetTable<Customer>();
            var result = customers
                .Select(c => new
                {
                    c.FirstName,
                    c.LastName
                })
                .ToString();

            Console.WriteLine(result);
        }

        private static void ShowSelectRequestBasicForm()
        {
            var dbProvider = new DBProvider(string.Empty);
            var customers = dbProvider.GetTable<Customer>();
            var result = customers
                .Select(c => c.FirstName)
                .ToString();

            Console.WriteLine(result);
        }

        private static void ExecuteSelectQuery()
        {
            using (var context = new CustomDbContext())
            {
                var result = context.Customers
                    .Select(c => c.FirstName)
                    .ToString();

                Console.WriteLine(result);
            }
        }

        static void Main(string[] args)
        {
            ShowSelectRequestWithAnonymousType();
            ShowSelectRequestBasicForm();
            ExecuteSelectQuery();

            Console.ReadLine();
        }
    }
}
