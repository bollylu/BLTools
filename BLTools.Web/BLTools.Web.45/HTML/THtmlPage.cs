using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLTools.Web.HTML {
  public class THtmlPage : THtml, IDisposable {

    public THtmlPage(Stream htmlStream)
      : base(htmlStream, "html") {
      DrawBeginLine();
    }

  }
}
