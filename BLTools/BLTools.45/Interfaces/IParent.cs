using System;
using System.Collections.Generic;
using System.Text;

namespace BLTools {
  public interface IParent {
    IParent Parent { get; set; }
    T GetParent<T>();
  }
}
