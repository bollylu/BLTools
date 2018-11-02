using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using BLTools;
using System.IO;

namespace BLTools.Json {
  public static class Json {

    internal readonly static char[] WHITE_SPACES = new char[] { ' ', '\b', '\t', '\r', '\n', '\f' };

    internal const char INNER_FIELD_SEPARATOR = ':';
    internal const char OUTER_FIELD_SEPARATOR = ',';

    public const int DEFAULT_INDENT = 2;

    internal const char CHR_SLASH = '/';
    internal const char CHR_BACKSLASH = '\\';
    internal const char CHR_DOUBLE_QUOTE = '\"';

    internal const char START_OF_ARRAY = '[';
    internal const char END_OF_ARRAY = ']';

    internal const char START_OF_OBJECT = '{';
    internal const char END_OF_OBJECT = '}';

    internal const string NAN = "NaN";
    internal const char DECIMAL_SEPARATOR = '.';

    internal const string TRUE_VALUE = "true";
    internal const string FALSE_VALUE = "false";

    internal const string NULL_VALUE = "null";

    public static string JsonEncode(this string source) {

      if ( source == null ) {
        return null;
      }

      if ( source == "" ) {
        return "";
      }

      StringBuilder RetVal = new StringBuilder(source).Replace("\\", "\\\\")
                                                      .Replace("/", "\\/")
                                                      .Replace("\t", "\\t")
                                                      .Replace("\n", "\\n")
                                                      .Replace("\r", "\\r")
                                                      .Replace("\f", "\\f")
                                                      .Replace("\b", "\\b")
                                                      .Replace("\\\"", "\\\\\"");
      return RetVal.ToString();

      
    }

    public static string JsonDecode(this string source) {

      if ( source == null ) {
        return null;
      }

      if ( source == "" ) {
        return "";
      }

      StringBuilder RetVal = new StringBuilder(source).Replace("\\\\", "\\")
                                                      .Replace("\\/", "/")
                                                      .Replace("\\t", "\t")
                                                      .Replace("\\n", "\n")
                                                      .Replace("\\r", "\r")
                                                      .Replace("\\f", "\f")
                                                      .Replace("\\b", "\b")
                                                      .Replace("\\\"", "\"");
      return RetVal.ToString();

    }
  }
}