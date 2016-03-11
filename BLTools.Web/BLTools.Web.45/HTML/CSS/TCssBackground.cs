using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLTools.Web.HTML {
  public class TCssBackgroundColor : TStyleAttribute {

    public TCssBackgroundColor() : base("background-color", "white") {
    }

    public TCssBackgroundColor(string color) : this() {
      Value = color;
    }
  }
}
