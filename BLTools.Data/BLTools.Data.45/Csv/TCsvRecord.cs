using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLTools.Data.Csv {
  public class TCsvRecord : List<TCsvColumn> {

    private const string DefaultSeparator = ";";
    public string Separator {
      get {
        if (_Separator == null) {
          return DefaultSeparator;
        }
        return _Separator;
      }
      set {
        _Separator = value;
      }
    }
    private string _Separator = null;

    public bool SurroundString { get; set; }
    public bool EmptyDatesValueBlank { get; set; }

    #region Constructor(s)
    public TCsvRecord()
      : base() {
      SurroundString = true;
      EmptyDatesValueBlank = true;

    }

    public TCsvRecord(IEnumerable<TCsvColumn> columns)
      : this() {
      if (columns == null) {
        return;
      }
      this.AddRange(columns);
    }

    public TCsvRecord(TCsvRecord record)
      : this() {
      this.AddRange(record);
    }
    #endregion Constructor(s)

    #region Converters
    public override string ToString() {
      StringBuilder RetVal = new StringBuilder();
      RetVal.AppendFormat("{0} column(s) : {1}", this.Count, string.Join(", ", this.Select(x => x.Name)));
      return RetVal.ToString();
    }
    #endregion Converters

    public TCsvColumn this[string name] {
      get {
        return this.FirstOrDefault(x => x.Name == name);
      }
    }

    public string ToCsvHeader() {
      StringBuilder RetVal = new StringBuilder();

      foreach (TCsvColumn ColumnItem in this) {
        RetVal.AppendFormat("\"{0}\";", ColumnItem.Name);
      }
      RetVal.Remove(RetVal.Length - 1, 1);

      return RetVal.ToString();

    }

    public string ToCsv() {
      StringBuilder RetVal = new StringBuilder();

      try {
        foreach (TCsvColumn ColumnItem in this) {

          #region Error
          if (ColumnItem.Value == null || ColumnItem.ValueType == null) {
            Trace.WriteLine(string.Format("Error in value {0} : null or type is null", ColumnItem.Name ?? ""));
            continue;
          } 
          #endregion Error

          #region DateTime
          if (ColumnItem.ValueType == typeof(DateTime)) {
            if ((DateTime)ColumnItem.Value == DateTime.MinValue && EmptyDatesValueBlank) {
              RetVal.Append(";");
              continue;
            }
            switch (ColumnItem.DateTimeFormat) {
              case EDateTimeFormat.DateOnly:
                RetVal.AppendFormat("{0};", ((DateTime)ColumnItem.Value).ToString("yyyy-MM-dd"));
                break;
              case EDateTimeFormat.TimeOnly:
                RetVal.AppendFormat("{0};", ((DateTime)ColumnItem.Value).ToString("HH:mm:ss"));
                break;
              case EDateTimeFormat.Custom:
                RetVal.AppendFormat("{0};", ((DateTime)ColumnItem.Value).ToString(ColumnItem.DateTimeFormatCustom));
                break;
              case EDateTimeFormat.DateAndTime:
              default:
                RetVal.AppendFormat("{0};", ((DateTime)ColumnItem.Value).ToString("yyyy-MM-dd HH:mm:ss"));
                break;
            }
            continue;
          }
          #endregion DateTime

          #region Bool
          if (ColumnItem.ValueType == typeof(bool)) {
            switch (ColumnItem.BoolFormat) {
              case EBoolFormat.OneOrZero:
                RetVal.AppendFormat("{0};", (bool)ColumnItem.Value ? "1" : "0");
                break;
              case EBoolFormat.TOrF:
                RetVal.AppendFormat("{0};", (bool)ColumnItem.Value ? "T" : "F");
                break;
              case EBoolFormat.TrueOrFalse:
                RetVal.AppendFormat("{0};", (bool)ColumnItem.Value ? "True" : "False");
                break;
              case EBoolFormat.YesOrNo:
                RetVal.AppendFormat("{0};", (bool)ColumnItem.Value ? "Yes" : "No");
                break;
              case EBoolFormat.YOrN:
                RetVal.AppendFormat("{0};", (bool)ColumnItem.Value ? "Y" : "N");
                break;
              case EBoolFormat.Custom:
                RetVal.AppendFormat("{0};", (bool)ColumnItem.Value ? ColumnItem.TrueValue : ColumnItem.FalseValue);
                break;
            }
            continue;
          }
          #endregion Bool

          #region Int and Long
          if (ColumnItem.ValueType == typeof(int) || ColumnItem.ValueType == typeof(long)) {
            RetVal.AppendFormat("{0};", ColumnItem.Value);
            continue;
          }
          #endregion Int and Long

          #region Float, Double and Decimal
          if (ColumnItem.ValueType == typeof(float) || ColumnItem.ValueType == typeof(double) || ColumnItem.ValueType == typeof(decimal)) {
            string FormatNumber;
            if (ColumnItem.DecimalDigits > 0) {
              FormatNumber = "#." + new string('#', ColumnItem.DecimalDigits);
            } else {
              FormatNumber = "#";
            }
            if (ColumnItem.ValueType == typeof(float)) {
              RetVal.AppendFormat("{0};", ((float)ColumnItem.Value).ToString(FormatNumber));
            }
            if (ColumnItem.ValueType == typeof(double)) {
              RetVal.AppendFormat("{0};", ((double)ColumnItem.Value).ToString(FormatNumber));
            }
            if (ColumnItem.ValueType == typeof(decimal)) {
              RetVal.AppendFormat("{0};", ((decimal)ColumnItem.Value).ToString(FormatNumber));
            }
            continue;
          }
          #endregion Float, Double and Decimal

          #region String
          if (SurroundString) {
            RetVal.AppendFormat("\"{0}\";", ColumnItem.Value);
          } else {
            RetVal.AppendFormat("{0};", ColumnItem.Value);
          }
          continue;
          #endregion String

        }
      } catch (Exception ex) {
        Trace.WriteLine(string.Format("Error : {0}", ex.Message));
      }

      RetVal.Remove(RetVal.Length - Separator.Length, Separator.Length); // Remove last separator

      return RetVal.ToString();
    }

    public TCsvRecord FromCsv(string csv) {

      #region Validate parameters
      if (string.IsNullOrWhiteSpace(csv)) {
        Trace.WriteLine("Unable to parse csv, string is null or empty");
        return null;
      }
      string[] RowItems = csv.Split(';');
      if (RowItems.Count() != this.Count) {
        Trace.WriteLine(string.Format("Number of columns is invalid : {0} for schema, {1} for data", this.Count, RowItems.Count()));
        return null;
      } 
      #endregion Validate parameters

      try {

        int i = 0;
        foreach (TCsvColumn ColumnItem in this) {
          #region Error
          if (ColumnItem.ValueType == null) {
            Trace.WriteLine(string.Format("Column type is missing : {0}", ColumnItem.Name ?? ""));
            continue;
          }
          #endregion Error

          string CurrentItem = RowItems[i++];

          #region DateTime
          if (ColumnItem.ValueType == typeof(DateTime)) {
            ColumnItem.Value = BLConverter.BLConvert<DateTime>(CurrentItem, CultureInfo.CurrentCulture, DateTime.MinValue);
            continue;
          }
          #endregion DateTime

          #region Bool
          if (ColumnItem.ValueType == typeof(bool)) {
            ColumnItem.Value = BLConverter.BLConvert<bool>(CurrentItem, CultureInfo.CurrentCulture, false);
            continue;
          }
          #endregion Bool

          #region Int and Long
          if (ColumnItem.ValueType == typeof(int) || ColumnItem.ValueType == typeof(long)) {
            ColumnItem.Value = BLConverter.BLConvert<long>(CurrentItem, CultureInfo.CurrentCulture, 0);
            continue;
          }
          #endregion Int and Long

          #region Float, Double and Decimal
          if (ColumnItem.ValueType == typeof(float) || ColumnItem.ValueType == typeof(double) || ColumnItem.ValueType == typeof(decimal)) {
            ColumnItem.Value = BLConverter.BLConvert<double>(CurrentItem, CultureInfo.CurrentCulture, 0d);
            continue;
          }
          #endregion Float, Double and Decimal

          #region String
          if (SurroundString) {
            ColumnItem.Value = CurrentItem.Trim('"');
          } else {
            ColumnItem.Value = CurrentItem;
          }
          continue;
          #endregion String
        }
      } catch (Exception ex) {
        Trace.WriteLine(string.Format("Error : {0}", ex.Message));
      }

      return this;

    }
  }
}
