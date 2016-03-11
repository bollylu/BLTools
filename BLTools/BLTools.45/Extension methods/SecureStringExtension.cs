using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace BLTools {
  /// <summary>
  /// Compares two SecureString for equality without converting them to string
  /// </summary>
  public static class SecureStringExtension {
    public static bool IsEqualTo(this SecureString stringA, SecureString stringB) {
      IntPtr bstrA = IntPtr.Zero;
      IntPtr bstrB = IntPtr.Zero;
      try {
        bstrA = Marshal.SecureStringToBSTR(stringA);
        bstrB = Marshal.SecureStringToBSTR(stringB);
        int length1 = Marshal.ReadInt32(bstrA, -4);
        int length2 = Marshal.ReadInt32(bstrB, -4);
        if (length1 != length2) {
          return false;
        }
        for (int x = 0; x < length1; ++x) {
          byte b1 = Marshal.ReadByte(bstrA, x);
          byte b2 = Marshal.ReadByte(bstrB, x);
          if (b1 != b2) {
            return false;
          }
        }
        return true;
      } finally {
        if (bstrB != IntPtr.Zero) {
          Marshal.ZeroFreeBSTR(bstrB);
        }
        if (bstrA != IntPtr.Zero) {
          Marshal.ZeroFreeBSTR(bstrA);
        }
      }
    }
  }
}
