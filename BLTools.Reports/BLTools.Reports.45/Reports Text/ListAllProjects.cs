using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CaratFileManagementLib;
using System.Printing;

namespace CaratManagementReports.SmartReports {

  [SmartReport(ReportTypeEnum.Table, "List all projects", PageOrientation.Landscape, ReportDestinationEnum.Text)]
  public class ListAllProjects : SmartReport, IReportPrint, IReportEmail {

    private TCaratProjectCollection Projects;

    public ListAllProjects(TCaratProjectCollection projects, string title = "") : base() {
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
      Header.Append("Id.");
      Header.AppendFormat(" | {0}", "Name".PadRight(100, '.'));
      Header.AppendFormat(" | {0}", "Files".PadRight(8, '.'));
      Header.AppendFormat(" | {0}", "Ok".PadRight(8, '.'));
      Header.AppendFormat(" | {0}", "Bad".PadRight(8, '.'));
      Header.AppendFormat(" | {0}", "Dupes".PadRight(8, '.'));

      NewReport.AppendLine(ReportHelper.OpenBox(Header.ToString()));

      foreach (TCaratProject ProjectItem in Projects.OrderBy(p => p.ProjectId)) {
        NewReport.AppendFormat("{0}", ProjectItem.ProjectId);
        NewReport.AppendFormat(" | {0}", ProjectItem.Name.PadRight(100, '.'));
        NewReport.AppendFormat(" | {0}", ProjectItem.CaratFiles.Count.ToString().PadLeft(8, '.'));
        NewReport.AppendFormat(" | {0}", ProjectItem.ValidCaratFilesCount.ToString().PadLeft(8, '.'));
        NewReport.AppendFormat(" | {0}", ProjectItem.InvalidCaratFilesCount.ToString().PadLeft(8, '.'));
        NewReport.AppendFormat(" | {0}", ProjectItem.DupesCount.ToString().PadLeft(8, '.'));
        NewReport.AppendLine();
      }

      NewReport.AppendLine();
      NewReport.AppendLine(ReportHelper.DisplayStatistics(Projects));

      LastResult = NewReport.ToString();
      return LastResult;
    }

  }
}
