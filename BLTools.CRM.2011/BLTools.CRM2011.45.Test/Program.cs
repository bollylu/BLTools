using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xrm.Client;
using Microsoft.Xrm.Sdk.Client;
using Microsoft.Xrm.Sdk;
using Microsoft.Crm.Sdk;
using BLTools.CRM2011;
using BLTools;
using BLTools.ConsoleExtension;
using Microsoft.Xrm.Sdk.Query;
using phacobel.crm2011;

namespace DynCRM2011Test {
  class Program {

    static void Main(string[] args) {

      TChrono Chrono = new TChrono();
      Chrono.Start();

      using (TCrmDatabase CrmDatabase = new TCrmDatabase("crm.phacobel.be:8443", "phacobel", "interne", "CRMPhaco1$")) {

        PhacobelContext PhacobelDataContext = new PhacobelContext(CrmDatabase.OSP);
        var Accounts = PhacobelDataContext
          .AccountSet
          .Select(x => new { x.OwnerId, x.Name, x.Address1_Name, x.Address1_PostalCode, x.Address1_City });

        //new Account().

        Console.WriteLine("{0} records found", Accounts.AsEnumerable().Count());

        foreach (var AccountItem in Accounts) {
          Console.Write("Found account {0}", AccountItem.Name);
          //Console.Write(", {0}", AccountItem.OwnerId);
          Console.Write(", {0}", AccountItem.Address1_Name);
          Console.Write(", {0}", AccountItem.Address1_PostalCode);
          Console.Write(", {0}", AccountItem.Address1_City);
          Console.WriteLine();
          break;
        }

      }

      Console.WriteLine("Elapsed time: {0}", Chrono.ElapsedTime.ToString());
      ConsoleExtension.Pause();

    }
  }
}
