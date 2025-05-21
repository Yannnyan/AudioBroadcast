using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AudioBroadcast
{
    public class Temperment
    {
        private readonly Repeater m_Repeater;
        public Temperment() {
            this.m_Repeater = new Repeater();
        }

        public void StartJob()
        {
            m_Repeater.Start();
            Console.WriteLine("Recording... press any key to stop.");
            Console.ReadKey();
            m_Repeater.Stop();
        }

    }
}
