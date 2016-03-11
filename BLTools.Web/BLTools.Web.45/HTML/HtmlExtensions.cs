using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLTools.Web.HTML {
  /// <summary>
  /// 
  /// </summary>
  public static class HtmlExtensions {

    public static void AddTableHeader(this THtmlTableRow htmlTableRow, string content = "", string name = "") {
      THtmlTableHeader Header = new THtmlTableHeader(htmlTableRow, content, name);
      Header.Dispose();
    }
    public static void AddTableHeader(this THtmlTableRow htmlTableRow, THtmlAttributes attributes, string content = "", string name = "") {
      THtmlTableHeader Header = new THtmlTableHeader(htmlTableRow, attributes, content, name);
      Header.Dispose();
    }
    public static void AddTableHeader(this THtmlTableRow htmlTableRow, TStyleAttributes styleAttributes, string content = "", string name = "") {
      THtmlTableHeader Header = new THtmlTableHeader(htmlTableRow, styleAttributes, content, name);
      Header.Dispose();
    }
    public static void AddTableHeader(this THtmlTableRow htmlTableRow, THtmlAttributes attributes, TStyleAttributes styleAttributes, string content = "", string name = "") {
      THtmlTableHeader Header = new THtmlTableHeader(htmlTableRow, attributes, styleAttributes, content, name);
      Header.Dispose();
    }


    public static void AddTableCell(this THtmlTableRow htmlTableRow, string content = "", string name = "") {
      THtmlTableCell Cell = new THtmlTableCell(htmlTableRow, content, name);
      Cell.Dispose();
    }
    public static void AddTableCell(this THtmlTableRow htmlTableRow, THtmlAttributes htmlAttributes, string content = "", string name = "") {
      THtmlTableCell Cell = new THtmlTableCell(htmlTableRow, htmlAttributes, content, name);
      Cell.Dispose();
    }
    public static void AddTableCell(this THtmlTableRow htmlTableRow, TStyleAttributes styleAttributes, string content = "", string name = "") {
      THtmlTableCell Cell = new THtmlTableCell(htmlTableRow, styleAttributes, content, name);
      Cell.Dispose();
    }
    public static void AddTableCell(this THtmlTableRow htmlTableRow, THtmlAttributes htmlAttributes, TStyleAttributes styleAttributes, string content = "", string name = "") {
      THtmlTableCell Cell = new THtmlTableCell(htmlTableRow, htmlAttributes, styleAttributes, content, name);
      Cell.Dispose();
    }
  }

}
