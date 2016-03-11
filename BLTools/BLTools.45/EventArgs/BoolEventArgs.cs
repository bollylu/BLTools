using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BLTools {
  /// <summary>
  /// EventArgs holding a boolean value
  /// </summary>
  public class BoolEventArgs : EventArgs {
    /// <summary>
    /// The boolean value
    /// </summary>
    public bool Result { get; protected set; }
    /// <summary>
    /// Parameterless constructor
    /// </summary>
    public BoolEventArgs() {
      Result = false;
    }
    /// <summary>
    /// Constructor with boolean result
    /// </summary>
    /// <param name="result">The boolean result</param>
    public BoolEventArgs(bool result) {
      Result = result;
    }
  }
}
