using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace BLTools.Google.Apis.Direction {
  public class DirectionsResponseStep {

    internal static string THIS_ELEMENT = "step";
    internal static string STEP_ELEMENT_HTML_INSTRUCTIONS = "html_instructions";
    internal static string STEP_ELEMENT_TRAVEL_MODE = "travel_mode";

    public DirectionsResponseDuration Duration { get; private set; }
    public DirectionsResponseDistance Distance { get; private set; }
    public string HtmlInstructions { get; private set; }

    public DirectionsResponseStep(XElement step) {
      Duration = new DirectionsResponseDuration(step.SafeReadElement(DirectionsResponseDuration.THIS_ELEMENT));
      Distance = new DirectionsResponseDistance(step.SafeReadElement(DirectionsResponseDistance.THIS_ELEMENT));
      HtmlInstructions = step.SafeReadElementValue<string>(STEP_ELEMENT_HTML_INSTRUCTIONS, "");
    }
  }
}
