using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLTools.SQL.Management {
  public partial class TSqlDatabaseManager : TSqlDatabase {

    #region Events
    public event EventHandler<BoolEventArgs> OnBackupCompleted;
    public event EventHandler<BoolEventArgs> OnRestoreCompleted;

    public event EventHandler<BoolAndMessageEventArgs> OnDatabaseAttached;
    public event EventHandler<BoolAndMessageEventArgs> OnDatabaseExists;
    public event EventHandler<BoolAndMessageEventArgs> OnDatabaseCreated;
    public event EventHandler<BoolAndMessageEventArgs> OnDatabaseDropped;

    public event EventHandler OnTransactionStarted;
    public event EventHandler OnTransactionCommit;
    public event EventHandler OnTransactionRollback;

    public event EventHandler<BoolAndMessageEventArgs> OnTableDropped;
    #endregion Events


  }
}
