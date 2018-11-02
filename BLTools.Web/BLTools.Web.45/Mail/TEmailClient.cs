using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace BLTools.Web {
  public class TEmailClient {

    internal const string DEFAULT_MAIL_SERVER = "127.0.0.1";
    internal const int DEFAULT_MAIL_PORT = 25;
    internal const string DEFAULT_SENDER = "EmailClient@bltools.priv";

    public static string SessionMailServer {
      get {
        if (string.IsNullOrWhiteSpace(_SessionMailServer)) {
          return DEFAULT_MAIL_SERVER;
        } else {
          return _SessionMailServer;
        }
      }
      set {
        _SessionMailServer = value;
      }
    }
    private static string _SessionMailServer;

    public static string SessionSender {
      get {
        if (string.IsNullOrWhiteSpace(_SessionSender)) {
          return DEFAULT_SENDER;
        } else {
          return _SessionSender;
        }
      }
      set {
        _SessionSender = value;
      }
    }
    private static string _SessionSender;

    public static int SessionMailPort {
      get {
        if (_SessionMailPort == 0) {
          return DEFAULT_MAIL_PORT;
        } else {
          return _SessionMailPort;
        }
      }
      set {
        _SessionMailPort = value;
      }
    }
    private static int _SessionMailPort;

    public readonly List<Attachment> Attachments;
    public bool IsBodyHtml { get;set; }

    public TEmailClient() {
      Attachments = new List<Attachment>();
      IsBodyHtml = false;
    }

    public void SendEmail(string subject, string message, string emailRecipient) {
      SendEmail(subject, message, SessionSender, emailRecipient, "");
    }
    public void SendEmail(string subject, string message, string emailSender, string emailRecipient) {
      SendEmail(subject, message, emailSender, emailRecipient, "");
    }
    public void SendEmail(string subject, string message, string emailSender, string emailRecipient, string emailRecipientCC) {

      if (string.IsNullOrWhiteSpace(emailSender)) {
        emailSender = SessionSender;
      }

      if (string.IsNullOrWhiteSpace(emailRecipient)) {
        return;
      }

      try {
        using (MailMessage Message = new MailMessage(emailSender, emailRecipient)) {
          Message.Subject = subject;
          Message.Body = message;
          Message.IsBodyHtml = IsBodyHtml;
          foreach (Attachment AttachmentItem in Attachments) {
            Message.Attachments.Add(AttachmentItem);
          }
          using (SmtpClient Smtp = new SmtpClient(SessionMailServer, SessionMailPort)) {
            Smtp.Send(Message);
          }
        }
      } catch (Exception ex) {
        Trace.WriteLine(string.Format("Error while sending email to {0} : {1}", emailRecipient, ex.Message));
      }
    }

  }
}
