using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RarLib {
  public class TRarElement {
    public string Name { get; set; }
    public string Pathname { get; set; }
    public string Fullname {
      get {
        return ((Pathname ?? "") == "" ? "" : Pathname + "\\") + Name;
      }
    }
    public double CompressionRatio { get; private set; }
    public long UncompressedSize {get; private set;}
    public long CompressedSize { get; private set; }
    public string Attributes {get;private set;}
    public bool IsFolder { get; private set; }

    #region Constructor(s)
    public TRarElement() {
      Name = "";
      Pathname = "";
      CompressionRatio = 0d;
    }
    public TRarElement(string name, string codedInfo = "")
      : this() {
      if (name.Contains("\\")) {
        Name = name.Substring(name.LastIndexOf("\\") + 1);
        Pathname = name.Substring(0, name.LastIndexOf("\\"));
      } else {
        Name = name;
        Pathname = "";
      }
      if (codedInfo != "") {
        _Parse(codedInfo);
      }
    }

    private void _Parse(string codedInfo) {
      string[] SplittedInfo = codedInfo.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
      UncompressedSize = long.Parse(SplittedInfo[0]);
      CompressedSize = long.Parse(SplittedInfo[1]);
      CompressionRatio = double.Parse(SplittedInfo[2].TrimEnd('%'));
      Attributes = SplittedInfo[5];
      IsFolder = Attributes.Contains('D');
    }
    #endregion Constructor(s)

    public override string ToString() {
      StringBuilder RetVal = new StringBuilder();
      RetVal.AppendFormat("\"{0}\"", Fullname);
      if (!IsFolder) {
        RetVal.AppendFormat(", Compression Ratio={0}%", CompressionRatio);
        RetVal.AppendFormat(", Size={0}", UncompressedSize);
        RetVal.AppendFormat(", Compressed={0}", CompressedSize);
      }
      return RetVal.ToString();
    }
  }
}
