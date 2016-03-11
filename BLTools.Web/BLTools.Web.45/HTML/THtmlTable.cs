using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace BLTools.Web.HTML {
  
  public class THtmlTable : THtml, IDisposable {


    public THtmlTable(THtml htmlContainer, TStyleAttributes styleAttributes, string name = "") 
      : base(htmlContainer.HtmlStream, "table", styleAttributes, name) {
      DrawBeginLine();
    }

    public THtmlTable(THtml htmlContainer, THtmlAttributes attributes, string name = "")
      : base(htmlContainer.HtmlStream, "table", attributes, name) {
      DrawBeginLine();
    }

    public THtmlTable(THtml htmlContainer, THtmlAttributes attributes, TStyleAttributes styleAttributes, string name = "")
      : base(htmlContainer.HtmlStream, "table", attributes, styleAttributes, name ) {
      DrawBeginLine();
    }
  }

}
