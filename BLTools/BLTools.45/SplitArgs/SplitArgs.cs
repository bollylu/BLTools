using System;
using System.Text;
using System.Collections.Generic;
using System.Globalization;
using System.Collections.Specialized;

namespace BLTools {

  /// <summary>
  /// Splits arguments of CommandLine. You can use either / or - as parameter prefix or nothing but the keyword.
  /// If values are specified, they are separated from the keyword by an = sign.
  /// (c) 2004-2012 Luc Bolly
  /// </summary>
  public class SplitArgs : List<ArgElement> {

    #region Public static properties
    /// <summary>
    /// Get or Set the CultureInfo used to parse DateTime and numbers (decimal point)
    /// </summary>
    public static CultureInfo CurrentCultureInfo = CultureInfo.CurrentCulture;
    /// <summary>
    /// Get or Set the parameters name case sensitivity
    /// </summary>
    public static bool IsCaseSensitive = false;
    /// <summary>
    /// Get or Set the separator used when reading an array from parameters (default value is ',')
    /// </summary>
    public static char Separator = ',';
    #endregion Public static properties

    #region Constructors
    /// <summary>
    /// Creates a dictonnary of command line arguments from the args parameters list provided to Main function
    /// </summary>
    /// <param name="arrayOfValues">An array of parameters</param>
    public SplitArgs(IEnumerable<string> arrayOfValues) {
      if ( arrayOfValues == null ) {
        throw new ArgumentNullException("arrayOfValues", "you must pass a valid IEnumerable[] arrayOfValues argument");
      }
      _ParseValues(arrayOfValues);
    }
    /// <summary>
    /// Creates a dictionnary of command line parameters from a given command line 
    /// </summary>
    /// <param name="cmdLine">The command line</param>
    public SplitArgs(string cmdLine) {
      #region Validate parameters
      if (cmdLine == null) {
        throw new ArgumentNullException("cmdLine", "you must pass a valid string cmdLine argument");
      } 
      #endregion Validate parameters

      string PreprocessedLine = cmdLine.Trim();
      List<string> CmdLineValues = new List<string>();
      StringBuilder TempStr = new StringBuilder("");

      int i = 0;
      bool InQuote = false;

      while ( i < PreprocessedLine.Length ) {
      
        if ( PreprocessedLine[i] == '"' ) {
          InQuote = !( InQuote );
        }

        if ( PreprocessedLine[i] == ' ' ) {
          if ( InQuote ) {
            TempStr.Append(PreprocessedLine[i]);
          } else {
            if ( !( TempStr.Length == 0 ) ) {
              CmdLineValues.Add(TempStr.ToString());
              TempStr = new StringBuilder("");
            }
          }
        } else {
          if ( PreprocessedLine[i] != '"' ) {
            TempStr.Append(PreprocessedLine[i]);
          }
        }
        i++;
      }

      if ( !( TempStr.Length == 0 ) ) {
        CmdLineValues.Add(TempStr.ToString());
      }

      _ParseValues(CmdLineValues.ToArray());

    }
    /// <summary>
    /// Copy constructor
    /// </summary>
    /// <param name="otherSpliArgs"></param>
    public SplitArgs(SplitArgs otherSplitArgs) {
      this.AddRange(otherSplitArgs);
    }
    /// <summary>
    /// Create a dictionnary of url parameters from a Request.QueryString
    /// </summary>
    /// <param name="queryStringItems">A Request.QueryString</param>
    public SplitArgs(NameValueCollection queryStringItems) {
      if (queryStringItems == null || queryStringItems.Count == 0) {
        return;
      }
      foreach (string QueryStringItem in queryStringItems) {
        this.Add(new ArgElement(0, QueryStringItem, queryStringItems[QueryStringItem]));
      }
    }
    #endregion Constructors

    #region Private methods
    private void _ParseValues(IEnumerable<string> arrayOfValues) {
      int Position = 0;
      foreach ( string ValueItem in arrayOfValues ) {
        if ( ValueItem.StartsWith("/") || ValueItem.StartsWith("-") ) {
          if ( ValueItem.IndexOf("=") != -1 ) {
            Add(new ArgElement(Position, ValueItem.Substring(1, ValueItem.IndexOf("=") - 1).Trim(), ValueItem.Substring(ValueItem.IndexOf("=") + 1).Trim()));
          } else {
            Add(new ArgElement(Position, ValueItem.Substring(1).Trim().ToLower(CultureInfo.CurrentCulture), ""));
          }
        } else {
          if ( ValueItem.IndexOf("=") != -1 ) {
            Add(new ArgElement(Position, ValueItem.Substring(0, ValueItem.IndexOf("=")).Trim(), ValueItem.Substring(ValueItem.IndexOf("=") + 1).Trim()));
          } else {
            Add(new ArgElement(Position, ValueItem.Trim().ToLower(CultureInfo.CurrentCulture), ""));
          }
        }
        Position++;
      }
    }
    #endregion Private methods

    #region Public properties
    public new ArgElement this[int index] {
      get {
        ArgElement CurrentElement = Find((a) => a.Id == index);
        if ( CurrentElement == null ) {
          return new ArgElement(0, "", "");
        } else {
          return CurrentElement;
        }
      }
    }
    #endregion Public properties

    #region Public methods
    /// <summary>
    /// Return the value of a named parameter as a string array. The source parameter string will be splitted based
    /// on an array of chars
    /// </summary>
    /// <param name="key">The name of the parameter</param>
    /// <param name="separator">The array of possible separators</param>
    /// <returns>The array of values or an empty array if parameter is not found</returns>
    [Obsolete("Use generic version instead.")]
    public string[] GetValueAsStringArray(string key, char[] separator) {
      if ( key == null || this.Count == 0 ) {
        return new string[] { };
      }
      string KeyLower = key.ToLower(CultureInfo.CurrentCulture);
      ArgElement CurrentElement = Find((a) => a.Name.ToLower() == KeyLower);
      if ( CurrentElement != null ) {
        return CurrentElement.Value.Split(separator);
      } else {
        return new string[] { };
      }
    }

    /// <summary>
    /// Return the value of a named parameter as a int array. The source parameter string will be splitted based
    /// on an array of chars
    /// </summary>
    /// <param name="key">The name of the parameter</param>
    /// <param name="separator">The array of possible separators</param>
    /// <returns>The array of values or an empty array if parameter is not found. Each unconvertible value is rendered as 0.</returns>
    [Obsolete("Use generic version instead.")]
    public int[] GetValueAsIntArray(string key, char[] separator) {
      string KeyLower = key.ToLower(CultureInfo.CurrentCulture);
      ArgElement CurrentElement = Find((a) => a.Name.ToLower() == KeyLower);
      if ( CurrentElement != null ) {
        string[] aTemp = CurrentElement.Value.Split(separator);
        int[] RetVal = new int[aTemp.Length];
        for ( int i = 0; i < aTemp.Length; i++ ) {
          try {
            RetVal[i] = Int32.Parse(aTemp[i]);
          } catch ( FormatException ) {
            RetVal[i] = 0;
          } catch ( OverflowException ) {
            RetVal[i] = 0;
          }
        }
        return (int[])RetVal.Clone();
      } else {
        return new int[] { };
      }
    }

    #region GetValueAsString
    /// <summary>
    /// Return the string that match the parameter at the specified position (0 based)
    /// </summary>
    /// <param name="position">The position of the parameter in the command line</param>
    /// <returns>The value of the parameter or an empty string is parameter does not exist</returns>
    [Obsolete("Use generic version instead.")]
    public string GetValueAsString(int position) {
      return GetValueAsString(position, "");
    }
    /// <summary>
    /// Return the string that match the parameter at the specified position (0 based)
    /// </summary>
    /// <param name="position">The position of the parameter in the command line</param>
    /// <param name="defaultValue">The default value to return</param>
    /// <returns>The value of the parameter or the default value is parameter does not exist</returns>
    [Obsolete("Use generic version instead.")]
    public string GetValueAsString(int position, string defaultValue) {
      if ( position >= 0 && position <= this.Count ) {
        return this[position].Value;
      } else {
        return defaultValue;
      }
    }
    /// <summary>
    /// Return the string that match the named parameter
    /// </summary>
    /// <param name="key">The name of the parameter</param>
    /// <returns>The value of the parameter or an empty string is parameter does not exist</returns>
    [Obsolete("Use generic version instead.")]
    public string GetValueAsString(string key) {
      return GetValueAsString(key, "");
    }
    /// <summary>
    /// Return the string that match the named parameter
    /// </summary>
    /// <param name="key">The name of the parameter</param>
    /// <param name="defaultValue">The default value to return</param>
    /// <returns>The value of the parameter or the default value is parameter does not exist</returns>
    [Obsolete("Use generic version instead.")]
    public string GetValueAsString(string key, string defaultValue) {
      string KeyLower = key.ToLower(CultureInfo.CurrentCulture);
      ArgElement CurrentElement = Find((a) => a.Name.ToLower() == KeyLower);
      if ( CurrentElement != null ) {
        return CurrentElement.Value;
      } else {
        return defaultValue;
      }
    }
    #endregion GetValueAsString

    #region GetValueAsInt
    [Obsolete("Use generic version instead.")]
    public int GetValueAsInt(string key) {
      return GetValueAsInt(key, 0);
    }
    [Obsolete("Use generic version instead.")]
    public int GetValueAsInt(string key, int defaultValue) {
      string KeyLower = key.ToLower(CultureInfo.CurrentCulture);
      ArgElement CurrentElement = Find((a) => a.Name.ToLower() == KeyLower);
      if ( CurrentElement != null ) {
        try {
          return Int32.Parse(CurrentElement.Value);
        } catch ( FormatException ) {
          return defaultValue;
        } catch ( OverflowException ) {
          return defaultValue;
        }
      } else {
        return defaultValue;
      }
    }
    [Obsolete("Use generic version instead.")]
    public int GetValueAsInt(int position) {
      return GetValueAsInt(position, 0);
    }
    [Obsolete("Use generic version instead.")]
    public int GetValueAsInt(int position, int defaultValue) {
      if ( position >= 0 && position <= this.Count ) {
        try {
          return Int32.Parse(this[position].Value);
        } catch ( FormatException ) {
          return defaultValue;
        } catch ( OverflowException ) {
          return defaultValue;
        }
      } else {
        return defaultValue;
      }
    }
    #endregion GetValueAsInt

    #region GetValueAsLong
    [Obsolete("Use generic version instead.")]
    public long GetValueAsLong(string key) {
      return GetValueAsLong(key, 0);
    }
    [Obsolete("Use generic version instead.")]
    public long GetValueAsLong(string key, long defaultValue) {
      string KeyLower = key.ToLower(CultureInfo.CurrentCulture);
      ArgElement CurrentElement = Find((a) => a.Name.ToLower() == KeyLower);
      if ( CurrentElement != null ) {
        try {
          return Int64.Parse(CurrentElement.Value);
        } catch ( FormatException ) {
          return defaultValue;
        } catch ( OverflowException ) {
          return defaultValue;
        }
      } else {
        return defaultValue;
      }
    }
    [Obsolete("Use generic version instead.")]
    public long GetValueAsLong(int position) {
      return GetValueAsLong(position, 0);
    }
    [Obsolete("Use generic version instead.")]
    public long GetValueAsLong(int position, long defaultValue) {
      if ( position >= 0 && position <= this.Count ) {
        try {
          return long.Parse(this[position].Value);
        } catch ( FormatException ) {
          return defaultValue;
        } catch ( OverflowException ) {
          return defaultValue;
        }
      } else {
        return defaultValue;
      }
    }
    #endregion GetValueAsLong

    #region GetValueAsFloat
    [Obsolete("Use generic version instead.")]
    public float GetValueAsFloat(string key) {
      return GetValueAsFloat(key, 0);
    }
    [Obsolete("Use generic version instead.")]
    public float GetValueAsFloat(string key, float defaultValue) {
      string KeyLower = key.ToLower(CultureInfo.CurrentCulture);
      ArgElement CurrentElement = Find((a) => a.Name.ToLower() == KeyLower);
      if ( CurrentElement != null ) {
        try {
          return (float)Double.Parse(CurrentElement.Value);
        } catch ( FormatException ) {
          return defaultValue;
        } catch ( OverflowException ) {
          return defaultValue;
        }
      } else {
        return defaultValue;
      }
    }
    [Obsolete("Use generic version instead.")]
    public float GetValueAsFloat(int position) {
      return GetValueAsFloat(position, 0);
    }
    [Obsolete("Use generic version instead.")]
    public float GetValueAsFloat(int position, float defaultValue) {
      if ( position >= 0 && position <= this.Count ) {
        try {
          return float.Parse(this[position].Value);
        } catch ( FormatException ) {
          return defaultValue;
        } catch ( OverflowException ) {
          return defaultValue;
        }
      } else {
        return defaultValue;
      }
    }
    #endregion GetValueAsFloat

    #region GetValueAsDouble
    [Obsolete("Use generic version instead.")]
    public double GetValueAsDouble(string key) {
      return GetValueAsDouble(key, 0);
    }
    [Obsolete("Use generic version instead.")]
    public double GetValueAsDouble(string key, double defaultValue) {
      string KeyLower = key.ToLower(CultureInfo.CurrentCulture);
      ArgElement CurrentElement = Find((a) => a.Name.ToLower() == KeyLower);
      if ( CurrentElement != null ) {
        try {
          return Double.Parse(CurrentElement.Value);
        } catch ( FormatException ) {
          return defaultValue;
        } catch ( OverflowException ) {
          return defaultValue;
        }
      } else {
        return defaultValue;
      }
    }
    [Obsolete("Use generic version instead.")]
    public double GetValueAsDouble(int position) {
      return GetValueAsDouble(position, 0);
    }
    [Obsolete("Use generic version instead.")]
    public double GetValueAsDouble(int position, double defaultValue) {
      if ( position >= 0 && position <= this.Count ) {
        try {
          return double.Parse(this[position].Value);
        } catch ( FormatException ) {
          return defaultValue;
        } catch ( OverflowException ) {
          return defaultValue;
        }
      } else {
        return defaultValue;
      }
    }
    #endregion GetValueAsDouble

    #region GetValueAsDateTime
    [Obsolete("Use generic version instead.")]
    public DateTime GetValueAsDateTime(string key) {
      return GetValueAsDateTime(key, DateTime.MinValue);
    }
    [Obsolete("Use generic version instead.")]
    public DateTime GetValueAsDateTime(string key, DateTime defaultValue) {
      string KeyLower = key.ToLower(CultureInfo.CurrentCulture);
      ArgElement CurrentElement = Find((a) => a.Name.ToLower() == KeyLower);
      if ( CurrentElement != null ) {
        try {
          return DateTime.Parse(CurrentElement.Value);
        } catch ( FormatException ) {
          return defaultValue;
        }
      } else {
        return defaultValue;
      }
    }
    [Obsolete("Use generic version instead.")]
    public DateTime GetValueAsDateTime(int position) {
      return GetValueAsDateTime(position, DateTime.MinValue);
    }
    [Obsolete("Use generic version instead.")]
    public DateTime GetValueAsDateTime(int position, DateTime defaultValue) {
      if ( position >= 0 && position <= this.Count ) {
        try {
          return DateTime.Parse(this[position].Value);
        } catch ( FormatException ) {
          return defaultValue;
        } catch ( OverflowException ) {
          return defaultValue;
        }
      } else {
        return defaultValue;
      }
    }
    #endregion GetValueAsDateTime

    public bool IsDefined(string key) {
      if ( key == null || this.Count == 0 ) {
        return false;
      }
      string KeyLower = key.ToLower(CultureInfo.CurrentCulture);
      ArgElement CurrentElement = Find((a) => a.Name.ToLower() == KeyLower);
      if ( CurrentElement != null ) {
        return true;
      } else {
        return false;
      }

    }

    #region Generic version of the GetValue
    /// <summary>
    /// Generic version of GetValue
    /// </summary>
    /// <typeparam name="T">The type of the returned value</typeparam>
    /// <param name="key">The key name of the value</param>
    /// <returns>The value</returns>
    public T GetValue<T>(string key) {
      if ( CurrentCultureInfo != null ) {
        return GetValue(key, default(T), CurrentCultureInfo);
      } else {
        return GetValue(key, default(T), CultureInfo.CurrentCulture);
      }
    }
    /// <summary>
    /// Generic version of GetValue
    /// </summary>
    /// <typeparam name="T">The type of the returned value</typeparam>
    /// <param name="key">The key name of the value</param>
    /// <param name="defaultValue">The default value to be returned if the key name is invalid</param>
    /// <returns>The value</returns>
    public T GetValue<T>(string key, T defaultValue) {
      if ( CurrentCultureInfo != null ) {
        return GetValue(key, defaultValue, CurrentCultureInfo);
      } else {
        return GetValue(key, defaultValue, CultureInfo.CurrentCulture);
      }
    }
    /// <summary>
    /// Generic version of GetValue
    /// </summary>
    /// <typeparam name="T">The type of the returned value</typeparam>
    /// <param name="key">The key name of the value</param>
    /// <param name="defaultValue">The default value to be returned if the key name is invalid</param>
    /// <returns>The value</returns>
    public T GetValue<T>(string key, T defaultValue, CultureInfo culture) {
      if ( key == null || this.Count == 0 ) {
        return defaultValue;
      }
      try {
        ArgElement CurrentElement;

        if ( IsCaseSensitive ) {
          CurrentElement = Find((a) => a.Name == key);
        } else {
          CurrentElement = Find((a) => a.Name.ToLower() == key.ToLower(culture));
        }

        if ( CurrentElement != null ) {
          return BLConverter.BLConvert<T>(CurrentElement.Value, culture, defaultValue);
        } else {
          return defaultValue;
        }
      } catch {
        return defaultValue;
      }
    }

    /// <summary>
    /// Generic version of GetValue
    /// </summary>
    /// <typeparam name="T">The type of the returned value</typeparam>
    /// <param name="position">The position (counted from 0) of the value</param>
    /// <returns>The value</returns>
    public T GetValue<T>(int position) {
      if ( CurrentCultureInfo != null ) {
        return GetValue(position, default(T), CurrentCultureInfo);
      } else {
        return GetValue(position, default(T), CultureInfo.CurrentCulture);
      }
    }

    /// <summary>
    /// Generic version of GetValue
    /// </summary>
    /// <typeparam name="T">The type of the returned value</typeparam>
    /// <param name="position">The position (counted from 0) of the value</param>
    /// <param name="defaultValue">The default value to be returned if the position is invalid</param>
    /// <returns>The value</returns>
    public T GetValue<T>(int position, T defaultValue) {
      if ( CurrentCultureInfo != null ) {
        return GetValue(position, defaultValue, CurrentCultureInfo);
      } else {
        return GetValue(position, defaultValue, CultureInfo.CurrentCulture);
      }
    }

    /// <summary>
    /// Generic version of GetValue
    /// </summary>
    /// <typeparam name="T">The type of the returned value</typeparam>
    /// <param name="position">The position (counted from 0) of the value</param>
    /// <param name="defaultValue">The default value to be returned if the key name is invalid</param>
    /// <returns>The value</returns>
    public T GetValue<T>(int position, T defaultValue, CultureInfo culture) {
      if ( this.Count == 0 || position < 0 || position > this.Count ) {
        return defaultValue;
      }
      ArgElement CurrentElement = this[position];
      return BLConverter.BLConvert<T>(CurrentElement.Value, culture, defaultValue);

    }
    #endregion Generic version of the GetValue

    #endregion Public methods
  }
}
