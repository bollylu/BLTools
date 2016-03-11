using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace StockRic {
  class InputFile : List<InputLine> {
    public string Name { get; set; }
    public FileSchema Schema { get; set; }
    
    private StreamReader _InputStreamReader;
    private bool _IsOpened;

    public void Open() {
      _InputStreamReader = File.OpenText(Name);
      _IsOpened = true;
    }
    public void Close() {
      _InputStreamReader.Close();
      _IsOpened = false;
    }

    public void FillSchema() {
    }

    public void ReadLine() {
      foreach ( DataField DataFieldItem in Schema ) {
      }
    }

    public void ReadFile() {
      Open();

      Close();
    }
  }
}
