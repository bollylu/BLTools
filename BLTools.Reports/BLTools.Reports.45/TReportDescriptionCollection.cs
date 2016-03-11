using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CaratManagementReports {
  public class TReportDescriptionCollection : List<TReportDescription> {
    public override string ToString() {
      StringBuilder RetVal = new StringBuilder();
      this.ForEach(r => RetVal.AppendFormat("{0}\n", r.ToString()));
      return RetVal.ToString();
    }
  }
}
