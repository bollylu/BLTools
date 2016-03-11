using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BLTools {
  /// <summary>
  /// EventArgs holding any object
  /// </summary>
  public class ObjectEventArgs : EventArgs {
    /// <summary>
    /// The object to pass
    /// </summary>
    public object Value { get; protected set; }
    /// <summary>
    /// Parameterless constructor
    /// </summary>
    public ObjectEventArgs() {
      Value = null;
    }
    /// <summary>
    /// Constructor with the object
    /// </summary>
    /// <param name="value"></param>
    public ObjectEventArgs(object value) {
      Value = value;
    }
  }
}
