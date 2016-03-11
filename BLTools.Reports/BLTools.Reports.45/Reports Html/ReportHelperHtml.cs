using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CaratManagementReports {
  public static class ReportHelperHtml {
    public static string BuildHead(string title) {
      StringBuilder RetVal = new StringBuilder();
      RetVal.AppendLine("<HEAD>");
      RetVal.AppendLine("<meta http-equiv='Content-Type' content='text/html; charset=UTF-8'/>");
      RetVal.AppendLine(string.Format("<title>{0}</title>", title));
      RetVal.AppendLine("</HEAD>");
      return RetVal.ToString();
    }
  }
}
