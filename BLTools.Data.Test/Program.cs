using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BLTools.Data;
using BLTools;
using ConExt = BLTools.ConsoleExtension;
using System.Diagnostics;

namespace BLTools.Data.Test {
  class Program {
    static void Main(string[] args) {

     

      using (TCsvTestFile TestFile = new TCsvTestFile("CA2014-B.csv")) {
        TestFile.Separator = ';';
        TCsvTestRecord TestRecord = new TCsvTestRecord();
        TestRecord.Apb = "123456";
        TestRecord.Name = "BOLLY, LUC";
        TestRecord.CA2014 = 987654.321f;
        TestFile.Add(TestRecord);
        TestFile.Save();
      }

      using (TCsvTestFile TestFile = new TCsvTestFile("CA2014-B.csv")) {
        TestFile.Separator = ';';
        TCsvTestRecord TestRecord = new TCsvTestRecord();
        TestRecord.Apb = "789456";
        TestRecord.Name = "BOLLYLU";
        TestRecord.CA2014 = 9876.21f;
        TestFile.Add(TestRecord);
        TestFile.Save(true);
      }

      using (TCsvTestFile TestFile = new TCsvTestFile("CA2014-B.csv")) {
        TestFile.Read();

        foreach (TCsvTestRecord RecordItem in TestFile) {
          Console.WriteLine(RecordItem.ToString());
        }

      }

      //Trace.WriteLine("----------------");

      ConExt.ConsoleExtension.Pause();

    }
  }
}
