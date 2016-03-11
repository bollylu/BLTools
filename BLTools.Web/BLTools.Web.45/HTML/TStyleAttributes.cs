using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLTools.Web.HTML {
  public class TStyleAttributes : List<TStyleAttribute> {

    public TStyleAttributes() { }

    public TStyleAttributes(params string[] attributes) {
      if (attributes == null) {
        return;
      }
      if (attributes.Count() == 0) {
        return;
      }
      foreach (string AttributeItem in attributes) {
        this.Add(new TStyleAttribute(AttributeItem));
      }
    }

    public TStyleAttributes(params TStyleAttributes[] attributes) {
      if (attributes == null) {
        return;
      }
      if (attributes.Count() == 0) {
        return;
      }
      foreach (TStyleAttributes AttributesItem in attributes) {
        foreach (TStyleAttribute AttributeItem in AttributesItem) {
          this.Add(AttributeItem);
        }
      }
    }

    public TStyleAttributes(params TStyleAttribute[] attributes) {
      if (attributes == null) {
        return;
      }
      if (attributes.Count() == 0) {
        return;
      }
      foreach (TStyleAttribute AttributeItem in attributes) {
        this.Add(AttributeItem);
      }
    }


    public override string ToString() {
      StringBuilder RetVal = new StringBuilder("style=\"");
      foreach (TStyleAttribute AttributeItem in this) {
        RetVal.AppendFormat("{0} ", AttributeItem.ToString());
      }
      RetVal.Trim();
      RetVal.Append("\"");
      return RetVal.ToString();
    }
  }
}
