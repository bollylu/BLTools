using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLTools.WPF.Controls {
  public class ProgressBarEventArgs : EventArgs {
    public int MinValue { get; private set; }
    public int MaxValue { get; private set; }
    public int Value { get; private set; }

    public ProgressBarEventArgs(int minValue, int maxValue, int value) {
      MinValue = minValue;
      MaxValue = maxValue;
      Value = value;
    }

  }
}
