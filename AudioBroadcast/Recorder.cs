using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NAudio.Wave;

namespace AudioBroadcast
{
    public class Recorder
    {
        public delegate void RecordedDataHandler(object? a,WaveInEventArgs waveInEventArgs);
        public event RecordedDataHandler RecordedData;
        
        private readonly WasapiLoopbackCapture m_Capture;
        public Recorder() => m_Capture = new WasapiLoopbackCapture();

        public void StartRecording()
        {
            m_Capture.DataAvailable += (s, a) => {
                RecordedData?.Invoke(s, a);
            };

            m_Capture.RecordingStopped += (s, a) => {
                m_Capture.Dispose();
            };

            m_Capture.StartRecording();
            try
            {
                while (true)
                {
                    Thread.Sleep(10000);
                }
            }
            catch (ThreadInterruptedException)
            {
                Console.WriteLine("Stopping active thread");
                StopRecording();
            }
        }

        public void StopRecording()
        {
            m_Capture.StopRecording();
        }
    }
}
