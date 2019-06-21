
using MySql.Data.MySqlClient;
using Server.JWT.Managers;
using Server.JWT.Models;
using Server.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;


namespace Server
{
    class Program
    {

        public static DESCryptoServiceProvider des = new DESCryptoServiceProvider();

        private static byte[] desKey;
        private static byte[] desIv;
        public static string serverMessage;


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
            Kthehu:
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
                        byte[] bytePlainText = System.Text.Encoding.UTF8.GetBytes(signUp[4]); ;
                        byte[] byteSalt = CreateSalt();
                        string salt = System.Convert.ToBase64String(byteSalt);
                        String hashedSaltedPass = GenerateSaltedHash(bytePlainText, byteSalt);

                        connection = new MySqlConnection(connectionString);
                        connection.Open();
                        MySqlCommand cmd = new MySqlCommand();
                        cmd.Connection = connection;
                        cmd.CommandText = "INSERT INTO `users`(`firstname`, `lastname`, `email`, `username`, `password`,`salt`) VALUES(@fn, @ln, @email, @usn, @pass,@salt)";
                        cmd.Prepare();

                        cmd.Parameters.AddWithValue("@fn", signUp[0]);
                        cmd.Parameters.AddWithValue("@ln", signUp[1]);
                        cmd.Parameters.AddWithValue("@email", signUp[2]);
                        cmd.Parameters.AddWithValue("@usn", signUp[3]);
                        cmd.Parameters.AddWithValue("@pass", hashedSaltedPass);
                        cmd.Parameters.AddWithValue("@salt", salt);




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

                    // *-*-/- 
                    IAuthContainerModel model = GetJWTContainerModel(signUp[3], signUp[2]);
                    IAuthService authService = new JWTService(model.SecretKey);

                    string token = authService.GenerateToken(model);

                    if (!authService.IsTokenValid(token))
                        throw new UnauthorizedAccessException();
                    else
                    {
                        List<Claim> claims = authService.GetTokenClaims(token).ToList();

                        Console.WriteLine(claims.FirstOrDefault(e => e.Type.Equals(ClaimTypes.Name)).Value);
                        Console.WriteLine(claims.FirstOrDefault(e => e.Type.Equals(ClaimTypes.Email)).Value);
                    }
                    // *-*-/- 



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

                else {
                    logIn = result;

                    string connectionString = @"server=localhost;userid=root;password=1234;database=user_db";

                    MySqlConnection connection = null;
                    MySqlDataReader reader = null;
                    try
                    {
                        connection = new MySqlConnection(connectionString);
                        connection.Open();


                        string stm = "SELECT * FROM `users` WHERE `username` = '" + logIn[0] + "'"; //and `password` = '" +logIn[1]+"'";
                        MySqlDataAdapter dataAdapter = new MySqlDataAdapter();
                        dataAdapter.SelectCommand = new MySqlCommand(stm, connection);
                        DataTable table = new DataTable();
                        dataAdapter.Fill(table);
                        if (table.Rows.Count > 0)
                        {
                            Console.WriteLine("Username found");
                            string salt = table.Rows[0]["salt"].ToString();
                            string pass = table.Rows[0]["password"].ToString();
                            string id = table.Rows[0]["id"].ToString();
                            byte[] byteSalt = System.Convert.FromBase64String(salt);
                            byte[] bytePlainText = System.Text.Encoding.UTF8.GetBytes(logIn[1]);
                            string hashedSaltedPass = GenerateSaltedHash(bytePlainText, byteSalt);
                            if (pass.Equals(hashedSaltedPass))
                            {
                                Console.WriteLine("Loged in");
                                string query = "SELECT * FROM `grades` WHERE `userid` =' " + id + "'";
                                dataAdapter = new MySqlDataAdapter();
                                dataAdapter.SelectCommand = new MySqlCommand(query, connection);
                                DataTable table1 = new DataTable();
                                dataAdapter.Fill(table1);
                                string test = null;
                                for (int i = 0; table1.Rows.Count > i; i++)
                                {
                                    test += table1.Rows[i]["course"].ToString() + " " + table1.Rows[i]["grade"].ToString() + "\n";
                                }

                                byte[] packetData = System.Text.ASCIIEncoding.ASCII.GetBytes(test);
                                newSocket.SendTo(packetData, tempRemote);

                            }
                            else
                            {
                                Console.WriteLine("Wrong password/username");
                                byte[] packetData = System.Text.ASCIIEncoding.ASCII.GetBytes("Wrong password/username");
                                newSocket.SendTo(packetData, tempRemote);
                                goto Kthehu;
                            }
                        }
                        else
                        {
                            Console.WriteLine("Wrong password/username");
                            byte[] packetData = System.Text.ASCIIEncoding.ASCII.GetBytes("Wrong password/username");
                            newSocket.SendTo(packetData, tempRemote);
                            goto Kthehu;
                        }
                    }
                    finally
                    {
                        if (reader != null)
                            reader.Close();
                        if (connection != null)
                            connection.Close();
                    }







                }

            }
        }
        // *-*-/- 

        private static JWTContainerModel GetJWTContainerModel(string name, string email)
        {
            return new JWTContainerModel()
            {
                Claims = new Claim[]
                {
                    new Claim(ClaimTypes.Name, name),
                    new Claim(ClaimTypes.Email, email)
                }
            };
        }

       

        private static byte[] CreateSalt()
        {
            //Generate a cryptographic random number.
            RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();
            byte[] buff = new byte[16];
            rng.GetBytes(buff);

            // Return a Base64 string representation of the random number.
            return buff;
        }
        private static string GenerateSaltedHash(byte[] plainText, byte[] salt)
        {
            HashAlgorithm algorithm = new SHA256Managed();

            byte[] plainTextWithSaltBytes =
              new byte[plainText.Length + salt.Length];

            for (int i = 0; i < plainText.Length; i++)
            {
                plainTextWithSaltBytes[i] = plainText[i];
            }
            for (int i = 0; i < salt.Length; i++)
            {
                plainTextWithSaltBytes[plainText.Length + i] = salt[i];
            }
            byte[] hash = algorithm.ComputeHash(plainTextWithSaltBytes);
            return System.Convert.ToBase64String(hash); ;

        }


        public static byte[] Enkripto(String plaintext)
        {
            des.Padding = PaddingMode.Zeros;
            des.Key = desKey;
            des.IV = desIv;


            byte[] bytePlaintexti = Encoding.UTF8.GetBytes(plaintext);

            MemoryStream ms = new MemoryStream();
            CryptoStream cs = new CryptoStream(ms,
                                des.CreateEncryptor(),
                                CryptoStreamMode.Write);
            cs.Write(bytePlaintexti, 0, bytePlaintexti.Length);
            cs.Close();

            byte[] byteCiphertexti = ms.ToArray();

            return byteCiphertexti;

        }


        public static byte[] DekriptoDes(string ciphertext)
        {
            des.Padding = PaddingMode.Zeros;
            des.Mode = CipherMode.CBC;
            des.Key = desKey;
            des.IV = desIv;


            byte[] byteCiphertexti =
                Convert.FromBase64String(ciphertext);
            MemoryStream ms = new MemoryStream(byteCiphertexti);
            CryptoStream cs =
                new CryptoStream(ms,
                des.CreateDecryptor(),
                CryptoStreamMode.Read);

            byte[] byteTextiDekriptuar = new byte[ms.Length];
            cs.Read(byteTextiDekriptuar, 0, byteTextiDekriptuar.Length);
            cs.Close();

            return byteTextiDekriptuar;
        }
    }

}

        

