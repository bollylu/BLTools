using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.Diagnostics;
using System.IO;

namespace BLTools.Security.Authorization {
  public abstract class TSecurityStorage {
    #region Public properties
    public string Name { get; set; }
    public TSecurityUserCollection Users { get; set; }
    public TSecurityGroupCollection Groups { get; set; }
    #endregion Public properties

    public TSecurityStorage() {}

    public TSecurityStorage(TSecurityStorage storage) {
      Name = storage.Name;
      Users = new TSecurityUserCollection(storage.Users);
      Groups = new TSecurityGroupCollection(storage.Groups);
    }

    public abstract void Save();

    public abstract void Save(string name);
  }
}
