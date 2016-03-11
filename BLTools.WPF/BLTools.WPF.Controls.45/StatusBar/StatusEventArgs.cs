using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BLTools.WPF.Controls {
  public class StatusEventArgs : EventArgs {
    public enum StatusEventLocationEnum {
      Left,
      Right
    }
    public string Message { get; set; }
    public StatusEventLocationEnum StatusLocation { get; set; }
    public StatusEventArgs(StatusEventLocationEnum statusLocation, string message = "") {
      StatusLocation = statusLocation;
      Message = message;

    }
  }
}
