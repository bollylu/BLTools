using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CaratFileManagementLib;
using System.Printing;

namespace CaratManagementReports.SmartReports {

  [SmartReport(ReportTypeEnum.Table, "List projects with no dupes", PageOrientation.Landscape, ReportDestinationEnum.Text)]
  public class ListProjectsWithNoDupes : SmartReport, IReportPrint, IReportEmail {

    private TCaratProjectCollection Projects;

    public ListProjectsWithNoDupes(TCaratProjectCollection projects, string title = "")
      : base() {
      Projects = new TCaratProjectCollection(projects);
      LastResult = "";
      Title = title;
    }

    public override string Execute() {
      ListAllProjects AllProjectReport = new ListAllProjects(new TCaratProjectCollection(Projects.ToList().Where(p => !p.ContainsDuped)), Title == "" ? DefaultDescription : Title);
      LastResult = AllProjectReport.Execute();
      return LastResult;
    }

  }
}
