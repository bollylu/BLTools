using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLTools.CRM2011 {
  public interface IError {
    bool HasError { get; }
    IEnumerable<string> ErrorLines { get; }
    string ErrorText { get; }
  }
}
