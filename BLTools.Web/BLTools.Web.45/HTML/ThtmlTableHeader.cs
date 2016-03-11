using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLTools.Web.HTML {
  public class THtmlTableHeader : THtml, IDisposable {

    public THtmlTableHeader(THtml htmlContainer, string content = "", string name = "")
      : base(htmlContainer.HtmlStream, "th", name) {
      Content = content;
      DrawBegin();
    }

    public THtmlTableHeader(THtml htmlContainer, THtmlAttributes attributes, string content = "", string name = "")
      : base(htmlContainer.HtmlStream, "th", attributes, name) {
      Content = content;
      DrawBegin();
    }

    public THtmlTableHeader(THtml htmlContainer, TStyleAttributes styleAttributes, string content = "", string name = "")
      : base(htmlContainer.HtmlStream, "th", styleAttributes, name) {
      Content = content;
      DrawBegin();
    }

    public THtmlTableHeader(THtml htmlContainer, THtmlAttributes attributes, TStyleAttributes styleAttributes, string content = "", string name = "")
      : base(htmlContainer.HtmlStream, "th", attributes, styleAttributes, name) {
      Content = content;
      DrawBegin();
    }

  }
}
