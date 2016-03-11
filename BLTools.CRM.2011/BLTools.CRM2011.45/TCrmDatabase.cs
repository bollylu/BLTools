using Microsoft.Xrm.Sdk.Client;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLTools.CRM2011 {
  public class TCrmDatabase : IDisposable {

    protected const string DISCOVERY_URI_FORMAT_HTTP = "http://{0}/XRMServices/2011/Discovery.svc";
    protected const string DISCOVERY_URI_FORMAT_HTTPS = "https://{0}/XRMServices/2011/Discovery.svc";

    protected const string ORGANIZATIONSERVICE_URI_FORMAT_HTTP = "http://{0}/{1}/XRMServices/2011/Organization.svc";
    protected const string ORGANIZATIONSERVICE_URI_FORMAT_HTTPS = "https://{0}/{1}/XRMServices/2011/Organization.svc";

    protected const string HOMEREALM_URI_FORMAT_HTTP = "http://{0}/{1}/XRMServices/2011/Organization.svc";
    protected const string HOMEREALM_URI_FORMAT_HTTPS = "https://{0}/{1}/XRMServices/2011/Organization.svc";

    public string ServerAddress { get; set; }
    public string OrganizationName { get; set; }
    public string UserName { get; set; }
    public string Password { get; set; }

    public OrganizationServiceProxy OSP;

    public static bool IsDebug = false;

    public TCrmDatabase(string serverAddress, string organizationName, string userName, string password, bool useSSL = true) {

      ServerAddress = serverAddress;
      OrganizationName = organizationName;
      UserName = userName;
      Password = password;

      if (IsDebug) {
        Trace.WriteLine(string.Format("Connection to {0}:{1} using {2}", serverAddress, organizationName, userName));
      }
      ServerConnection.Configuration CurrentConfiguration = new ServerConnection.Configuration();
      CurrentConfiguration.EndpointType = AuthenticationProviderType.ActiveDirectory;
      CurrentConfiguration.ServerAddress = ServerAddress;
      CurrentConfiguration.OrganizationName = organizationName;
      if (useSSL) {
        CurrentConfiguration.DiscoveryUri = new Uri(string.Format(DISCOVERY_URI_FORMAT_HTTPS, ServerAddress));
        CurrentConfiguration.OrganizationUri = new Uri(string.Format(ORGANIZATIONSERVICE_URI_FORMAT_HTTPS, ServerAddress, OrganizationName));
      } else {
        CurrentConfiguration.DiscoveryUri = new Uri(string.Format(DISCOVERY_URI_FORMAT_HTTP, ServerAddress));
        CurrentConfiguration.OrganizationUri = new Uri(string.Format(ORGANIZATIONSERVICE_URI_FORMAT_HTTP, ServerAddress, OrganizationName));
      }
      CurrentConfiguration.Credentials = new System.ServiceModel.Description.ClientCredentials();
      CurrentConfiguration.Credentials.UserName.UserName = UserName;
      CurrentConfiguration.Credentials.UserName.Password = Password;

      OSP = ServerConnection.GetOrganizationProxy(CurrentConfiguration);
      OSP.Timeout = new TimeSpan(0, 5, 0);
      OSP.EnableProxyTypes();

    }

    public TCrmDatabase()
      : this(new ServerConnection()) {
    }

    public TCrmDatabase(ServerConnection serverConnection) {
      if (IsDebug) {
        Trace.WriteLine("Connection through automatic system");
      }
      serverConnection.ReadConfigurations();
      if (serverConnection.configurations != null && serverConnection.configurations.Count > 0) {
        OSP = ServerConnection.GetOrganizationProxy(serverConnection.configurations[0]);
      } else {
        OSP = ServerConnection.GetOrganizationProxy(serverConnection.GetServerConfiguration());
      }
      OSP.Timeout = new TimeSpan(0, 5, 0);
      OSP.EnableProxyTypes();
    }

    public void Dispose() {
      if (OSP != null) {
        OSP.Dispose();
      }
    }








  }
}
