using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BLTools.Data;
using System.Reflection;
using System.Globalization;
using System.Security.Permissions;
using System.Security;

namespace BLTools.Data.Csv {
  /// <summary>
  /// Describes a CSV datafile
  /// </summary>
  /// <typeparam name="T">The type of TCsvRecord of this data file</typeparam>
  public class TCsvDataFile<T> : List<T>, IDisposable where T : TCsvRecordX, new() {

    private const char DEFAULT_SEPARATOR = ';';

    #region Public properties
    /// <summary>
    /// The name of the file
    /// </summary>
    public string Name { get; set; }
    /// <summary>
    /// The type of the record contained in the file
    /// </summary>
    public Type RecordType { get; set; }
    /// <summary>
    /// Does the file has headers in the first line
    /// </summary>
    public bool HasHeaders {
      get {
        return _HasHeaders;
      }
      set {
        _HasHeaders = value;
        if (_HasHeaders) {
          HeaderColumns.Clear();
          int i = 0;
          foreach (PropertyInfo PropertyItem in CsvProperties) {
            i++;
            string VarName = PropertyItem.Name;
            TCsvDataFieldAttribute CsvAttribute = ((TCsvDataFieldAttribute)Attribute.GetCustomAttribute(PropertyItem, typeof(TCsvDataFieldAttribute)));
            string FieldName = CsvAttribute.FieldName;
            int FieldPosition = CsvAttribute.FieldPosition;
            HeaderColumns.Add(string.IsNullOrWhiteSpace(FieldName) ? VarName: FieldName, FieldPosition == -1 ? i : FieldPosition);
          }
        }
      }
    }
    private bool _HasHeaders;
    /// <summary>
    /// The encoding of the file
    /// </summary>
    public Encoding DataEncoding { get; set; }
    /// <summary>
    /// Are the empty dates left blank or filled with a specific value
    /// </summary>
    public bool EmptyDatesValueBlank { get; set; }
    /// <summary>
    /// The character used as separator between fields
    /// </summary>
    public char Separator {
      get {
        if (_Separator == '\0') {
          return DEFAULT_SEPARATOR;
        }
        return _Separator;
      }
      set {
        _Separator = value;
      }
    }
    private char _Separator = '\0';
    /// <summary>
    /// Are the strings in the file surrounded with a quote sign
    /// </summary>
    public bool SurroundString { get; set; }
    #endregion Public properties

    #region Private variables
    //private FileStream _Stream;
    //private BufferedStream _BufferedStream;
    private TextReader _StreamReader;
    private TextWriter _StreamWriter;
    private bool _IsOpened;
    private EOpenMode _FileOpenMode;
    private readonly Dictionary<string, int> HeaderColumns;
    /// <summary>
    /// The list of CSV fields properties for the associated TCsvRecord
    /// </summary>
    protected List<PropertyInfo> CsvProperties {
      get {
        return typeof(T).GetProperties().Where(x => x.HasAttribute(typeof(TCsvDataFieldAttribute))).ToList();
      }
    }
    #endregion Private variables

    #region Constructor(s)
    /// <summary>
    /// Blank constructor, all values to default
    /// </summary>
    public TCsvDataFile()
      : base() {
      HeaderColumns = new Dictionary<string, int>();
      Name = "";
      RecordType = typeof(T);
      HasHeaders = true;
      DataEncoding = Encoding.Default;
      EmptyDatesValueBlank = true;
      SurroundString = true;
    }

    /// <summary>
    /// Builds the object with the name of the file
    /// </summary>
    /// <param name="name">The name of the file</param>
    public TCsvDataFile(string name)
      : this() {
      Name = name;
    }

    /// <summary>
    /// Builds the object with name of the file and a specific encoding
    /// </summary>
    /// <param name="name">The name of the file</param>
    /// <param name="encoding">The encoding used to read/write the file</param>
    public TCsvDataFile(string name, Encoding encoding)
      : this(name) {
      DataEncoding = encoding;
    }

    /// <summary>
    /// Builds the object from another TCsvDataFile
    /// </summary>
    /// <param name="csvDataFile">Another TCsvDataFile&lt;T&gt;</param>
    public TCsvDataFile(TCsvDataFile<T> csvDataFile) : this() {
      Name = csvDataFile.Name;
      HeaderColumns = new Dictionary<string, int>();
      RecordType = csvDataFile.RecordType;
      HasHeaders = csvDataFile.HasHeaders;
      DataEncoding = csvDataFile.DataEncoding;
      EmptyDatesValueBlank = csvDataFile.EmptyDatesValueBlank;
      SurroundString = csvDataFile.SurroundString;
    }

    /// <summary>
    /// Cleanup
    /// </summary>
    public void Dispose() {
      HeaderColumns.Clear();
      if (_IsOpened) {
        Close();
      }
    }
    #endregion Constructor(s)

    #region I/O operations
    /// <summary>
    /// Initialize the file (delete it if it exists)
    /// </summary>
    public void Init() {
      if (string.IsNullOrWhiteSpace(Name)) {
        return;
      }
      if (File.Exists(Name)) {
        try {
          File.Delete(Name);
        } catch (Exception ex) {
          Trace.WriteLine(string.Format("Unable to delete file \"{0}\" : {1}", Name, ex.Message));
        }
      }
    }

    /// <summary>
    /// Open the file
    /// </summary>
    /// <param name="mode">Specifiy whether the file is to be opened R/O or R/W</param>
    public void Open(EOpenMode mode) {
      if (_IsOpened) {
        return;
      }
      try {
        switch (mode) {
          case EOpenMode.Read:
            //_Stream = new FileStream(Name, FileMode.Open, FileAccess.Read, FileShare.Read);
            //_BufferedStream = new BufferedStream(_Stream);
            //_StreamReader = new StreamReader(_Stream, DataEncoding);
            _StreamReader = new StreamReader(Name, DataEncoding);
            _FileOpenMode = EOpenMode.Read;
            break;
          case EOpenMode.Create:
            //_Stream = new FileStream(Name, FileMode.Create, FileAccess.Write, FileShare.ReadWrite);
            //_BufferedStream = new BufferedStream(_Stream);
            //_StreamWriter = new StreamWriter(_Stream, DataEncoding);
            _StreamWriter = new StreamWriter(Name, false, DataEncoding);
            _FileOpenMode = EOpenMode.Create;
            break;
          case EOpenMode.Append:
            FileIOPermission AppendPermission = new FileIOPermission(FileIOPermissionAccess.Append, Path.GetFullPath(Name));
            try {
              AppendPermission.Demand();
              //_Stream = new FileStream(Name, FileMode.Append, FileAccess.Write, FileShare.ReadWrite);
              //_BufferedStream = new BufferedStream(_Stream);
              //_StreamWriter = new StreamWriter((_Stream, DataEncoding);
              _StreamWriter = new StreamWriter(Name, true, DataEncoding);
              _FileOpenMode = EOpenMode.Append;
            } catch (SecurityException ex) {
              Trace.WriteLine(string.Format("Append Access right not granted for file {0} : {1}", Name, ex.Message));
              _IsOpened = false;
              return;
            }
            break;
        }
        _IsOpened = true;
      } catch (Exception ex) {
        Trace.WriteLine(string.Format("Error opening file : {0}", ex.Message));
        _IsOpened = false;
      }
    }

    /// <summary>
    /// Close the file
    /// </summary>
    public void Close() {
      if (!_IsOpened) {
        return;
      }
      if (_IsOpened) {
        switch (_FileOpenMode) {
          case EOpenMode.Read:
            if (_StreamReader != null) {
              _StreamReader.Close();
            }

            break;
          case EOpenMode.Create:
          case EOpenMode.Append:
            if (_StreamWriter != null) {
              _StreamWriter.Flush();
              _StreamWriter.Close();
            }
            break;
        }
        //if (_BufferedStream != null) {
        //  _BufferedStream.Close();
        //}
        //if (_Stream != null) {
        //  _Stream.Close();
        //}
        _IsOpened = false;
      }
    }
    #endregion I/O operations

    #region Reading data
    /// <summary>
    /// Read the headers of the columns from the TCsvDataFile
    /// </summary>
    protected virtual void ReadHeaderColumns() {
      #region Validate parameters
      if (string.IsNullOrWhiteSpace(Name)) {
        Trace.WriteLine("Unable to read file : filename is missing", Severity.Error);
        return;
      }
      if (!File.Exists(Name)) {
        Trace.WriteLine("Error : Unable to read file : the file is missing or access is denied");
        return;
      }
      #endregion Validate parameters

      bool LocalOpen = false;

      try {
        if (!_IsOpened) {
          Open(EOpenMode.Read);
          LocalOpen = true;
        }
        if (HasHeaders) {
          string HeaderLine = _StreamReader.ReadLine();
          int i = 0;
          HeaderColumns.Clear();
          foreach (string HeaderItem in HeaderLine.Split(Separator)) {
            HeaderColumns.Add(HeaderItem.Trim('"').Trim().ToLower(), i++);
          }
        }
      } catch (Exception ex) {
        Trace.WriteLine(string.Format("Unable to read header columns from file {0} : {1}", Name, ex.Message));
      } finally {
        if (LocalOpen) {
          Close();
        }
      }
    }

    /// <summary>
    /// Read the content of a TCsvDataFile
    /// </summary>
    public virtual void Read() {
      #region Validate parameters
      if (string.IsNullOrWhiteSpace(Name)) {
        Trace.WriteLine("Unable to read file : filename is missing", Severity.Error);
        return;
      }
      if (!File.Exists(Name)) {
        Trace.WriteLine("Error : Unable to read file : the file is missing or access is denied");
        return;
      }
      #endregion Validate parameters

      string CsvRow = "";
      try {
        Open(EOpenMode.Read);

        if (HasHeaders) {
          ReadHeaderColumns();
        }

        while ((CsvRow = _StreamReader.ReadLine()) != null) {
          Add(ParseCsv(CsvRow));
        }
      } catch (Exception ex) {
        Trace.WriteLine(string.Format("Error while reading data from file {0} : {1} : {2}", Name, ex.Message, CsvRow));
      } finally {
        Close();
      }

    }

    /// <summary>
    /// Convert a row from its string representation to the field values in a TCsvRecord  
    /// </summary>
    /// <param name="CsvRow">The row as a string</param>
    /// <returns>The correctly typed TCsvRecord filled with values</returns>
    protected T ParseCsv(string CsvRow) {
      #region Validate parameters
      if (string.IsNullOrWhiteSpace(CsvRow)) {
        return default(T);
      }
      #endregion Validate parameters

      //TODO: check for inner ; or "
      string[] Items = CsvRow.Split(Separator);

      T RetVal = new T();

      foreach (PropertyInfo PropertyInfoItem in CsvProperties) {

        TCsvDataFieldAttribute CurrentFieldAttribute = (TCsvDataFieldAttribute)Attribute.GetCustomAttribute(PropertyInfoItem, typeof(TCsvDataFieldAttribute));

        #region Check if column exists
        int ColumnLocation = -1;

        if (HasHeaders) {
          if (HeaderColumns.Keys.Contains(PropertyInfoItem.Name.Trim().ToLower())) {
            ColumnLocation = HeaderColumns[PropertyInfoItem.Name.Trim().ToLower()];
          } else {
            if (HeaderColumns.Keys.Contains(CurrentFieldAttribute.FieldName.Trim().ToLower())) {
              ColumnLocation = HeaderColumns[CurrentFieldAttribute.FieldName.Trim().ToLower()];
            }
          }
        } else {
          ColumnLocation = CurrentFieldAttribute.FieldPosition;
        }

        if (ColumnLocation == -1) {
          continue;
        }
        #endregion Check if column exists

        Type FieldType = PropertyInfoItem.PropertyType;
        string RawFieldContent = Items[ColumnLocation];

        try {
          PropertyInfoItem.SetValue(RetVal, ConvertValueFromString(RawFieldContent, FieldType, CurrentFieldAttribute), null);
        } catch (Exception ex) {
          Trace.WriteLine(string.Format("Conversion error : {0}", ex.Message));
        }

      }

      return RetVal;
    }

    /// <summary>
    /// Convert a value from its string representation to its real value, based on type and attribute parameters
    /// </summary>
    /// <param name="sourceValue">The source string</param>
    /// <param name="newType">The requested type</param>
    /// <param name="attribute">The attribute of the field</param>
    /// <returns>The converted value</returns>
    protected object ConvertValueFromString(string sourceValue, Type newType, TCsvDataFieldAttribute attribute) {

      try {

        #region String
        if (newType == typeof(string)) {
          return sourceValue;
        }
        #endregion String

        #region Int
        if (newType == typeof(int)) {
          try {
            return int.Parse(sourceValue.Trim());
          } catch {
            return 0;
          }
        }
        #endregion Int

        #region Long
        if (newType == typeof(long)) {
          try {
            return long.Parse(sourceValue.Trim());
          } catch {
            return 0;
          }
        }
        #endregion Long

        #region Float
        if (newType == typeof(float)) {
          if (sourceValue.Trim() == "") {
            return 0f;
          } else {
            try {
              return float.Parse(sourceValue.Trim());
            } catch {
              return 0f;
            }
          }
        }
        #endregion Float

        #region Double
        if (newType == typeof(double)) {
          if (sourceValue.Trim() == "") {
            return 0d;
          } else {
            try {
              return double.Parse(sourceValue.Trim());
            } catch {
              return 0d;
            }
          }
        }
        #endregion Double

        #region Decimal
        if (newType == typeof(decimal)) {
          if (sourceValue.Trim() == "") {
            return 0m;
          } else {
            try {
              return decimal.Parse(sourceValue.Trim());
            } catch {
              return 0m;
            }
          }
        }
        #endregion Decimal

        #region Bool
        if (newType == typeof(bool)) {
          switch (attribute.BoolFormat) {
            case EBoolFormat.OneOrZero:
            case EBoolFormat.TOrF:
            case EBoolFormat.TrueOrFalse:
            case EBoolFormat.YesOrNo:
            case EBoolFormat.YOrN:
              return sourceValue.ToBool();
            case EBoolFormat.Custom:
              return sourceValue.ToBool(attribute.TrueValue, attribute.FalseValue);
            default:
              Trace.WriteLine(string.Format("Error : Attempt to convert unknown value to boolean : {0}", sourceValue), Severity.Warning);
              return false;
          }
        }
        #endregion Bool

        #region DateTime
        if (newType == typeof(DateTime)) {

          int Year;
          int Month;
          int Day;
          int Hour;
          int Minute;
          int Second;

          switch (attribute.DateTimeFormat) {
            case EDateTimeFormat.DateOnly:
              if (sourceValue.Trim() == "") {
                if (EmptyDatesValueBlank) {
                  return DateTime.MinValue;
                } else {
                  Trace.WriteLine(string.Format("Error : Attempt to read a empty DateTime while this is not allowed for this record"));
                  return DateTime.MinValue;
                }
              }
              Year = Int32.Parse(sourceValue.Left(4));
              Month = Int32.Parse(sourceValue.Substring(4, 2));
              Day = Int32.Parse(sourceValue.Right(2));
              return new DateTime(Year, Month, Day);
            case EDateTimeFormat.TimeOnly:
              Hour = Int32.Parse(sourceValue.Left(2));
              Minute = Int32.Parse(sourceValue.Substring(2, 2));
              Second = Int32.Parse(sourceValue.Substring(4, 2));
              return new DateTime(1, 1, 1, Hour, Minute, Second);
            case EDateTimeFormat.Custom:
              throw new ApplicationException("Custom Datetime format are not supported for reading");
            case EDateTimeFormat.DateAndTime:
            default:
              Year = Int32.Parse(sourceValue.Left(4));
              Month = Int32.Parse(sourceValue.Substring(4, 2));
              Day = Int32.Parse(sourceValue.Substring(6, 2));
              Hour = Int32.Parse(sourceValue.Substring(8, 2));
              Minute = Int32.Parse(sourceValue.Substring(10, 2));
              Second = Int32.Parse(sourceValue.Substring(12, 2));
              return new DateTime(Year, Month, Day, Hour, Minute, Second);
          }
        }
        #endregion DateTime

        return sourceValue;

      } catch (Exception ex) {
        Trace.WriteLine(string.Format("Conversion error : {0} => {1} : {2}", sourceValue, newType.Name, ex.Message));
        return newType.GetConstructor(System.Type.EmptyTypes).Invoke(null);
      }
    }

    #endregion Reading data

    #region Saving data
    /// <summary>
    /// Write the headers of the columns to the file
    /// </summary>
    protected virtual void WriteHeaderColumns() {
      #region Validate parameters
      if (string.IsNullOrWhiteSpace(Name)) {
        Trace.WriteLine("Unable to write file : filename is missing", Severity.Error);
        return;
      }
      #endregion Validate parameters

      bool LocalOpen = false;

      try {
        if (!_IsOpened) {
          Open(EOpenMode.Create);
          LocalOpen = true;
        }
        if (HasHeaders) {
          StringBuilder HeaderLine = new StringBuilder();
          foreach (string HeaderItem in HeaderColumns.OrderBy(x => x.Value).Select(x => x.Key)) {
            HeaderLine.AppendFormat("\"{0}\"{1}", HeaderItem, Separator);
          }
          _StreamWriter.WriteLine(HeaderLine.Truncate(1).ToString());
        }
      } catch (Exception ex) {
        Trace.WriteLine(string.Format("Unable to write header columns to file {0} : {1}", Name, ex.Message));
      } finally {
        if (LocalOpen) {
          Close();
        }
      }
    }

    /// <summary>
    /// Save the content of the list to a file
    /// </summary>
    public void Save(bool append = false) {
      #region Validate parameters
      if (string.IsNullOrWhiteSpace(Name)) {
        Trace.WriteLine("Unable to save file : filename is missing", Severity.Error);
        return;
      }
      #endregion Validate parameters

      try {
        if (append) {
          bool IsFileEmpty = true;
          if (File.Exists(Name) && (new FileInfo(Name)).Length > 0) {
            IsFileEmpty = false;
          }
          Open(EOpenMode.Append);
          if (HasHeaders && IsFileEmpty) {
            WriteHeaderColumns();
          }
        } else {
          Open(EOpenMode.Create);
          if (HasHeaders) {
            WriteHeaderColumns();
          }
        }

        foreach (T CsvRowItem in this) {
          Trace.WriteLine(BuildCsv(CsvRowItem));
          _StreamWriter.WriteLine(BuildCsv(CsvRowItem));
          _StreamWriter.Flush();
        }

      } catch (Exception ex) {
        Trace.WriteLine(string.Format("Error while saving data to file {0} : {1}", Name, ex.Message));
      } finally {
        Close();
      }
    }

    /// <summary>
    /// Convert a TCsvRecord to a string
    /// </summary>
    /// <param name="CsvRow">Any TCsvRecord</param>
    /// <returns>The string to write to the file</returns>
    protected string BuildCsv(T CsvRow) {
      #region Validate parameters
      if (CsvRow == null) {
        return null;
      }
      #endregion Validate parameters

      Dictionary<int, string> OutputColumns = new Dictionary<int, string>();

      foreach (PropertyInfo PropertyInfoItem in CsvProperties) {
        TCsvDataFieldAttribute CurrentFieldAttribute = (TCsvDataFieldAttribute)Attribute.GetCustomAttribute(PropertyInfoItem, typeof(TCsvDataFieldAttribute));
        #region Check if column exists
        int ColumnLocation = -1;

        if (HasHeaders) {
          if (HeaderColumns.Keys.Contains(PropertyInfoItem.Name.Trim(), StringComparer.InvariantCultureIgnoreCase)) {
            ColumnLocation = HeaderColumns[PropertyInfoItem.Name.Trim()];
          } else {
            if (HeaderColumns.Keys.Contains(CurrentFieldAttribute.FieldName.Trim(), StringComparer.InvariantCultureIgnoreCase)) {
              ColumnLocation = HeaderColumns[CurrentFieldAttribute.FieldName.Trim()];
            }
          }
        } else {
          ColumnLocation = CurrentFieldAttribute.FieldPosition;
        }

        if (ColumnLocation == -1) {
          continue;
        }
        #endregion Check if column exists
        OutputColumns.Add(ColumnLocation, ConvertValueToString(PropertyInfoItem.GetValue(CsvRow, null), PropertyInfoItem.PropertyType, CurrentFieldAttribute));
      }

      StringBuilder RetVal = new StringBuilder();
      foreach(KeyValuePair<int, string> ColumnItem in OutputColumns.OrderBy(x=>x.Key)) { 
        RetVal.Append(ColumnItem.Value);
        RetVal.Append(Separator);
      }

      return RetVal.Truncate(1).ToString();
    }

    /// <summary>
    /// Convert a value of a certain type to its string representation, based on rules of the data file (surround string with quotes, ...)
    /// </summary>
    /// <param name="sourceValue">The source value</param>
    /// <param name="actualType">The type of the source</param>
    /// <param name="attribute">The attribute of the current field</param>
    /// <returns>The value converted to string</returns>
    protected string ConvertValueToString(object sourceValue, Type actualType, TCsvDataFieldAttribute attribute) {

      #region String
      if (actualType == typeof(string)) {
        if (SurroundString) {
          return string.Format("\"{0}\"", (string)sourceValue);
        } else {
          return (string)sourceValue;
        }
      }
      #endregion String

      #region Int and long
      if (actualType == typeof(int) || actualType == typeof(long)) {
        return sourceValue.ToString();
      }
      #endregion Int and long

      #region Float
      if (actualType == typeof(float)) {
        string FormatNumber;
        if (attribute.DecimalDigits > 0) {
          FormatNumber = "#." + new string('#', attribute.DecimalDigits);
        } else {
          FormatNumber = "#";
        }
        return ((float)sourceValue).ToString(FormatNumber);
      }
      #endregion Float

      #region Double
      if (actualType == typeof(double)) {
        string FormatNumber;
        if (attribute.DecimalDigits > 0) {
          FormatNumber = "#." + new string('#', attribute.DecimalDigits);
        } else {
          FormatNumber = "#";
        }
        return ((double)sourceValue).ToString(FormatNumber);
      }
      #endregion Double

      #region Decimal
      if (actualType == typeof(decimal)) {
        string FormatNumber;
        if (attribute.DecimalDigits > 0) {
          FormatNumber = "#." + new string('#', attribute.DecimalDigits);
        } else {
          FormatNumber = "#";
        }
        return ((decimal)sourceValue).ToString(FormatNumber);
      }
      #endregion Decimal

      #region Bool
      if (actualType == typeof(bool)) {
        switch (attribute.BoolFormat) {
          case EBoolFormat.OneOrZero:
            return ((bool)sourceValue ? "1" : "0");
          case EBoolFormat.TOrF:
            return ((bool)sourceValue ? "T" : "F");
          case EBoolFormat.TrueOrFalse:
            return ((bool)sourceValue ? "True" : "False");
          case EBoolFormat.YesOrNo:
            return ((bool)sourceValue ? "Yes" : "No");
          case EBoolFormat.YOrN:
            return ((bool)sourceValue ? "Y" : "N");
          case EBoolFormat.Custom:
            return ((bool)sourceValue ? attribute.TrueValue : attribute.FalseValue);
        }
      }
      #endregion Bool

      #region DateTime
      if (actualType == typeof(DateTime)) {

        DateTime CurrentValue = (DateTime)sourceValue;

        switch (attribute.DateTimeFormat) {
          case EDateTimeFormat.DateOnly:
            return CurrentValue.ToDMY();
          case EDateTimeFormat.TimeOnly:
            return CurrentValue.ToHMS();
          case EDateTimeFormat.Custom:
            return CurrentValue.ToString(attribute.DateTimeFormatCustom);
          case EDateTimeFormat.DateAndTime:
          default:
            return CurrentValue.ToDMYHMS();
        }
      }
      #endregion DateTime

      Trace.WriteLine(string.Format("Unable to convert value {0} from {1} to {2}", sourceValue.ToString(), sourceValue.GetType().Name, actualType.Name));
      return "";

    }
    #endregion Saving data


  }


}
