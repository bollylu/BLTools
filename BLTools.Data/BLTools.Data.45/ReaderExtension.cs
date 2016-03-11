using System;
using System.Data;
using System.Diagnostics;
using System.Globalization;

namespace BLTools.Data {
  public static class ReaderExtension {

    /// <summary>
    /// Read a field from an IDataReader, allowing a default value  in case the field is null or an exception occurs during the read.
    /// </summary>
    /// <typeparam name="T">The type of the return value</typeparam>
    /// <param name="reader">The DataReader</param>
    /// <param name="fieldName">The name of the field</param>
    /// <returns>Either the value of the field converted to the correct type, or the default value</returns>
    public static T SafeRead<T>(this IDataReader reader, string fieldName) {
      return SafeRead<T>(reader, fieldName, default(T), CultureInfo.CurrentCulture);
    }

    /// <summary>
    /// Read a field from an IDataReader, allowing a default value  in case the field is null or an exception occurs during the read.
    /// </summary>
    /// <typeparam name="T">The type of the return value</typeparam>
    /// <param name="reader">The DataReader</param>
    /// <param name="fieldName">The name of the field</param>
    /// <param name="defaultValue">The default value</param>
    /// <returns>Either the value of the field converted to the correct type, or the default value</returns>
    public static T SafeRead<T>(this IDataReader reader, string fieldName, T defaultValue) {
      return SafeRead<T>(reader, fieldName, defaultValue, CultureInfo.CurrentCulture);
    }

    /// <summary>
    /// Read a field from an IDataReader, allowing a default value  in case the field is null or an exception occurs during the read.
    /// </summary>
    /// <typeparam name="T">The type of the return value</typeparam>
    /// <param name="reader">The DataReader</param>
    /// <param name="fieldName">The name of the field</param>
    /// <param name="defaultValue">The default value</param>
    /// <param name="culture">The culture used to convert strings to float, double or DateTime</param>
    /// <returns>Either the value of the field converted to the correct type, or the default value</returns>
    public static T SafeRead<T>( this IDataReader reader, string fieldName, T defaultValue, CultureInfo culture ) {
      if ( string.IsNullOrWhiteSpace(fieldName) ) {
        return defaultValue;
      }
      if ( culture == null ) {
        culture = CultureInfo.CurrentCulture;
      }
      try {
        object o = reader[fieldName];
        if ( o == null || o == System.DBNull.Value ) {
          return defaultValue;
        } 

        return BLConverter.BLConvert<T>(o, culture, defaultValue);

      } catch (Exception ex) {
        Trace.WriteLine(string.Format("Error during SafeRead of {0} from DataReader: {1}", fieldName, ex.Message));
        return defaultValue;
      }
    }
  }
}
