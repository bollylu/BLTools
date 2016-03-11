using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StockRicDatafiles {
  class Parts : DataFileFixedLength {

    public string PartNo { get; set; }
    public string TypoModello { get; set; }

    public Parts(string PhysicalName) : base(PhysicalName, 701, typeof(PartsRecord)) {
      Name = Name;
    }

  }

  public class PartsRecord : DataRecord {
  }
}
