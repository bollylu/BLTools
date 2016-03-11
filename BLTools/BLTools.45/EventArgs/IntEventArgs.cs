using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLTools {
  /// <summary>
  /// EventArgs hold an int
  /// </summary>
  public class IntEventArgs : EventArgs {
    /// <summary>
    /// The int result to pass
    /// </summary>
    public int Value { get; protected set; }
    /// <summary>
    /// Parameterless constructor
    /// </summary>
    public IntEventArgs() {
      Value = 0;
    }
    /// <summary>
    /// Constructor with the int
    /// </summary>
    /// <param name="value"></param>
    public IntEventArgs(int value) {
      Value = value;
    }
  }
}
