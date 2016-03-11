using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLTools.Web.HTML {
  public class THtmlHx : THtml, IDisposable {

    public THtmlHx(THtml htmlContainer, int level = 1, string name = "", params string[] styleAttributes)
      : base(htmlContainer.HtmlStream, string.Format("h{0}", level)) {
      Name = name;
      Styles.AddRange(new TStyleAttributes(styleAttributes));
      DrawBegin();
    }

  }
}
