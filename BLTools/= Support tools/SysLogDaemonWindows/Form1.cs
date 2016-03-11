using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using SysLog;
using System.Net;
using System.Diagnostics;

namespace SysLogDaemonWindows {
  public partial class Form1 : Form {

    private SyslogServer oServer;
    private delegate void DelegateUpdateText( string textLine, IPAddress sourceIP );
    private DelegateUpdateText DUT;
    SyslogSourceCollection Sources;

    public Form1() {
      InitializeComponent();
    }

    private void Form1_Load( object sender, EventArgs e ) {
      btnListen.Enabled = true;
      btnStop.Enabled = false;
      Sources = new SyslogSourceCollection();
      Sources.Add(new SyslogSource("10.100.200.15", SyslogRecord.SyslogType.VmwareESX));
      Sources.Add(new SyslogSource("10.100.202.1", SyslogRecord.SyslogType.Linksys));
      oServer = new SyslogServer(Sources);
      textBox1.Text = "Stopped";
      DUT = new DelegateUpdateText(UpdateText);
      tabControl1.TabPages.Clear();
    }

    private void oServer_DataReceived( object sender, EventArgs e ) {
      SyslogRecord ReceivedRecord;
      try {
        while ( (ReceivedRecord = oServer.GetData()) != null ) {
          string TextToDisplay = ReceivedRecord.ToString();
          if ( tabControl1.InvokeRequired ) {
            tabControl1.Invoke(DUT, new object[] { TextToDisplay, ReceivedRecord.SourceIP });
          } else {
            UpdateText(ReceivedRecord.ToString(), ReceivedRecord.SourceIP);
          }
        }
      } catch { }
    }

    private void UpdateText( string textLine, IPAddress sourceIP ) {
      string TabName = sourceIP.ToString();
      if ( !tabControl1.TabPages.ContainsKey(TabName) ) {
        TabPage NewTabPage = new TabPage();
        NewTabPage.Name = TabName;
        NewTabPage.Text = TabName;
        RichTextBox NewRtbDisplay = new RichTextBox();
        NewRtbDisplay.Name = TabName;
        NewRtbDisplay.Dock = DockStyle.Fill;
        NewRtbDisplay.Font = new Font("Courier New", 10);
        NewRtbDisplay.WordWrap = false;
        NewRtbDisplay.ReadOnly = true;
        NewTabPage.Controls.Add(NewRtbDisplay);
        tabControl1.TabPages.Add(NewTabPage);
        tabControl1.CreateControl();
      }
      RichTextBox rtbDisplay = (RichTextBox)tabControl1.TabPages[TabName].Controls[TabName];
      rtbDisplay.SuspendLayout();
      rtbDisplay.Text += textLine + Environment.NewLine;
      rtbDisplay.SelectionStart = rtbDisplay.Text.Length;
      rtbDisplay.ScrollToCaret();
      rtbDisplay.ResumeLayout();
    }

    private void btnListen_Click( object sender, EventArgs e ) {
      textBox1.Text = "Listen";
      oServer.DataReceived += new EventHandler(oServer_DataReceived);
      oServer.ListenAsync();
      btnListen.Enabled = false;
      btnStop.Enabled = true;
    }

    private void btnStop_Click( object sender, EventArgs e ) {
      textBox1.Text = "Stopped";
      oServer.DataReceived -= new EventHandler(oServer_DataReceived);
      oServer.AbortListen();
      btnListen.Enabled = true;
      btnStop.Enabled = false;
    }

    private void Form1_FormClosing( object sender, FormClosingEventArgs e ) {
      oServer.DataReceived -= new EventHandler(oServer_DataReceived);
      oServer.AbortListen();
    }
  }
}
