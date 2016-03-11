using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BLTools;
using BLTools.FileManagement;
using System.IO;

namespace xdir {
  class Program {
    static void Main(string[] args) {
      SplitArgs oArgs = new SplitArgs(args);
      if ( oArgs.Count == 0 ) {
        Usage("Missing parameters");
      }
      if ( oArgs.IsDefined("?") || oArgs.IsDefined("help") ) {
        Usage();
      }

      string Item = oArgs[0].Name;
      if ( string.IsNullOrWhiteSpace(Item) ) {
        Item = ".";
      }

      string PathPart;
      string FilePart;

      if ( Item.Contains('*') || Item.Contains('?') ) {
        if ( Item.Contains('\\') ) {
          PathPart = Item.Left(Item.LastIndexOf("\\") + 1);
          FilePart = Item.Substring(Item.LastIndexOf("\\") + 1);
        } else if ( Item.Contains(':') ) {
          PathPart = Item.Left(Item.IndexOf(":") + 1);
          FilePart = Item.Substring(Item.IndexOf(":") + 1);
        } else {
          PathPart = Environment.CurrentDirectory;
          FilePart = Item;
        }
      } else {
        if ( Item.Contains('\\') ) {
          if ( Directory.Exists(Item) ) {
            PathPart = Item;
            FilePart = "*.*";
          } else {
            PathPart = Item.Left(Item.LastIndexOf("\\") + 1);
            FilePart = Item.Substring(Item.LastIndexOf("\\") + 1);
          }
        } else if ( Item.Contains(':') ) {
          if ( Directory.Exists(Item.Left(Item.IndexOf(":") + 1) + "\\" + Item.Substring(Item.IndexOf(":") + 1)) ) {
            PathPart = Item.Left(Item.IndexOf(":") + 1) + "\\" + Item.Substring(Item.IndexOf(":") + 1);
            FilePart = "*.*";
          } else {
            PathPart = Item.Left(Item.IndexOf(":") + 1);
            FilePart = Item.Substring(Item.IndexOf(":") + 1);
          }
        } else {
          PathPart = Environment.CurrentDirectory;
          FilePart = Item;
        }
      }

      if ( FilePart == "." || FilePart == ".." || FilePart == "" ) {
        FilePart = "*.*";
      }

      //Console.WriteLine("Item = \"{0}\"", Item);
      //Console.WriteLine("Current directory = \"{0}\"", Environment.CurrentDirectory);
      //Console.WriteLine("PathPart = \"{0}\"", PathPart);
      //Console.WriteLine("FilePart = \"{0}\"", FilePart);

      if ( !Directory.Exists(PathPart) ) {
        Usage(string.Format("Invalid path : \"{0}\"", PathPart));
      }

      bool IsRecursive = oArgs.IsDefined("s");

      Console.WriteLine("\nDirectory of {0}\n", PathPart);
      FileManager FM = new FileManager();
      IEnumerable<ExtendedFileVersionInfo> EFVIs = FM.GetFileVersionInfo(PathPart, FilePart, IsRecursive);
      int PaddingName = 0;
      int PaddingFileVersion = 0;
      if ( EFVIs.Count() > 0 ) {
        PaddingName = EFVIs.AsParallel().Select(x => x.BasicFileVersionInfo.FileName.Length).Max()+1;
        PaddingFileVersion = Math.Max(12,EFVIs.AsParallel().Select(x => x.BasicFileVersionInfo.FileVersion.Length).Max()+1);
      }

      Console.Write("Name".PadRight(PaddingName));
      Console.Write(" File version".PadRight(PaddingFileVersion));
      Console.WriteLine();
      foreach ( ExtendedFileVersionInfo EFVIItem in EFVIs ) {
        Console.Write("{0}", EFVIItem.BasicFileVersionInfo.FileName.PadRight(PaddingName, '.'));
        Console.Write(" {0}", EFVIItem.BasicFileVersionInfo.FileVersion.PadRight(PaddingFileVersion, '.'));
        Console.Write(" {0}", EFVIItem.TargetDotNet);
        Console.WriteLine();
      }

    }

    static void Usage() {
      Usage("");
    }
    static void Usage(string message) {
      if ( !string.IsNullOrWhiteSpace(message) ) {
        Console.WriteLine(message);
      }
      Console.WriteLine("XDir v{0} - (c) 2010 Luc Bolly");
      Console.WriteLine("Displays extended information about executable files.");
      Console.WriteLine("Usage: xdir <filename> | <foldername>");
      Environment.Exit(1);
    }

  }
}
