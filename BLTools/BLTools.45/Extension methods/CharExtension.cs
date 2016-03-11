using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLTools {
  public static class CharExtension {

    /// <summary>
    /// Indicates if a char is a numeric characters (0-9)
    /// </summary>
    /// <param name="sourceValue">The source string</param>
    /// <returns>True if the assertion succeeds</returns>
    public static bool IsNumeric(this char sourceValue) {
      return "0123456789".Contains(sourceValue);
    }

    /// <summary>
    /// Indicates if a char is a numeric characters (0-9)
    /// </summary>
    /// <param name="sourceValue">The source string</param>
    /// <returns>True if the assertion succeeds</returns>
    public static bool IsNumericOrBlank(this char sourceValue) {
      return "0123456789 ".Contains(sourceValue);
    }

    /// <summary>
    /// Indicates if a char is a numeric characters (0-9) or separator
    /// </summary>
    /// <param name="sourceValue">The source string</param>
    /// <returns>True if the assertion succeeds</returns>
    public static bool IsNumericOrSeparator(this char sourceValue) {
      return "0123456789.,-".Contains(sourceValue);
    }

    /// <summary>
    /// Indicates if a char is an alpha character (A-Z or a-z)
    /// </summary>
    /// <param name="sourceValue">The source string</param>
    /// <returns>True if the assertion succeeds</returns>
    public static bool IsAlpha(this char sourceValue) {
      return "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz".Contains(sourceValue);
    }

    /// <summary>
    /// Indicates if a char is an alpha character (A-Z or a-z) or a blank
    /// </summary>
    /// <param name="sourceValue">The source string</param>
    /// <returns>True if the assertion succeeds</returns>
    public static bool IsAlphaOrBlank(this char sourceValue) {
      return "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz ".Contains(sourceValue);
    }

    /// <summary>
    /// Indicates if a char is an alpha character (A-Z or a-z) or a numeric character
    /// </summary>
    /// <param name="sourceValue">The source string</param>
    /// <returns>True if the assertion succeeds</returns>
    public static bool IsAlphaOrNumeric(this char sourceValue) {
      return sourceValue.IsAlpha() || sourceValue.IsNumeric();
    }

    /// <summary>
    /// Indicates if a char is an alpha character (A-Z or a-z) or a blank or a numeric character
    /// </summary>
    /// <param name="sourceValue">The source string</param>
    /// <returns>True if the assertion succeeds</returns>
    public static bool IsAlphaOrNumericOrBlank(this char sourceValue) {
      return sourceValue.IsAlphaOrBlank() || sourceValue.IsNumeric();
    }
  }
}
