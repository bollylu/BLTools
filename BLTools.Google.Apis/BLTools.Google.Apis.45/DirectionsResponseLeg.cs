using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace BLTools.Google.Apis.Direction {
  public class DirectionsResponseLeg {

    internal static string THIS_ELEMENT = "leg";
    internal static string LEG_ELEMENT_START_ADDRESS = "start_address";
    internal static string LEG_ELEMENT_END_ADDRESS = "end_address";

    public List<DirectionsResponseStep> Steps { get; private set; }
    public DirectionsResponseDuration Duration { get; private set; }
    public DirectionsResponseDistance Distance { get; private set; }
    public string StartAddress { get; private set; }
    public string EndAddress { get; private set; }

    public DirectionsResponseLeg(XElement leg) {
      Steps = new List<DirectionsResponseStep>();
      if (leg.HasElements && leg.Elements(DirectionsResponseStep.THIS_ELEMENT).Count() > 0) {
        foreach (XElement StepItem in leg.Elements(DirectionsResponseStep.THIS_ELEMENT)) {
          Steps.Add(new DirectionsResponseStep(StepItem));
        }
      }
      Duration = new DirectionsResponseDuration(leg.SafeReadElement(DirectionsResponseDuration.THIS_ELEMENT));
      Distance = new DirectionsResponseDistance(leg.SafeReadElement(DirectionsResponseDistance.THIS_ELEMENT));
      StartAddress = leg.SafeReadElementValue<string>(LEG_ELEMENT_START_ADDRESS, "");
      EndAddress = leg.SafeReadElementValue<string>(LEG_ELEMENT_END_ADDRESS, "");
    }
  }
}
