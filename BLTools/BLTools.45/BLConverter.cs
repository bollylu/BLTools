using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;
using System.Diagnostics;

namespace BLTools {
  public static class BLConverter {

    /// <summary>
    /// Set to true to obtain additional debug info
    /// </summary>
    public static bool TraceError = false;

    /// <summary>
    /// Convert a value from one type to another (possibly through an evaluation of the value : e.g. "0", "True", "T" all becomes True)
    /// </summary>
    /// <typeparam name="T">Requested output type</typeparam>
    /// <param name="source">Data source</param>
    /// <param name="culture">Culture used to performed certain conversions</param>
    /// <param name="defaultValue">What to return when unable to convert</param>
    /// <returns>The source value converted to a new type</returns>
    public static T BLConvert<T>(object source, CultureInfo culture, T defaultValue) {
      try {

        if ( source.GetType().Name == typeof(T).Name ) {
          return (T)source;
        }

        if ( culture == null ) {
          culture = CultureInfo.CurrentCulture;
        }

        switch ( typeof(T).Name ) {
          case "Double":
          case "Single":
            if ( source is string ) {
              string TestSource = source as string;
              char DecimalSeparator = culture.NumberFormat.NumberDecimalSeparator[0];
              if ( TestSource.Count(x => !x.IsNumeric() && x != DecimalSeparator) > 0 ) {
                _OutputError(string.Format("Bad format for conversion : {0} : unknown non numeric characters", TestSource));
                return defaultValue;
              }
              if ( TestSource.Count(x => !x.IsNumeric()) > 1 ) {
                _OutputError(string.Format("Bad format for conversion : {0} : too many non numeric characters", TestSource));
                return defaultValue;
              }
              if ( TestSource.Count(x => x == DecimalSeparator) > 1 ) {
                _OutputError(string.Format("Bad format for conversion : {0} : too many decimal separators ({1})", (string)source, DecimalSeparator));
                return defaultValue;
              }
            }
            return (T)Convert.ChangeType(source, typeof(T), culture.NumberFormat);

          case "DateTime":
            return (T)Convert.ChangeType(source, typeof(T), culture.DateTimeFormat);

          case "String[]":
            if ( !( source is string ) ) {
              _OutputError(string.Format("Bad format for conversion to string[] : {0} : source is not a string", source.GetType().Name));
              return defaultValue;
            }
            return (T)Convert.ChangeType(( (string)source ).Split(SplitArgs.Separator), typeof(T));

          case "Int32[]": {
              if ( !( source is string ) ) {
                _OutputError(string.Format("Bad format for conversion to int32[] : {0} : source is not a string", source.GetType().Name));
                return defaultValue;
              }
              string[] aTemp = ( (string)source ).Split(SplitArgs.Separator);
              List<int> RetVal = new List<int>();
              foreach ( string Item in aTemp ) {
                RetVal.Add(Int32.Parse(Item));
              }
              return (T)Convert.ChangeType(RetVal.ToArray(), typeof(T));
            }

          case "Int64[]": {
              if ( !( source is string ) ) {
                _OutputError(string.Format("Bad format for conversion to int64[] : {0} : source is not a string", source.GetType().Name));
                return defaultValue;
              }
              string[] aTemp = ( (string)source ).Split(SplitArgs.Separator);
              List<long> RetVal = new List<long>();
              foreach ( string Item in aTemp ) {
                RetVal.Add(long.Parse(Item));
              }
              return (T)Convert.ChangeType(RetVal.ToArray(), typeof(T));
            }

          case "Double[]": {
              if ( !( source is string ) ) {
                _OutputError(string.Format("Bad format for conversion to double[] : {0} : source is not a string", source.GetType().Name));
                return defaultValue;
              }
              string[] aTemp = ( (string)source ).Split(SplitArgs.Separator);
              List<double> RetVal = new List<double>();
              foreach ( string Item in aTemp ) {
                RetVal.Add(double.Parse(Item, culture.NumberFormat));
              }
              return (T)Convert.ChangeType(RetVal.ToArray(), typeof(T));
            }

          case "Single[]": {
              if ( !( source is string ) ) {
                _OutputError(string.Format("Bad format for conversion to float[] : {0} : source is not a string", source.GetType().Name));
                return defaultValue;
              }
              string[] aTemp = ( (string)source ).Split(SplitArgs.Separator);
              List<float> RetVal = new List<float>();
              foreach ( string Item in aTemp ) {
                RetVal.Add(float.Parse(Item, culture.NumberFormat));
              }
              return (T)Convert.ChangeType(RetVal.ToArray(), typeof(T));
            }

          case "Boolean":
            if ( source is string ) {
              return (T)Convert.ChangeType(( (string)source ).ToBool(), typeof(T));
            }
            switch ( source.GetType().Name ) {
              case "Int16":
              case "Int32":
              case "Int64":
              case "SByte":
                Int64 SignedSourceValue = (Int64)source;
                return (T)Convert.ChangeType(SignedSourceValue == 1 ? true : false, typeof(T));
              case "UInt16":
              case "UInt32":
              case "UInt64":
              case "Byte":
                UInt64 UnsignedSourceValue = (UInt64)source;
                return (T)Convert.ChangeType(UnsignedSourceValue == 1 ? true : false, typeof(T));
            }
            _OutputError(string.Format("Error during conversion of \"{0}\" to {1} : {2} is an unhandled source type", source, typeof(T).Name, source.GetType().Name));
            return defaultValue;

          case "Guid":
            if ( source is string ) {
              if ( ( (string)source ).Trim() == "" ) {
                return (T)Convert.ChangeType(new Guid(), typeof(T));
              }
              return (T)Convert.ChangeType(new Guid((string)source), typeof(T));
            }
            if ( source is Guid ) {
              return (T)Convert.ChangeType(source, typeof(T));
            }
            return defaultValue;

          default:
            return (T)Convert.ChangeType(source, typeof(T));

        }
      } catch ( Exception ex ) {
        _OutputError(string.Format("Error during conversion of \"{0}\" to {1} : {2}", source, typeof(T).Name, ex.Message));
        return defaultValue;
      }
    }

    private static void _OutputError(string message) {
      if ( TraceError ) {
        Trace.WriteLine(message, Severity.Error);
      }
    }
  }
}
