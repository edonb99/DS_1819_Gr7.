using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace login_and_registration
{
    class DES
    {

        private DESCryptoServiceProvider desObj = new DESCryptoServiceProvider();

        public DES()
        {
            desObj.GenerateKey();
            desObj.GenerateIV();
            desObj.Padding = PaddingMode.Zeros;
            desObj.Mode = CipherMode.CBC;
        }
        public byte[] getKey()
        {
            return desObj.Key;
        }
        public byte[] getIV()
        {
            return desObj.IV;
        }

        public byte[] Enkripto(String plaintext)
        {



            byte[] bytePlaintexti = Encoding.UTF8.GetBytes(plaintext);

            MemoryStream ms = new MemoryStream();
            CryptoStream cs = new CryptoStream(ms,
                                desObj.CreateEncryptor(),
                                CryptoStreamMode.Write);
            cs.Write(bytePlaintexti, 0, bytePlaintexti.Length);
            cs.Close();

            byte[] byteCiphertexti = ms.ToArray();

            return byteCiphertexti;

        }
        public byte[] Dekripto(String ciphertext)
        {


            byte[] byteCiphertexti =
                Convert.FromBase64String(ciphertext);
            MemoryStream ms = new MemoryStream(byteCiphertexti);
            CryptoStream cs =
                new CryptoStream(ms,
                desObj.CreateDecryptor(),
                CryptoStreamMode.Read);

            byte[] byteTextiDekriptuar = new byte[ms.Length];
            cs.Read(byteTextiDekriptuar, 0, byteTextiDekriptuar.Length);
            cs.Close();

            return byteTextiDekriptuar;

        }
    }
}

