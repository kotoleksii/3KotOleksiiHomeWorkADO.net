using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace _3KotOleksiiHomeWorkADO.net
{
    class Program
    {
        static string connString;
        static SqlConnection connection;
        static string serverName = "localDbCS";

        static void Main(string[] args)
        {         
            connString = ConfigurationManager
                .ConnectionStrings[serverName]
                .ConnectionString;

            connection = new SqlConnection(connString);

            var work = true;

            do
            {                
                Console.Write("cout"); Console.ForegroundColor = ConsoleColor.Cyan; Console.Write(" - connect to the database\n"); Console.ResetColor();
                Console.Write("endl"); Console.ForegroundColor = ConsoleColor.Cyan; Console.Write(" - disconnect from the database\n"); Console.ResetColor();
                Console.Write("db--query"); Console.ForegroundColor = ConsoleColor.Cyan; Console.Write(" - execute the request\n"); Console.ResetColor();
                Console.Write("db--all"); Console.ForegroundColor = ConsoleColor.Cyan; Console.Write(" - display a list of databases on the server\n"); Console.ResetColor();
                Console.Write("db--proc"); Console.ForegroundColor = ConsoleColor.Cyan; Console.Write(" - run a stored procedur\n"); Console.ResetColor();
                Console.Write("exit"); Console.ForegroundColor = ConsoleColor.Cyan; Console.Write(" - exit the program\n"); Console.ResetColor();

                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.Write("\nSelect a task: ");
                Console.ResetColor();

                var choice = "";
                choice = Console.ReadLine();
                Console.Clear();

                switch (choice)
                {
                    case "cout":                       
                        dbConnection();                     
                        break;
                    case "endl":
                        dbDisconnection();
                        break;
                    case "db--query":
                        dbQuery();
                        break;
                    case "db--all":
                        dbList();
                        break;
                    case "db--proc":                                     
                        dbProc();
                        break;
                    case "exit":                                              
                        work = false;
                        break;
                    default:
                       
                        break;
                }
            } while (work);       
        }

        static void dbConnection()
        {
            Console.Clear();

            Console.ForegroundColor = ConsoleColor.Cyan; Console.Write("Connecting to SQL Server");
            Console.ResetColor(); Console.Write($" {serverName}"); 
            Console.ForegroundColor = ConsoleColor.Cyan; Console.Write("... ");
            Console.ResetColor();

            try
            {
                connection.Open();

                Console.ForegroundColor = ConsoleColor.Cyan; 
                Console.WriteLine("Done.");
                Console.ResetColor();
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine($"\n\n{ex.Message}\nPress any key to continue . . ."); 
                Console.ResetColor();
            }

            //Console.Write("\nAll done. Press any key to finish...");
            Console.ReadKey(true);

            Console.Clear();
        }

        static void dbDisconnection()
        {
            Console.Clear();

            Console.Write($"Disconnecting SQL Server {serverName}... ");
            try
            {
                if (connection != null)
                    connection.Close();

                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine("Done.");
                Console.ResetColor();
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine($"Error: {ex.Message}");
                Console.ResetColor();
            }

            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.Write("\nAll done. Press any key to finish...");
            Console.ResetColor();

            Console.ReadKey(true);

            Console.Clear();
        }


        static void dbQuery()
        {
            Console.Clear();

            try
            {
                var work = true;

                do
                {
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    Console.Write("Enter '"); Console.ResetColor();
                    Console.Write("sql"); Console.ForegroundColor = ConsoleColor.Cyan;
                    Console.Write("' to create a query or '"); Console.ResetColor();
                    Console.Write("endl"); Console.ForegroundColor = ConsoleColor.Cyan;
                    Console.Write("' to exit - "); Console.ResetColor();

                    var choice = "";
                    choice = Console.ReadLine();
                    Console.Write("\n");

                    switch (choice)
                    {
                        case "sql":

                            Console.ForegroundColor = ConsoleColor.Cyan;
                            Console.Write("Enter a query: ");
                            Console.ResetColor();

                            string query = "";
                            query = Console.ReadLine();
                            Console.Write("\n");
                            Console.Clear();

                            Console.ForegroundColor = ConsoleColor.Cyan;
                            Console.Write("Your query: ");
                            Console.ResetColor();
                            Console.WriteLine($"{query}\n");

                            SqlCommand command = new SqlCommand(query, connection);
                            SqlDataReader reader = command.ExecuteReader();

                            for (int i = 0; i < reader.FieldCount; i++)
                            {
                                Console.Write($"{reader.GetName(i)}\t");
                            }
                            Console.Write("\n");

                            while (reader.Read())
                            {
                                for (int i = 0; i < reader.FieldCount; ++i)
                                    Console.Write($"{reader[i]}\t");
                                Console.WriteLine();
                            }
                            Console.Write("\n");

                            if (reader != null)
                                reader.Close();

                            break;
                        case "endl":
                            work = false;
                            Console.Clear();
                            return;
                    }
                } while (work);
            }
            catch (InvalidOperationException io)
            {
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine($"Error: {io.Message}");
                Console.ResetColor();

                if (connection != null)
                {
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    Console.Write("\nThe connection is closed, do you want to open it? ("); Console.ResetColor();
                    Console.Write("Y/N"); Console.ForegroundColor = ConsoleColor.Cyan; Console.Write(")"); Console.ResetColor();

                    string ch;
                    ch = Console.ReadLine();
                    Console.Clear();

                    switch (ch)
                    {
                        case "Y":
                        case "y":
                            dbConnection();
                            dbQuery();
                            break;
                        case "N":
                        case "n":
                            Console.Write("Press any key to continue...");
                            Console.ReadKey(true);
                            Console.Clear();
                            return;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine($"Error: {ex.Message}");
                Console.ResetColor();

                Console.Clear();
            }
        }

        static void dbList()
        {
            Console.Clear();

            try
            {
                DataTable table = null;
                string serverDbName = "Databases";
                table = connection.GetSchema(serverDbName);

                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.Write("Databases on "); Console.ResetColor();
                Console.WriteLine($"{serverName}\n");

                if (table != null)
                {
                    foreach (DataRow row in table.Rows)
                    {
                        foreach (var item in row.ItemArray)
                        {
                            Console.Write("{0} \t", item);
                        }
                        Console.WriteLine();
                    }
                }
            }
            catch (InvalidOperationException io)
            {
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine($"Error: {io.Message}");
                Console.ResetColor();

                if (connection != null)
                {
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    Console.Write("\nThe connection is closed, do you want to open it? ("); Console.ResetColor();
                    Console.Write("Y/N"); Console.ForegroundColor = ConsoleColor.Cyan; Console.Write(")"); Console.ResetColor();

                    string ch;
                    ch = Console.ReadLine();
                    Console.Clear();
                    switch (ch)
                    {
                        case "Y":
                        case "y":
                            dbConnection();
                            dbList();
                            return;
                        case "N":
                        case "n":
                            Console.Write("Press any key to continue...");
                            Console.ReadKey(true);
                            Console.Clear();
                            return;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine($"Error: {ex.Message}");
                Console.ResetColor();
            }

            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.Write("\nAll done. Press any key to finish...");
            Console.ResetColor();

            Console.ReadKey(true);

            Console.Clear();
        }

        static void dbProc()
        {
            Console.Clear();

            try
            {
                //string procedure = "getImgCountByUser";
                string email = "vasia@mail.com";
                string procedure = "";

                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.Write("Enter the name of the stored procedure: ");
                Console.ResetColor();

                procedure = Console.ReadLine();
                Console.Write("\n");

                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.Write("Your procedure: "); Console.ResetColor();
                Console.WriteLine($"{procedure}\n");

                SqlCommand cmd = new SqlCommand(procedure, connection);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add("@userEmail", SqlDbType.NVarChar).Value = email;
                cmd.Parameters.Add(
                        new SqlParameter("@imgCount", SqlDbType.Int)
                        {
                            Direction = ParameterDirection.Output
                        }
                    );

                cmd.ExecuteNonQuery();

                Console.WriteLine($"Image count for {email} is {cmd.Parameters["@imgCount"].Value}");
            }
            catch (InvalidOperationException io)
            {
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine($"Error: {io.Message}");
                Console.ResetColor();

                if (connection != null)
                {
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    Console.Write("\nThe connection is closed, do you want to open it? ("); Console.ResetColor();
                    Console.Write("Y/N"); Console.ForegroundColor = ConsoleColor.Cyan; Console.Write(")"); Console.ResetColor();

                    string ch;
                    ch = Console.ReadLine();
                    Console.Clear();
                    switch (ch)
                    {
                        case "Y":
                        case "y":
                            dbConnection();
                            dbProc();
                            return;
                        case "N":
                        case "n":
                            Console.ForegroundColor = ConsoleColor.Cyan;
                            Console.Write("Press any key to continue...");
                            Console.ResetColor();
                            Console.ReadKey(true);
                            Console.Clear();
                            return;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine($"Error: {ex.Message}");
                Console.ResetColor();        
            }

            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.Write("\nAll done. Press any key to finish...");
            Console.ResetColor();
            Console.ReadKey(true);
            Console.Clear();
        }
    }
}