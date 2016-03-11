using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLTools.Data.FixedLength {
  public class TFixedLengthFiller : TFixedLengthField {
    public TFixedLengthFiller(char padding, int length) : base("filler", padding, length) { }
  }
}
