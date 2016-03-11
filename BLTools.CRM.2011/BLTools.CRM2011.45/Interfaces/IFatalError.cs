using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLTools.CRM2011 {
  public interface IFatalError {
    bool HasFatalError { get; }
    IEnumerable<string> FatalErrorLines { get; }
    string FatalErrorText { get; }
  }
}
