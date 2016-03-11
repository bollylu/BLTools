using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace BLTools {
  public static class PropertyInfoExtensions {
    public static bool HasAttribute(this PropertyInfo Info, Type attributeType) {
      return (Attribute.GetCustomAttribute(Info, attributeType) != null);
    }
  }
}
