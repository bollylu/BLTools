using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media.Imaging;

namespace BLTools.WPF {
  /// <summary>
  /// Test for difference
  /// </summary>
  public class ValueNotEqualToConverter : IValueConverter {

    public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture) {
      switch(value.GetType().Name.ToUpper()) {
        case "INT32":
          return (int)value != System.Convert.ToInt32(parameter);
        case "STRING":
          return (string)value != (string)parameter;
        default:
          Trace.WriteLine(string.Format("value is of type {0} : {1}" ,value.GetType().Name, value));
          return value != parameter;
      }
      
    }

    public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture) {
      return Binding.DoNothing;
    }
  }
}
