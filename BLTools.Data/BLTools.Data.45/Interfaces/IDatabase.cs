using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BLTools;

namespace BLTools.Data {
  public interface IDatabase {
    string ConnectionString { get; }
    
    bool TryOpen();
    void TryClose();
    bool IsOpened { get; }

    IDbTransaction StartTransaction();
    void CommitTransaction();
    void RollbackTransaction();

    event EventHandler OnTransactionStarted;
    event EventHandler OnTransactionCommit;
    event EventHandler OnTransactionRollback;

  }
}
