using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLTools.Data {
  public static class SqlMaker {

    public static string InConditionFactory(string dataFieldName, IEnumerable<string> items) {
      if (string.IsNullOrWhiteSpace(dataFieldName)) {
        throw new ArgumentException("Must specify data field name", "dataFieldName");
      }
      if ((items == null) || items.Count() == 0) {
        throw new ArgumentException("Must specify list of possible items", "items");
      }
      return string.Format("{0} IN ({1})", dataFieldName, string.Join(", ", items.Select(x => string.Format("'{0}'", x))));
    }

    public static string InConditionFactory(string dataFieldName, IEnumerable<int> items) {
      if (string.IsNullOrWhiteSpace(dataFieldName)) {
        throw new ArgumentException("Must specify data field name", "dataFieldName");
      }
      if ((items == null) || items.Count() == 0) {
        throw new ArgumentException("Must specify list of possible items", "items");
      }
      return string.Format("{0} IN ({1})", dataFieldName, string.Join(", ", items));
    }
  }
}
