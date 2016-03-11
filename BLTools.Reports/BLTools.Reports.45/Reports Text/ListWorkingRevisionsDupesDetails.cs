using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CaratFileManagementLib;
using System.Printing;

namespace CaratManagementReports.SmartReports {

  [SmartReport(ReportTypeEnum.Details, "List all dupes from working revisions", PageOrientation.Portrait, ReportDestinationEnum.Text)]
  public class ListWorkingRevisionsDupesDetails : SmartReport, IReportPrint, IReportEmail {

    private TCaratProjectCollection Projects;

    public ListWorkingRevisionsDupesDetails(TCaratProjectCollection projects, string title = "")
      : base() {
      Projects = new TCaratProjectCollection(projects);
      LastResult = "";
      Title = title;
    }

    public override string Execute() {
      StringBuilder NewReport = new StringBuilder();
      string CurrentTitle = string.Format("{0} - {1}", DateTime.Now.ToString("dd/MM/yyyy - HH:mm"), string.IsNullOrWhiteSpace(Title) ? DefaultDescription : Title);
      NewReport.AppendLine(ReportHelper.FullBox(CurrentTitle));
      NewReport.AppendLine();

      StringBuilder Header = new StringBuilder();
      Header.AppendFormat("Id. - {0}", "Name");
      NewReport.AppendLine(ReportHelper.OpenBox(Header.ToString()));

      foreach (TCaratProject ProjectItem in Projects.Where(p => p.ContainsDuped).OrderBy(p => p.ProjectId)) {

        NewReport.AppendFormat("{0}", ProjectItem.ProjectId);
        NewReport.AppendFormat(" - {0}", ProjectItem.Name);
        NewReport.AppendLine();
        NewReport.AppendLine();
        foreach (TCaratFile DupeFileItem in ProjectItem.CaratFiles.Where(f => f.IsDuped)) {
          foreach (TCaratRevision RevisionFileItem in DupeFileItem.WorkingRevisions.OrderBy(r => r.RevisionSymbol)) {
            NewReport.AppendFormat("         - {0}", RevisionFileItem.FullName);
            NewReport.AppendLine();
          }
          NewReport.AppendLine();
        }
        NewReport.AppendLine();
      }

      NewReport.AppendLine();
      NewReport.AppendLine(ReportHelper.DisplayStatistics(Projects));

      LastResult = NewReport.ToString();
      return LastResult;
    }

  }
}
