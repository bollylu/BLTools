using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;

namespace BLTools.Json {
  public class JsonValue {

    public static IJsonValue Parse(string source) {

      if ( string.IsNullOrWhiteSpace(source) ) {
        return JsonNull.Default;
      }

      string ProcessedSource = source.Trim();

      if ( ProcessedSource == Json.TRUE_VALUE || ProcessedSource == Json.FALSE_VALUE ) {
        return new JsonBool(ProcessedSource.ToBool());
      }

      if ( ProcessedSource == Json.NULL_VALUE ) {
        return JsonNull.Default;
      }

      char FirstChar = ProcessedSource.First();
      char LastChar = ProcessedSource.Last();

      if ( FirstChar == Json.CHR_DOUBLE_QUOTE && LastChar == Json.CHR_DOUBLE_QUOTE ) {
        return new JsonString(ProcessedSource.RemoveExternalQuotes());
      }

      if ( ProcessedSource.IsNumericOnly() ) {
        int Dummy;
        if ( int.TryParse(ProcessedSource, out Dummy) ) {
          return new JsonInt(ProcessedSource);
        }
        return new JsonLong(ProcessedSource);
      }

      if ( ProcessedSource.IsNumeric() ) {
        return new JsonDouble(ProcessedSource);
      }

      if ( FirstChar == Json.START_OF_ARRAY && LastChar == Json.END_OF_ARRAY ) {
        return JsonArray.Parse(ProcessedSource);
      }

      if ( FirstChar == Json.START_OF_OBJECT && LastChar == Json.END_OF_OBJECT ) {
        return JsonObject.Parse(ProcessedSource);
      }

      return JsonNull.Default;

    }

    public static T Parse<T>(string source) where T : class, IJsonValue {
      return Parse(source) as T;
    }

    public static IJsonValue Load(string filename) {
      const string ErrorMessageBase = "Unable to load Json content";

      if ( string.IsNullOrWhiteSpace(filename) ) {
        return JsonNull.Default;
      }

      if ( !File.Exists(filename) ) {
        Trace.WriteLine($"{ErrorMessageBase} from \"{filename}\" : file is missing or access denied");
        return JsonNull.Default;
      }

      try {
        return Parse(File.ReadAllText(filename));
      } catch (Exception ex) {
        Trace.WriteLine($"{ErrorMessageBase} from \"{filename}\" : {ex.Message}");
        return JsonNull.Default;
      }
    }

    public static void Save(string filename, IJsonValue jsonValue, bool isOutputFormatted = true) {
      const string ErrorMessageBase = "Unable to save Json content";

      if ( string.IsNullOrWhiteSpace(filename) ) {
        Trace.WriteLine($"{ErrorMessageBase} : filename is missing");
        return;
      }

      if ( jsonValue == null ) {
        Trace.WriteLine($"{ErrorMessageBase} to \"{filename}\" : jsonValue is missing");
        return;
      }

      try {
        File.WriteAllText(filename, jsonValue.RenderAsString(isOutputFormatted));
      } catch (Exception ex) {
        Trace.WriteLine($"{ErrorMessageBase} to \"{filename}\" : {ex.Message}");
        return;
      }
    }
  }
}
