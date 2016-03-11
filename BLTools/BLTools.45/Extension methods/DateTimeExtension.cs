using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLTools {
  /// <summary>
  /// DateTime extensions
  /// </summary>
  public static class DateTimeExtension {

    /// <summary>
    /// Returns a DateTime as a string formatted as "yyyy-MM-dd"
    /// </summary>
    /// <param name="datetime">The source Datetime</param>
    /// <returns>A DateTime as a string formatted as "yyyy-MM-dd"</returns>
    public static string ToYMD(this DateTime datetime) {
      return datetime.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
    }

    /// <summary>
    /// Returns a DateTime as a string formatted as "dd/MM/yyyy"
    /// </summary>
    /// <param name="datetime">The source Datetime</param>
    /// <returns>A DateTime as a string formatted as "dd/MM/yyyy"</returns>
    public static string ToDMY(this DateTime datetime) {
      return datetime.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
    }

    /// <summary>
    /// Returns a DateTime as a string formatted as "yyyy-MM-dd HH:mm:ss"
    /// </summary>
    /// <param name="datetime">The source Datetime</param>
    /// <returns>A DateTime as a string formatted as "yyyy-MM-dd HH:mm:ss"</returns>
    public static string ToYMDHMS(this DateTime datetime) {
      return datetime.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture);
    }

    /// <summary>
    /// Returns a DateTime as a string formatted as "dd/MM/yyyy HH:mm:ss"
    /// </summary>
    /// <param name="datetime">The source Datetime</param>
    /// <returns>A DateTime as a string formatted as "dd/MM/yyyy HH:mm:ss"</returns>
    public static string ToDMYHMS(this DateTime datetime) {
      return datetime.ToString("dd/MM/yyyy HH:mm:ss", CultureInfo.InvariantCulture);
    }

    /// <summary>
    /// Returns a DateTime as a string formatted as "HH:mm:ss"
    /// </summary>
    /// <param name="datetime">The source Datetime</param>
    /// <returns>A DateTime as a string formatted as "HH:mm:ss"</returns>
    public static string ToHMS(this DateTime datetime) {
      return datetime.ToString("HH:mm:ss", CultureInfo.InvariantCulture);
    }

    /// <summary>
    /// Returns a UTC DateTime converted into local DateTime
    /// </summary>
    /// <param name="datetime">The source Datetime</param>
    /// <returns>A UTC DateTime converted into local DateTime</returns>
    public static DateTime FromUTC(this DateTime datetime) {
      return TimeZone.CurrentTimeZone.ToLocalTime(datetime);
    }

    /// <summary>
    /// Returns a DateTime as a formatted string or a specific value for empty dates
    /// </summary>
    /// <param name="datetime">The source DateTime</param>
    /// <param name="valueForEmptyDate">The string value to return when DateTime is DateTime.MinValue (default = "-")</param>
    /// <returns>The DateTime converted or the default value</returns>
    public static string EmptyDateAs(this DateTime datetime, string valueForEmptyDate = "-") {
      if (datetime.Date == DateTime.MinValue.Date) {
        return valueForEmptyDate;
      }
      return datetime.DateOrDateTime();
    }

    /// <summary>
    /// Returns a DateTime as a formatted string or a "-" for empty dates
    /// </summary>
    /// <param name="datetime">The source DateTime</param>
    /// <returns>The DateTime converted or "-"</returns>
    public static string EmptyDateAsDash(this DateTime datetime) {
      return datetime.EmptyDateAs("-");
    }

    /// <summary>
    /// Returns a DateTime as a formatted string or blank ("") for empty dates
    /// </summary>
    /// <param name="datetime">The source DateTime</param>
    /// <returns>The DateTime converted or blank ("")</returns>
    public static string EmptyDateAsBlank(this DateTime datetime) {
      return datetime.EmptyDateAs("");
    }

    /// <summary>
    /// Returns a DateTime as a formatted string with date only or date and time depending if time is 00:00:00 or not
    /// </summary>
    /// <param name="datetime">The source DateTime</param>
    /// <returns>The formatted string</returns>
    public static string DateOrDateTime(this DateTime datetime) {
      if (datetime.TimeOfDay == DateTime.MinValue.TimeOfDay) {
        return datetime.ToYMD();
      } else {
        return datetime.ToYMDHMS();
      }
    }

    /// <summary>
    /// Returns The time part of the date time
    /// </summary>
    /// <param name="datetime">The source DateTime</param>
    /// <returns>A DateTime containing only time</returns>
    public static DateTime Time(this DateTime datetime) {
      return default(DateTime).Add(datetime.TimeOfDay);
    }
  }
}
