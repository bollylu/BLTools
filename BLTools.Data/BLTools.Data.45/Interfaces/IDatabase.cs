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

    bool Exists();
    bool Create();
    bool Drop(bool killConnections);

    bool TableExists(string table);
    IEnumerable<string> ListTables();
    void CreateTables();
    void DropTables();
    void DropTable(string table);

    IDbTransaction StartTransaction();
    void CommitTransaction();
    void RollbackTransaction();

    bool Backup(string backupFullName);
    void BackupAsync(string backupFullName);
    bool Restore(string backupFullName);

    event EventHandler<BoolEventArgs> OnBackupCompleted;
    event EventHandler<BoolEventArgs> OnRestoreCompleted;

    event EventHandler<BoolAndMessageEventArgs> OnDatabaseAttached;
    event EventHandler<BoolAndMessageEventArgs> OnDatabaseExists;
    event EventHandler<BoolAndMessageEventArgs> OnDatabaseCreated;
    event EventHandler<BoolAndMessageEventArgs> OnDatabaseDropped;

    event EventHandler OnTransactionStarted;
    event EventHandler OnTransactionCommit;
    event EventHandler OnTransactionRollback;

  }
}
