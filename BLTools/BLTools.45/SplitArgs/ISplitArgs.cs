using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLTools {
  public interface ISplitArgs {
    bool IsDefined(string key);
    T GetValue<T>(string key);
    T GetValue<T>(string key, T defaultValue);
    T GetValue<T>(string key, T defaultValue, CultureInfo cultureInfo);

    T GetValue<T>(int pos);
    T GetValue<T>(int pos, T defaultValue);
    T GetValue<T>(int pos, T defaultValue, CultureInfo cultureInfo);

    ArgElement this[int index] { get; }
  }
}
