using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Xml.Linq;

namespace BLTools.Google.Apis.Direction {

  public class DirectionsQuery {
    private static string DefaultQueryUrl = "https://maps.googleapis.com/maps/api/directions/xml?";


    public DirectionsQueryParameters Parameters { get; private set; }
    public DirectionsResponse Response { get; private set; }

    public DirectionsQuery(DirectionsQueryParameters parameters) {
      Parameters = new DirectionsQueryParameters(parameters);
    }

    public DirectionsQuery() {
    }

    public async Task ExecuteAsync() {
      if (Parameters == null) {
        throw new ApplicationException("Missing parameters for querying google");
      }
      await ExecuteAsync(Parameters);
    }

    public async Task ExecuteAsync(string origin, string destination) {
      await ExecuteAsync(new DirectionsQueryParameters(origin, destination));
    }

    public async Task ExecuteAsync(DirectionsQueryParameters parameters) {

      try {
        using (HttpClient Client = new HttpClient()) {
          foreach (string QueryItem in parameters.BuildQuery()) {
            string QueryUrl = string.Format("{0}{1}", DefaultQueryUrl, QueryItem);
            //Trace.WriteLine(QueryUrl);

            HttpResponseMessage ResponseMessage = await Client.GetAsync(QueryUrl);
            if (ResponseMessage.IsSuccessStatusCode) {
              string RawResponse = await ResponseMessage.Content.ReadAsStringAsync();
              //Trace.WriteLine(RawResponse);
              XDocument XmlResponse = XDocument.Parse(RawResponse);
              if (Response == null) {
                Response = new DirectionsResponse(XmlResponse.Root);
                if (Response.Status != EDirectionsResponseStatus.OK) {
                  break;
                }
              } else {
                DirectionsResponse NextResponse = new DirectionsResponse(XmlResponse.Root);
                if (NextResponse.Status != EDirectionsResponseStatus.OK) {
                  break;
                }
                Response.Route.Legs.AddRange(NextResponse.Route.Legs);
              }
            }

          }
        }
      } catch (Exception ex) {
        Trace.WriteLine(string.Format("Error getting url : {0} : {1}", DefaultQueryUrl, ex.Message));
      }

      if (OnQueryCompleted != null) {
        OnQueryCompleted(this, EventArgs.Empty);
      }

    }

    public DirectionsQuery Execute() {
      if (Parameters == null) {
        throw new ApplicationException("Missing parameters for querying google");
      }
      return Execute(Parameters);
    }

    public DirectionsQuery Execute(string origin, string destination) {
      return Execute(new DirectionsQueryParameters(origin, destination));
    }

    public DirectionsQuery Execute(DirectionsQueryParameters parameters) {

      ExecuteAsync().Wait();
      return this;

    }

    public event EventHandler OnQueryCompleted;

    public override string ToString() {
      StringBuilder RetVal = new StringBuilder();
      RetVal.AppendLine(Parameters.ToString());
      if (Response != null) {
        RetVal.AppendLine(Response.ToString());
      }
      return RetVal.ToString();
    }
  }
}
