using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLTools.Data.FixedLength {
  public class TFixedLengthField {
    public string Name { get; set; }
    public char Padding { get; set; }
    public int Length { get; set; }

    public TFixedLengthField() { }

    public TFixedLengthField(string name, char padding, int length) {
      Name = name;
      Padding = padding;
      Length = length;
    }
  }
}
