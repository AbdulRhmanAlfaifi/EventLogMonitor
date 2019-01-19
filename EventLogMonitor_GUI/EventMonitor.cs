﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Windows.Forms;
using System.IO;
using System.Diagnostics.Eventing.Reader;
using System.Xml;

namespace EventLogMonitor_GUI
{
    public partial class mainForm : Form
    {
        // List of strings that contents all successful hooked logs
        List<String> HookedLogs = new List<string>();
        // List that contains all the events captured. This list is used to export the events.
        List<EventRecord> Events = new List<EventRecord>();

        public mainForm()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            table.CellMouseDoubleClick += ShowLogDetails;
            // Gell all the event logs that can be hooked and add there names to "logName" list.
            List<String> logNames = new List<String>();
            foreach (EventLog myEvtLog in EventLog.GetEventLogs())
            {
                logNames.Add(myEvtLog.Log);
            }
            // Add the setup event log to the hooked list.
            logNames.Add("setup");

            // Make a new thread for the log hooking and event capturing.
            Thread LogHookManager = new Thread(delegate ()
            {
                StartEventLogHook(logNames);
            }
            );
            LogHookManager.Start();
            LogHookManager.Name = "LogHookManager";
        }

        private void ShowLogDetails(object sender, DataGridViewCellMouseEventArgs e)
        {
            // If the user double clicked on a cell on the "Event Details" colmun, Then display the details for that event.
            if (e.ColumnIndex == 3)
            {
                MessageBox.Show(table.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString(), "Data", MessageBoxButtons.OK, MessageBoxIcon.None);
            }
        }

        // This function recives a list of event logs to be hooked and add "EventRecordWritten" event for each one.
        private void StartEventLogHook(List<String> logNames)
        {
            AutoResetEvent signal = new AutoResetEvent(false);
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
                    Debug.WriteLine("Faild Registre to : " + logName);
                }
            }
            Debug.WriteLine("Hooked Logs : ");
            foreach (String HookedLog in HookedLogs)
            {
                Debug.WriteLine(HookedLog);
            }

            // enable the "Display Hooked Logs" button when finished hooking.
            Invoke((MethodInvoker)delegate {
                DisplayHookedLogsBtn.Enabled = true;
            });
            // Do not exit from the thread.
            signal.WaitOne();

        }

        // This function get triggered every time an event get writen to a hooked log.
        private void OnEntryWritten(object sender, EventRecordWrittenEventArgs e)
        {
            EventRecord entry = e.EventRecord;
            Events.Add(entry);
            DataGridViewRow row = (DataGridViewRow)table.Rows[0].Clone();

            row.Cells[0].Value = entry.TimeCreated;
            row.Cells[1].Value = entry.ProviderName;
            row.Cells[2].Value = entry.Id;

            // The following code beautify the XML.
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(entry.ToXml());
            StringWriter sw = new StringWriter();
            xmlDoc.Save(sw);
            row.Cells[3].Value = sw.ToString();

            // Add the new row with the event entry data to the table.
            table.Invoke((MethodInvoker)delegate {
                table.Rows.Add(row);
            });
        }

        // This function get triggered when the clear btn get clicked which clears the events :)
        private void monitorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            table.Rows.Clear();
            Events.Clear();
        }

        // Save the events captured to the spicifed path.
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

        // Open dialog for the user to choose where to save the XML report. Then call the function "SaveReportasXML()"
        // to save the report to the chosen path.
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

        // This will get triggered when the "Dispaly Hooked Logs" get clicked. This will display a MessageBox that 
        // contains all the hooked logs.
        private void displayHookedLogs(object sender, EventArgs e)
        {
            String hookedLogs = $"The following is a list of the logs that is being monitored for changes:{Environment.NewLine}{Environment.NewLine}";
            foreach (String logName in HookedLogs)
            {
                hookedLogs += logName + '\n';
            }

            MessageBox.Show(hookedLogs,"Hooked Logs",MessageBoxButtons.OK,MessageBoxIcon.Information);
        }
    }
}
