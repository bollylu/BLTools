using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BLTools.Reports;

namespace BLTools._45.Reports.Test {
  class Program {
    static void Main(string[] args) {

      Trace.WriteLine(TSmartReport.AvailableReports.ToString());

      List<string> Demo = new List<string>() { "Carat", "Duchatelet", "Cars", "Company" };

      TSmartReport Report = TSmartReport.Factory<List<string>>("ListAllNames", Demo, "test");
      string Result = Report.Execute();

      Report.SendTo("l.bolly@caratsecurity.com", "itmanager@carat-duchatelet.local", "Liste des noms", "gnlieav1.green.rootlinka.net");

    }
  }
}
