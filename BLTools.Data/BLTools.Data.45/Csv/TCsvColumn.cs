using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BLTools.Data;

namespace BLTools.Data.Csv {
  public class TCsvColumn {
    public string Name { get; set; }
    public Type ValueType { get; set; }
    public object Value { get; set; }
    public int DecimalDigits { get; set; }
    public EDateTimeFormat DateTimeFormat { get; set; }
    public string DateTimeFormatCustom { get; set; }
    public EBoolFormat BoolFormat { get; set; }
    public string TrueValue { get; private set; }
    public string FalseValue { get; private set; }

    public TCsvColumn() {
      Name = "";
      ValueType = null;
      Value = null;
      DecimalDigits = 0;
      DateTimeFormat = EDateTimeFormat.DateAndTime;
      DateTimeFormatCustom = "";
      BoolFormat = EBoolFormat.TrueOrFalse;
      TrueValue = "True";
      FalseValue = "False";
    }

    public TCsvColumn(string name, Type typeValue)
      : this() {
      Name = name;
      ValueType = typeValue;
    }

    public TCsvColumn(string name, EDateTimeFormat dateTimeFormat, string dateTimeFormatCustom = "")
      : this() {
      Name = name;
      ValueType = typeof(DateTime);
      DateTimeFormat = dateTimeFormat;
      DateTimeFormatCustom = dateTimeFormatCustom;
    }


    public TCsvColumn(string name, EBoolFormat boolFormat, string trueValue = "T", string falseValue = "F")
      : this() {
      Name = name;
      ValueType = typeof(bool);
      BoolFormat = boolFormat;
      TrueValue = trueValue;
      FalseValue = falseValue;
    }

    public TCsvColumn(string name, Type valueType, int decimalDigits = 0)
      : this() {
      Name = name;
      ValueType = valueType;
      DecimalDigits = decimalDigits;
    }



  }
}
