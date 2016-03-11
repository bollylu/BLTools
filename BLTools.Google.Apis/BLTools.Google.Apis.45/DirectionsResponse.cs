using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using BLTools;

namespace BLTools.Google.Apis.Direction {
  public class DirectionsResponse : IToXml {

    internal static string THIS_ELEMENT = "DirectionsResponse";
    internal static string RESPONSE_ELEMENT_STATUS = "status";
    internal string RESPONSE_ELEMENT_ROUTE = "route";

    public EDirectionsResponseStatus Status { get; set; }
    public DirectionsResponseRoute Route { get; set; }

    public int CompleteDistanceValue {
      get {
        return Route.Legs.Sum(x => x.Distance.Value);
      }
    }
    public string CompleteDistanceText {
      get {
        return string.Format("{0} Km", CompleteDistanceValue/1000.0);
      }
    }

    public DirectionsResponse(XElement directionResponse) {
      #region Validate parameters
      if (directionResponse == null) {
        throw new ArgumentNullException("directionResponse");
      }
      if (!directionResponse.HasElements) {
        throw new ArgumentException("directionResponse");
      } 
      #endregion Validate parameters
      Status = (EDirectionsResponseStatus)Enum.Parse(typeof(EDirectionsResponseStatus), directionResponse.SafeReadElementValue<string>(RESPONSE_ELEMENT_STATUS, "Unknown"));
      Route = new DirectionsResponseRoute(directionResponse.SafeReadElement(RESPONSE_ELEMENT_ROUTE));
    }

    public override string ToString() {
      StringBuilder RetVal = new StringBuilder();
      RetVal.AppendFormat("Response : {0}", Status.ToString());
      RetVal.AppendFormat("\nRoute : {0}", Route.ToString());
      return RetVal.ToString();
    }

    public XElement ToXml() {
      XElement RetVal = new XElement(THIS_ELEMENT);
      RetVal.SetElementValue(RESPONSE_ELEMENT_STATUS, Status.ToString());
      RetVal.Add(Route.ToXml());
      return RetVal;
    }
  }
}
