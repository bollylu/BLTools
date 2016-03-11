using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CaratFileManagementLib;
using System.Printing;

namespace CaratManagementReports {
  class ReportDetails {

    [TCaratReport(ReportTypeEnum.Details, "List all dupes from working revisions", PageOrientation.Portrait)]
    public static string ProjectsDetailsListDupesWorkingRevisions(TCaratProjectCollection projects, string title = "") {
      StringBuilder NewReport = new StringBuilder();
      string Title = string.Format("{0} - {1}", DateTime.Now.ToString("dd/MM/yyyy - HH:mm"), string.IsNullOrWhiteSpace(title) ? "List of projects with dupes files details from working revisions" : title);
      NewReport.AppendLine(ReportHelpers.FullBox(Title));
      NewReport.AppendLine();

      StringBuilder Header = new StringBuilder();
      Header.Append("Id.");
      Header.AppendFormat(" - {0}", "Name");
      NewReport.AppendLine(ReportHelpers.OpenBox(Header.ToString()));

      foreach (TCaratProject ProjectItem in projects.Where(p => p.ContainsDuped).OrderBy(p => p.ProjectId)) {

        NewReport.AppendFormat("{0}", ProjectItem.ProjectId);
        NewReport.AppendFormat(" - {0}", ProjectItem.Name);
        NewReport.AppendLine();
        NewReport.AppendLine();
        foreach (TCaratFile DupeFileItem in ProjectItem.CaratFiles.Where(f => f.IsDuped)) {
          foreach (TCaratRevision RevisionFileItem in DupeFileItem.WorkingRevisions.OrderBy(r=>r.RevisionSymbol)) {
            NewReport.AppendFormat("         - {0}", RevisionFileItem.FullName);
            NewReport.AppendLine();
          }
          NewReport.AppendLine();
        }
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

    [TCaratReport(ReportTypeEnum.Details, "List all dupes", PageOrientation.Portrait)]
    public static string ProjectsDetailsListDupes(TCaratProjectCollection projects, string title = "") {
      StringBuilder NewReport = new StringBuilder();
      string Title = string.Format("{0} - {1}", DateTime.Now.ToString("dd/MM/yyyy - HH:mm"), string.IsNullOrWhiteSpace(title) ? "List of projects with dupes files details" : title);
      NewReport.AppendLine(ReportHelpers.FullBox(Title));
      NewReport.AppendLine();

      StringBuilder Header = new StringBuilder();
      Header.Append("Id.");
      Header.AppendFormat(" - {0}", "Name");
      NewReport.AppendLine(ReportHelpers.OpenBox(Header.ToString()));

      foreach (TCaratProject ProjectItem in projects.Where(p => p.ContainsDuped).OrderBy(p => p.ProjectId)) {

        NewReport.AppendFormat("{0}", ProjectItem.ProjectId);
        NewReport.AppendFormat(" - {0}", ProjectItem.Name);
        NewReport.AppendLine();
        NewReport.AppendLine();
        foreach (TCaratFile DupeFileItem in ProjectItem.CaratFiles.Where(f => f.IsDuped)) {
          foreach (TCaratRevision RevisionFileItem in DupeFileItem.Revisions.OrderBy(r => r.RevisionSymbol)) {
            NewReport.AppendFormat("         - {0}", RevisionFileItem.FullName);
            NewReport.AppendLine();
          }
          NewReport.AppendLine();
        }
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


  }
}
