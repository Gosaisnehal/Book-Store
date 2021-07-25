using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using ConsoleTables;

namespace Book_Store
{
   
    class Program
    {
       
        static void Main(string[] args)
        {   //connection string 
            string conn = ConfigurationManager.ConnectionStrings["DbConn"].ConnectionString;
            //create instanace of database connection
            SqlConnection sqlConn = new SqlConnection(conn);

            {
               try
                {
                    Console.WriteLine("\n\n ------------------------------------------------Welcome------------------------------------------------");
                    // Print Menu option
                    Console.WriteLine("Please choose your option:\n 1 : Print order \n 2 : Add a new book \n 3 : Display all books");
                    // Reading the user input
                    int task = Convert.ToInt32(Console.ReadLine());
                    try 
                    {
                        switch (task)
                        {
                            // Print order 
                            case 1:
                                //displaying all books
                                DisplayBooks();
                                Console.WriteLine("Please enter Book ID to add to your Cart:");
                                string addtocart = Console.ReadLine();


                                //order books to print
                                order(addtocart);
                                break;

                            // Add a new Book
                            case 2:
                                Console.WriteLine("Please enter book details:");
                                AddBook();
                                break;

                            // displaying all books
                            case 3:
                                Console.WriteLine("Books list:");
                                DisplayBooks();
                                break;

                            default:
                                Console.WriteLine("No option choosen");
                                break;
                        }
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("Error: " + e.Message);
                    }
                              

                }
                catch (Exception e)
                {
                    Console.WriteLine("Error: " + e.Message);
                }

                Console.Read();
                //Menu();
            }
        }

       
        static void DisplayBooks()
        {

            string conn = ConfigurationManager.ConnectionStrings["DbConn"].ConnectionString;
            SqlConnection sqlConn = new SqlConnection(conn);
            //open connection
            sqlConn.Open();
            
            // sql query to get all the books 
            SqlCommand cmd = new SqlCommand("select bookid, bookTitle,bookAuthor, bookGenre, bookUnitPrice from books", sqlConn);

            // get query results
            SqlDataReader rdr = cmd.ExecuteReader();

            // print each record
            
            string[] val;

            // table header
            var table = new ConsoleTable("ID", "Title", "Author","Genre" , "Price");
            //rdr = cmd.ExecuteReader();
            if (rdr.HasRows)
            {
                while (rdr.Read())
                {
                    val = new string[rdr.FieldCount];
                    for (int i = 0; i < rdr.FieldCount; i++)
                        val[i] = Convert.ToString(rdr.GetValue(i));
                    table.AddRow(val[0], val[1], val[2],val[3] ,"$" + val[4].ToString());
                }
                table.Write();
                Console.WriteLine();
            }
            else
                Console.WriteLine("No Records available in the database....\n");
            // Closing connection
            sqlConn.Close();
        }

        private static void order(string addtocart) 
        {
           
            string conn = ConfigurationManager.ConnectionStrings["DbConn"].ConnectionString;
            SqlConnection sqlConn = new SqlConnection(conn);
            //open connection
            sqlConn.Open();

            SqlDataReader rdr,rdr2 = null;
            SqlCommand command,command2;
            //SqlDataAdapter adapter = new SqlDataAdapter();

            //Console.WriteLine("Please enter Book ID to add to your Cart:");
            //int addtocart = int.Parse(Console.ReadLine());
            //string sql = "";


            // Displaying selected books
            string[] val;
            var table = new ConsoleTable("ID", "Title");

            // sql query to get all the books 
            string sql = "select bookId,bookTitle from books Where bookId IN(" + addtocart + ")";
            command = new SqlCommand(sql, sqlConn);

            // get query results 
            rdr = command.ExecuteReader();

            // displaying to the table 
            if (rdr.HasRows)
            {
                while (rdr.Read())
                {
                    val = new string[rdr.FieldCount];
                    for (int i = 0; i < rdr.FieldCount; i++)
                        val[i] = Convert.ToString(rdr.GetValue(i));
                    table.AddRow(val[0], val[1]);
                }
                table.Write();
                Console.WriteLine();
            }
            else
                Console.WriteLine("No Records available in the database....\n");
            // command disposed
            command.Dispose();
            // Datareader closed
            rdr.Close();

            // sql query to get order value 
            // while checking any book from crime category if yes then provide 5% discount  
            string sql2 = "select sum( case bookGenre when 'crime' then bookUnitPrice-bookUnitPrice * 0.05 else bookUnitPrice END ) from[Book Store].dbo.books where bookId IN(" + addtocart + ")";
            command2 = new SqlCommand(sql2, sqlConn);
            // get query results
            rdr2 = command2.ExecuteReader();

            // 
            while (rdr2.Read())
            {
                
                const double gstFee = 0.1;   // GST Fee 10%
                decimal order = Convert.ToDecimal(rdr2[0]); // converting to decimal 
                double orderWithoutGST = Convert.ToDouble(order); // converting to Double to apply GST
                double orderWithGST = orderWithoutGST + orderWithoutGST * gstFee; // adding GST fee
                             
                const double deliveryFee = 5.95; // delivery fee for order less than $20
                const double minOrder = 20.00;

                // apply delivery fee if order value less than $20
                if (orderWithoutGST < minOrder)
                {
                    double totalCostWithoutGST = orderWithoutGST + deliveryFee;
                    double totalCostWithGST = orderWithGST + deliveryFee;

                    // Printing total cost with and without GST
                    Console.WriteLine(" The total cost of the order without GST is: $" + totalCostWithoutGST);
                    Console.WriteLine(" The total cost of the order with applied GST is: $" + totalCostWithGST);
                }
                else // No delivery charges 
                {
                    // Printing total cost with and without GST
                    Console.WriteLine(" The total cost of the order without GST is: $" + orderWithoutGST);
                    Console.WriteLine("The total cost of the order  with applied GST is: $" + orderWithGST);
                }
                            

            }

            command2.Dispose(); // command disposed 
            rdr2.Close();       // sqlDatareader closed
            sqlConn.Close();    // connection closed
           
        }

        // Add a new book to the database
        private static void AddBook()
        {
            
            string conn = ConfigurationManager.ConnectionStrings["DbConn"].ConnectionString;
            SqlConnection sqlConn = new SqlConnection(conn);
            // conncection open
            sqlConn.Open();

            // getting book details 
            Console.WriteLine("Enter book name: ");
            var title = Console.ReadLine();
            Console.WriteLine("Enter Author name/s:");
            var author = Console.ReadLine();
            Console.WriteLine("Enter Genre name:");
            var genre = Console.ReadLine();
            Console.WriteLine("Enter book price without tax:");
            var price = Console.ReadLine();


            SqlCommand insertCommand = new SqlCommand("INSERT INTO books (bookTitle, bookAuthor, bookGenre, bookUnitPrice) VALUES (@bookTitle, @bookAuthor, @bookGenre, @bookUnitPrice)", sqlConn);
            insertCommand.Parameters.Add(new SqlParameter("bookTitle", title));
            insertCommand.Parameters.Add(new SqlParameter("bookAuthor", author));
            insertCommand.Parameters.Add(new SqlParameter("bookGenre", genre));
            insertCommand.Parameters.Add(new SqlParameter("bookUnitPrice", price));
            Console.WriteLine("Commands executed! Total rows affected are " + insertCommand.ExecuteNonQuery());
            Console.ReadLine();
            Console.Clear();
            // connection closed 
            sqlConn.Close();

        }
    }
}
            
    

    

