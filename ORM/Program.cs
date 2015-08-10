using System;
using System.Data;
using System.Data.SqlClient;

namespace ORM
{
    class Program
    {
        private static void SelectCustomers()
        {
            using (var sqlConnection = new SqlConnection(""))
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
                        Console.WriteLine(reader.GetGuid(0));
                    }
                }


                sqlConnection.Close();
            }

            context.Products;
            
        }

        static void Main(string[] args)
        {
            SelectCustomers();
        }
    }
}
