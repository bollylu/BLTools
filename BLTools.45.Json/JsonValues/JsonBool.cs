using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace BLTools.Json {
  public class JsonBool : IJsonValue<bool?> {

    public bool? Value { get; set; }

    public static JsonBool Default {
      get {
        if ( _Default == null ) {
          _Default = new JsonBool();
        }
        return _Default;
      }
    }
    private static JsonBool _Default;

    #region --- Constructor(s) --------------------------------------------
    public JsonBool() {
      Value = default;
    }

    public JsonBool(bool jsonBool) {
      Value = jsonBool;
    }

    public JsonBool(JsonBool jsonBool) {
      Value = jsonBool.Value;
    }

    public JsonBool(string jsonBool) {
      Value = jsonBool.ToBool();
    }

    public void Dispose() {
      Value = null;
    }
    #endregion --- Constructor(s) --------------------------------------------


    public override string ToString() {
      StringBuilder RetVal = new StringBuilder();
      RetVal.Append(Value.HasValue ? Value.ToString() : Json.NULL_VALUE);
      return RetVal.ToString();
    }

    public string RenderAsString(bool formatted = false, int indent = 0, bool needFrontSpaces = true) {
      StringBuilder RetVal = new StringBuilder();

      if (formatted && needFrontSpaces ) {
        RetVal.Append($"{StringExtension.Spaces(indent)}");
      }
      if ( !Value.HasValue ) {
        RetVal.Append(JsonNull.Default.RenderAsString());
      } else {
        RetVal.Append((bool)Value ? Json.TRUE_VALUE : Json.FALSE_VALUE);
      }

      return RetVal.ToString();
    }

    public byte[] RenderAsBytes(bool formatted = false, int indent = 0) {

      using ( MemoryStream RetVal = new MemoryStream() ) {
        using ( JsonWriter Writer = new JsonWriter(RetVal) ) {

          if ( formatted ) {
            Writer.Write($"{StringExtension.Spaces(indent)}");
          }
          if ( !Value.HasValue ) {
            Writer.Write(JsonNull.Default.RenderAsBytes());
          } else {
            Writer.Write((bool)Value ? Json.TRUE_VALUE : Json.FALSE_VALUE);
          }

          return RetVal.ToArray();
        }
      }
    }

    
  }
}
