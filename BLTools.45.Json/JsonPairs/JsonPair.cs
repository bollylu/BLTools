using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Linq;
using System.IO;
using System.Globalization;
using BLTools;

namespace BLTools.Json {
  public class JsonPair : IDisposable, IJsonPair {

    public static JsonPair Default => new JsonPair("(default)", new JsonNull());

    public string Key { get; private set; }

    public IJsonValue Content { get; private set; }

    #region --- Typed content --------------------------------------------
    public JsonString StringContent => Content is JsonString Temp ? Temp : null;

    public JsonInt IntContent => Content is JsonInt Temp ? Temp : null;

    public JsonLong LongContent => Content is JsonLong Temp ? Temp : null;

    public JsonFloat FloatContent => Content is JsonFloat Temp ? Temp : null;

    public JsonDouble DoubleContent => Content is JsonDouble Temp ? Temp : null;

    public JsonBool BoolContent => Content is JsonBool Temp ? Temp : null;

    public JsonDateTime DateTimeContent => Content is JsonDateTime Temp ? Temp : null;

    public JsonArray ArrayContent => Content is JsonArray Temp ? Temp : null;

    public JsonObject ObjectContent => Content is JsonObject Temp ? Temp : null;
    #endregion --- Typed content --------------------------------------------

    #region --- Constructor(s) ---------------------------------------------------------------------------------
    public JsonPair() { }

    public JsonPair(string key, IJsonValue jsonValue) {
      Key = key;
      Content = jsonValue;
    }

    public JsonPair(IJsonPair jsonPair) {
      Key = jsonPair.Key;
      Content = jsonPair.Content;
    }

    public JsonPair(string key, string content) {
      Key = key;
      Content = new JsonString(content);
    }

    public JsonPair(string key, int content) {
      Key = key;
      Content = new JsonInt(content);
    }

    public JsonPair(string key, long content) {
      Key = key;
      Content = new JsonLong(content);
    }

    public JsonPair(string key, float content) {
      Key = key;
      Content = new JsonFloat(content);
    }

    public JsonPair(string key, double content) {
      Key = key;
      Content = new JsonDouble(content);
    }

    public JsonPair(string key, DateTime content) {
      Key = key;
      Content = new JsonDateTime(content);
    }

    public JsonPair(string key, bool content) {
      Key = key;
      Content = new JsonBool(content);
    }

    public void Dispose() {
      Key = null;
      Content.Dispose();
    }
    #endregion --- Constructor(s) ------------------------------------------------------------------------------

    public override string ToString() {
      StringBuilder RetVal = new StringBuilder();
      RetVal.Append($"{Key} {Json.INNER_FIELD_SEPARATOR} {Content.ToString()}");
      return RetVal.ToString();
    }

    public string RenderAsString(bool formatted = false, int indent = 0, bool needFrontSpaces = true) {
      StringBuilder RetVal = new StringBuilder();

      if ( formatted && needFrontSpaces ) {
        RetVal.Append($"{StringExtension.Spaces(indent)}");
      }

      if ( formatted && !needFrontSpaces ) {
        RetVal.Append(StringExtension.Spaces(1));
      }

      if ( formatted ) {
        RetVal.Append($"\"{Key}\" {Json.INNER_FIELD_SEPARATOR} ");
      } else {
        RetVal.Append($"\"{Key}\"{Json.INNER_FIELD_SEPARATOR}");
      }

      RetVal.Append(Content.RenderAsString(formatted, indent, false));

      return RetVal.ToString();
    }

    public byte[] RenderAsBytes(bool formatted = false, int indent = 0) {

      using ( MemoryStream RetVal = new MemoryStream() ) {
        using ( JsonWriter Writer = new JsonWriter(RetVal) ) {

          if ( formatted ) {
            Writer.Write($"{StringExtension.Spaces(indent)}");
            Writer.Write($"\"{Key}\" {Json.INNER_FIELD_SEPARATOR} ");
          } else {
            Writer.Write($"\"{Key}\"{Json.INNER_FIELD_SEPARATOR}");
          }

          Writer.Write(Content.RenderAsBytes(formatted));

          return RetVal.ToArray();
        }
      }
    }

    public static IJsonPair Parse(string source) {

      return Parse(source, Default);

    }
    public static IJsonPair Parse(string source, IJsonPair defaultValue) {
      #region === Validate parameters ===
      if ( string.IsNullOrWhiteSpace(source) ) {
        Trace.WriteLine("Unable to parse Json string : source is null or empty");
        return defaultValue;
      }

      if ( !source.Contains(":") ) {
        Trace.WriteLine("Unable to parse Json string : source is invalid");
        return defaultValue;
      }

      string ProcessedSource = source.Trim();

      if ( !ProcessedSource.StartsWith("\"") ) {
        Trace.WriteLine("Unable to parse Json string : source is invalid");
        return defaultValue;
      }
      #endregion === Validate parameters ===

      #region --- Get the key --------------------------------------------
      int LengthOfSource = ProcessedSource.Length;
      bool InQuote = false;
      bool GotTheKey = false;
      int i = 0;

      StringBuilder TempKey = new StringBuilder();

      while ( i < LengthOfSource && !GotTheKey ) {

        char CurrentChar = ProcessedSource[i];

        if ( i == 0 && CurrentChar == Json.CHR_DOUBLE_QUOTE ) {
          InQuote = true;
          i++;
          continue;
        }

        if ( i > 0 && CurrentChar == Json.CHR_DOUBLE_QUOTE && ProcessedSource[i - 1] != Json.CHR_BACKSLASH ) {
          if ( InQuote ) {
            i++;
            InQuote = false;
            GotTheKey = true;
            continue;
          } else {
            Trace.WriteLine("Unable to parse Json string : source is invalid");
            return defaultValue;
          }
        }

        if ( i > 0 && InQuote ) {
          TempKey.Append(CurrentChar);
          i++;
          continue;
        }

        i++;
        continue;

      }

      if ( InQuote || !GotTheKey ) {
        Trace.WriteLine("Unable to parse Json string : source is invalid");
        return defaultValue;
      }
      #endregion --- Get the key --------------------------------------------

      // Skip white spaces
      while ( i < ProcessedSource.Length && ProcessedSource[i].IsWhiteSpace() ) {
        i++;
      }

      if ( ProcessedSource[i] != Json.INNER_FIELD_SEPARATOR ) {
        Trace.WriteLine("Unable to parse Json string : source is invalid");
        return defaultValue;
      }

      #region --- Get the value --------------------------------------------
      string TempContent = ProcessedSource.Substring(i + 1).TrimStart();
      #endregion --- Get the value --------------------------------------------

      JsonPair RetVal = new JsonPair {
        Key = TempKey.ToString(),
        Content = JsonValue.Parse(TempContent)
      };

      return RetVal;
    }

    public T SafeGetValue<T>(T defaultValue) {
      try {

        if ( typeof(T) == typeof(string) ) {
          if ( Content is JsonString ) {
            return (T)Convert.ChangeType(StringContent.Value, typeof(T));
          }
          if ( Content is JsonBool ) {
            return (T)Convert.ChangeType(BoolContent.RenderAsString(), typeof(T));
          }
          if ( Content is JsonDateTime ) {
            return (T)Convert.ChangeType(DateTimeContent.RenderAsString(), typeof(T));
          }
          if ( Content is JsonInt ) {
            return (T)Convert.ChangeType(IntContent.RenderAsString(), typeof(T));
          }
          if ( Content is JsonLong ) {
            return (T)Convert.ChangeType(LongContent.RenderAsString(), typeof(T));
          }
          if ( Content is JsonFloat ) {
            return (T)Convert.ChangeType(FloatContent.RenderAsString(), typeof(T));
          }
          if ( Content is JsonDouble ) {
            return (T)Convert.ChangeType(DoubleContent.RenderAsString(), typeof(T));
          }
          if ( Content is JsonArray || Content is JsonObject ) {
            return (T)Convert.ChangeType(Content.RenderAsString(), typeof(T));
          }
        }

        if ( typeof(T) == typeof(bool) ) {
          if ( Content is JsonBool ) {
            return (T)Convert.ChangeType(BoolContent.Value, typeof(T));
          }
          if ( Content is JsonString ) {
            return (T)Convert.ChangeType(StringContent.Value.ToBool(), typeof(T));
          }
        }

        if ( typeof(T) == typeof(JsonArray) && Content is JsonArray ) {
          return (T)Convert.ChangeType(Content, typeof(T));
        }

        if ( typeof(T) == typeof(JsonObject) && Content is JsonObject ) {
          return (T)Convert.ChangeType(Content, typeof(T));
        }

        if ( typeof(T) == typeof(DateTime) ) {
          if ( Content is JsonDateTime ) {
            return (T)Convert.ChangeType(DateTimeContent.Value, typeof(T));
          }
          if ( Content is JsonString ) {
            return (T)Convert.ChangeType(DateTime.Parse(StringContent.Value, DateTimeFormatInfo.InvariantInfo), typeof(T));
          }
        }

        if ( typeof(T) == typeof(float) || typeof(T) == typeof(double) || typeof(T) == typeof(int) || typeof(T) == typeof(long) ) {
          if ( Content is JsonFloat ) {
            return (T)Convert.ChangeType(FloatContent.Value, typeof(T));
          }
          if ( Content is JsonDouble ) {
            return (T)Convert.ChangeType(DoubleContent.Value, typeof(T));
          }
          if ( Content is JsonInt ) {
            return (T)Convert.ChangeType(IntContent.Value, typeof(T));
          }
          if ( Content is JsonLong ) {
            return (T)Convert.ChangeType(LongContent.Value, typeof(T));
          }
        }

        return defaultValue;

      } catch {
        return defaultValue;
      }
    }

  }


}
