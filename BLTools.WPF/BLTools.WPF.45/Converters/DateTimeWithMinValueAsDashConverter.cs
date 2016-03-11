using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace BLTools.WPF {
  public class DateTimeWithMinValueAsDashConverter : IValueConverter {
    public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture) {
      if (!(value is DateTime)) {
        return Binding.DoNothing;
      }

      DateTime DateTimeValue = (DateTime)value;

      if (DateTimeValue == DateTime.MinValue) {
        return "-";
      }
      if (parameter == null || !(parameter is string)) {
        return DateTimeValue.ToDMY();
      } else {
        return DateTimeValue.ToString((string)parameter);
      }
    }

    public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture) {
      return Binding.DoNothing;
    }
  }
}
