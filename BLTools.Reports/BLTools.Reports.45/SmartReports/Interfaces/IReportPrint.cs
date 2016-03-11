using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BLTools.Reports {
  public interface IReportPrint {
    void Print(bool preview);
  }
}
