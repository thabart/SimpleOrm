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

        static void Main(string[] args)
        {
            // SelectCustomers();

            using (var context = new CustomDbContext())
            {
                context.Customers.All();
            }

            Console.ReadLine();
        }
    }
}
