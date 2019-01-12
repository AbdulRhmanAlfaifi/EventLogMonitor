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

namespace EventLogMonitor_GUI
{
    public partial class mainForm : Form
    {
        List<String> HookedLogs = new List<string>();
        List<EventLogEntry> Events = new List<EventLogEntry>();

        public mainForm()
        {
            InitializeComponent();
            table.CellMouseDoubleClick += Table_CellMouseDoubleClick;
            String[] logNames = Directory.GetFiles("C:\\Windows\\System32\\winevt\\Logs","*.evtx",SearchOption.TopDirectoryOnly);
            String[] logNames_formatted = new String[logNames.Length];
            for (int i = 0;i<logNames.Length;i++)
            {
                logNames_formatted[i] = System.Net.WebUtility.UrlDecode(logNames[i].Split('\\').Last().Split('.')[0]);
            }
            //for (int i = 0; i < logNames_formatted.Length; i++)
            //{
            //    Debug.WriteLine(logNames_formatted[i]);
            //}
            Thread LogHookManager = new Thread(delegate() 
            {
                StartEventLogHook(logNames_formatted);
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
        private void StartEventLogHook(String[] logNames)
        {
            signal = new AutoResetEvent(false);
            foreach (String logName in logNames)
            {
                EventLog myLog = new EventLog(logName);

                // set event handler
                myLog.EntryWritten += new EntryWrittenEventHandler(OnEntryWritten);
                try
                {
                    myLog.EnableRaisingEvents = true;
                    HookedLogs.Add(logName);
                }
                catch (InvalidOperationException e)
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

        private void OnEntryWritten(object sender, EntryWrittenEventArgs e)
        {
            EventLogEntry entry = e.Entry;
            Events.Add(entry);
            DataGridViewRow row = (DataGridViewRow)table.Rows[0].Clone();

            row.Cells[0].Value = entry.TimeGenerated;
            row.Cells[1].Value = entry.Source;
            row.Cells[2].Value = entry.EventID;
            row.Cells[3].Value = entry.Message;
            table.Invoke((MethodInvoker)delegate {
                table.Rows.Add(row);
            });
            //table.Rows.Add(row);
            //signal.Set();
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
            TextWriter tr = new StreamWriter(path);
            tr.Write("<Events>\n");
            tr.Close();
            tr = new StreamWriter(path, true);
            foreach (EventLogEntry Event in Events)
            {
                EventEntry newEntry = new EventEntry();
                newEntry.Category = Event.Category;
                newEntry.CategoryNumber = Event.CategoryNumber;
                newEntry.EntryType = Event.EntryType.ToString();
                newEntry.EventID = Event.InstanceId;
                newEntry.Index = Event.Index;
                newEntry.MachineName = Event.MachineName;
                newEntry.Message = Event.Message;
                newEntry.Source = Event.Source;
                newEntry.Time = Event.TimeGenerated.ToString();
                newEntry.UserName = Event.UserName;
                XmlSerializer ser = new XmlSerializer(typeof(EventEntry));
                ser.Serialize(tr, newEntry);
            }
            tr.Write("\n</Events>");
            tr.Flush();
            tr.Close();
        }

        private void exportXMLToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog sd = new SaveFileDialog();
            if (sd.ShowDialog() == DialogResult.OK)
            {
                SaveReportasXML(sd.FileName);
            }
        }

        private void exportJSONToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog sd = new SaveFileDialog();
            if (sd.ShowDialog() == DialogResult.OK)
            {
                SaveReportasJSON(sd.FileName);
            }
        }

        private void SaveReportasJSON(string path)
        {
            String Results = "[";
            TextWriter tr = new StreamWriter(path);
            foreach (EventLogEntry Event in Events)
            {
                EventEntry newEntry = new EventEntry();
                newEntry.Category = Event.Category;
                newEntry.CategoryNumber = Event.CategoryNumber;
                newEntry.EntryType = Event.EntryType.ToString();
                newEntry.EventID = Event.InstanceId;
                newEntry.Index = Event.Index;
                newEntry.MachineName = Event.MachineName;
                newEntry.Message = Event.Message;
                newEntry.Source = Event.Source;
                newEntry.Time = Event.TimeGenerated.ToString();
                newEntry.UserName = Event.UserName;
                Results += Newtonsoft.Json.JsonConvert.SerializeObject(newEntry) + ",";
            }
            JsonFormatter jf = new JsonFormatter();
            Results = Results.Substring(0, Results.Length - 1)+"]";
            Results = jf.FormatJson(Results);
            tr.Write(Results);
            tr.Flush();
            tr.Close();
        }

        private void displayHookedLogsToolStripMenuItem_Click(object sender, EventArgs e)
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
