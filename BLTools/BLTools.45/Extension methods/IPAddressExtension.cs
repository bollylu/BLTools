using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;

namespace BLTools {
  /// <summary>
  /// Extensions for IPAddress
  /// </summary>
  public static class IPAddressExtension {

    /// <summary>
    /// Obtain the subnet for a given IPAddress and a netmask
    /// </summary>
    /// <param name="netmask">The netmask as a byte array of 4 bytes</param>
    /// <returns>The subnet</returns>
    public static IPAddress GetSubnet(this IPAddress ipAddress, byte[] netmask) {
      byte[] SourceIP = ipAddress.GetAddressBytes();
      byte[] RetVal = new byte[4];

      for (int i = 0; i < 3; i++) {
        RetVal[i] = (byte)((byte)SourceIP[i] & (byte)netmask[i]);
      }

      return new IPAddress(RetVal);
    }

    /// <summary>
    /// Obtain the subnet for a given IPAddress and a netmask
    /// </summary>
    /// <param name="netmask">The netmask as an IPAddress</param>
    /// <returns>The subnet</returns>
    public static IPAddress GetSubnet(this IPAddress ipAddress, IPAddress netmask) {
      return ipAddress.GetSubnet(netmask.GetAddressBytes());
    }
  }
}
