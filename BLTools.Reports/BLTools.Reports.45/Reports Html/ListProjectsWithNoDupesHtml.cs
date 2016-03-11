using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CaratFileManagementLib;
using System.Printing;
using System.Web.UI.WebControls;
using System.Web.UI;
using System.IO;
using System.Drawing;

namespace CaratManagementReports.SmartReports {

  [SmartReport(ReportTypeEnum.Table, "List all projects with no dupes in HTML table", PageOrientation.Portrait, ReportDestinationEnum.Html)]
  public class ListProjectsWithNoDupesHtml : SmartReport, IReportPrint, IReportEmail {

    private TCaratProjectCollection Projects;

    public ListProjectsWithNoDupesHtml(TCaratProjectCollection projects, string title = "")
      : base() {
      Projects = new TCaratProjectCollection(projects);
      LastResult = "";
      Title = title;
    }

    public override string Execute() {
      ListAllProjectsHtml AllProjectsHtmlReport = new ListAllProjectsHtml(new TCaratProjectCollection(Projects.ToList().Where(p => !p.ContainsDuped)), Title == "" ? DefaultDescription : Title);
      LastResult = AllProjectsHtmlReport.Execute();

      return LastResult;
    }

  }
}
