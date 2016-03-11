using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLTools.Data.FixedLength {
  public class TFixedLengthSchema : List<TFixedLengthField> {

    public TFixedLengthField this[string name] {
      get {
        TFixedLengthField RetVal = this.Find(d => d.Name == name);
        if (RetVal != null) {
          return RetVal;
        } else {
          return null;
        }
      }
    }

  }
}
