using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Printing;

namespace BLTools.Reports {
  public class TSmartReportMetadata {
    public string Name { get; set; }
    public string Fullname { get; set; }
    public string Description { get; set; }
    public EReportDestination Destination { get; set; }
    public EReportType ReportType { get; set; }
    public PageOrientation Orientation { get; set; }
    public Type Report { get; set; }

    public TSmartReportMetadata(string name, string description, EReportDestination destination, EReportType type, PageOrientation orientation) {
      Fullname = name;
      Name = name.Substring(name.LastIndexOf('.')+1);
      Description = description;
      Destination = destination;
      ReportType = type;
      Orientation = orientation;
    }

    public override string ToString() {
      StringBuilder RetVal = new StringBuilder();
      RetVal.AppendFormat("Report: {0}", Fullname);
      RetVal.AppendFormat(", type={0}", ReportType.ToString());
      RetVal.AppendFormat(", description=\"{0}\"", Description);
      RetVal.AppendFormat(", orientation={0}", Orientation.ToString());
      RetVal.AppendFormat(", destination={0}", Destination.ToString());
      return RetVal.ToString();
    }
  }
}
