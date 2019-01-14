using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Net;
using System.Xml.Serialization;
using System.Diagnostics.Eventing.Reader;
using System.Xml;

namespace EventLogMonitor_GUI
{
    public partial class mainForm : Form
    {
        List<String> HookedLogs = new List<string>();
        List<EventRecord> Events = new List<EventRecord>();

        public mainForm()
        {
            InitializeComponent();
            table.CellMouseDoubleClick += Table_CellMouseDoubleClick;
            List<String> logNames = new List<String>();
            foreach (EventLog myEvtLog in EventLog.GetEventLogs())
            {
                logNames.Add(myEvtLog.Log);
            }
            logNames.Add("setup");

            Thread LogHookManager = new Thread(delegate() 
            {
                StartEventLogHook(logNames);
            }
            );
            LogHookManager.Start();
            LogHookManager.Name = "LogHookManager";

        }

        private void Table_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.ColumnIndex == 3)
            {
                DialogResult res = MessageBox.Show(table.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString(), "Data", MessageBoxButtons.OK, MessageBoxIcon.None);
            }
        }

        static AutoResetEvent signal;
        private void StartEventLogHook(List<String> logNames)
        {
            signal = new AutoResetEvent(false);
            foreach (String logName in logNames)
            {
                try
                {
                    EventLogQuery query = new EventLogQuery(logName, PathType.LogName);
                    EventLogWatcher ew = new EventLogWatcher(query);
                    ew.EventRecordWritten += new EventHandler<EventRecordWrittenEventArgs>(OnEntryWritten);
                    ew.Enabled = true;
                    Debug.WriteLine("Registred to : " + logName);
                    HookedLogs.Add(logName);
                }
                catch (Exception e)
                {
                    
                }
            }
            Debug.WriteLine("Hooked Logs : ");
            foreach (String HookedLog in HookedLogs)
            {
                Debug.WriteLine(HookedLog);
            }
            Invoke((MethodInvoker)delegate {
                mainForm.ActiveForm.Icon = EventLogMonitor_GUI.Properties.Resources.GreenDot;
                DisplayHookedLogsBtn.Enabled = true;
            });
            signal.WaitOne();

        }

        private void OnEntryWritten(object sender, EventRecordWrittenEventArgs e)
        {
            EventRecord entry = e.EventRecord;
            Events.Add(entry);
            DataGridViewRow row = (DataGridViewRow)table.Rows[0].Clone();

            row.Cells[0].Value = entry.TimeCreated;
            row.Cells[1].Value = entry.ProviderName;
            row.Cells[2].Value = entry.Id;

            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(entry.ToXml());
            StringWriter sw = new StringWriter();
            xmlDoc.Save(sw);
            row.Cells[3].Value = sw.ToString();
            table.Invoke((MethodInvoker)delegate {
                table.Rows.Add(row);
            });
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void monitorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            table.Rows.Clear();
            Events.Clear();
        }


        private void SaveReportasXML(String path)
        {
            String Results = "<Events>";
            TextWriter tr = new StreamWriter(path);
            foreach (EventRecord Event in Events)
            {
                Results += Event.ToXml();
            }
            Results += "</Events>";

            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(Results);
            xmlDoc.Save(tr);
            tr.Flush();
            tr.Close();
        }

        private void exportXML(object sender, EventArgs e)
        {
            SaveFileDialog sd = new SaveFileDialog();
            if (sd.ShowDialog() == DialogResult.OK)
            {
                SaveReportasXML(sd.FileName);
            }
        }

        private void SaveReportasJSON(string path)
        {
            //TODO: Implement this function (Low Priority)
        }

        private void displayHookedLogs(object sender, EventArgs e)
        {
            String hookedLogs = "The following is a list of the logs that is being monitored for changes:\n\n";
            foreach (String logName in HookedLogs)
            {
                hookedLogs += logName + '\n';
            }

            MessageBox.Show(hookedLogs,"Hooked Logs",MessageBoxButtons.OK,MessageBoxIcon.Information);
        }
    }
}
