using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BLTools;
using BLTools.ADS;

namespace TestAdsBackup {
  class Program {
    static void Main(string[] args) {

      TAdsDatabase.AdsBackupLocation = "";
      using (TAdsDatabase Db = new TAdsDatabase()) {
        Db.Backup(@"\\gnlied0041\test\dbbackup");
      }
    }
  }
}
