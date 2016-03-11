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
using CaratManagementReports;

namespace CaratManagementReports.SmartReports {

  [SmartReport(ReportTypeEnum.Table, "List all projects in HTML table", PageOrientation.Portrait, ReportDestinationEnum.Html)]
  public class ListAllProjectsHtml : SmartReport, IReportPrint, IReportEmail {

    private TCaratProjectCollection Projects;

    public ListAllProjectsHtml(TCaratProjectCollection projects, string title = "")
      : base() {
      Projects = new TCaratProjectCollection(projects);
      LastResult = "";
      Title = title;
    }

    public override string Execute() {
      StringBuilder RetVal = new StringBuilder();
      StringWriter RetValOutput = new StringWriter(RetVal);
      HtmlTextWriter HtmlOutput = new HtmlTextWriter(RetValOutput);

      string CurrentTitle = string.Format("{0} - {1}", DateTime.Now.ToString("dd/MM/yyyy - HH:mm"), string.IsNullOrWhiteSpace(Title) ? "List of projects with dupes files details from working revisions" : Title);

      RetVal.AppendLine("<HTML>");
      RetVal.AppendLine(ReportHelperHtml.BuildHead(CurrentTitle));
      RetVal.AppendLine("<BODY>");

      RetVal.AppendLine(string.Format("<H1>{0}</H1>", CurrentTitle));
      RetVal.AppendLine("<hr/>");

      #region Projects table
      Table ProjectTable = new Table();
      ProjectTable.Width = new Unit(100, UnitType.Percentage);

      TableHeaderRow HeaderRow = new TableHeaderRow();

      TableHeaderCell HeaderCellId = new TableHeaderCell();
      HeaderCellId.Width = new Unit(10, UnitType.Percentage);
      HeaderCellId.Text = "Id";
      HeaderRow.Cells.Add(HeaderCellId);

      TableHeaderCell HeaderCellName = new TableHeaderCell();
      HeaderCellName.Width = new Unit(40, UnitType.Percentage);
      HeaderCellName.Text = "Name";
      HeaderRow.Cells.Add(HeaderCellName);

      TableHeaderCell HeaderCellDescription = new TableHeaderCell();
      HeaderCellDescription.Width = new Unit(40, UnitType.Percentage);
      HeaderCellDescription.Text = "Description";
      HeaderRow.Cells.Add(HeaderCellDescription);

      TableHeaderCell HeaderCellDupes = new TableHeaderCell();
      HeaderCellDupes.Width = new Unit(10, UnitType.Percentage);
      HeaderCellDupes.Text = "Dupes";
      HeaderRow.Cells.Add(HeaderCellDupes);

      ProjectTable.Rows.Add(HeaderRow);


      bool IsAlternate = false;
      foreach (TCaratProject ProjectItem in Projects) {
        TableRow NewRow = new TableRow();

        TableCell CellId = new TableCell();
        CellId.Text = ProjectItem.ProjectId.ToString();
        NewRow.Cells.Add(CellId);

        TableCell CellName = new TableCell();
        CellName.Text = ProjectItem.Name;
        NewRow.Cells.Add(CellName);

        TableCell CellDescription = new TableCell();
        CellDescription.Text = ProjectItem.Properties.ShortDescription;
        NewRow.Cells.Add(CellDescription);

        TableCell CellDupes = new TableCell();
        CellDupes.Text = ProjectItem.DupesCount.ToString();
        NewRow.Cells.Add(CellDupes);

        IsAlternate = !IsAlternate;
        if (IsAlternate) {
          NewRow.BackColor = Color.Wheat;
        }
        ProjectTable.Rows.Add(NewRow);
      }
      ProjectTable.RenderControl(HtmlOutput);

      #endregion Projects table

      RetVal.AppendLine("</BODY>");
      RetVal.AppendLine("</HTML>");

      LastResult = RetVal.ToString();

      return LastResult;
    }

  }
}
