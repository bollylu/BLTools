using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace BLTools.WPF {
  public class MultiBoolStatesToBrushConverter : IMultiValueConverter {
    public IEnumerable<Brush> Brushes { get; set; }
    public Brush AllFalseBrush { get; set; }

    public object Convert(object[] value, Type targetType, object parameter, System.Globalization.CultureInfo culture) {

      if (Brushes == null || Brushes.Count() == 0) {
        return Binding.DoNothing;
      }

      List<bool> BoolValues = value.Where(x => x is bool).Cast<bool>().ToList();

      // All items false, return allfalseBrush
      if (BoolValues.All(x => x == false)) {
        return AllFalseBrush;
      }

      // Search for the last item = true, then returns its picture
      int HighestTrueValue = BoolValues.LastIndexOf(true);
      if (HighestTrueValue < 0) {
        return Binding.DoNothing;
      }

      if (HighestTrueValue >= Brushes.Count()) {
        return Binding.DoNothing;
      } else {
        return Brushes.ElementAt(HighestTrueValue);
      }
    }

    public object[] ConvertBack(object value, Type[] targetTypes, object parameter, System.Globalization.CultureInfo culture) {
      object[] RetVal = new object[targetTypes.Length];
      for (int i = 0; i < RetVal.Length; i++) {
        RetVal[i] = Binding.DoNothing;
      }
      return RetVal;
    }
  }
}
