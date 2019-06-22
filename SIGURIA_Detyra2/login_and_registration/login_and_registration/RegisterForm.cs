using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace login_and_registration
{
    public partial class RegisterForm : Form
    {
        public RegisterForm()
        {
            InitializeComponent();
        }

        private void RegisterForm_Load(object sender, EventArgs e)
        {
            // largo focus prej textboxes
            this.ActiveControl = label1;

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void textBoxFirstName_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBoxFirstName_Enter(object sender, EventArgs e)
        {
            String fname = textBoxFirstName.Text;
            if(fname.ToLower().Trim().Equals("first name"))
            {
                textBoxFirstName.Text = "";
                textBoxFirstName.ForeColor = Color.Black;
            }
        }

        private void textBoxFirstName_Leave(object sender, EventArgs e)
        {
            String fname = textBoxFirstName.Text;
            if (fname.ToLower().Trim().Equals("first name") || fname.Trim().Equals(""))
            {
                textBoxFirstName.Text = "first name";
                textBoxFirstName.ForeColor = Color.Gray;
            }
        }

        private void textBoxLastName_Enter(object sender, EventArgs e)
        {

            String lname = textBoxLastName.Text;
            if (lname.ToLower().Trim().Equals("last name"))
            {
                textBoxLastName.Text = "";
                textBoxLastName.ForeColor = Color.Black;
            }


        }

        private void textBoxLastName_Leave(object sender, EventArgs e)
        {

            String lname = textBoxLastName.Text;
            if (lname.ToLower().Trim().Equals("last name") || lname.Trim().Equals(""))
            {
                textBoxLastName.Text = "last name";
                textBoxLastName.ForeColor = Color.Gray;
            }

        }

        private void textBoxEmail_Enter(object sender, EventArgs e)
        {

            String email = textBoxEmail.Text;
            if (email.ToLower().Trim().Equals("email"))
            {
                textBoxEmail.Text = "";
                textBoxEmail.ForeColor = Color.Black;
            }


        }

        private void textBoxEmail_Leave(object sender, EventArgs e)
        {

            String email = textBoxEmail.Text;
            if (email.ToLower().Trim().Equals("email") || email.Trim().Equals(""))
            {
                textBoxEmail.Text = "email";
                textBoxEmail.ForeColor = Color.Gray;
            }

        }

        private void textBoxUsername_Enter(object sender, EventArgs e)
        {

            String username = textBoxUsername.Text;
            if (username.ToLower().Trim().Equals("username"))
            {
                textBoxUsername.Text = "";
                textBoxUsername.ForeColor = Color.Black;
            }

        }

        private void textBoxUsername_Leave(object sender, EventArgs e)
        {

            String username = textBoxUsername.Text;
            if (username.ToLower().Trim().Equals("username") || username.Trim().Equals(""))
            {
                textBoxUsername.Text = "username";
                textBoxUsername.ForeColor = Color.Gray;
            }

        }

        private void textBoxPassword_Enter(object sender, EventArgs e)
        {

            String password = textBoxPassword.Text;
            if (password.ToLower().Trim().Equals("password"))
            {
                textBoxPassword.Text = "";
                textBoxPassword.UseSystemPasswordChar = true;
                textBoxPassword.ForeColor = Color.Black;
            }

        }

        private void textBoxPassword_Leave(object sender, EventArgs e)
        {

            String password = textBoxPassword.Text;
            if (password.ToLower().Trim().Equals("password") || password.Trim().Equals(""))
            {
                textBoxPassword.Text = "password";
                textBoxPassword.UseSystemPasswordChar = false;
                textBoxPassword.ForeColor = Color.Gray;
            }

        }

        private void textBoxPasswordConfirm_Enter(object sender, EventArgs e)
        {

            String cpassword = textBoxPasswordConfirm.Text;
            if (cpassword.ToLower().Trim().Equals("confirmpassword"))
            {
                textBoxPasswordConfirm.Text = "";
                textBoxPasswordConfirm.UseSystemPasswordChar = true;
                textBoxPasswordConfirm.ForeColor = Color.Black;
            }

        }

        private void textBoxPasswordConfirm_Leave(object sender, EventArgs e)
        {

            String cpassword = textBoxPasswordConfirm.Text;
            if (cpassword.ToLower().Trim().Equals("confirmpassword") ||
                cpassword.ToLower().Trim().Equals("password") ||
                cpassword.Trim().Equals("")) 
            {
                textBoxPasswordConfirm.Text = "confirmpassword";
                textBoxPasswordConfirm.UseSystemPasswordChar = false;
                textBoxPasswordConfirm.ForeColor = Color.Gray;
            }

        }

        private void label4_Click(object sender, EventArgs e)
        {
            // this.Close();
            Application.Exit();

        }

        private static X509Certificate2 GetCertificateFromStore(string certName)
        {

            // Get the certificate store for the current user.
            X509Store store = new X509Store(StoreLocation.CurrentUser);
            try
            {
                store.Open(OpenFlags.ReadOnly);

                // Place all certificates in an X509Certificate2Collection object.
                X509Certificate2Collection certCollection = store.Certificates;
                X509Certificate2Collection currentCerts = certCollection.Find(X509FindType.FindByTimeValid, DateTime.Now, false);
                X509Certificate2Collection signingCert = currentCerts.Find(X509FindType.FindBySubjectDistinguishedName, certName, false);
                if (signingCert.Count == 0)
                    return null;
                // Return the first certificate in the collection, has the right name and is current.
                return signingCert[0];
            }
            finally
            {
                store.Close();
            }

        }

        public static byte[] EncryptDataOaepSha1(X509Certificate2 cert, byte[] data)
        {

            using (RSA rsa = cert.GetRSAPublicKey())
            {

                return rsa.Encrypt(data, RSAEncryptionPadding.OaepSHA1);
            }
        }

            private void buttonCreateAccount_Click(object sender, EventArgs e)
        {

            //  *************




                UdpClient klienti = new UdpClient();
                IPEndPoint ep = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 12000);
                klienti.Connect(ep);

                byte[] bytesend = Encoding.ASCII.GetBytes
                                  (
                                        textBoxFirstName.Text + " " +
                                        textBoxLastName.Text + " " +
                                        textBoxEmail.Text + " " +
                                        textBoxUsername.Text + " " +
                                        textBoxPassword.Text + " " +
                                        textBoxPasswordConfirm.Text
                                  );

                klienti.Send(bytesend, bytesend.Length);


            //
            X509Certificate2 cert = GetCertificateFromStore("CN=RootCA");
            if (cert == null)
            {
                Console.WriteLine("Certificate 'CN=CERT_SIGN_TEST_CERT' not found.");
                Console.ReadLine();
            }



          
                string email = textBoxUsername.Text.Trim();
                string password = textBoxPassword.Text.Trim();


                DES des = new DES();

                string mesazhi = email + ":" + password + ":" + "blla";
                Console.WriteLine(mesazhi);
                byte[] encrytedData = des.Enkripto(mesazhi);

                byte[] IV = des.getIV();
                byte[] key = des.getKey();

                byte[] encryptedKey = EncryptDataOaepSha1(cert, key);

                Console.WriteLine(encryptedKey.Length);
                Console.WriteLine(Convert.ToBase64String(encryptedKey));

                Console.WriteLine(Convert.ToBase64String(key));
                Console.WriteLine(Convert.ToBase64String(DecryptDataOaepSha1(cert, encryptedKey)));


                string delimiter = ".";
                string fullmessageEncrypted = Convert.ToBase64String(IV) + delimiter + Convert.ToBase64String(encryptedKey) + delimiter + Convert.ToBase64String(encrytedData);

        //


    }
        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void labelGoToLogin_Click(object sender, EventArgs e)
        {
            this.Hide();
            Form1 form1 = new Form1();
            form1.Show();
        }

        private static X509Certificate2 GetCertificateFromStore(string certName)
        {

            // Get the certificate store for the current user.
            X509Store store = new X509Store(StoreLocation.CurrentUser);
            try
            {
                store.Open(OpenFlags.ReadOnly);

                // Place all certificates in an X509Certificate2Collection object.
                X509Certificate2Collection certCollection = store.Certificates;
                // If using a certificate with a trusted root you do not need to FindByTimeValid, instead:
                // currentCerts.Find(X509FindType.FindBySubjectDistinguishedName, certName, true);
                X509Certificate2Collection currentCerts = certCollection.Find(X509FindType.FindByTimeValid, DateTime.Now, false);
                X509Certificate2Collection signingCert = currentCerts.Find(X509FindType.FindBySubjectDistinguishedName, certName, false);
                if (signingCert.Count == 0)
                    return null;
                // Return the first certificate in the collection, has the right name and is current.
                return signingCert[0];
            }
            finally
            {
                store.Close();
            }

        }


        public static byte[] DecryptDataOaepSha1(X509Certificate2 cert, byte[] data)
        {
            // GetRSAPrivateKey returns an object with an independent lifetime, so it should be
            // handled via a using statement.
            using (RSA rsa = cert.GetRSAPrivateKey())
            {
                return rsa.Decrypt(data, RSAEncryptionPadding.OaepSHA1);
            }
        }
    }

}

   
