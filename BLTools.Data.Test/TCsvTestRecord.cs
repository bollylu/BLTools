using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BLTools.Data;
using BLTools;
using BLTools.Data.Csv;

namespace BLTools.Data.Test {
  public class TCsvTestRecord : TCsvRecordX {

    [TCsvDataField(FieldName = "Customer", FieldPosition = 2)]
    public string Name { get; set; }

    [TCsvDataField(DecimalDigits = 2, FieldName = "CA 2014", FieldPosition = 1)]
    public float CA2014 { get; set; }

    [TCsvDataField()]
    public string Apb { get; set; }

  }
}
