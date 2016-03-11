using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StockRic {
  public class DataField {
    public string Name { get; set; }
    public int StartPos { get; set; }
    public int Length { get; set; }
    public Type FieldType { get; set; }

    public DataField() { }

    public DataField( string name, int startPos, int length, Type fieldType ) {
      Name = name;
      StartPos = startPos;
      Length = length;
      FieldType = fieldType;
    }
  }

  public class FileSchema : List<DataField> {

    public DataField this[string name] {
      get {
        DataField RetVal = this.Find(d => d.Name == name);
        if ( RetVal != null ) {
          return RetVal;
        } else {
          return null;
        }
      }
    }
  }
}
