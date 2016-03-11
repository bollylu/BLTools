using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StockRic {
  public class DataField {
    public string Name { get; set; }
    public int StartPos { get; set; }
    public int Length { get; set; }
    public string FieldType { get; set; }

    public DataField( string name, int startPos, int length, string fieldType ) {
      Name = name;
      StartPos = startPos;
      Length = length;
      FieldType = fieldType;
    }
  }

  class FileSchema : List<DataField> {
  }
}
