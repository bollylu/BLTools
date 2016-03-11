using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BLTools.Reports {
  public class TSmartReportMetadataCollection : List<TSmartReportMetadata> {

    public TSmartReportMetadata this[string reportName] {
      get {
        return Find(s => s.Fullname.ToLower().EndsWith(reportName.ToLower()));
      }
    }

    public bool IsReportAvailable(string reportname) {
      return Exists(r => r.Fullname.ToLower() == reportname.ToLower());
    }

    public override string ToString() {
      StringBuilder RetVal = new StringBuilder();
      ForEach(r => RetVal.AppendLine(r.ToString()));
      return RetVal.ToString();
    }
  }
}
