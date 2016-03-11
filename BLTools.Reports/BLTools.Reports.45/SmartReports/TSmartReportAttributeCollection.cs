using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BLTools.Reports {
  public class TSmartReportAttributeCollection : Dictionary<string, TSmartReportAttribute> {
    public override string ToString() {
      StringBuilder RetVal = new StringBuilder();
      this.Keys.ToList().ForEach(k => RetVal.AppendFormat("{0} : {1}\n", k, this[k].ToString()));
      return RetVal.ToString();
    }
  }
}
