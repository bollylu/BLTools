using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLTools.WPF {
  /// <summary>
  /// Allow to enable/disable dynamically graphical components.
  /// </summary>
  public interface IEnablers {
    /// <summary>
    /// Is called whenever to refresh to IsEnabled (or other properties) of the components
    /// </summary>
    void ApplyEnablers();
    /// <summary>
    /// Hook to call whenever ApplyEnablers() is called
    /// </summary>
    event EventHandler OnApplyEnablers;
  }
}
