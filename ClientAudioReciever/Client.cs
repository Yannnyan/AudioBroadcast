using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using NAudio.Wave;

namespace AudioBroadcast
{
    public class Client
    {
        private Thread recvThread;
        Socket clientSock;
        IPEndPoint iPEndPoint;
        WasapiOut outDevice;
        public Client() {
            outDevice = new WasapiOut(NAudio.CoreAudioApi.AudioClientShareMode.Shared, false, 100);
            recvThread = new Thread(BindToServer);
            iPEndPoint = new IPEndPoint(IPAddress.Parse("192.168.137.172"), 12000);
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
            var bufferedWaveProvider = new BufferedWaveProvider(new WasapiLoopbackCapture().WaveFormat);
            bufferedWaveProvider.DiscardOnBufferOverflow = true;
            bufferedWaveProvider.BufferLength = buffer.Length * 10;
            outDevice.Init(bufferedWaveProvider);
            outDevice.Play();
            while (true)
            {
                recvAmount = clientSock.Receive(buffer);
                bufferedWaveProvider.AddSamples(buffer, 0, recvAmount);
            }
        }
    }
}
