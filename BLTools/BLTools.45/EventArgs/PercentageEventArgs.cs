using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLTools {
  /// <summary>
  /// EventArgs holding an int value as percentage
  /// </summary>
  public class PercentageEventArgs : IntEventArgs {
    /// <summary>
    /// Constructor with the percentage value
    /// </summary>
    /// <param name="value"></param>
    public PercentageEventArgs(int value) {
      Value = value;
    }
  }
}
