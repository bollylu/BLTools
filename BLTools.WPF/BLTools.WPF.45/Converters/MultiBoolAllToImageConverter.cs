using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media.Imaging;

namespace BLTools.WPF {
  public class MultiBoolAllToImageConverter : IMultiValueConverter {
    public string TrueImage { get; set; }
    public string FalseImage { get; set; }

    public object Convert(object[] value, Type targetType, object parameter, System.Globalization.CultureInfo culture) {

      IEnumerable<bool> BoolValues = value.Where(x => x is bool).Cast<bool>();

      if (BoolValues.All(x => x == true)) {
        return new BitmapImage(new Uri(TrueImage ?? "", UriKind.Relative));
      } else {
        return new BitmapImage(new Uri(FalseImage ?? "", UriKind.Relative));
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
