using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Reflection;
using System.Printing;
using System.Diagnostics;
using BLTools;
using System.Net.Mail;

namespace BLTools.Reports {
  public abstract class TSmartReport : IReportSave {

    public static TSmartReportMetadataCollection AvailableReports { get; private set; }

    #region Public properties
    public string LastResult { get; protected set; }
    public string Title { get; protected set; }
    public string Name {
      get {
        return this.GetType().Name;
      }
    }
    public string Fullname {
      get {
        return this.GetType().FullName;
      }
    }
    public string Description { get; protected set; }
    protected string DefaultDescription { get; private set; }
    public EReportDestination Destination { get; protected set; }
    public EReportType ReportType { get; protected set; }
    public PageOrientation Orientation { get; protected set; }
    #endregion Public properties

    #region Private variables
    private WebPrint WPrinter;
    #endregion Private variables

    #region Constructor(s)
    static TSmartReport() {
      AvailableReports = new TSmartReportMetadataCollection();
      IEnumerable<Type> SmartReportClasses = Assembly.GetEntryAssembly().GetTypes().Where(x => x.BaseType == typeof(TSmartReport));

      foreach (Type TypeItem in SmartReportClasses) {
        object[] Attributes = TypeItem.GetCustomAttributes(typeof(TSmartReportAttribute), true);
        if (Attributes.Length > 0) {
          TSmartReportAttribute CurrentSmartReportAttribute = Attributes[0] as TSmartReportAttribute;
          string SmartReportName = string.Format("{0}.{1}", TypeItem.Namespace, TypeItem.Name);
          string SmartReportDescription = CurrentSmartReportAttribute.Description;
          EReportType SmartReportReportType = CurrentSmartReportAttribute.ReportType;
          EReportDestination SmartReportDestination = CurrentSmartReportAttribute.Destination;
          PageOrientation SmartReportOrientation = CurrentSmartReportAttribute.Orientation;
          TSmartReportMetadata Metadata = new TSmartReportMetadata(SmartReportName, SmartReportDescription, SmartReportDestination, SmartReportReportType, SmartReportOrientation);
          Metadata.Report = TypeItem;
          AvailableReports.Add(Metadata);
        }
      }

    }
    public TSmartReport() {
      object[] Attributes = this.GetType().GetCustomAttributes(typeof(TSmartReportAttribute), true);
      if (Attributes.Length > 0) {
        TSmartReportAttribute CurrentSmartReportAttribute = Attributes[0] as TSmartReportAttribute;
        Description = CurrentSmartReportAttribute.Description;
        DefaultDescription = CurrentSmartReportAttribute.Description;
        ReportType = CurrentSmartReportAttribute.ReportType;
        Destination = CurrentSmartReportAttribute.Destination;
        Orientation = CurrentSmartReportAttribute.Orientation;
      }
    }
    #endregion Constructor(s)


    public static TSmartReport Factory<D>(string reportname, D dataSource, string title="") {
      Type RequestedReport = AvailableReports[reportname].Report;
      
      return Factory<D>(RequestedReport, dataSource, title);
    }
    public static TSmartReport Factory<D>(Type T, D dataSource, string title = "") {
      Type RequestedReport = T;
      ConstructorInfo Ctor = RequestedReport.GetConstructor(new Type[] { typeof(D), typeof(string) });
      return (TSmartReport)Ctor.Invoke(new object[] { dataSource, title });
    }

    public abstract string Execute();

    #region IReportSave
    public virtual string Save() {
      System.Windows.Forms.SaveFileDialog SFD = new System.Windows.Forms.SaveFileDialog();
      SFD.AddExtension = true;
      switch (Destination) {
        case EReportDestination.Text:
          SFD.Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*";
          break;
        case EReportDestination.Html:
          SFD.Filter = "Html files (*.htm;*.html)|*.htm;*.html|All files (*.*)|*.*";
          break;
      }
      if (SFD.ShowDialog() == System.Windows.Forms.DialogResult.OK) {
        return Save(SFD.FileName);
      }
      return null;
    }
    public virtual string Save(string filename) {
      try {
        File.WriteAllText(filename, LastResult);
        return filename;
      } catch (Exception ex) {
        Trace.WriteLine(string.Format("Unable to save report at \"{0}\" : {1}", filename, ex.Message));
        return null;
      }
    }
    #endregion IReportSave

    #region IReportEmail
    public virtual bool SendTo(string from = "", string to = "", string subject = "Test", string smtpServer = "127.0.0.1", int smtpPort = 25) {
      if (string.IsNullOrWhiteSpace(smtpServer)) {
        string Msg = "SMTP server is missing, please verify parameters";
        Trace.WriteLine(Msg, Severity.Error);
        return false;
      }

      MailAddress AddressTo = new MailAddress(to);
      MailAddress AddressFrom = new MailAddress(from);
      MailMessage NewMailMessage = new MailMessage(AddressFrom, AddressTo);
      NewMailMessage.IsBodyHtml = true;
      NewMailMessage.Body = (Destination == EReportDestination.Html ? LastResult : FixedFontHtml(LastResult));
      NewMailMessage.Subject = subject;

      SmtpClient NewSmtpClient = new SmtpClient();
      NewSmtpClient.Host = smtpServer;
      NewSmtpClient.Port = smtpPort;

      try {
        NewSmtpClient.Send(NewMailMessage);
      } catch (Exception ex) {
        string Msg = string.Format("Unable to send message : {0}", ex.Message);
        Trace.WriteLine(Msg);
        return false;
      }

      return true;
    }

    protected string FixedFontHtml(string source) {
      StringBuilder RetVal = new StringBuilder();
      RetVal.AppendLine("<HTML>");
      RetVal.AppendLine("<HEAD>");
      RetVal.AppendLine("<meta http-equiv='Content-Type' content='text/html; charset=UTF-8'/>");
      RetVal.AppendLine("</HEAD>");
      RetVal.AppendLine("<BODY>");
      RetVal.AppendLine(string.Format("<PRE>{0}</PRE>", source));
      RetVal.AppendLine("</BODY>");
      RetVal.AppendLine("</HTML>");
      return RetVal.ToString();
    }
    #endregion IReportEmail

    #region IReportPrint
    public event EventHandler OnPrintDone;

    public virtual void Print(bool preview = true) {
      switch (Destination) {
        case EReportDestination.Text:
          SimplifiedPrinting.Orientation = Orientation;
          SimplifiedPrinting.Print(LastResult, "First print");
          break;
        case EReportDestination.Html:
          if (WPrinter != null) {
            WPrinter.Close();
          }
          WPrinter = new WebPrint();
          WPrinter.OnPrintDone += new EventHandler(WPrinter_OnPrintDone);
          WPrinter.StartPrint(LastResult, preview);
          break;
      }
    }
    private void WPrinter_OnPrintDone(object sender, EventArgs e) {
      WPrinter.OnPrintDone -= new EventHandler(WPrinter_OnPrintDone);
      WPrinter.Close();
      if (OnPrintDone != null) {
        OnPrintDone(this, EventArgs.Empty);
      }
    }
    #endregion IReportPrint
  }
}
