using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace BLTools.Json {
  public class JsonLong : IJsonValue<long?> {

    public long? Value { get; set; }

    #region --- Constructor(s) --------------------------------------------
    public JsonLong() {
      Value = default;
    }

    public JsonLong(long jsonLong) {
      Value = jsonLong;
    }

    public JsonLong(JsonLong jsonLong) {
      Value = jsonLong.Value;
    }

    public JsonLong(string jsonLong) {
      try {
        Value = long.Parse(jsonLong);
      } catch ( Exception ex ) {
        Trace.WriteLine($"Unable to parse long : {jsonLong} : {ex.Message}");
        Value = null;
      }
    }

    public void Dispose() {
      Value = null;
    } 
    #endregion --- Constructor(s) --------------------------------------------

    public override string ToString() {
      StringBuilder RetVal = new StringBuilder();
      RetVal.Append(Value.HasValue ? Value.ToString() : Json.NAN);
      return RetVal.ToString();
    }

    public string RenderAsString(bool formatted = false, int indent = 0, bool needFrontSpaces = true) {
      StringBuilder RetVal = new StringBuilder();

      if ( formatted && needFrontSpaces ) {
        RetVal.Append($"{StringExtension.Spaces(indent)}");
      }
      if ( Value == null ) {
        RetVal.Append(Json.NAN);
      } else {
        RetVal.Append(Value.ToString());
      }

      return RetVal.ToString();
    }

    public byte[] RenderAsBytes(bool formatted = false, int indent = 0) {

      using ( MemoryStream RetVal = new MemoryStream() ) {
        using ( JsonWriter Writer = new JsonWriter(RetVal) ) {

          if ( formatted ) {
            Writer.Write($"{StringExtension.Spaces(indent)}");
          }
          if ( Value == null ) {
            Writer.Write(Json.NAN);
          } else {
            Writer.Write(Value.ToString());
          }

          return RetVal.ToArray();
        }
      }
    }

  }
}
