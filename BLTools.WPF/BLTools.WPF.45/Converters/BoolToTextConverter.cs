using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media.Imaging;

namespace BLTools.WPF {
  public class BoolToTextConverter : IValueConverter {
    public string NullText { get; set; }
    public string TrueText { get; set; }
    public string FalseText { get; set; }

    public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture) {
      if (!(value is bool?)) {
        return Binding.DoNothing;
      }

      if (((bool?)value) == null) {
        return NullText ?? "";
      }

      if ((bool)value) {
        return TrueText ?? "";
      } else {
        return FalseText ?? "";
      }
    }

    public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture) {
      return Binding.DoNothing;
    }
  }
}
