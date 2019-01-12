﻿namespace EventLogMonitor_GUI
{
    partial class mainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(mainForm));
            this.table = new System.Windows.Forms.DataGridView();
            this.Time = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.EventSource = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.EventID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.EventMsg = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.monitorToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exportLogsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exportXMLToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exportJSONToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.DisplayHookedLogsBtn = new System.Windows.Forms.ToolStripMenuItem();
            ((System.ComponentModel.ISupportInitialize)(this.table)).BeginInit();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // table
            // 
            this.table.AccessibleName = "table";
            this.table.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.table.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.table.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Time,
            this.EventSource,
            this.EventID,
            this.EventMsg});
            this.table.Dock = System.Windows.Forms.DockStyle.Fill;
            this.table.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.table.Location = new System.Drawing.Point(0, 24);
            this.table.Name = "table";
            this.table.Size = new System.Drawing.Size(797, 307);
            this.table.TabIndex = 0;
            // 
            // Time
            // 
            this.Time.HeaderText = "Time";
            this.Time.Name = "Time";
            this.Time.Width = 54;
            // 
            // EventSource
            // 
            this.EventSource.HeaderText = "Event Source";
            this.EventSource.Name = "EventSource";
            this.EventSource.Width = 88;
            // 
            // EventID
            // 
            this.EventID.HeaderText = "EventID";
            this.EventID.Name = "EventID";
            this.EventID.Width = 71;
            // 
            // EventMsg
            // 
            this.EventMsg.HeaderText = "Event Message";
            this.EventMsg.Name = "EventMsg";
            this.EventMsg.Width = 96;
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.monitorToolStripMenuItem,
            this.exportLogsToolStripMenuItem,
            this.DisplayHookedLogsBtn});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(797, 24);
            this.menuStrip1.TabIndex = 1;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // monitorToolStripMenuItem
            // 
            this.monitorToolStripMenuItem.Name = "monitorToolStripMenuItem";
            this.monitorToolStripMenuItem.Size = new System.Drawing.Size(46, 20);
            this.monitorToolStripMenuItem.Text = "Clear";
            this.monitorToolStripMenuItem.Click += new System.EventHandler(this.monitorToolStripMenuItem_Click);
            // 
            // exportLogsToolStripMenuItem
            // 
            this.exportLogsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.exportXMLToolStripMenuItem,
            this.exportJSONToolStripMenuItem});
            this.exportLogsToolStripMenuItem.Name = "exportLogsToolStripMenuItem";
            this.exportLogsToolStripMenuItem.Size = new System.Drawing.Size(80, 20);
            this.exportLogsToolStripMenuItem.Text = "Export Logs";
            // 
            // exportXMLToolStripMenuItem
            // 
            this.exportXMLToolStripMenuItem.Name = "exportXMLToolStripMenuItem";
            this.exportXMLToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.exportXMLToolStripMenuItem.Text = "Export XML";
            this.exportXMLToolStripMenuItem.Click += new System.EventHandler(this.exportXMLToolStripMenuItem_Click);
            // 
            // exportJSONToolStripMenuItem
            // 
            this.exportJSONToolStripMenuItem.Name = "exportJSONToolStripMenuItem";
            this.exportJSONToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.exportJSONToolStripMenuItem.Text = "Export JSON";
            this.exportJSONToolStripMenuItem.Click += new System.EventHandler(this.exportJSONToolStripMenuItem_Click);
            // 
            // DisplayHookedLogsBtn
            // 
            this.DisplayHookedLogsBtn.Enabled = false;
            this.DisplayHookedLogsBtn.Name = "DisplayHookedLogsBtn";
            this.DisplayHookedLogsBtn.Size = new System.Drawing.Size(130, 20);
            this.DisplayHookedLogsBtn.Text = "Display Hooked Logs";
            this.DisplayHookedLogsBtn.Click += new System.EventHandler(this.displayHookedLogsToolStripMenuItem_Click);
            // 
            // mainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(797, 331);
            this.Controls.Add(this.table);
            this.Controls.Add(this.menuStrip1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "mainForm";
            this.Text = "EventLogMonitor";
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.table)).EndInit();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView table;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem monitorToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exportLogsToolStripMenuItem;
        private System.Windows.Forms.DataGridViewTextBoxColumn Time;
        private System.Windows.Forms.DataGridViewTextBoxColumn EventSource;
        private System.Windows.Forms.DataGridViewTextBoxColumn EventID;
        private System.Windows.Forms.DataGridViewTextBoxColumn EventMsg;
        private System.Windows.Forms.ToolStripMenuItem exportXMLToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exportJSONToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem DisplayHookedLogsBtn;
    }
}
