using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace BLTools.Encryption {
  public class TPIV {
    public byte[] Password;
    public byte[] IV;

    #region Constructor(s)
    public TPIV(byte[] password, byte[] iv) {
      Password = password;
      IV = iv;
    }
    public TPIV(string password, string iv) {
      Password = Encoding.Default.GetBytes(password);
      IV = Encoding.Default.GetBytes(iv);
    }
    public TPIV(string password, string iv, Encoding encoding) {
      Password = encoding.GetBytes(password);
      IV = encoding.GetBytes(iv);
    }

    public static TPIV Generate(string password, int requestedPasswordLength = 16, int requestedIVLength = 16) {
      Rfc2898DeriveBytes DerivedBytes = new Rfc2898DeriveBytes(password, Encoding.Default.GetBytes(password.Reverse().ToString()), 128);
      TPIV RetVal = new TPIV(DerivedBytes.GetBytes(requestedPasswordLength), DerivedBytes.GetBytes(requestedIVLength));
      return RetVal;
    }
    #endregion Constructor(s)
  }
}
