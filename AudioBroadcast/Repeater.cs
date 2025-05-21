using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace AudioBroadcast
{
    public class Repeater
    {
        private List<Thread> endpointRepeaters = [];
        private Thread accThread;
        Socket listener;
        IPEndPoint iPEndPoint;
        public Repeater() {
            accThread = new Thread(IncomingEndpoints);
            iPEndPoint = new IPEndPoint(IPAddress.Any, 12000);
            listener = new(
                iPEndPoint.AddressFamily,
                SocketType.Stream,
                ProtocolType.Tcp
            );
        }

        public void Start()
        {
            accThread.Start();
        }

        private void IncomingEndpoints()
        {
            
            listener.Bind(iPEndPoint);
            listener.Listen();
            try
            {
                while (true)
                {
                    var handler = listener.Accept();
                    AddBroardcastEndpoint(handler);
                }
            }
            catch(SocketException) {
                Console.WriteLine("Stopped listening");
            }
        }

        private void AddBroardcastEndpoint(Socket endpoint)
        {
            Recorder recorder = new Recorder();
            recorder.RecordedData += (s, a) =>
            {
                endpoint.Send(a.Buffer);
            };

            Thread repeater = new Thread(recorder.StartRecording);
            repeater.Start();
            endpointRepeaters.Add(repeater);
        }

        public void Stop()
        {
            foreach (var repeater in endpointRepeaters)
            {
                repeater.Interrupt();
            }
            listener.Dispose();
        }
    }
}
