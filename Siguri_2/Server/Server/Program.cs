
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    class Program
    {
        static void Main(string[] args)
        {
            string[] signUp;
            string[] logIn = null;
            int recv;
            byte[] data = new byte[1024];
            IPEndPoint endpoint = new IPEndPoint(IPAddress.Any, 12000);

            Socket newSocket = new Socket(AddressFamily.InterNetwork,
                                        SocketType.Dgram, ProtocolType.Udp);    //Ruajtja e connection qe e marrim
            newSocket.Bind(endpoint);   //lidhja e cdo connection ne mberritje

            Console.WriteLine("Duke pritur per nje klient.....");

            IPEndPoint sender = new IPEndPoint(IPAddress.Any, 12000);   //Lidhje e cdo pajisjeje(klienti) me qfardo IP dhe porti: 12000
            EndPoint tempRemote = (EndPoint)sender;     //variabla qe e ruan klinetin

            while (true)
            {
                data = new byte[1024];      //resetimi i byte[]
                recv = newSocket.ReceiveFrom(data, ref tempRemote);
                Console.WriteLine(Encoding.ASCII.GetString(data, 0, recv));     //nese ka te dhena per tu lexuar, atehere i shfaqim ato 
                string[] result = Encoding.ASCII.GetString(data, 0, recv).Split(' ');

                if (result.Length > 2)
                {
                    
                        signUp = result;

                    //**********************************

                    string connectionString = @"server=localhost;userid=root;password=1234;database=user_db";

                    MySqlConnection connection = null;
                     try
                    {
                        connection = new MySqlConnection(connectionString);
                        connection.Open();
                        MySqlCommand cmd = new MySqlCommand();
                        cmd.Connection = connection;
                        cmd.CommandText = "INSERT INTO `users`(`firstname`, `lastname`, `email`, `username`, `password`) VALUES(@fn, @ln, @email, @usn, @pass)";
                        cmd.Prepare();
                        
                        cmd.Parameters.AddWithValue("@fn", signUp[0]);
                        cmd.Parameters.AddWithValue("@ln", signUp[1]);
                        cmd.Parameters.AddWithValue("@email", signUp[2]);
                        cmd.Parameters.AddWithValue("@usn", signUp[3]);
                        cmd.Parameters.AddWithValue("@pass", SaltedHash.getSaltedHash(signUp[4]));
                        //cmd.ExecuteNonQuery();
                    //}
                    //finally
                    //{
                     //   if (connection != null)
                       //     connection.Close();
                    //}

                    //**********************************


                   /* DB db = new DB();
                        MySqlCommand command = new MySqlCommand("INSERT INTO `users`(`firstname`, `lastname`, `email`, `username`, `password`) VALUES (@fn, @ln, @email, @usn, @pass)", db.getConnection());

                        command.Parameters.Add("@fn", MySqlDbType.VarChar).Value = signUp[0].ToString();
                        command.Parameters.Add("@ln", MySqlDbType.VarChar).Value = signUp[1].ToString();
                        command.Parameters.Add("@email", MySqlDbType.VarChar).Value = signUp[2].ToString();
                        command.Parameters.Add("@usn", MySqlDbType.VarChar).Value = signUp[3].ToString();
                        command.Parameters.Add("@pass", MySqlDbType.VarChar).Value = signUp[4].ToString();

                        // open the connection
                        db.openConnection();
                        */
                        // check if the textboxes contains the default values 
                        if (!checkTextBoxesValues())
                        {
                            // check if the password equal the confirm password
                            if (signUp[4].Equals(signUp[5]))
                            {
                                // check if this username already exists
                                if (checkUsername())
                                {
           
                                    Console.WriteLine("This Username Already Exists, Select A Different One", "Duplicate Username");
                                }
                                else
                                {
                                    // execute the query
                                    if (cmd.ExecuteNonQuery() == 1)
                                    {
                                    Console.WriteLine("Your Account Has Been Created", "Account Created");
                                    }
                                    else
                                    {
                                    Console.WriteLine("ERROR");
                                    }
                                }
                            }
                            else
                            {
                            Console.WriteLine("Wrong Confirmation Password", "Password Error");
                            }

                        }
                        else
                        {
                        Console.WriteLine("Enter Your Informations First", "Empty Data");
                        }


                        }
                        finally
                        {
                           if (connection != null)
                             connection.Close();
                        }




                        // check if the username already exists
                        Boolean checkUsername()
                    {
                        DB db = new DB();

                        String username = signUp[3];

                        DataTable table = new DataTable();

                        MySqlDataAdapter adapter = new MySqlDataAdapter();

                        MySqlCommand command = new MySqlCommand("SELECT * FROM `users` WHERE `username` = @usn", db.getConnection());

                        command.Parameters.Add("@usn", MySqlDbType.VarChar).Value = username;

                        adapter.SelectCommand = command;

                        adapter.Fill(table);

                        // check if this username already exists in the database
                        if (table.Rows.Count > 0)
                        {
                            return true;
                        }
                        else
                        {
                            return false;
                        }

                    }

                    // check if the textboxes contains the default values
                    Boolean checkTextBoxesValues()
                    {
                        String fname = signUp[0];
                        String lname = signUp[1];
                        String email = signUp[2];
                        String username = signUp[3];
                        String password = signUp[4];

                        if (fname.Equals("first name") || lname.Equals("last name") ||
                            email.Equals("email") || username.Equals("username")
                            || password.Equals("password"))
                        {
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }
                }

                else
                {
                    logIn = result;

                    string connectionString = @"server=localhost;userid=root;password=1234;database=user_db";

                    MySqlConnection connection = null;
                    MySqlDataReader reader = null;
                    try
                    {
                        connection = new MySqlConnection(connectionString);
                        connection.Open();

                        string stm = "SELECT * FROM `users` WHERE `username` = '" +logIn[0]+"' and `password` = '" +logIn[1]+"'";
                        MySqlDataAdapter dataAdapter = new MySqlDataAdapter();
                        dataAdapter.SelectCommand = new MySqlCommand(stm, connection);
                        DataTable table = new DataTable();
                        dataAdapter.Fill(table);
                        if (table.Rows.Count > 0)
                        {
                            Console.WriteLine("Successfully logged in");
                            byte[] packetData = System.Text.ASCIIEncoding.ASCII.GetBytes("palidhje");
                            //newSocket.SendTo(packetData, endpoint); 
                        }
                        else
                        {
                            Console.WriteLine("There is no such user");
                        }

                    }
                    finally
                    {
                        if (reader != null)
                            reader.Close();
                        if (connection != null)
                            connection.Close();
                    }

                    /* DB db = new DB();



                     String username = logIn[0];
                     String password = logIn[1];

                     DataTable table = new DataTable();

                     MySqlDataAdapter adapter = new MySqlDataAdapter();

                     MySqlCommand command = new MySqlCommand("SELECT * FROM `users` WHERE `username` = @usn and `password` = @pass", db.getConnection());

                     command.Parameters.Add("@usn", MySqlDbType.VarChar).Value = username;
                     command.Parameters.Add("@pass", MySqlDbType.VarChar).Value = password;

                     adapter.SelectCommand = command;

                     adapter.Fill(table);

                     // shikoje nese user ekziston ose jo
                     if (table.Rows.Count > 0)
                     {

                     }
                     else
                     {

                     }*/
                }
            }
        }
    }
}

        

