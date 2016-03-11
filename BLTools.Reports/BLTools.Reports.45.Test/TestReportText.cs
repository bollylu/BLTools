using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BLTools.Reports;
using BLTools;
using BLTools.Debugging;
using BLTools.Text;
using System.Printing;

namespace BLTools._45.Reports.Test {

  [TSmartReport(EReportType.Table, "List all names", PageOrientation.Portrait, EReportDestination.Text)]
  public class ListAllNames : TSmartReport, IReportPrint, IReportEmail {

    private List<string> Names;

    public ListAllNames(IEnumerable<string> names, string title = "")
      : base() {
      Names = new List<string>(names);
      LastResult = "";
      Title = title;
    }

    public override string Execute() {
      StringBuilder NewReport = new StringBuilder();

      string CurrentTitle = string.Format("{0} - {1}", DateTime.Now.ToString("dd/MM/yyyy - HH:mm"), string.IsNullOrWhiteSpace(Title) ? DefaultDescription : Title);
      NewReport.AppendLine(TextBox.BuildDynamic(CurrentTitle));
      NewReport.AppendLine();

      StringBuilder Header = new StringBuilder();
      Header.Append("Id.");
      Header.AppendFormat(" | {0}", "Name".PadRight(20, '.'));
      NewReport.AppendLine(TextBox.BuildDynamic(Header.ToString()));

      int i = 0;
      foreach (string StringItem in Names.OrderBy(p => p)) {
        NewReport.AppendFormat("{0}. ", i++);
        NewReport.AppendFormat("{0}", StringItem);
        NewReport.AppendLine();
      }

      NewReport.AppendLine();

      LastResult = NewReport.ToString();
      return LastResult;
    }



    
  }
}
