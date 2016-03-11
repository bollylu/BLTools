using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BLTools;
using BLTools.ConsoleExtension;
using BLTools.FileManagement;
using BLTools.Debugging;
using BLTools.Text;
using System.Diagnostics;
using System.Net;

namespace BLToolsTests {
  class Program {
    static void Main(string[] args) {
      SplitArgs oArgs = new SplitArgs(args);

      ApplicationInfo.ApplicationStart();

      //Trace.WriteLine(TextBox.BuildDynamic("This is a title"));
      //Trace.WriteLine(TextBox.BuildDynamic("This is a title", 0));
      //Trace.WriteLine(TextBox.BuildDynamic("This is a title\nand some other lines\nof text", 10));
      //Trace.WriteLine(TextBox.BuildDynamic("This is a title\nand some other lines\nof text", 10, TextBox.StringAlignmentEnum.Left, '#'));
      //Trace.WriteLine(TextBox.BuildDynamic("This is a title\nand some other lines\nof text", 10, TextBox.StringAlignmentEnum.Right, '#'));
      //Trace.WriteLine(TextBox.BuildDynamic("This is a title\nand some other lines\nof text", 10, TextBox.StringAlignmentEnum.Center, '#'));

      Trace.WriteLine(TextBox.BuildFixedWidth("012345678]012345678]012345678]012345678]012345678]012345678]012345678]012345678]"));
      Trace.WriteLine(TextBox.BuildFixedWidth("01234567890123456789", 20));
      Trace.WriteLine(TextBox.BuildFixedWidth("This is a title\r\nand some other lines\r\nof text", 80, TextBox.StringAlignmentEnum.Left, ' ', "+=+|+=+|"));
      Trace.WriteLine(TextBox.BuildFixedWidth("This is a title\r\nand some other lines\r\nof text", 120, TextBox.StringAlignmentEnum.Left, '.'));
      Trace.WriteLine(TextBox.BuildFixedWidth("This is a title\r\nand some other lines\r\nof text", 120, TextBox.StringAlignmentEnum.Right, '#'));
      Trace.WriteLine(TextBox.BuildFixedWidth("This is a title\r\nand some other lines\r\nof text", 120, TextBox.StringAlignmentEnum.Center, '#'));

      Trace.WriteLine(TextBox.BuildFixedWidthIBM("This is a title"));
      //Trace.WriteLine(TextBox.BuildFixedWidthIBM("This is a title"));
      Trace.WriteLine(TextBox.BuildFixedWidthIBM("This is a title\r\nand some other lines\r\nof text", 10));
      Trace.WriteLine(TextBox.BuildFixedWidthIBM("This is a title\r\nand some other lines\r\nof text", 10, TextBox.StringAlignmentEnum.Left));
      Trace.WriteLine(TextBox.BuildFixedWidthIBM("This is a title\r\nand some other lines\r\nof text", 10, TextBox.StringAlignmentEnum.Right));
      Trace.WriteLine(TextBox.BuildFixedWidthIBM("This is a title\r\nand some other lines\r\nof text", 10, TextBox.StringAlignmentEnum.Center));


      //TestSplitArgs(oArgs);


      //TestFileManager();

      //foreach ( ExtendedFileVersionInfo Item in FM.GetFileVersionInfo("i:\\dev.2010", "*.dll") ) {
      //  Console.WriteLine("{0}{1}{2}", Item.ExecutableType.ToString(), Item.BasicFileVersionInfo.FileName.PadRight(120, '.'), Item.BasicFileVersionInfo.FileVersion);
      //}

      ConsoleExtension.Pause(15000, true, true);

      ApplicationInfo.ApplicationStop();
    }

    private static void TestFileManager() {
      FileManager FM = new FileManager();

      foreach (ExtendedFileVersionInfo EFVIItem in FM.GetFileVersionInfo("i:\\dev.2010", "*.exe")) {
        if (((EFVIItem.Characteristics & ExtendedFileVersionInfo.PE_CharacteristicsEnum.Dll) != 0) || ((EFVIItem.Characteristics & ExtendedFileVersionInfo.PE_CharacteristicsEnum.ExecutableImage) != 0)) {
          Console.WriteLine("{0} - {1}{2}", EFVIItem.ExecutableType.ToString(), EFVIItem.BasicFileVersionInfo.FileName.PadRight(120, '.'), EFVIItem.BasicFileVersionInfo.FileVersion);
          Console.WriteLine("Target machine : {0}", EFVIItem.TargetMachine);
          Console.WriteLine("Date de création : {0}", EFVIItem.DateCreated.ToString("yyyy-MM-dd HH:mm:ss"));
          Console.WriteLine("Caractéristiques : {0}", EFVIItem.Characteristics.ToString());
          Console.WriteLine("LinkerVersion : {0}", EFVIItem.PE_LinkerVersion);
          Console.WriteLine("OperatingSystemVersion : {0}", EFVIItem.PE_OperatingSystemVersion);
          Console.WriteLine("ImageVersion : {0}", EFVIItem.PE_ImageVersion);
          Console.WriteLine("SubSystem {0} - SubSystemVersion : {1}", EFVIItem.Subsystem, EFVIItem.PE_SubSystemVersion);
          Console.WriteLine();
        }
      }
    }

    private static void TestSplitArgs(SplitArgs oArgs) {
      byte[] TestBytes = new byte[] { 12, 26, 35, 8 };

      Trace.Listeners.Add(new TimeStampTraceListener("execution.log"));
      Trace.Listeners.Add(new TimeStampTextWriterTraceListener(Console.Out));
      Trace.AutoFlush = true;

      Trace.WriteLine(TestBytes.ToHexString());
      Trace.WriteLine(TestBytes.ToHexString(" "));
      Trace.WriteLine(TestBytes.ToHexString(", "));

      Trace.WriteLine("Test of trace");
      Trace.WriteLine(string.Format("Number of arguments = {0}", oArgs.Count));
      foreach (ArgElement ArgumentItem in oArgs) {
        Trace.WriteLine(string.Format("{0} = {1}", ArgumentItem.Name, ArgumentItem.Value));
      }

      Trace.WriteLine(string.Format("Argument 3 = {0}", oArgs[3].Name));
      var Orange = oArgs.GetValue<int>("orange");
      Trace.WriteLine(string.Format("orange\n {0} =\n {1}", Orange.GetType().ToString(), Orange));
    }

  }
}
