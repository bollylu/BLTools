using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLTools.Web.HTML {
  public class THtmlAttributes : List<THtmlAttribute> {

    public THtmlAttributes(params string[] attributes) {
      if (attributes == null) {
        return;
      }
      if (attributes.Count() == 0) {
        return;
      }
      foreach (string AttributeItem in attributes) {
        this.Add(new THtmlAttribute(AttributeItem));
      }
    }

    public THtmlAttributes(THtmlAttributes attributes) {
      if (attributes == null) {
        return;
      }
      if (attributes.Count() == 0) {
        return;
      }
      foreach (THtmlAttribute AttributeItem in attributes) {
        this.Add(AttributeItem);
      }
    }

    public override string ToString() {
      StringBuilder RetVal = new StringBuilder();
      foreach (THtmlAttribute AttributeItem in this) {
        RetVal.AppendFormat("{0}=\"{1}\" ", AttributeItem.Key, AttributeItem.Value);
      }
      return RetVal.ToString();
    }
  }
}
