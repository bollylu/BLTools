using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Printing;

namespace CaratManagementReports {
  public enum ReportTypeEnum {
    Table,
    Details
  }

  public enum ReportDestinationEnum {
    Text,
    Html,
    Rtf,
    ReportEngine
  }

  public class TCaratReportAttribute : Attribute {
    public ReportTypeEnum ReportType { get; set; }
    public string Description { get; set; }
    public PageOrientation Orientation { get; set; }
    public ReportDestinationEnum Destination { get; set; }

    public TCaratReportAttribute(ReportTypeEnum reportType, string description = "") {
      ReportType = reportType;
      Description = description;
      Orientation = PageOrientation.Portrait;
      Destination = ReportDestinationEnum.Text;
    }

    public TCaratReportAttribute(ReportTypeEnum reportType, string description, PageOrientation orientation, ReportDestinationEnum destination = ReportDestinationEnum.Text) {
      ReportType = reportType;
      Description = description;
      Orientation = orientation;
      Destination = destination;
    }

    public TCaratReportAttribute(TCaratReportAttribute reportAttribute) {
      ReportType = reportAttribute.ReportType;
      Description = reportAttribute.Description;
      Orientation = reportAttribute.Orientation;
      Destination = reportAttribute.Destination;
    }

    public override string ToString() {
      StringBuilder RetVal = new StringBuilder();
      RetVal.AppendFormat("type: {0}", ReportType.ToString());
      RetVal.AppendFormat("description, {0}", Description);
      RetVal.AppendFormat("orientation, {0}", Orientation.ToString());
      RetVal.AppendFormat("destination, {0}", Destination.ToString());
      return RetVal.ToString();
    }
  }
}
