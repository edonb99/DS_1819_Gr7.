using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UdpServer
{
    class Program
    {
        static void Main(string[] args)
        {

            while (true)
            {
                UDPServer udpserver = new UDPServer("127.0.0.1", 12000);
                Console.WriteLine("Serveri eshte i gatshem per kerkesa");

                while (true)
                {
                    byte[] receivedata = udpserver.SendandReceive(Encoding.UTF8.GetBytes("OK"));

                   // Console.WriteLine("Te dhenat:" + Convert.ToBase64String(receivedata));
                   // Console.WriteLine("Te dhenat:" + Encoding.UTF8.GetString(receivedata));
                }
            }
        }
    }
}
