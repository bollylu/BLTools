using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CaratFileManagementLib;
using System.Printing;

namespace CaratManagementReports {
  public static class ReportTable {

    [TCaratReport(ReportTypeEnum.Table, "List all projects", PageOrientation.Landscape)]
    public static string ProjectsListTable(TCaratProjectCollection projects, string title="") {
      StringBuilder NewReport = new StringBuilder();

      string Title = string.Format("{0} - {1}", DateTime.Now.ToString("dd/MM/yyyy - HH:mm"), string.IsNullOrWhiteSpace(title) ? "List of projects" : title);
      NewReport.AppendLine(ReportHelpers.FullBox(Title));
      NewReport.AppendLine();

      StringBuilder Header = new StringBuilder();
      Header.Append("Id.");
      Header.AppendFormat(" | {0}", "Name".PadRight(100, '.'));
      Header.AppendFormat(" | {0}", "Files".PadRight(8, '.'));
      Header.AppendFormat(" | {0}", "Ok".PadRight(8, '.'));
      Header.AppendFormat(" | {0}", "Bad".PadRight(8, '.'));
      Header.AppendFormat(" | {0}", "Dupes".PadRight(8, '.'));

      NewReport.AppendLine(ReportHelpers.OpenBox(Header.ToString()));

      foreach (TCaratProject ProjectItem in projects.OrderBy(p => p.ProjectId)) {
        NewReport.AppendFormat("{0}", ProjectItem.ProjectId);
        NewReport.AppendFormat(" | {0}", ProjectItem.Name.PadRight(100, '.'));
        NewReport.AppendFormat(" | {0}", ProjectItem.CaratFiles.Count.ToString().PadLeft(8, '.'));
        NewReport.AppendFormat(" | {0}", ProjectItem.ValidCaratFilesCount.ToString().PadLeft(8, '.'));
        NewReport.AppendFormat(" | {0}", ProjectItem.InvalidCaratFilesCount.ToString().PadLeft(8, '.'));
        NewReport.AppendFormat(" | {0}", ProjectItem.DupesCount.ToString().PadLeft(8, '.'));
        NewReport.AppendLine();
      }

      NewReport.AppendLine();
      NewReport.AppendLine("Statistics");
      NewReport.AppendLine("----------");
      NewReport.AppendFormat("Total of projects : {0}\r\n", projects.Count);
      NewReport.AppendFormat("Total of valid files : {0}\r\n", projects.Sum(p => p.ValidCaratFilesCount));
      NewReport.AppendFormat("Total of invalid files : {0}\r\n", projects.Sum(p => p.InvalidCaratFilesCount));
      NewReport.AppendFormat("Total of duplicate files : {0}\r\n", projects.Sum(p => p.DupesCount));

      return NewReport.ToString();
    }

    [TCaratReport(ReportTypeEnum.Table, "List projects containing dupes", PageOrientation.Landscape)]
    public static string ProjectsListTableDupesOnly(TCaratProjectCollection projects, string title = "") {
      return ReportTable.ProjectsListTable(new TCaratProjectCollection(projects.Where(p => p.ContainsDuped)), title);
    }

    [TCaratReport(ReportTypeEnum.Table, "List projects not containing dupes", PageOrientation.Landscape)]
    public static string ProjectsListTableWithoutDupes(TCaratProjectCollection projects, string title = "") {
      return ReportTable.ProjectsListTable(new TCaratProjectCollection(projects.Where(p => !p.ContainsDuped)), title);
    }
  }
}
