using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace login_and_registration
{
    class UDPClient
    {


        private Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
        private EndPoint epFrom = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 12000);
        private string address;
        private int port;

        public UDPClient(string address, int port)
        {


            this.address = address;
            this.port = port;
        }
        public byte[] SendAndReceive(byte[] data)
        {
            socket.SendBufferSize = 128;
            socket.ReceiveBufferSize = 128;
            IPEndPoint serverAddress = new IPEndPoint(IPAddress.Parse(address), port);
            socket.SendTo(data, 0, data.Length, SocketFlags.None, serverAddress);
            byte[] receivedData = new byte[1028];
            int length = socket.Receive(receivedData);

            byte[] incomingByte = new byte[length];

            Array.Copy(receivedData, incomingByte, length);
            return incomingByte;
        }
    }
}
