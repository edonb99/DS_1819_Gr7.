using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace login_and_registration
{
    public partial class Form1 : Form
    {


        public Form1()
        {
            InitializeComponent();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void labelClose_MouseEnter(object sender, EventArgs e)
        {

        }

        private void panel2_DoubleClick(object sender, EventArgs e)
        {
            labelClose.ForeColor = Color.Black;

        }

        private void panel2_MouseLeave(object sender, EventArgs e)
        {
            labelClose.ForeColor = Color.White;

        }

        private void labelClose_Click(object sender, EventArgs e)
        {
            //this.Close();
            Application.Exit();
        }

        private void buttonLogin_Click(object sender, EventArgs e)
        {
            UdpClient klienti = new UdpClient();
            Socket newSocket = new Socket(AddressFamily.InterNetwork,
                                        SocketType.Dgram, ProtocolType.Udp);
            IPEndPoint ep = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 12000);
            EndPoint tempRemote = (EndPoint)ep;
            klienti.Connect(ep);
          
            byte[] bytesend = Encoding.ASCII.GetBytes(textBoxUsername.Text + ' ' + textBoxPassword.Text);

            klienti.Send(bytesend, bytesend.Length);
            Console.WriteLine("AAAAAAAAAAAaaa");

            byte[] receivedData = klienti.Receive(ref ep);
            if (string.Equals(Encoding.ASCII.GetString(receivedData), "Wrong password/username"))
            {
                textBoxUsername.Text = "";
                textBoxPassword.Text = "";
                Console.WriteLine(Encoding.ASCII.GetString(receivedData));
                grade.Text = Encoding.ASCII.GetString(receivedData);
                
             
            }
               
            Console.WriteLine(Encoding.ASCII.GetString(receivedData));
            grade.Text = Encoding.ASCII.GetString(receivedData);
        }
        //********************************** 
        private bool validatePass()
        {
            string pattern = "^[\\S*$]"; // no spaces
            if (textBoxPassword.Text.Length > 6 && Regex.IsMatch(textBoxPassword.Text, pattern))
            {
                textBoxPassword.Text = "";
                return true;
            }
            else
            {
                textBoxPassword.Text = "*Input more chars";
                return false;
            }
        }
        /*
            private bool validateEmail()
            {
                string pattern = @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z";
                if (Regex.IsMatch(textBoxUsername.Text, pattern, RegexOptions.IgnoreCase))
                {
                textBoxUsername.Text = "";
                    return true;
                }
                else
                {
                textBoxUsername.Text = "* Wrong email";
                    return false;
                }
            }
            */
        //************************







        /*
                    DB db = new DB();



                    String username = textBoxUsername.Text;
                    String password = textBoxPassword.Text;

                    DataTable table = new DataTable();

                    MySqlDataAdapter adapter = new MySqlDataAdapter();

                    MySqlCommand command = new MySqlCommand("SELECT * FROM `users` WHERE `username` = @usn and `password` = @pass", db.getConnection());

                    command.Parameters.Add("@usn", MySqlDbType.VarChar).Value = username;
                    command.Parameters.Add("@pass", MySqlDbType.VarChar).Value = password;

                    adapter.SelectCommand = command;

                    adapter.Fill(table);

                    // shikoje nese user ekziston ose jo
                    if(table.Rows.Count > 0)
                    {
                        MessageBox.Show("YES");
                    }
                    else
                    {
                        MessageBox.Show("NO");
                    }

                    */
    

    

                    private void labelGoToSignUp_Click(object sender, EventArgs e)
                    {
                        this.Hide();
                        RegisterForm registerform = new RegisterForm();
                        registerform.Show();
                    }

                    private void labelGoToSignUp_Enter(object sender, EventArgs e)
                    {
                        labelGoToSignUp.ForeColor = Color.Yellow;
                    }

        private void textBoxUsername_TextChanged(object sender, EventArgs e)
        {

        }

        private void label1_Click_1(object sender, EventArgs e)
        {

        }
    }
}

