using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace BLTools.Json {
  public class JsonWriter : BinaryWriter {

    public JsonWriter(Stream stream) : base(stream) { }

    public override void Write(string value) {
      base.Write(value.ToArray());
    }

    public void WriteLine() {
      base.Write(Environment.NewLine.ToArray());
    }

    public void WriteLine(string value) {
      if (!string.IsNullOrEmpty(value)) {
        base.Write(value.ToArray());
      }
      base.Write(Environment.NewLine.ToArray());
    }
  }
}
