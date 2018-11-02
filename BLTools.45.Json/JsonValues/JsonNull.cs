using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace BLTools.Json {
  public class JsonNull : IJsonValue<object> {
    public object Value { get => null; set { } }

    public void Dispose() {
    }

    public string RenderAsString(bool formatted = false, int indent = 0, bool needFrontSpaces = true) {
      if ( formatted && needFrontSpaces ) {
        return $"{StringExtension.Spaces(indent)}null";
      } else {
        return "null";
      }
    }

    public byte[] RenderAsBytes(bool formatted = false, int indent = 0) {

      using ( MemoryStream RetVal = new MemoryStream() ) {
        using ( JsonWriter Writer = new JsonWriter(RetVal) ) {
          if ( formatted ) {
            Writer.Write($"{StringExtension.Spaces(indent)}null");
            return RetVal.ToArray();
          } else {
            Writer.Write("null");
            return RetVal.ToArray();
          }
        }
      }
    }

    public static JsonNull Default {
      get {
        if ( _Default == null ) {
          _Default = new JsonNull();
        }
        return _Default;
      }
    }
    private static JsonNull _Default;

  }
}
