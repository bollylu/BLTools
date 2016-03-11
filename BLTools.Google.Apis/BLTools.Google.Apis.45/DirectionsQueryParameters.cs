using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace BLTools.Google.Apis.Direction {
  public class DirectionsQueryParameters {

    private const int MAX_WAYPOINTS = 7;

    private static string DefaultGoogleKey = "AIzaSyDTaT71vHQDaLd810wVpJIlr6e5L5SApL8";

    public bool Sensor { get; private set; }
    public string Origin { get; set; }
    public string Destination { get; set; }
    public List<string> WayPoints { get; set; }

    public DirectionsQueryParameters() {
      Sensor = false;
      WayPoints = new List<string>();
    }

    public DirectionsQueryParameters(string origin = "", string destination = "")
      : this() {
      Origin = origin;
      Destination = destination;
    }

    public DirectionsQueryParameters(string origin, string destination, IEnumerable<string> wayPoints)
      : this() {
      Origin = origin;
      Destination = destination;
      WayPoints = new List<string>(wayPoints);
    }

    /// <summary>
    /// Builds a DirectionQueryParameters from another DirectionQueryParameters
    /// </summary>
    /// <param name="parameters">The DirectionQueryParameters to copy</param>
    public DirectionsQueryParameters(DirectionsQueryParameters parameters)
      : this() {
      Sensor = parameters.Sensor;
      Origin = parameters.Origin;
      Destination = parameters.Destination;
      WayPoints = new List<string>(parameters.WayPoints);
    }

    /// <summary>
    /// Build a list of queries to submit to Google maps
    /// </summary>
    /// <param name="googleKey">A google key (optional)</param>
    /// <returns>The list of queries</returns>
    public List<string> BuildQuery(string googleKey = "") {

      List<string> RetVal = new List<string>();

      #region No waypoint
      if (WayPoints == null || WayPoints.Count == 0) {
        StringBuilder Query = new StringBuilder("sensor=false");
        if (googleKey != "") {
          Query.AppendFormat("&key={0}", googleKey);
        } else {
          Query.AppendFormat("&key={0}", DefaultGoogleKey);
        }
        Query.AppendFormat("&origin={0}", HttpUtility.UrlEncode(Origin));
        Query.AppendFormat("&destination={0}", HttpUtility.UrlEncode(Destination));
        RetVal.Add(Query.ToString());
        return RetVal;
      }
      #endregion No waypoint

      int LastWaypoint = 0;
      while (LastWaypoint < WayPoints.Count) {
        StringBuilder Query = new StringBuilder("sensor=false");

        if (googleKey != "") {
          Query.AppendFormat("&key={0}", googleKey);
        } else {
          Query.AppendFormat("&key={0}", DefaultGoogleKey);
        }

        IEnumerable<string> CurrentWaypoints;
        if (LastWaypoint == 0) {
          Query.AppendFormat("&origin={0}", HttpUtility.UrlEncode(Origin));
          CurrentWaypoints = WayPoints.Take(Math.Min(MAX_WAYPOINTS, WayPoints.Count - LastWaypoint));
        } else {
          Query.AppendFormat("&origin={0}", HttpUtility.UrlEncode(WayPoints.Skip(LastWaypoint).First()));
          CurrentWaypoints = WayPoints.Skip(LastWaypoint+1).Take(Math.Min(MAX_WAYPOINTS, WayPoints.Count - LastWaypoint));
        }

        if (CurrentWaypoints.Count() == 1) {
          Query.AppendFormat("&waypoints={0}", HttpUtility.UrlEncode(CurrentWaypoints.First()));
        } else {
          Query.AppendFormat("&waypoints={0}", HttpUtility.UrlEncode(string.Join("|", CurrentWaypoints)));
        }

        if ((LastWaypoint + MAX_WAYPOINTS) >= WayPoints.Count) {
          Query.AppendFormat("&destination={0}", HttpUtility.UrlEncode(Destination));
        } else {
          Query.AppendFormat("&destination={0}", HttpUtility.UrlEncode(WayPoints[LastWaypoint + MAX_WAYPOINTS]));
        }

        RetVal.Add(Query.ToString());

        LastWaypoint += MAX_WAYPOINTS;
      }

      return RetVal;
    }

    public override string ToString() {
      StringBuilder RetVal = new StringBuilder();
      foreach (string QueryItem in BuildQuery()) {
        RetVal.Append(QueryItem);
      }
      return RetVal.ToString();
    }
  }
}
