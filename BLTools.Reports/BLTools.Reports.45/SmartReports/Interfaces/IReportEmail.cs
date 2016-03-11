using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BLTools.Reports {
  public interface IReportEmail {
    bool SendTo(string from, string to, string subject, string smtpServer, int smtpPort);
  }
}
