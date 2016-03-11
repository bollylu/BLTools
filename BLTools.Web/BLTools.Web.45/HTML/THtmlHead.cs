using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLTools.Web.HTML {
  public class THtmlHead : THtml, IDisposable {

    public THtmlHead(THtmlPage htmlPage)
      : base(htmlPage.HtmlStream, "head") {
      DrawBeginLine();
    }

  }
}
