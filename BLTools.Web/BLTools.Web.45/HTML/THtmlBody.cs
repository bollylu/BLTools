using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLTools.Web.HTML {
  public class THtmlBody : THtml, IDisposable {

    public THtmlBody(THtmlPage htmlPage)
      : base(htmlPage.HtmlStream, "body") {
      DrawBeginLine();
    }
    public THtmlBody(THtmlPage htmlPage, THtmlAttributes attributes)
      : base(htmlPage.HtmlStream, "body", attributes) {
      DrawBeginLine();
    }
    public THtmlBody(THtmlPage htmlPage, TStyleAttributes styleAttributes)
      : base(htmlPage.HtmlStream, "body", styleAttributes) {
      DrawBeginLine();
    }

    public THtmlBody(THtmlPage htmlPage, THtmlAttributes attributes, TStyleAttributes styleAttributes)
      : base(htmlPage.HtmlStream, "body", attributes, styleAttributes) {
      DrawBeginLine();
    }

    

  }
}
