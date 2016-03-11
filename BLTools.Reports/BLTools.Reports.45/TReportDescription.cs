using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Printing;

namespace CaratManagementReports {
  public class TReportDescription {
    public string Name { get; set; }
    public ReportTypeEnum ReportType { get; set; }
    public string Description { get; set; }
    public PageOrientation Orientation { get; set; }

    public TReportDescription(string name, ReportTypeEnum reportType, string description = "") {
      Name = name;
      ReportType = reportType;
      Description = description;
    }

    public TReportDescription(string name, ReportTypeEnum reportType, string description, PageOrientation orientation) {
      Name = name;
      ReportType = reportType;
      Description = description;
      Orientation = orientation;
    }
    public override string ToString() {
      StringBuilder RetVal = new StringBuilder();
      RetVal.AppendFormat("Report: {0}", Name);
      RetVal.AppendFormat(", type: {0}", ReportType.ToString());
      RetVal.AppendFormat(", {0}", Description);
      RetVal.AppendFormat(", {0}", Orientation.ToString());
      return RetVal.ToString();
    }
  }
}
