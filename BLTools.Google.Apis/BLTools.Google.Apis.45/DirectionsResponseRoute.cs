using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace BLTools.Google.Apis.Direction {
  public class DirectionsResponseRoute : IToXml {

    internal static string THIS_ELEMENT = "route";
    internal static string ROUTE_ELEMENT_LEG = "leg";
    internal static string ROUTE_ELEMENT_SUMMARY = "summary";

    public string Summary { get; set; }
    public readonly List<DirectionsResponseLeg> Legs;

    public DirectionsResponseRoute() {
      Legs = new List<DirectionsResponseLeg>();
    }

    public DirectionsResponseRoute(XElement route) : this() {
      Summary = route.SafeReadElementValue<string>(ROUTE_ELEMENT_SUMMARY, "");
      
      if (route.HasElements && route.Elements(ROUTE_ELEMENT_LEG).Count() > 0) {
        foreach (XElement RouteLegItem in route.Elements(ROUTE_ELEMENT_LEG)) {
          Legs.Add(new DirectionsResponseLeg(RouteLegItem));
        }
      }
    }

    public System.Xml.Linq.XElement ToXml() {
      throw new NotImplementedException();
    }
  }
}
