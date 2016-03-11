using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BLTools {
  /// <summary>
  /// EventArgs holding both an int and a message
  /// </summary>
  public class IntAndMessageEventArgs : EventArgs {
    /// <summary>
    /// The int result to pass
    /// </summary>
    public int Result { get; protected set; }
    /// <summary>
    /// The message
    /// </summary>
    public string Message { get; protected set; }
    /// <summary>
    /// Parameterless constructor
    /// </summary>
    public IntAndMessageEventArgs() {
      Result = 0;
      Message = "";
    }
    /// <summary>
    /// Constructor with int result and a message
    /// </summary>
    /// <param name="result">The result to pass</param>
    /// <param name="message">The message</param>
    public IntAndMessageEventArgs(int result, string message = "") {
      Result = result;
      Message = message;
    }
  }
}
