using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BLTools;
using BLTools.ConsoleExtension;
using BLTools.Web;
using BLTools.Web.HTML;
using System.Diagnostics;
using System.IO;
using BLTools.Debugging;

namespace BLTools.Web.ConsoleTest {
  class Program {
    static void Main(string[] args) {

      TraceFactory.AddTraceConsole();
      //MemoryStream OutputResponse = new MemoryStream();

      //using (THtmlPage HtmlPage = new THtmlPage(OutputResponse)) {
      //  using (THtmlHead HtmlHead = new THtmlHead(HtmlPage)) { }
      //  using (THtmlBody HtmlBody = new THtmlBody(HtmlPage)) {
      //    using (THtmlTable HtmlTable = new THtmlTable(HtmlBody, new TStyleAttributes("width:95%", "left-margin:auto", "right-margin:auto"))) {
      //      using (THtmlTableRow HtmlRow = new THtmlTableRow(HtmlTable)) {
      //        HtmlRow.AddTableHeader(new TStyleAttributes("width:12%"), "Product number");
      //        HtmlRow.AddTableHeader(new TStyleAttributes("width:40%"), "Description");
      //        HtmlRow.AddTableHeader(new TStyleAttributes("width:12%"), "Threshold");
      //        HtmlRow.AddTableHeader(new TStyleAttributes("width:12%"), "Stock PDC");
      //        HtmlRow.AddTableHeader(new TStyleAttributes("width:12%"), "Forecast PDC");
      //        HtmlRow.AddTableHeader(new TStyleAttributes("width:12%"), "Forecast Phacobel");
      //      }
      //      using (THtmlTableRow HtmlRow = new THtmlTableRow(HtmlTable)) {
      //        HtmlRow.AddTableCell(new TStyleAttributes("background-color:red"), "FCM-001-MPO");
      //      }
      //    }
      //  }
      //}

      //TextReader Reader = new StreamReader(OutputResponse);
      //OutputResponse.Seek(0, SeekOrigin.Begin);
      //Trace.WriteLine(Reader.ReadToEnd());

      TFtpClient BelmedisFtp = new TFtpClient("order.belmedis.be", "PHACOBEL", "LEBOCAPH5");
      Console.WriteLine(string.Join("\n", BelmedisFtp.List("in")));
      Console.WriteLine(BelmedisFtp.FileExist("in", "VERB05102015001105752502975.TXT"));




      //TFtpClient MyFtpclient = new TFtpClient("ftphost.pharmadistricenter.be", "phacobel", "2WBRQqWh");
      //TFtpClient MyQAFtpClient = new TFtpClient("ftphost.pharmadistricenter.be", "phacobel_qa", "p1K47XRH");

      //Console.WriteLine(string.Join("\n", MyFtpclient.List("out")));
      //Console.WriteLine(string.Join("\n", MyFtpclient.ListDetails("out")));

      //MyFtpclient.MoveFileFromFtp("out", "SHP140821135914.TXT", ETransferType.Text);

      //Console.WriteLine("------");
      //Console.WriteLine(string.Join("\n", MyFtpclient.List("out")));


      //List<TFtpFile> Files = MyQAFtpClient.ListDetails("out").ToList();

      //foreach (TFtpFile FtpFileItem in Files) {
      //  Console.WriteLine(FtpFileItem.ToString());
      //}

      //MyQAFtpClient.MoveFileFromFtp("out", "toto");


      //foreach (string FilenameItem in MyFtpclient.List("out")) {
      //  MyFtpclient.GetFile("out", FilenameItem, ETransferType.Text);
      //}




      //Console.WriteLine(string.Join("\n", MyQAFtpClient.List("in")));
      ////if (!MyQAFtpClient.FileExist("in", "pdcorders.txt")) {
      //  MyQAFtpClient.PutFile("pdcorders.txt", ETransferType.Binary, "in", true);
      ////} else {
      ////  Console.WriteLine("Unable to send file : file already exists on destination");
      ////}
      //Console.WriteLine("------");
      //Console.WriteLine(string.Join("\n", MyQAFtpClient.List("in")));

      ConsoleExtension.ConsoleExtension.Pause();
    }
  }
}
