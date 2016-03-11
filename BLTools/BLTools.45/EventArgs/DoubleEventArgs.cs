using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLTools {
  /// <summary>
  /// EventArgs holding a double
  /// </summary>
  public class DoubleEventArgs : EventArgs {
    /// <summary>
    /// The double value to pass
    /// </summary>
    public double Value { get; protected set; }
    /// <summary>
    /// Parameterless constructor
    /// </summary>
    public DoubleEventArgs() {
      Value = 0.0d;
    }
    /// <summary>
    /// Constructor with the double value
    /// </summary>
    /// <param name="value">The double result</param>
    public DoubleEventArgs(double value) {
      Value = value;
    }
  }
}
