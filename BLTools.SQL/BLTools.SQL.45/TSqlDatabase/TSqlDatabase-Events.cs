using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLTools.SQL {
  public partial class TSqlDatabase {

    #region Events
    public event EventHandler OnTransactionStarted;
    public event EventHandler OnTransactionCommit;
    public event EventHandler OnTransactionRollback;
    #endregion Events

  }
}
