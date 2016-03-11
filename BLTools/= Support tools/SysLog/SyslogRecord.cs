using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Net;

namespace SysLog {
  public class SyslogRecord {
    #region Enums
    /// <summary>
    /// Facility  according to http://www.ietf.org/rfc/rfc3164.txt 4.1.1 PRI Part
    /// </summary>
    public enum FacilityEnum : int {
      unknown = -1,
      kernel = 0,	// kernel messages
      user = 1,	// user-level messages
      mail = 2,	// mail system
      system = 3,	// system daemons
      security = 4,	// security/authorization messages (note 1)
      syslogd = 5,	// messages generated internally by syslogd
      printer = 6,	// line printer subsystem
      news = 7,	// network news subsystem
      uucp = 8,	// UUCP subsystem
      clock = 9,	// clock daemon (note 2) changed to cron
      security2 = 10,	// security/authorization messages (note 1)
      ftp = 11,	// FTP daemon
      ntp = 12,	// NTP subsystem
      audit = 13,	// log audit (note 1)
      alert = 14,	// log alert (note 1)
      clock2 = 15,	// clock daemon (note 2)
      local0 = 16,	// local use 0  (local0)
      local1 = 17,	// local use 1  (local1)
      local2 = 18,	// local use 2  (local2)
      local3 = 19,	// local use 3  (local3)
      local4 = 20,	// local use 4  (local4)
      local5 = 21,	// local use 5  (local5)
      local6 = 22,	// local use 6  (local6)
      local7 = 23	// local use 7  (local7)
    }
    /// <summary>
    /// Severity  according to http://www.ietf.org/rfc/rfc3164.txt 4.1.1 PRI Part
    /// </summary>
    public enum SeverityEnum : int {
      unknown = -1,
      emergency = 0,	// Emergency: system is unusable
      alert = 1,	// Alert: action must be taken immediately
      critical = 2,	// Critical: critical conditions
      error = 3,	// Error: error conditions
      warning = 4,	// Warning: warning conditions
      notice = 5,	// Notice: normal but significant condition
      info = 6,	// Informational: informational messages
      debug = 7	// Debug: debug-level messages
    }

    public enum SyslogType : int {
      unknown = -1,
      RFC3164 = 0,
      BSD = 1,
      Linksys = 2,
      VmwareESX = 3
    }
    #endregion Enums

    #region Inner classes
    public class FieldPRI {
      public FacilityEnum Facility { get; set; }
      public SeverityEnum Severity { get; set; }
      public FieldPRI( string strPri ) {
        try {
          int intPri = Convert.ToInt32(strPri);
          int intFacility = intPri >> 3;
          int intSeverity = intPri & 0x7;
          this.Facility = (FacilityEnum)Enum.Parse(typeof(FacilityEnum), intFacility.ToString());
          this.Severity = (SeverityEnum)Enum.Parse(typeof(SeverityEnum), intSeverity.ToString());
        } catch {
          Facility = FacilityEnum.unknown;
          Severity = SeverityEnum.unknown;
        }
      }
      public FieldPRI( FacilityEnum facility, SeverityEnum severity ) {
        Facility = facility;
        Severity = severity;
      }

      public override string ToString() {
        return string.Format("{0}.{1}", this.Facility, this.Severity);
      }
    }
    public class FieldHeader {
      public string Hostname { get; set; }
      public DateTime Timestamp { get; set; }
      public FieldHeader( string timestamp, string hostname ) {
        Timestamp = DateTime.Parse(string.Format("{0} {1}", DateTime.Today.Year, timestamp));
        Hostname = hostname;
      }
      public FieldHeader( DateTime timestamp, string hostname ) {
        Timestamp = timestamp;
        Hostname = hostname;
      }
    }
    public class FieldMsg {
      public string Tag { get; set; }
      public string Message { get; set; }
      public FieldMsg( string message ) {
        Tag = "";
        Message = message;
      }
      public override string ToString() {
        return Message;
      }
    } 
    #endregion Inner classes

    #region Public properties
    public IPAddress SourceIP { get; set; }
    public FieldPRI PRI { get; set; }
    public FieldHeader Header { get; set; }
    public FieldMsg Msg { get; set; } 
    #endregion Public properties

    #region Constructor(s)
    public SyslogRecord( string syslogRawRecord ) : this(syslogRawRecord, IPAddress.Loopback) { }
    public SyslogRecord( string syslogRawRecord, IPAddress sourceIP ) : this(syslogRawRecord, IPAddress.Loopback, SyslogType.RFC3164) { }
    public SyslogRecord( string syslogRawRecord, IPAddress sourceIP, SyslogType syslogType ) {
      Match SplitParts;
      SplitParts = Regex.Match(syslogRawRecord, "^<(?'PRI'.*?)>.*$");
      PRI = new FieldPRI(SplitParts.Groups["PRI"].Value);
      switch ( syslogType ) {
        case SyslogType.Linksys:
          SplitParts = Regex.Match(syslogRawRecord, "^<(?'PRI'.*?)>(?'TS'...\\s[\\s\\d]\\d\\s[\\s\\d]\\d:[\\s\\d]\\d:[\\s\\d]\\d?)\\s(?'MSG'.*$?)");
          Header = new FieldHeader(SplitParts.Groups["TS"].Value, "");
          Msg = new FieldMsg(SplitParts.Groups["MSG"].Value);
          break;
        case SyslogType.VmwareESX:
          Header = new FieldHeader(DateTime.Now, sourceIP.ToString());
          Msg = new FieldMsg(syslogRawRecord.Substring(syslogRawRecord.IndexOf('>') + 1));
          break;
        case SyslogType.RFC3164:
        case SyslogType.BSD:
        default:
          SplitParts = Regex.Match(syslogRawRecord, "^<(?'PRI'.*?)>(?'TS'...\\s[\\s\\d]\\d\\s[\\s\\d]\\d:[\\s\\d]\\d:[\\s\\d]\\d?)\\s(?'HOST'.*?)\\s(?'MSG'.*$?)");
          try {
            Header = new FieldHeader(SplitParts.Groups["TS"].Value, SplitParts.Groups["HOST"].Value);
          } catch {
            Header = new FieldHeader(DateTime.Today.ToString("MMM dd HH:mm:ss"), "(unknown)");
          }
          try {
            Msg = new FieldMsg(SplitParts.Groups["MSG"].Value);
          } catch {
            Msg = new FieldMsg(syslogRawRecord);
          }
          break;
      }
      SourceIP = sourceIP;
    } 
    #endregion Constructor(s)

    #region Public methods
    public override string ToString() {
      StringBuilder RetVal = new StringBuilder();
      if ( SourceIP != null ) {
        RetVal.Append(SourceIP.ToString().PadRight(16,'.'));
      }
      RetVal.AppendFormat(" {0}.{1} {2} : {3} : {4}", PRI.Facility.ToString().PadRight(10, '.'), PRI.Severity.ToString().PadRight(10, '.'), Header.Timestamp.ToString("yyyy-MM-dd HH:mm:ss"), Header.Hostname.PadRight(20, '.'), Msg.ToString());
      return RetVal.ToString();
    } 
    #endregion Public methods
  }
}
