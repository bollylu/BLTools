using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLTools.Web.HTML {
  public class THtmlAttribute {
    public string Key { get; set; }
    public string Value { get; set; }

    public THtmlAttribute() {
      Key = "";
      Value = "";
    }

    public THtmlAttribute(string key, string value) {
      Key = key;
      Value = value;
    }

    public THtmlAttribute(string keyValue)
      : this() {
      if (!keyValue.Contains("=")) {
        return;
      }
      string[] KeyValue = keyValue.Split('=');
      Key = KeyValue[0];
      Value = KeyValue[1];
    }

    public override string ToString() {
      StringBuilder RetVal = new StringBuilder();
      RetVal.AppendFormat("{0}={1};", Key, Value);
      return RetVal.ToString();
    }
  }
}
