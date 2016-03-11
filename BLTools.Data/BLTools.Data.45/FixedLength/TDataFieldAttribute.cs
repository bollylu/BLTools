using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLTools.Data.FixedLength {
  [global::System.AttributeUsage(AttributeTargets.Property, Inherited = false, AllowMultiple = false)]
  sealed public class TDataFieldAttribute : Attribute {

    /// <summary>
    /// Startup position in the record
    /// </summary>
    public int StartPos { get; set; }
    /// <summary>
    /// Length of the field in bytes
    /// </summary>
    public int Length { get; set; }
    /// <summary>
    /// Number of decimal digits
    /// </summary>
    public int DecimalDigits { get; set; }
    /// <summary>
    /// The decimal separator is virtual and not physically stored
    /// </summary>
    public bool DecimalIsVirtual { get; set; }
    /// <summary>
    /// When physically stored, the char used to separate int from decimal data
    /// </summary>
    public char DecimalSeparator { get; set; }
    /// <summary>
    /// Determines when number need left padding with zero (ex. 002)
    /// </summary>
    public bool ZeroPadding { get; set; }

    /// <summary>
    /// Determines wether the value has sign value in position 0
    /// </summary>
    public bool IsSigned { get; set; }

    /// <summary>
    /// Values for the date and time format
    /// </summary>
    public EDateTimeFormat DateTimeFormat { get; set; }
    /// <summary>
    /// If Custom date and time format, the description of the format
    /// </summary>
    public string DateTimeFormatCustom { get; set; }
    /// <summary>
    /// The values for the bool format
    /// </summary>
    public EBoolFormat BoolFormat { get; set; }
    /// <summary>
    /// If custom bool format, the value representing true
    /// </summary>
    public string TrueValue { get; set; }
    /// <summary>
    /// If custom bool format, the value representing false
    /// </summary>
    public string FalseValue { get; set; }

    /// <summary>
    /// Describes a data field
    /// </summary>
    public TDataFieldAttribute() {
      StartPos = 0;
      Length = 0;
      DecimalDigits = 0;
      DecimalIsVirtual = true;
      DecimalSeparator = CultureInfo.CurrentCulture.NumberFormat.CurrencyDecimalSeparator[0];
      DateTimeFormat = EDateTimeFormat.DateAndTime;
      DateTimeFormatCustom = "";
      BoolFormat = EBoolFormat.TOrF;
      TrueValue = "T";
      FalseValue = "F";
      ZeroPadding = false;
      IsSigned = false;
    }

    ///// <summary>
    ///// Only start position and length, for string or int or long
    ///// </summary>
    ///// <param name="startPos">Startup position of the field in record</param>
    ///// <param name="length">Length of the field in record (in bytes)</param>
    //public TDataFieldAttribute(int startPos, int length) : this() {
    //  StartPos = startPos;
    //  Length = length;
    //}

    ///// <summary>
    ///// Start position, length , data type as float, double or decimal, and number of decimal digits. Comma separator is not included.
    ///// </summary>
    ///// <param name="startPos">Startup position of the field in record</param>
    ///// <param name="length">Length of the field in record (in bytes)</param>
    ///// <param name="decimalDigits">Number of digits after the comma</param>
    ///// <param name="decimalIsVirtual">Is the decimal separator really stored</param>
    ///// <param name="decimalSeparator">The char used as decimal separator</param>
    //public TDataFieldAttribute(int startPos, int length, int decimalDigits = 0, bool decimalIsVirtual = true, char decimalSeparator = '\0')
    //  : this() {
    //  StartPos = startPos;
    //  Length = length;
    //  DecimalDigits = decimalDigits;
    //  DecimalIsVirtual = decimalIsVirtual;
    //  if (decimalSeparator != '\0') {
    //    DecimalSeparator = decimalSeparator;
    //  }
    //}

    ///// <summary>
    ///// Start position, length, component of datetime to include, custom format
    ///// </summary>
    ///// <param name="startPos">Startup position of the field in record</param>
    ///// <param name="length">Length of the field in record (in bytes)</param>
    ///// <param name="dateTimeFormat">What compoent of datetime to include</param>
    ///// <param name="dateTimeFormatCustom">if component set to custom, a custom format</param>
    //public TDataFieldAttribute(int startPos, int length, EDateTimeFormat dateTimeFormat, string dateTimeFormatCustom = "")
    //  : this() {
    //  StartPos = startPos;
    //  Length = length;
    //  DateTimeFormat = dateTimeFormat;
    //  DateTimeFormatCustom = dateTimeFormatCustom;
    //}

    ///// <summary>
    ///// Start position, length, how to format the bool
    ///// </summary>
    ///// <param name="startPos">Startup position of the field in record</param>
    ///// <param name="length">Length of the field in record (in bytes)</param>
    ///// <param name="boolFormat">How to format the bool</param>
    ///// <param name="trueValue">If format set to custom, value to represent true</param>
    ///// <param name="falseValue">If format set to custom, value to represent false</param>
    //public TDataFieldAttribute(int startPos, int length, EBoolFormat boolFormat, string trueValue = "T", string falseValue = "F") : this() {
    //  StartPos = startPos;
    //  Length = length;
    //  BoolFormat = BoolFormat;
    //  TrueValue = trueValue;
    //  FalseValue = falseValue;
    //}
  }
}
