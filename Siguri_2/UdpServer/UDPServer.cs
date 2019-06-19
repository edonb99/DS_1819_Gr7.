using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace UdpServer
{
    class UDPServer
    {
        private Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
        private EndPoint client;
        public UDPServer(string address,int port)
        {
            socket.SetSocketOption(SocketOptionLevel.IP, SocketOptionName.ReuseAddress, true);
           client= new IPEndPoint(IPAddress.Any,0);
            socket.Bind(new IPEndPoint(IPAddress.Parse(address),port));
        }
        public byte[] SendandReceive(byte[] data)
        {
            socket.SendBufferSize = 128;
            socket.ReceiveBufferSize = 128;
            EndPoint point = (EndPoint)client;
            byte[] receivedData = new byte[1024];
            int length = socket.ReceiveFrom(receivedData, SocketFlags.None, ref point);
            byte[] incomingByte = new byte[length];
            Array.Copy(receivedData, incomingByte, length);
            socket.SendTo(data,0,data.Length,SocketFlags.None,point);
           
            return incomingByte;
        }
    }
}
