using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace AudioBroadcast
{
    public class Client
    {
        private Thread recvThread;
        Socket clientSock;
        IPEndPoint iPEndPoint;
        public Client() {
            recvThread = new Thread(BindToServer);
            iPEndPoint = new IPEndPoint(IPAddress.Loopback, 12000);
            clientSock = new(
                iPEndPoint.AddressFamily,
                SocketType.Stream,
                ProtocolType.Tcp
            );
        }

        public void Start()
        {
            recvThread.Start();
        }
        private void BindToServer()
        {
            clientSock.Connect(iPEndPoint);
            byte[] buffer = new byte[8192];
            int recvAmount;
            while (true)
            {
                recvAmount = clientSock.Receive(buffer);
                Console.WriteLine(recvAmount);
                Console.WriteLine(Encoding.UTF8.GetString(buffer));
            }
        }
    }
}
