using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLTools {
  /// <summary>
  /// EventArgs holding a string
  /// </summary>
  public class StringEventArgs : EventArgs {
    /// <summary>
    /// String argument to pass
    /// </summary>
    public string Value { get; protected set; }

    /// <summary>
    /// Build parameterless
    /// </summary>
    public StringEventArgs() {
      Value = "";
    }
    /// <summary>
    /// Build with a string argument
    /// </summary>
    /// <param name="value"></param>
    public StringEventArgs(string value) {
      Value = value;
    }
  }
}
