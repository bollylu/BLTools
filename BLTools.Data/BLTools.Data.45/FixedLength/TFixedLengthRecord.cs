using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using BLTools;

namespace BLTools.Data.FixedLength {

  /// <summary>
  /// Describes a fixed length record
  /// </summary>
  public class TFixedLengthRecord : IFixedLengthRecord {

    /// <summary>
    /// Additional debug messages when true
    /// </summary>
    public static bool IsDebug = false;

    /// <summary>
    /// Gets the record length based on field declarations
    /// </summary>
    public int RecLen {
      get {
        if (_RecLen == 0) {
          PropertyInfo LastPropertyInfo = this.GetType()
                                              .GetProperties()
                                              .Where(x => x.HasAttribute(typeof(TDataFieldAttribute)))
                                              .OrderBy(x => ((TDataFieldAttribute)Attribute.GetCustomAttribute(x, typeof(TDataFieldAttribute))).StartPos)
                                              .Last();
          TDataFieldAttribute CurrentFieldAttribute = (TDataFieldAttribute)Attribute.GetCustomAttribute(LastPropertyInfo, typeof(TDataFieldAttribute));
          _RecLen = CurrentFieldAttribute.StartPos + CurrentFieldAttribute.Length;
        }
        return _RecLen;
      }
    }
    private int _RecLen;

    /// <summary>
    /// True if empty date values are to left blank (spaces), false otherwise
    /// </summary>
    public bool EmptyDatesValueBlank { get; set; }

    #region Constructor(s)
    public TFixedLengthRecord() { }
    public TFixedLengthRecord(string rawData)
      : this(rawData, Encoding.Default) {
    }
    public TFixedLengthRecord(string rawData, Encoding encoding) {
      FromRawData(encoding.GetBytes(rawData));
    }
    public TFixedLengthRecord(byte[] rawData) {
      FromRawData(rawData);
    }
    #endregion Constructor(s)

    #region Public methods
    /// <summary>
    /// Converts the fields in the record into a array of bytes
    /// </summary>
    /// <returns></returns>
    public virtual byte[] ToRawData(Encoding encoding) {
      StringBuilder RawData = new StringBuilder();
      try {
        foreach (PropertyInfo PropertyInfoItem in this.GetType()
                                                      .GetProperties()
                                                      .Where(x => x.HasAttribute(typeof(TDataFieldAttribute)))
                                                      .OrderBy(x => ((TDataFieldAttribute)Attribute.GetCustomAttribute(x, typeof(TDataFieldAttribute))).StartPos)) {

          TDataFieldAttribute CurrentFieldAttribute = (TDataFieldAttribute)Attribute.GetCustomAttribute(PropertyInfoItem, typeof(TDataFieldAttribute));

          try {

            #region Error
            if (PropertyInfoItem.GetValue(this, null) == null) {
              Trace.WriteLine(string.Format("Error in value {0} : null", PropertyInfoItem.Name));
              RawData.Append(new string(' ', CurrentFieldAttribute.Length));
              continue;
            }
            #endregion Error

            #region DateTime
            if (PropertyInfoItem.PropertyType == typeof(DateTime)) {
              DateTime CurrentValue = (DateTime)PropertyInfoItem.GetValue(this, null);
              if (CurrentValue == null || CurrentValue == DateTime.MinValue) {
                if (EmptyDatesValueBlank) {
                  RawData.Append(new string(' ', CurrentFieldAttribute.Length));
                  continue;
                }
                CurrentValue = DateTime.MinValue;
              }
              switch (CurrentFieldAttribute.DateTimeFormat) {
                case EDateTimeFormat.DateOnly:
                  RawData.Append(CurrentValue.ToString("yyyyMMdd").PadRight(CurrentFieldAttribute.Length).Left(CurrentFieldAttribute.Length));
                  continue;
                case EDateTimeFormat.TimeOnly:
                  RawData.Append(CurrentValue.ToString("HHmmss").PadRight(CurrentFieldAttribute.Length).Left(CurrentFieldAttribute.Length));
                  continue;
                case EDateTimeFormat.Custom:
                  RawData.Append(CurrentValue.ToString(CurrentFieldAttribute.DateTimeFormatCustom).PadRight(CurrentFieldAttribute.Length).Left(CurrentFieldAttribute.Length));
                  continue;
                case EDateTimeFormat.DateAndTime:
                default:
                  RawData.Append(CurrentValue.ToString("yyyyMMddHHmmss").PadRight(CurrentFieldAttribute.Length).Left(CurrentFieldAttribute.Length));
                  continue;
              }
            }
            #endregion DateTime

            #region Bool
            if (PropertyInfoItem.PropertyType == typeof(bool)) {
              bool CurrentValue = (bool)PropertyInfoItem.GetValue(this, null);
              switch (CurrentFieldAttribute.BoolFormat) {
                case EBoolFormat.OneOrZero:
                  RawData.Append((CurrentValue ? "1" : "0").PadRight(CurrentFieldAttribute.Length));
                  continue;
                case EBoolFormat.TOrF:
                  RawData.Append((CurrentValue ? "T" : "F").PadRight(CurrentFieldAttribute.Length));
                  continue;
                case EBoolFormat.TrueOrFalse:
                  RawData.Append((CurrentValue ? "True" : "False").PadRight(CurrentFieldAttribute.Length));
                  continue;
                case EBoolFormat.YesOrNo:
                  RawData.Append((CurrentValue ? "Yes" : "No").PadRight(CurrentFieldAttribute.Length));
                  continue;
                case EBoolFormat.YOrN:
                  RawData.Append((CurrentValue ? "Y" : "N").PadRight(CurrentFieldAttribute.Length));
                  continue;
                case EBoolFormat.Custom:
                  RawData.Append((CurrentValue ? CurrentFieldAttribute.TrueValue : CurrentFieldAttribute.FalseValue).PadRight(CurrentFieldAttribute.Length));
                  continue;
              }
            }
            #endregion Bool

            #region Int
            if (PropertyInfoItem.PropertyType == typeof(int)) {
              int TempInt = (int)PropertyInfoItem.GetValue(this, null);
              if (CurrentFieldAttribute.IsSigned) {
                RawData.Append(TempInt >= 0 ? " " : "-");
              }
              if (CurrentFieldAttribute.ZeroPadding) {
                RawData.Append(Math.Abs(TempInt).ToString().PadLeft(CurrentFieldAttribute.Length, '0').Right(CurrentFieldAttribute.Length - (CurrentFieldAttribute.IsSigned ? 1 : 0)));
              } else {
                RawData.Append(Math.Abs(TempInt).ToString().PadLeft(CurrentFieldAttribute.Length).Right(CurrentFieldAttribute.Length - (CurrentFieldAttribute.IsSigned ? 1 : 0)));
              }
              continue;
            }
            #endregion Int

            #region Long
            if (PropertyInfoItem.PropertyType == typeof(long)) {
              long TempLong = (long)PropertyInfoItem.GetValue(this, null);
              if (CurrentFieldAttribute.IsSigned) {
                RawData.Append(TempLong >= 0 ? " " : "-");
              }
              if (CurrentFieldAttribute.ZeroPadding) {
                RawData.Append(Math.Abs(TempLong).ToString().PadLeft(CurrentFieldAttribute.Length, '0').Right(CurrentFieldAttribute.Length - (CurrentFieldAttribute.IsSigned ? 1 : 0)));
              } else {
                RawData.Append(Math.Abs(TempLong).ToString().PadLeft(CurrentFieldAttribute.Length).Right(CurrentFieldAttribute.Length - (CurrentFieldAttribute.IsSigned ? 1 : 0)));
              }
              continue;
            }
            #endregion Long

            #region Float
            if (PropertyInfoItem.PropertyType == typeof(float)) {
              float TempFloat = (float)PropertyInfoItem.GetValue(this, null);
              if (CurrentFieldAttribute.IsSigned) {
                RawData.Append(TempFloat >= 0 ? " " : "-");
              }
              string FormatNumber = string.Format("F{0}", CurrentFieldAttribute.DecimalDigits);
              if (CurrentFieldAttribute.DecimalIsVirtual) {
                #region Cobol case, decimals (0 => 9) and virtual separator (ex. 999V99)
                RawData.Append(Math.Abs(TempFloat).ToString(FormatNumber, CultureInfo.InvariantCulture)
                                                  .Replace(CultureInfo.InvariantCulture.NumberFormat.NumberDecimalSeparator, "")
                                                  .PadLeft(CurrentFieldAttribute.Length, CurrentFieldAttribute.ZeroPadding ? '0' : ' ')
                                                  .Right(CurrentFieldAttribute.Length - (CurrentFieldAttribute.IsSigned ? 1 : 0)));
                continue;
                #endregion Cobol case, decimals (0 => 9) and virtual separator (ex. 999V99)
              } else {
                #region Standard case, decimals and not virtual separator (separator counts in the field length)
                CultureInfo TempCultureInfo = new CultureInfo(CultureInfo.CurrentCulture.LCID);
                TempCultureInfo.NumberFormat.CurrencyDecimalSeparator = CurrentFieldAttribute.DecimalSeparator.ToString();
                TempCultureInfo.NumberFormat.NumberDecimalSeparator = CurrentFieldAttribute.DecimalSeparator.ToString();
                RawData.Append(Math.Abs(TempFloat).ToString(FormatNumber, TempCultureInfo)
                                                  .PadLeft(CurrentFieldAttribute.Length, CurrentFieldAttribute.ZeroPadding ? '0' : ' ')
                                                  .Right(CurrentFieldAttribute.Length - (CurrentFieldAttribute.IsSigned ? 1 : 0)));
                continue;
                #endregion Standard case, decimals and not virtual separator (separator counts in the field length)
              }
            }
            #endregion Float

            #region Double
            if (PropertyInfoItem.PropertyType == typeof(double)) {
              double TempDouble = (double)PropertyInfoItem.GetValue(this, null);
              if (CurrentFieldAttribute.IsSigned) {
                RawData.Append(TempDouble >= 0 ? " " : "-");
              }
              string FormatNumber = string.Format("F{0}", CurrentFieldAttribute.DecimalDigits);
              if (CurrentFieldAttribute.DecimalIsVirtual) {
                #region Cobol case, decimals (0 => 9) and virtual separator (ex. 999V99)
                RawData.Append(Math.Abs(TempDouble).ToString(FormatNumber, CultureInfo.InvariantCulture)
                                                   .Replace(CultureInfo.InvariantCulture.NumberFormat.NumberDecimalSeparator, "")
                                                   .PadLeft(CurrentFieldAttribute.Length, CurrentFieldAttribute.ZeroPadding ? '0' : ' ')
                                                   .Right(CurrentFieldAttribute.Length - (CurrentFieldAttribute.IsSigned ? 1 : 0)));
                continue;
                #endregion Cobol case, decimals (0 => 9) and virtual separator (ex. 999V99)
              } else {
                #region Standard case, decimals and not virtual separator (separator counts in the field length)
                CultureInfo TempCultureInfo = new CultureInfo(CultureInfo.CurrentCulture.LCID);
                TempCultureInfo.NumberFormat.CurrencyDecimalSeparator = CurrentFieldAttribute.DecimalSeparator.ToString();
                TempCultureInfo.NumberFormat.NumberDecimalSeparator = CurrentFieldAttribute.DecimalSeparator.ToString();
                RawData.Append(Math.Abs(TempDouble).ToString(FormatNumber, TempCultureInfo)
                                                   .PadLeft(CurrentFieldAttribute.Length, CurrentFieldAttribute.ZeroPadding ? '0' : ' ')
                                                   .Right(CurrentFieldAttribute.Length - (CurrentFieldAttribute.IsSigned ? 1 : 0)));
                continue;
                #endregion Standard case, decimals and not virtual separator (separator counts in the field length)
              }
            }
            #endregion Double

            #region Decimal
            if (PropertyInfoItem.PropertyType == typeof(decimal)) {
              decimal TempDecimal = (decimal)PropertyInfoItem.GetValue(this, null);
              if (CurrentFieldAttribute.IsSigned) {
                RawData.Append(TempDecimal >= 0 ? " " : "-");
              }
              string FormatNumber = string.Format("F{0}", CurrentFieldAttribute.DecimalDigits);
              if (CurrentFieldAttribute.DecimalIsVirtual) {
                #region Cobol case, decimals (0 => 9) and virtual separator (ex. 999V99)
                RawData.Append(Math.Abs(TempDecimal).ToString(FormatNumber, CultureInfo.InvariantCulture)
                                                    .Replace(CultureInfo.InvariantCulture.NumberFormat.NumberDecimalSeparator, "")
                                                    .PadLeft(CurrentFieldAttribute.Length, CurrentFieldAttribute.ZeroPadding ? '0' : ' ')
                                                    .Right(CurrentFieldAttribute.Length - (CurrentFieldAttribute.IsSigned ? 1 : 0)));
                continue;
                #endregion Cobol case, decimals (0 => 9) and virtual separator (ex. 999V99)
              } else {
                #region Standard case, decimals and not virtual separator (separator counts in the field length)
                CultureInfo TempCultureInfo = new CultureInfo(CultureInfo.CurrentCulture.LCID);
                TempCultureInfo.NumberFormat.CurrencyDecimalSeparator = CurrentFieldAttribute.DecimalSeparator.ToString();
                TempCultureInfo.NumberFormat.NumberDecimalSeparator = CurrentFieldAttribute.DecimalSeparator.ToString();
                RawData.Append(Math.Abs(TempDecimal).ToString(FormatNumber, TempCultureInfo)
                                                    .PadLeft(CurrentFieldAttribute.Length, CurrentFieldAttribute.ZeroPadding ? '0' : ' ')
                                                    .Right(CurrentFieldAttribute.Length - (CurrentFieldAttribute.IsSigned ? 1 : 0)));
                continue;
                #endregion Standard case, decimals and not virtual separator (separator counts in the field length)
              }
            }
            #endregion Decimal

            #region String
            RawData.Append(PropertyInfoItem.GetValue(this, null).ToString().PadRight(CurrentFieldAttribute.Length).Left(CurrentFieldAttribute.Length));
            continue;
            #endregion String

          } catch (Exception ex) {
            Trace.WriteLine(string.Format("Error : {0}", ex.Message));
            continue;
          }
        }

      } catch (Exception ex) {
        Trace.WriteLine(string.Format("Error : {0}", ex.Message));
      }
      if (encoding == null) {
        encoding = Encoding.ASCII;
      }
      return encoding.GetBytes(RawData.ToString());
    }

    /// <summary>
    /// Converts a raw record into fields
    /// </summary>
    /// <param name="data">The raw record as a string</param>
    /// <param name="encoding">The encoding used to convert string to raw data</param>
    public virtual void FromRawData(string data, Encoding encoding) {
      Trace.WriteLineIf(IsDebug, data);
      if (encoding == null) {
        encoding = Encoding.Default;
      }
      FromRawData(encoding.GetBytes(data));
    }

    /// <summary>
    /// Converts a raw record into fields
    /// </summary>
    /// <param name="rawData">The raw record as an array of bytes</param>
    public virtual void FromRawData(byte[] rawData) {


      foreach (PropertyInfo PropertyInfoItem in this.GetType()
                                                    .GetProperties()
                                                    .Where(x => x.HasAttribute(typeof(TDataFieldAttribute)))) {
        TDataFieldAttribute CurrentFieldAttribute = (TDataFieldAttribute)Attribute.GetCustomAttribute(PropertyInfoItem, typeof(TDataFieldAttribute));
        if (CurrentFieldAttribute != null) {
          int StartPos = CurrentFieldAttribute.StartPos;
          int Length = CurrentFieldAttribute.Length;
          Type FieldType = PropertyInfoItem.PropertyType;
          string RawFieldContent = rawData.Skip(StartPos).Take(Length).ToArray().ToCharString();

          if (IsDebug) {
            Trace.WriteLine(string.Format("Converting \"{0}\" to Field {1} ({2})", RawFieldContent, FieldType.Name, PropertyInfoItem.Name));
          }

          try {

            #region Int
            if (FieldType == typeof(int)) {
              PropertyInfoItem.SetValue(this, Int32.Parse(RawFieldContent.Replace(" ", "")), null);
              continue;
            }
            #endregion Int

            #region Long
            if (FieldType == typeof(long)) {
              PropertyInfoItem.SetValue(this, long.Parse(RawFieldContent.Replace(" ", "")), null);
              continue;
            }
            #endregion Long

            #region Float
            if (FieldType == typeof(float)) {

              #region Content of field is empty, return 0
              if (RawFieldContent.Trim() == "") {
                PropertyInfoItem.SetValue(this, 0f, null);
                continue;
              }
              #endregion Content of field is empty, return 0

              string StringValue;
              if (CurrentFieldAttribute.DecimalDigits > 0 && CurrentFieldAttribute.DecimalIsVirtual) {
                string StringValueLeft = RawFieldContent.Left(CurrentFieldAttribute.Length - CurrentFieldAttribute.DecimalDigits);
                string StringValueRight = RawFieldContent.Right(CurrentFieldAttribute.DecimalDigits);
                StringValue = string.Format("{0}{1}{2}", StringValueLeft, CurrentFieldAttribute.DecimalSeparator, StringValueRight);
              } else {
                StringValue = RawFieldContent.Replace(" ", "");
              }

              CultureInfo TempCultureInfo = new CultureInfo(CultureInfo.CurrentCulture.LCID);
              TempCultureInfo.NumberFormat.NumberDecimalSeparator = CurrentFieldAttribute.DecimalSeparator.ToString();
              TempCultureInfo.NumberFormat.CurrencyDecimalSeparator = CurrentFieldAttribute.DecimalSeparator.ToString();

              float TempFloat = float.Parse(StringValue, TempCultureInfo);
              PropertyInfoItem.SetValue(this, TempFloat, null);
              continue;
            }

            #endregion Float

            #region Double
            if (FieldType == typeof(double)) {

              #region Content of field is empty, return 0
              if (RawFieldContent.Trim() == "") {
                PropertyInfoItem.SetValue(this, 0d, null);
                continue;
              }
              #endregion Content of field is empty, return 0

              string StringValue;
              if (CurrentFieldAttribute.DecimalDigits > 0 && CurrentFieldAttribute.DecimalIsVirtual) {
                string StringValueLeft = RawFieldContent.Left(CurrentFieldAttribute.Length - CurrentFieldAttribute.DecimalDigits);
                string StringValueRight = RawFieldContent.Right(CurrentFieldAttribute.DecimalDigits);
                StringValue = string.Format("{0}{1}{2}", StringValueLeft, CurrentFieldAttribute.DecimalSeparator, StringValueRight);
              } else {
                StringValue = RawFieldContent.Replace(" ", "");
              }

              CultureInfo TempCultureInfo = new CultureInfo(CultureInfo.CurrentCulture.LCID);
              TempCultureInfo.NumberFormat.NumberDecimalSeparator = CurrentFieldAttribute.DecimalSeparator.ToString();
              TempCultureInfo.NumberFormat.CurrencyDecimalSeparator = CurrentFieldAttribute.DecimalSeparator.ToString();

              double TempDouble = double.Parse(StringValue, TempCultureInfo);
              PropertyInfoItem.SetValue(this, TempDouble, null);
              continue;
            }

            #endregion Double

            #region Decimal
            if (FieldType == typeof(decimal)) {

              #region Content of field is empty, return 0
              if (RawFieldContent.Trim() == "") {
                PropertyInfoItem.SetValue(this, 0m, null);
                continue;
              }
              #endregion Content of field is empty, return 0

              string StringValue;
              if (CurrentFieldAttribute.DecimalDigits > 0 && CurrentFieldAttribute.DecimalIsVirtual) {
                string StringValueLeft = RawFieldContent.Left(CurrentFieldAttribute.Length - CurrentFieldAttribute.DecimalDigits);
                string StringValueRight = RawFieldContent.Right(CurrentFieldAttribute.DecimalDigits);
                StringValue = string.Format("{0}{1}{2}", StringValueLeft, CurrentFieldAttribute.DecimalSeparator, StringValueRight);
              } else {
                StringValue = RawFieldContent.Replace(" ", "");
              }

              CultureInfo TempCultureInfo = new CultureInfo(CultureInfo.CurrentCulture.LCID);
              TempCultureInfo.NumberFormat.NumberDecimalSeparator = CurrentFieldAttribute.DecimalSeparator.ToString();
              TempCultureInfo.NumberFormat.CurrencyDecimalSeparator = CurrentFieldAttribute.DecimalSeparator.ToString();

              decimal TempDecimal = decimal.Parse(StringValue, TempCultureInfo);
              PropertyInfoItem.SetValue(this, TempDecimal, null);
              continue;
            }

            #endregion Decimal

            #region Bool
            if (FieldType == typeof(bool)) {
              switch (CurrentFieldAttribute.BoolFormat) {
                case EBoolFormat.OneOrZero:
                case EBoolFormat.TOrF:
                case EBoolFormat.TrueOrFalse:
                case EBoolFormat.YesOrNo:
                case EBoolFormat.YOrN:
                  PropertyInfoItem.SetValue(this, RawFieldContent.ToBool(), null);
                  continue;
                case EBoolFormat.Custom:
                  PropertyInfoItem.SetValue(this, RawFieldContent.ToBool(CurrentFieldAttribute.TrueValue, CurrentFieldAttribute.FalseValue), null);
                  continue;
                default:
                  Trace.WriteLine(string.Format("Error : Attempt to convert unknown value to boolean : {0}", RawFieldContent), Severity.Warning);
                  continue;
              }
            }
            #endregion Bool

            #region DateTime
            if (FieldType == typeof(DateTime)) {

              int Year;
              int Month;
              int Day;
              int Hour;
              int Minute;
              int Second;

              switch (CurrentFieldAttribute.DateTimeFormat) {
                case EDateTimeFormat.DateOnly:
                  if (RawFieldContent.Trim() == "") {
                    if (EmptyDatesValueBlank) {
                      PropertyInfoItem.SetValue(this, DateTime.MinValue, null);
                      break;
                    } else {
                      Trace.WriteLine(string.Format("Error : Attempt to read a empty DateTime while this is not allowed for this record"));
                      break;
                    }
                  }
                  Year = Int32.Parse(RawFieldContent.Left(4));
                  Month = Int32.Parse(RawFieldContent.Substring(4, 2));
                  Day = Int32.Parse(RawFieldContent.Right(2));
                  PropertyInfoItem.SetValue(this, new DateTime(Year, Month, Day), null);
                  break;
                case EDateTimeFormat.TimeOnly:
                  Hour = Int32.Parse(RawFieldContent.Left(2));
                  Minute = Int32.Parse(RawFieldContent.Substring(2, 2));
                  Second = Int32.Parse(RawFieldContent.Substring(4, 2));
                  PropertyInfoItem.SetValue(this, new DateTime(1, 1, 1, Hour, Minute, Second), null);
                  break;
                case EDateTimeFormat.Custom:
                  DateTime TempValue = DateTime.ParseExact(RawFieldContent, CurrentFieldAttribute.DateTimeFormatCustom, CultureInfo.InvariantCulture);
                  PropertyInfoItem.SetValue(this, TempValue, null);
                  break;
                case EDateTimeFormat.DateAndTime:
                default:
                  Year = Int32.Parse(RawFieldContent.Left(4));
                  Month = Int32.Parse(RawFieldContent.Substring(4, 2));
                  Day = Int32.Parse(RawFieldContent.Substring(6, 2));
                  Hour = Int32.Parse(RawFieldContent.Substring(8, 2));
                  Minute = Int32.Parse(RawFieldContent.Substring(10, 2));
                  Second = Int32.Parse(RawFieldContent.Substring(12, 2));
                  PropertyInfoItem.SetValue(this, new DateTime(Year, Month, Day, Hour, Minute, Second), null);
                  break;
              }
              continue;
            }
            #endregion DateTime

            #region Default case : string
            PropertyInfoItem.SetValue(this, Convert.ChangeType(RawFieldContent.TrimEnd(), FieldType), null);
            #endregion Default case : string

          } catch (Exception ex) {
            Trace.WriteLine(string.Format("Error : {0}", ex.Message));
            continue;
          }
        }
      }
    }
    #endregion Public methods

  }



}
