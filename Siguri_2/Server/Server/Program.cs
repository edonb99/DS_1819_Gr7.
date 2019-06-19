using System;
using System.Collections.Generic;
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
                }

                else
                {
                    logIn = result;
                }
            }
        }
    }
}

        

