using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Globalization;
using System.Threading;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;

namespace BLTools {

  public class SyslogTraceListeners : DefaultTraceListener {

    public IPAddress TargetIP {
      get {
        return _TargetIP;
      }
      set {
        _TargetIP = value;
        TargetIPEndPoint = new IPEndPoint(_TargetIP, _TargetPort);
        Name = string.Format("{0}:{1}", _TargetIP.ToString(), _TargetPort);
      }
    }
    private IPAddress _TargetIP;

    public int TargetPort {
      get {
        return _TargetPort;
      }
      set {
        if ( value < 1 || value > 65535 ) {
          Trace.WriteLine(string.Format("Syslog port is out of range : {0} : default value 514 is used instead", value), Severity.Warning);
          _TargetPort = 514;
        } else {
          _TargetPort = value;
        }
        TargetIPEndPoint = new IPEndPoint(_TargetIP, _TargetPort);
        Name = string.Format("{0}:{1}", _TargetIP.ToString(), _TargetPort);
      }
    }
    private int _TargetPort;
    private const int DEFAULT_SYSLOG_PORT = 514;

    private UdpClient CurrentUdpClient;
    private IPEndPoint TargetIPEndPoint;

    #region Constructor(s)
    public SyslogTraceListeners() {
      TargetIP = IPAddress.Loopback;
      TargetPort = DEFAULT_SYSLOG_PORT;
      CurrentUdpClient = new UdpClient();
    }

    public SyslogTraceListeners(int targetPort)
      : this() {
      TargetPort = targetPort;
    }

    public SyslogTraceListeners(string hostname)
      : this() {
      TargetIP = Dns.GetHostAddresses(hostname).Where(i => i.AddressFamily == AddressFamily.InterNetwork).First();
    }

    public SyslogTraceListeners(string hostname, int targetPort)
      : this() {
      TargetPort = targetPort;
      TargetIP = Dns.GetHostAddresses(hostname).Where(i => i.AddressFamily == AddressFamily.InterNetwork).First();
    }
    public SyslogTraceListeners(IPAddress targetAddress)
      : this() {
      TargetIP = targetAddress;
    }

    public SyslogTraceListeners(IPAddress targetAddress, int targetPort)
      : this() {
      TargetPort = targetPort;
      TargetIP = targetAddress;
    }
    #endregion Constructor(s)

    public override void WriteLine(string textLine) {
      Write(textLine);
    }
    public void Write(string textLine, int syslogSeverity, int syslogFacility) {
      StringBuilder sbHeader = new StringBuilder();
      int PRI = syslogFacility * 8 + syslogSeverity;
      sbHeader.AppendFormat("<{0}>", PRI.ToString("0##"));
      sbHeader.AppendFormat("{0} ", DateTime.Now.ToString("MMM dd HH:mm:ss", CultureInfo.GetCultureInfo("en-us").DateTimeFormat));
      sbHeader.AppendFormat("{0} ", Dns.GetHostName().ToLower());
      while ( textLine.Length > 0 ) {
        if ( textLine.IndexOfAny(new char[] { ' ' }) > 32 || textLine.IndexOfAny(new char[] { ' ' }) < 0 ) {
          textLine = textLine.Substring(0, 32) + " " + textLine.Substring(32);
        }
        string sBuffer = string.Concat(sbHeader.ToString(), textLine.Substring(0, Math.Min(textLine.Length, 950)));

        //Console.WriteLine(sBuffer.Length);
        //byte[] bBuffer = new byte[sBuffer.Length];
        byte[] bBuffer = Encoding.ASCII.GetBytes(sBuffer);
        //for ( int i = 0; i < bBuffer.Length; i++ ) {
        //  bBuffer[i] = (byte)sBuffer[i];
        //}
        CurrentUdpClient.Send(bBuffer, bBuffer.Length, TargetIPEndPoint);
        if ( textLine.Length > 950 ) {
          textLine = textLine.Substring(950);
        } else {
          textLine = "";
        }
      }
    }
    public override void Write(string sValue) {
      Write(sValue, 6, 1);
    }
    public void Write(string sValue, int iSeverity) {
      Write(sValue, iSeverity, 1);
    }
    public override void Close() {
      if ( CurrentUdpClient != null ) {
        CurrentUdpClient.Close();
      }
      base.Close();
    }

  }
}
