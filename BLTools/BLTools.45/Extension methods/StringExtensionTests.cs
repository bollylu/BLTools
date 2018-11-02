using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;
using System.Text.RegularExpressions;

namespace BLTools {

  /// <summary>
  /// Extensions for string
  /// </summary>
  public static partial class StringExtension {

    #region Tests
    /// <summary>
    /// Indicates if a string contains only alphabetic characters (A-Z and a-z)
    /// </summary>
    /// <param name="sourceValue">The source string</param>
    /// <returns>True if the assertion succeeds</returns>
    public static bool IsAlpha(this string sourceValue) {
      return Regex.IsMatch(sourceValue, @"^[A-Za-z]*$");
    }
    /// <summary>
    /// Indicates if a string contains only alphabetic characters (A-Z and a-z) or numeric characters (0-9)
    /// </summary>
    /// <param name="sourceValue">The source string</param>
    /// <returns>True if the assertion succeeds</returns>
    public static bool IsAlphaNumeric(this string sourceValue) {
      return Regex.IsMatch(sourceValue, "^[A-Za-z0-9]*$");
    }
    /// <summary>
    /// Indicates if a string contains only numeric characters (0-9) or separators (-.,)
    /// </summary>
    /// <param name="sourceValue">The source string</param>
    /// <returns>True if the assertion succeeds</returns>
    public static bool IsNumeric(this string sourceValue) {
      return Regex.IsMatch(sourceValue, @"^[-\d][\d\.,]*$");
    }
    /// <summary>
    /// Indicates if a string contains only numeric characters (0-9) or separators (-.,)
    /// </summary>
    /// <param name="sourceValue">The source string</param>
    /// <returns>True if the assertion succeeds</returns>
    public static bool IsNumericOnly(this string sourceValue) {
      return Regex.IsMatch(sourceValue, @"^[-\d][\d]*$");
    }
    /// <summary>
    /// Indicates if a string contains only alphabetic characters (A-Z and a-z) or blank
    /// </summary>
    /// <param name="sourceValue">The source string</param>
    /// <returns>True if the assertion succeeds</returns>
    public static bool IsAlphaOrBlank(this string sourceValue) {
      return Regex.IsMatch(sourceValue.Trim(), @"^[A-Za-z ]*$");
    }
    /// <summary>
    /// Indicates if a string contains only alphabetic characters (A-Z and a-z) or numeric characters (0-9) or blank
    /// </summary>
    /// <param name="sourceValue">The source string</param>
    /// <returns>True if the assertion succeeds</returns>
    public static bool IsAlphaNumericOrBlank(this string sourceValue) {
      return Regex.IsMatch(sourceValue.Trim(), @"^[A-Za-z\d ]*$");
    }
    /// <summary>
    /// Indicates if a string contains only numeric characters (0-9) or blank
    /// </summary>
    /// <param name="sourceValue">The source string</param>
    /// <returns>True if the assertion succeeds</returns>
    public static bool IsNumericOrBlank(this string sourceValue) {
      return Regex.IsMatch(sourceValue.Trim(), @"^[-\d][\d\., ]*$");
    }

    /// <summary>
    /// Indicates if a string contains only alphabetic characters (A-Z and a-z) or numeric characters (0-9) or blank or dashes
    /// </summary>
    /// <param name="sourceValue">The source string</param>
    /// <returns>True if the assertion succeeds</returns>
    public static bool IsAlphaNumericOrBlankOrDashes(this string sourceValue) {
      return Regex.IsMatch(sourceValue.Trim(), @"^[A-Za-z\d -]*$");
    }
    /// <summary>
    /// Indicates if a string contains only specified characters
    /// </summary>
    /// <param name="sourceValue">The source string</param>
    /// <param name="charList">The list of characters to test for</param>
    /// <returns>True if the assertion succeeds</returns>
    public static bool IsMadeOfTheseChars(this string sourceValue, params char[] charList) {
      return Regex.IsMatch(sourceValue.Trim(), string.Format(@"^[{0}]*$", string.Join("", charList)));
    }
    #endregion Tests

  }
}
