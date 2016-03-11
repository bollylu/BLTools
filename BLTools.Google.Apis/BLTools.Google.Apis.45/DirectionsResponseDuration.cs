using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace BLTools.Google.Apis.Direction {
  public class DirectionsResponseDuration : IToXml{

    internal static string THIS_ELEMENT = "duration";
    internal static string DURATION_ELEMENT_VALUE = "value";
    internal static string DURATION_ELEMENT_TEXT = "text";

    public int Value { get; private set; }
    public string Text { get; private set; }

    public DirectionsResponseDuration(XElement duration) {
      if (duration == null) {
        throw new ArgumentNullException("duration");
      }
      Value = duration.SafeReadElementValue<int>(DURATION_ELEMENT_VALUE, 0);
      Text = duration.SafeReadElementValue<string>(DURATION_ELEMENT_TEXT, "");
    }

    public override string ToString() {
      StringBuilder RetVal = new StringBuilder();
      RetVal.AppendFormat("Duration: {0}, {1}", Value, Text);
      return RetVal.ToString();
    }

    public XElement ToXml() {
      XElement RetVal = new XElement(THIS_ELEMENT);
      RetVal.SetElementValue(DURATION_ELEMENT_VALUE, Value);
      RetVal.SetElementValue(DURATION_ELEMENT_TEXT, Text);
      return RetVal;
    }
  }
}
