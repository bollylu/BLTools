using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Printing;
using CaratFileManagementLib;
using System.Web;
using System.Web.UI.WebControls;
using System.Web.UI;
using System.IO;
using System.Drawing;

namespace CaratManagementReports {
  public static class ReportHtmlTable {

    [TCaratReport(ReportTypeEnum.Table, "List all projects", orientation: PageOrientation.Portrait, destination: ReportDestinationEnum.Html)]
    public static string ListProjectsHtml(TCaratProjectCollection projects, string title = "") {

      StringBuilder RetVal = new StringBuilder();
      StringWriter RetValOutput = new StringWriter(RetVal);
      HtmlTextWriter HtmlOutput = new HtmlTextWriter(RetValOutput);

      string Title = string.Format("{0} - {1}", DateTime.Now.ToString("dd/MM/yyyy - HH:mm"), string.IsNullOrWhiteSpace(title) ? "List of projects with dupes files details from working revisions" : title);

      RetVal.AppendLine("<HTML>");
      RetVal.AppendLine("<HEAD>");
      RetVal.AppendLine("<meta http-equiv='Content-Type' content='text/html; charset=UTF-8'/>");
      RetVal.AppendLine(string.Format("<title>{0}</title>", title));
      RetVal.AppendLine("</HEAD>");
      RetVal.AppendLine("<BODY>");

      RetVal.AppendLine(string.Format("<H1>{0}</H1>", Title));
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
      foreach (TCaratProject ProjectItem in projects) {
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
      return RetVal.ToString();
    }
  }
}
