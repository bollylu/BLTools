using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Printing;

namespace BLTools.Reports {

  [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
  public sealed class TSmartReportAttribute : Attribute {

    #region Public properties
    public EReportType ReportType { get; private set; }
    public string Description { get; private set; }
    public PageOrientation Orientation { get; private set; }
    public EReportDestination Destination { get; private set; } 
    #endregion Public properties

    public TSmartReportAttribute(EReportType reportType, string description = "") {
      ReportType = reportType;
      Description = description;
      Orientation = PageOrientation.Portrait;
      Destination = EReportDestination.Text;
    }

    public TSmartReportAttribute(EReportType reportType, string description, PageOrientation orientation, EReportDestination destination = EReportDestination.Text) {
      ReportType = reportType;
      Description = description;
      Orientation = orientation;
      Destination = destination;
    }

    public TSmartReportAttribute(TSmartReportAttribute smartReportAttribute) {
      ReportType = smartReportAttribute.ReportType;
      Description = smartReportAttribute.Description;
      Orientation = smartReportAttribute.Orientation;
      Destination = smartReportAttribute.Destination;
    }

    #region Converters
    public override string ToString() {
      StringBuilder RetVal = new StringBuilder();
      RetVal.AppendFormat("type: {0}", ReportType.ToString());
      RetVal.AppendFormat("description, {0}", Description);
      RetVal.AppendFormat("orientation, {0}", Orientation.ToString());
      RetVal.AppendFormat("destination, {0}", Destination.ToString());
      return RetVal.ToString();
    }
    #endregion Converters
  }
  
}
