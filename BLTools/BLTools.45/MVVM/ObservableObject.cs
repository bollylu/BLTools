using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLTools.MVVM {

  /// <summary>
  /// Enable a class to notify of property changed
  /// </summary>
  public class ObservableObject : INotifyPropertyChanged {

    #region === INotifyPropertyChanged ============================================================
    /// <summary>
    /// Event handler for when a property was changed
    /// </summary>
    public event PropertyChangedEventHandler PropertyChanged;

    /// <summary>
    /// Calls any hook when the property was changed
    /// </summary>
    /// <param name="propertyName"></param>
    protected void NotifyPropertyChanged(string propertyName) {
      if (PropertyChanged != null) {
        PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
      }
    }

    //public static event PropertyChangedEventHandler GlobalPropertyChanged;
    //protected static void GlobalNotifyPropertyChanged(string propertyName) {
    //  if (GlobalPropertyChanged != null) {
    //    GlobalPropertyChanged(null, new PropertyChangedEventArgs(propertyName));
    //  }
    //}
    #endregion === INotifyPropertyChanged =========================================================


  }
}
