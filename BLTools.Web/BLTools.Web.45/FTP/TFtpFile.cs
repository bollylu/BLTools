using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BLTools;
using System.Diagnostics;

namespace BLTools.Web {
  public class TFtpFile {
    public string Name { get; set; }
    public int Size { get; set; }
    public DateTime LastModified { get; set; }

    public TFtpFile() {
      Name = "";
      Size = 0;
      LastModified = DateTime.MinValue;
    }

    public TFtpFile(string name, int size = 0) : this(name, size, DateTime.MinValue) { }

    public TFtpFile(string name, int size, DateTime lastModified) : this() {
      Name = name;
      Size = size;
      LastModified = lastModified;
    }

    //"-rw-rw-rw-   1 user     group        5473 Sep  3 21:07 20140820Phacobel_001.pdf"
    public TFtpFile(string textLineFromFtp) {
      try {
        string FileName = textLineFromFtp.Substring(55);
        string FileSize = textLineFromFtp.Substring(31, 10);
        Size = int.Parse(FileSize);
        string FileLastModifiedMonth = textLineFromFtp.Substring(42, 3);
        string FileLastModifiedDay = textLineFromFtp.Substring(46, 2);
        string FileLastModifyTime = textLineFromFtp.Substring(49, 5);
        Name = FileName;
        string TempLastModified = string.Format("{0} {1} {2} {3}", FileLastModifiedDay, FileLastModifiedMonth, DateTime.Today.Year, FileLastModifyTime);
        LastModified = DateTime.Parse(TempLastModified);

      } catch (Exception ex) {
        Trace.WriteLine(string.Format("Unable to create TftpFile : {0}", ex.Message));
      }
    }

    public override string ToString() {
      StringBuilder RetVal = new StringBuilder();
      RetVal.AppendFormat("{0}", Name.PadRight(50, '.'));
      RetVal.AppendFormat(" | {0}", Size.ToString().PadLeft(18, '.'));
      RetVal.AppendFormat(" | {0}", LastModified.ToString());
      return RetVal.ToString();
    }
  }
}
