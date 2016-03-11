using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SysLog;
using BLTools;
using BLTools.ConsoleExtension;
using System.Net;

namespace SysLogDaemonConsole {
  class Program {

    static SyslogServer oServer;

    static void Main( string[] args ) {
      //SyslogRecord ReceivedRecord = new SyslogRecord("<13>Oct 11 22:14:15 lucwks2 Message d'erreur");
      //Console.WriteLine(ReceivedRecord.ToString());
      //Console.WriteLine(new SyslogRecord(""));
      //ConsoleExtension.Pause();

      SyslogSourceCollection SyslogSources = new SyslogSourceCollection();
      SyslogSources.Add(new SyslogSource("10.100.200.15", SyslogRecord.SyslogType.VmwareESX));
      SyslogSources.Add(new SyslogSource("10.100.202.1", SyslogRecord.SyslogType.Linksys));
      oServer = new SyslogServer(SyslogSources);
      oServer.DataReceived += new EventHandler(oServer_DataReceived);

      oServer.ListenAsync();

      ConsoleExtension.Pause();

      oServer.AbortListen();

    }

    static private void oServer_DataReceived( object sender, EventArgs e ) {
      SyslogRecord ReceivedRecord = oServer.GetData();
      if ( ReceivedRecord != null ) {
        Console.WriteLine(ReceivedRecord.ToString());
      }
    }
  }
}
