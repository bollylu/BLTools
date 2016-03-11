using System;
using System.Collections.Generic;

namespace BLTools {

  /// <summary>
  /// Summary description for Severity.
  /// </summary>
  public class Severity {

    public ErrorLevel? Value {
      get;
      set;
    }

    private static Dictionary<ErrorLevel?, string> TranslatedValues;

    #region Constructor(s)
    static Severity() {
      if ( TranslatedValues == null ) {
        TranslatedValues = new Dictionary<ErrorLevel?, string>();
        TranslatedValues.Add(ErrorLevel.Info, "INFO");
        TranslatedValues.Add(ErrorLevel.Warning, "WARNING");
        TranslatedValues.Add(ErrorLevel.Error, "ERROR");
        TranslatedValues.Add(ErrorLevel.Fatal, "FATAL");
        TranslatedValues.Add(ErrorLevel.Unknown, "INFO");
      }
    }
    public Severity() {
      Value = ErrorLevel.Unknown;
    }
    public Severity(ErrorLevel severity) {
      Value = severity;
    }
    #endregion Constructor(s)

    public override string ToString() {
      string RetVal;
      if ( Value == null || !TranslatedValues.TryGetValue(Value, out RetVal) ) {
        TranslatedValues.TryGetValue(Value, out RetVal);
      }
      return RetVal;
    }

    static public string Information {
      get {
        string RetVal;
        TranslatedValues.TryGetValue(ErrorLevel.Info, out RetVal);
        return RetVal;
      }
    }
    static public string Warning {
      get {
        string RetVal;
        TranslatedValues.TryGetValue(ErrorLevel.Warning, out RetVal);
        return RetVal;
      }
    }
    static public string Error {
      get {
        string RetVal;
        TranslatedValues.TryGetValue(ErrorLevel.Error, out RetVal);
        return RetVal;
      }
    }
    static public string Fatal {
      get {
        string RetVal;
        TranslatedValues.TryGetValue(ErrorLevel.Fatal, out RetVal);
        return RetVal;
      }
    }

    static public ErrorLevel ErrorLevelInfo {
      get {
        return ErrorLevel.Info;
      }
    }
    static public ErrorLevel ErrorLevelError {
      get {
        return ErrorLevel.Error;
      }
    }
    static public ErrorLevel ErrorLevelFatal {
      get {
        return ErrorLevel.Fatal;
      }
    }
    static public ErrorLevel ErrorLevelWarning {
      get {
        return ErrorLevel.Warning;
      }
    }

    static public string GetSeverity(ErrorLevel errorlevel) {
      return (new Severity(errorlevel)).ToString();
    }
  }

  public enum ErrorLevel {
    Unknown,
    Info,
    Warning,
    Error,
    Fatal
  }
}
