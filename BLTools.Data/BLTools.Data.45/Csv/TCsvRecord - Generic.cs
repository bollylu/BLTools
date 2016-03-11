using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace BLTools.Data.Csv {
  public class TCsvRecordX {

    #region Converters
    public override string ToString() {
      StringBuilder RetVal = new StringBuilder();

      try {
        foreach (PropertyInfo PropertyInfoItem in this.GetType()
                                                      .GetProperties()
                                                      .Where(x => x.HasAttribute(typeof(TCsvDataFieldAttribute)))) {
          TCsvDataFieldAttribute CurrentFieldAttribute = (TCsvDataFieldAttribute)Attribute.GetCustomAttribute(PropertyInfoItem, typeof(TCsvDataFieldAttribute));
          RetVal.AppendFormat("{0}={1}, ", PropertyInfoItem.Name, PropertyInfoItem.GetValue(this, null));
        }
        if (RetVal.Length > 2) {
          RetVal.Remove(RetVal.Length - 2, 2);
        }
      } catch { }

      return RetVal.ToString();
    }
    #endregion Converters

  }
}
