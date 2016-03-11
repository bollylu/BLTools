using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLTools.Web.HTML {
  public abstract class THtml : IDisposable {

    public string Content { get; set; }

    internal string Name { get; set; }
    public readonly TStyleAttributes Styles;
    public readonly THtmlAttributes Attributes;

    internal TextWriter HtmlWriter { get; set; }
    internal Stream HtmlStream { get; set; }

    internal string TAG { get; set; }
    internal string TagStart {
      get {
        return string.Format("<{0} ", TAG);
      }
    }
    internal string TagEnd {
      get {
        return string.Format("</{0}>", TAG);
      }
    }

    public THtml(Stream htmlStream, string tagName, string name = "") {
      TAG = tagName;
      Name = name;
      HtmlStream = htmlStream;
      HtmlWriter = new StreamWriter(HtmlStream);
      Styles = new TStyleAttributes();
      Attributes = new THtmlAttributes();
    }

    public THtml(Stream htmlStream, string tagName, THtmlAttributes attributes, string name = "")
      : this(htmlStream, tagName, name) {
        foreach (THtmlAttribute HtmlAttributeItem in attributes) {
          Attributes.Add(HtmlAttributeItem);
        }
    }

    public THtml(Stream htmlStream, string tagName, THtmlAttributes attributes, TStyleAttributes styleAttributes, string name = "")
      : this(htmlStream, tagName, name) {
        foreach (THtmlAttribute HtmlAttributeItem in attributes) {
          Attributes.Add(HtmlAttributeItem);
        }
      Styles = new TStyleAttributes(styleAttributes);
    }

    public THtml(Stream htmlStream, string tagName, TStyleAttributes styleAttributes, string name = "")
      : this(htmlStream, tagName, name) {
      Styles = new TStyleAttributes(styleAttributes);
    }

    public override string ToString() {
      StringBuilder RetVal = new StringBuilder();
      long ActualPosition = HtmlStream.Position;
      HtmlStream.Seek(0, SeekOrigin.Begin);
      RetVal.Append((new StreamReader(HtmlStream)).ReadToEnd());
      HtmlStream.Seek(ActualPosition, SeekOrigin.Begin);
      return RetVal.ToString();
    }

    public virtual void DrawBegin() {
      StringBuilder NewTag = new StringBuilder(TagStart);
      if (Name != "") {
        NewTag.Append(string.Format("Name=\"{0}\" ", Name));
      }
      if (Attributes != null && Attributes.Count != 0) {
        NewTag.Append(Attributes.ToString());
      }
      if (Styles != null && Styles.Count != 0) {
        NewTag.Append(Styles.ToString());
      }
      NewTag.Trim();
      NewTag.Append(">");
      HtmlWriter.Write(NewTag.ToString());
      HtmlWriter.Flush();
    }

    public virtual void DrawBeginLine() {
      DrawBegin();
      HtmlWriter.WriteLine();
      HtmlWriter.Flush();

    }

    public virtual void DrawEnd() {
      HtmlWriter.WriteLine(TagEnd);
      HtmlWriter.Flush();
    }

    public virtual void Dispose() {
      if (!string.IsNullOrWhiteSpace(Content)) {
        HtmlWriter.Write(Content);
      }
      if (TagEnd != "") {
        DrawEnd();
      }
    }
  }
}
