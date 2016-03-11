using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLTools.Data.Csv {
  /// <summary>
  /// Describes the attributes to specify a CSV field property
  /// </summary>
  [global::System.AttributeUsage(AttributeTargets.Property, Inherited = false, AllowMultiple = false)]
  sealed public class TCsvDataFieldAttribute : Attribute {

    #region Public properties
    /// <summary>
    /// The name of the field in the headers (if any). Takes precedence over property name.
    /// </summary>
    public string FieldName { get; set; }
    /// <summary>
    /// The position of the field in the record. Takes precedences over property name and FieldName
    /// </summary>
    public int FieldPosition { get; set; }
    /// <summary>
    /// Number of digits after the decimal separator (default to 0)
    /// </summary>
    public int DecimalDigits { get; set; }
    /// <summary>
    /// Type of DateTime format (date, time, date and time, custom)
    /// </summary>
    public EDateTimeFormat DateTimeFormat { get; set; }
    /// <summary>
    /// Custom format for DateTime field. Requires DateTimeFormat to be set to EDateTimeFormat.Custom
    /// </summary>
    public string DateTimeFormatCustom { get; set; }
    /// <summary>
    /// Type of the format for boolean field (Y/N, 1/0, ..., custom)
    /// </summary>
    public EBoolFormat BoolFormat { get; set; }
    /// <summary>
    /// Text value representing true. Requires Bool format to set to EBoolFormat.Custom
    /// </summary>
    public string TrueValue { get; private set; }
    /// <summary>
    /// Text value representing false. Requires Bool format to set to EBoolFormat.Custom
    /// </summary>
    public string FalseValue { get; private set; } 
    #endregion Public properties

    /// <summary>
    /// Blank constructor, sets all default values
    /// </summary>
    public TCsvDataFieldAttribute() {
      FieldName = "";
      FieldPosition = -1;
      DecimalDigits = 0;
      DateTimeFormat = EDateTimeFormat.DateAndTime;
      DateTimeFormatCustom = "";
      BoolFormat = EBoolFormat.TOrF;
      TrueValue = "T";
      FalseValue = "F";
    }

    ///// <summary>
    ///// Constructor with FieldName and optionally, position. Position takes precedence over FieldName, and FieldName takes precedence over Property name.
    ///// </summary>
    ///// <param name="name">The name of the field, as specified in the header. It can include spaces.</param>
    ///// <param name="position">The optional position (numbered from 0)(</param>
    //public TCsvDataFieldAttribute(string name, int position = -1)
    //  : this() {
    //  FieldName = name;
    //  FieldPosition = position;
    //}

    ///// <summary>
    ///// Data type as float, double or decimal, and number of decimal digits. Comma separator is not included.
    ///// </summary>
    ///// <param name="decimalDigits">Number of digits after the comma</param>
    //public TCsvDataFieldAttribute(int decimalDigits = 0)
    //  : this() {
    //  DecimalDigits = decimalDigits;
    //}

    ///// <summary>
    ///// Component of datetime to include, custom format
    ///// </summary>
    ///// <param name="dateTimeFormat">What compoent of datetime to include</param>
    ///// <param name="dateTimeFormatCustom">if component set to custom, a custom format</param>
    //public TCsvDataFieldAttribute(EDateTimeFormat dateTimeFormat, string dateTimeFormatCustom = "")
    //  : this() {
    //  DateTimeFormat = dateTimeFormat;
    //  DateTimeFormatCustom = dateTimeFormatCustom;
    //}

    ///// <summary>
    ///// How to format the bool
    ///// </summary>
    ///// <param name="boolFormat">How to format the bool</param>
    ///// <param name="trueValue">If format set to custom, value to represent true</param>
    ///// <param name="falseValue">If format set to custom, value to represent false</param>
    //public TCsvDataFieldAttribute(EBoolFormat boolFormat, string trueValue = "T", string falseValue = "F")
    //  : this() {
    //  BoolFormat = BoolFormat;
    //  TrueValue = trueValue;
    //  FalseValue = falseValue;
    //}
  }
}
