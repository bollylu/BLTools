using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BLTools.Data;
using BLTools;
using BLTools.Data.Csv;

namespace BLTools.Data.Test {
  public class TCsvTestFile : TCsvDataFile<TCsvTestRecord> {
    public TCsvTestFile(string filename) {
      //this.HasHeaders = true;
      this.Name = filename;
      this.DataEncoding = Encoding.Default;
    }
  }
}
