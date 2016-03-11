using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;

namespace SysLog {
  public class SyslogSource : IComparable<SyslogSource> {
    public IPEndPoint Client { get; set; }
    public SyslogRecord.SyslogType ClientType { get; set; }

    public SyslogSource( string client, SyslogRecord.SyslogType clientType ) {
      Client = new IPEndPoint(IPAddress.Parse(client), 514);
      ClientType = clientType;
    }
    public SyslogSource( IPEndPoint client, SyslogRecord.SyslogType clientType ) {
      Client = client;
      ClientType = clientType;
    }

    #region IComparable<SyslogSource> Members

    public int CompareTo( SyslogSource other ) {
      return Client.Address.ToString().CompareTo(other.Client.Address.ToString());
    }

    #endregion
  }

  /*******************************************************************************************************************************/

  public class SyslogSourceCollection : List<SyslogSource> {

  }
}
