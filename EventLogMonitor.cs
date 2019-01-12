using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace EventLogMonitor
{
    class Program
    {
        static AutoResetEvent signal;
        static void Main(string[] args)
        {
            signal = new AutoResetEvent(false);
            EventLog myLog = new EventLog("Windows Powershell");

            // set event handler
            myLog.EntryWritten += new EntryWrittenEventHandler(OnEntryWritten);
            myLog.EnableRaisingEvents = true;
            signal.WaitOne();
        }

        private static void OnEntryWritten(object sender, EntryWrittenEventArgs e)
        {
            //TextWriter writer = new StreamWriter("TEST.xml");
            XmlSerializer ser = new XmlSerializer(e.Entry.GetType());
            ser.Serialize(Console.Out, e);
            //signal.Set();
        }
    }
}
