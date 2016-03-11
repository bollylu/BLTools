using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BLTools.Reports {
  public interface IReportSave {
    string Save();
    string Save(string filename);
  }
}
