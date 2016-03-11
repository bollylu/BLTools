using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RarLib;
using BLTools;
using System.IO;
using System.Threading;
using System.Diagnostics;

namespace RarLibCommandLineTest {
  class Program {
    static void Main(string[] args) {

      string Filename = "test.rar";
      if (File.Exists(Filename)) {
        File.Delete(Filename);
      }
      AutoResetEvent JobDone = new AutoResetEvent(false);
      TRarFile TestFile = new TRarFile(Filename, JobDone);

      

      TestFile.AddFilesAsync(Directory.GetFiles(".", "*.dll"));
      JobDone.WaitOne(10000);
      Trace.WriteLine(TestFile.ToString());

      TestFile.AddFilesAsync(Directory.GetFiles(".", "*.config"));
      JobDone.WaitOne(10000);
      Trace.WriteLine(TestFile.ToString());

      //TestFile.AddFolders(Directory.GetDirectories("."));
      //Console.WriteLine(TestFile.ToString());
      //Console.WriteLine("---");

      //TestFile.DeleteFile("bltools.45.dll");
      //Console.WriteLine(TestFile.ToString());
      //Console.WriteLine("---");

      //TestFile.DeleteFiles(Directory.GetFiles(".", "*.config").Select(x => Path.GetFileName(x)));
      //Console.WriteLine(TestFile.ToString());

      

      

    }
  }
}
