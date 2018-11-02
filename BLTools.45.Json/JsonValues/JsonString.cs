using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace BLTools.Json {
  public class JsonString : IJsonValue<string> {

    #region --- Public properties ------------------------------------------------------------------------------
    public string Value { get; set; } 
    #endregion --- Public properties ---------------------------------------------------------------------------

    public static JsonString Default {
      get {
        if ( _Default == null ) {
          _Default = new JsonString();
        }
        return _Default;
      }
    }
    private static JsonString _Default;

    public static JsonString Empty {
      get {
        if ( _Empty == null ) {
          _Empty = new JsonString("");
        }
        return _Empty;
      }
    }
    private static JsonString _Empty;

    #region --- Constructor(s) ---------------------------------------------------------------------------------
    public JsonString() {
      Value = default;
    }

    public JsonString(string jsonString) {
      Value = jsonString.JsonDecode();
    }

    public JsonString(JsonString jsonString) {
      Value = jsonString.Value;
    }

    public void Dispose() {
      Value = null;
    }
    #endregion --- Constructor(s) ------------------------------------------------------------------------------

    #region --- Converters -------------------------------------------------------------------------------------
    public override string ToString() {
      StringBuilder RetVal = new StringBuilder();
      RetVal.Append(Value);
      return RetVal.ToString();
    } 
    #endregion --- Converters -------------------------------------------------------------------------------------

    #region --- Rendering --------------------------------------------
    public string RenderAsString(bool formatted = false, int indent = 0, bool needFrontSpaces = true) {
      StringBuilder RetVal = new StringBuilder();

      if ( formatted && needFrontSpaces) {
        RetVal.Append($"{StringExtension.Spaces(indent)}");
      }
      if ( Value == null ) {
        RetVal.Append(JsonNull.Default.RenderAsString());
      } else {
        RetVal.Append($@"""{Value.JsonEncode()}""");
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
            Writer.Write(JsonNull.Default.RenderAsBytes());
          } else {
            Writer.Write($@"""{Value.JsonEncode()}""");
          }

          return RetVal.ToArray();
        }
      }
    }
    #endregion --- Rendering --------------------------------------------

    //#region --- Operators --------------------------------------------
    //public static implicit operator string(JsonString source) {
    //  if ( source == null ) {
    //    return null;
    //  }
    //  return source.Value;
    //}

    //public static implicit operator JsonString(string source) {
    //  if ( source == null ) {
    //    return null;
    //  }
    //  return new JsonString(source);
    //}
    //#endregion --- Operators --------------------------------------------
  }
}
