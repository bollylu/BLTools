using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLTools.Web.HTML {
  public class THtmlTableCell : THtml, IDisposable {

    public THtmlTableCell(THtml htmlContainer, string content = "", string name = "")
      : base(htmlContainer.HtmlStream, "td") {
      Name = name;
      Content = content;
      DrawBegin();
    }

    public THtmlTableCell(THtml htmlContainer, TStyleAttributes styleAttributes, string content = "", string name = "")
      : base(htmlContainer.HtmlStream, "td") {
      Name = name;
      Content = content;
      Styles.AddRange(new TStyleAttributes(styleAttributes));
      DrawBegin();
    }

    public THtmlTableCell(THtml htmlContainer, THtmlAttributes htmlAttributes, string content = "", string name = "")
      : base(htmlContainer.HtmlStream, "td") {
      Name = name;
      Content = content;
      Attributes.AddRange(new THtmlAttributes(htmlAttributes));
      DrawBegin();
    }

    public THtmlTableCell(THtml htmlContainer, THtmlAttributes htmlAttributes, TStyleAttributes styleAttributes, string content = "", string name = "")
      : base(htmlContainer.HtmlStream, "td") {
      Name = name;
      Content = content;
      Attributes.AddRange(new THtmlAttributes(htmlAttributes));
      Styles.AddRange(new TStyleAttributes(styleAttributes));
      DrawBegin();
    }
    

  }
}
