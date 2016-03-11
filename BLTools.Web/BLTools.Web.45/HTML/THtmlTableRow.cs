using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLTools.Web.HTML {
  public class THtmlTableRow : THtml, IDisposable {

    public THtmlTableRow(THtml htmlTable, string name = "")
      : base(htmlTable.HtmlStream, "tr", name) {
      DrawBeginLine();
    }

    public THtmlTableRow(THtml htmlTable, THtmlAttributes attributes, string name = "")
      : base(htmlTable.HtmlStream, "tr", attributes, name) {
      DrawBeginLine();
    }

    public THtmlTableRow(THtml htmlTable, TStyleAttributes styleAttributes, string name = "")
      : base(htmlTable.HtmlStream, "tr", styleAttributes, name) {
      DrawBeginLine();
    }

    public THtmlTableRow(THtml htmlTable, THtmlAttributes attributes, TStyleAttributes styleAttributes, string name = "")
      : base(htmlTable.HtmlStream, "tr", attributes, styleAttributes, name) {
      DrawBeginLine();
    }
  }
}
