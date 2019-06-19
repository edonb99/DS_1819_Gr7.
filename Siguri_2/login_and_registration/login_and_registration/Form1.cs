using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
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

            klienti.Connect(new IPEndPoint(IPAddress.Parse("127.0.0.1"), 12000));


            byte[] bytesend = Encoding.ASCII.GetBytes(textBoxUsername.Text);

            klienti.Send(bytesend, bytesend.Length);

            byte[] bytesend2 = Encoding.ASCII.GetBytes(textBoxPassword.Text);

            klienti.Send(bytesend2, bytesend2.Length);

            klienti.Close();











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
        }

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
    }
}
