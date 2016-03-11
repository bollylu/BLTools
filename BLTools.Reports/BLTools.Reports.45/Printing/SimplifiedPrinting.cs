using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Documents;
using System.Windows.Controls;
using System.Windows;
using System.Windows.Media;
using System.Printing;
using System.Windows.Xps;


namespace BLTools.Reports {
  public static class SimplifiedPrinting {

    public static FontFamily FontFamily { get; set; }
    public static double FontSize { get; set; }
    public static PageOrientation Orientation { get; set; }

    static SimplifiedPrinting() {
      FontSize = 10;
      FontFamily = new FontFamily("Courier New");
      Orientation = PageOrientation.Landscape;
    }

    public static void Print(string source, string title = "") {
      FlowDocument CurrentDoc = new FlowDocument(new Paragraph(new Run(source)));
      CurrentDoc.PagePadding = new Thickness(25);
      CurrentDoc.FontFamily = FontFamily;
      CurrentDoc.FontSize = FontSize;

      PrintDialog PD = new PrintDialog();
      PD.PrintTicket.PageOrientation = Orientation;

      if (PD.ShowDialog() == true) {
        CurrentDoc.ColumnGap = 0;
        CurrentDoc.PageWidth = PD.PrintableAreaWidth - CurrentDoc.PagePadding.Left * 2;
        CurrentDoc.ColumnWidth = PD.PrintableAreaWidth - CurrentDoc.PagePadding.Left * 2;
        CurrentDoc.PageHeight = PD.PrintableAreaHeight - CurrentDoc.PagePadding.Top * 2;
        PD.PrintDocument(((IDocumentPaginatorSource)CurrentDoc).DocumentPaginator, title);
      }
    }

    public static void Print(Visual visual, string title = "") {
     
      PrintDialog PD = new PrintDialog();
      PD.PrintTicket.PageOrientation = Orientation;

      if (PD.ShowDialog() == true) {
        XpsDocumentWriter XDW = PrintQueue.CreateXpsDocumentWriter(PD.PrintQueue);
        VisualsToXpsDocument VToXps = (VisualsToXpsDocument)XDW.CreateVisualsCollator();
        VToXps.BeginBatchWrite();
        VToXps.Write(visual, PD.PrintTicket);
        VToXps.EndBatchWrite();
      }
    }

    public static void Print(FlowDocument document, string title = "") {

      PrintDialog PD = new PrintDialog();
      PD.PrintTicket.PageOrientation = Orientation;

      if (PD.ShowDialog() == true) {
        document.ColumnGap = 0;
        document.PageWidth = PD.PrintableAreaWidth - document.PagePadding.Left * 2;
        document.ColumnWidth = PD.PrintableAreaWidth - document.PagePadding.Left * 2;
        document.PageHeight = PD.PrintableAreaHeight - document.PagePadding.Top * 2;
        PD.PrintDocument(((IDocumentPaginatorSource)document).DocumentPaginator, title);
      }
    }
  }
}
