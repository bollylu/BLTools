using System;
using System.Collections.Generic;
using System.Text;

namespace BLTools.Json {
  public interface IJsonPair : IDisposable {
    string Key { get; }
    IJsonValue Content { get; }

    JsonString StringContent { get; }
    JsonInt IntContent { get; }
    JsonLong LongContent { get; }
    JsonFloat FloatContent { get; }
    JsonDouble DoubleContent { get; }
    JsonBool BoolContent { get; }
    JsonDateTime DateTimeContent { get; }
    JsonArray ArrayContent { get; }
    JsonObject ObjectContent { get; }

    string RenderAsString(bool formatted = false, int indent = 0, bool needFrontSpaces = true);
    byte[] RenderAsBytes(bool formatted = false, int indent = 0);

    T SafeGetValue<T>(T defaultValue);
    
  }
}
