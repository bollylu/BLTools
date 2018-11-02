using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Text;

namespace BLTools.Json {
  public class JsonFloat : IJsonValue<float?> {

    public float? Value { get; set; }

    #region --- Constructor(s) ---------------------------------------------------------------------------------
    public JsonFloat() {
      Value = default;
    }

    public JsonFloat(float jsonFloat) {
      Value = jsonFloat;
    }

    public JsonFloat(JsonFloat jsonFloat) {
      Value = jsonFloat.Value;
    }

    public JsonFloat(string jsonFloat) {
      try {
        Value = float.Parse(jsonFloat, CultureInfo.InvariantCulture);
      } catch ( Exception ex ) {
        Trace.WriteLine($"Unable to parse float : {jsonFloat} : {ex.Message}");
        Value = null;
      }
    }

    public void Dispose() {
      Value = null;
    }
    #endregion --- Constructor(s) ------------------------------------------------------------------------------

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
        RetVal.Append(( (float)Value ).ToString(CultureInfo.InvariantCulture));
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
            Writer.Write(( (float)Value ).ToString(CultureInfo.InvariantCulture));
          }

          return RetVal.ToArray();
        }
      }
    }

  }
}
