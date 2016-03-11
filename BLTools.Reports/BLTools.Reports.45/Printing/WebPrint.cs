using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SHDocVw;
using System.IO;
using System.Threading;
using System.Diagnostics;
using System.Threading.Tasks;

namespace BLTools.Reports {
  public class WebPrint : IDisposable {

    private InternetExplorer PrintBrowser;
    //public object PrintTemplate = "e:\\PrintTemplate.htm";
    private string TempFilename;
    public bool IsPrintDone;

    public WebPrint() {
      Trace.WriteLine("creating IE");
      PrintBrowser = new InternetExplorer();
      Trace.WriteLine("IE created");
    }

    public void StartPrint(Uri url) {
      PrintBrowser.Navigate(url.AbsoluteUri);
      PrintBrowser.Navigate2(TempFilename);
      while (PrintBrowser.ReadyState != tagREADYSTATE.READYSTATE_COMPLETE) {
        Thread.Sleep(100);
      }
      PrintBrowser.Visible = false;

      IsPrintDone = false;
      PrintBrowser.PrintTemplateTeardown += new DWebBrowserEvents2_PrintTemplateTeardownEventHandler(PrintBrowser_PrintTemplateTeardown);
      PrintBrowser.ExecWB(OLECMDID.OLECMDID_PRINT, OLECMDEXECOPT.OLECMDEXECOPT_PROMPTUSER);
    }

    public void StartPrint(string content, bool preview = true, bool promptUser = true) {
      TempFilename = Path.GetTempFileName().ToLower().Replace(".tmp", ".htm");
      File.WriteAllText(TempFilename, content);
      PrintBrowser.Navigate2(TempFilename);
      while (PrintBrowser.ReadyState != tagREADYSTATE.READYSTATE_COMPLETE) {
        Thread.Sleep(100);
      }
      PrintBrowser.Visible = false;

      IsPrintDone = false;
      PrintBrowser.PrintTemplateTeardown += new DWebBrowserEvents2_PrintTemplateTeardownEventHandler(PrintBrowser_PrintTemplateTeardown);
      PrintBrowser.ExecWB(preview ? OLECMDID.OLECMDID_PRINTPREVIEW : OLECMDID.OLECMDID_PRINT, promptUser ? OLECMDEXECOPT.OLECMDEXECOPT_PROMPTUSER : OLECMDEXECOPT.OLECMDEXECOPT_DONTPROMPTUSER);
    }

    private void PrintBrowser_PrintTemplateTeardown(object pDisp) {
      PrintBrowser.PrintTemplateTeardown -= new DWebBrowserEvents2_PrintTemplateTeardownEventHandler(PrintBrowser_PrintTemplateTeardown);
      File.Delete(TempFilename);
      if (OnPrintDone != null) {
        OnPrintDone(this, EventArgs.Empty);
      }
      IsPrintDone = true;
    }


    public void Close() {
      if (PrintBrowser != null) {
        try {
          PrintBrowser.Quit();
        } catch { }
      }
    }

    public void Dispose() {
      Close();
      PrintBrowser = null;
    }

    public event EventHandler OnPrintDone;

  }
}
