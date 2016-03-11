using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace BLTools.Google.Apis.Direction {
  public class DirectionsResponseDistance {

    internal static string THIS_ELEMENT = "distance";
    internal static string DISTANCE_ELEMENT_VALUE = "value";
    internal static string DISTANCE_ELEMENT_TEXT = "text";

    public int Value { get; private set; }
    public string Text { get; private set; }

    public DirectionsResponseDistance(XElement distance) {
      if (distance == null) {
        throw new ArgumentNullException("distance");
      }
      Value = distance.SafeReadElementValue<int>(DISTANCE_ELEMENT_VALUE, 0);
      Text = distance.SafeReadElementValue<string>(DISTANCE_ELEMENT_TEXT, "");
    }

    public override string ToString() {
      StringBuilder RetVal = new StringBuilder();
      RetVal.AppendFormat("Distance: {0}, {1}", Value, Text);
      return RetVal.ToString();
    }

    public XElement ToXml() {
      XElement RetVal = new XElement(THIS_ELEMENT);
      RetVal.SetElementValue(DISTANCE_ELEMENT_VALUE, Value);
      RetVal.SetElementValue(DISTANCE_ELEMENT_TEXT, Text);
      return RetVal;
    }

  }
}
