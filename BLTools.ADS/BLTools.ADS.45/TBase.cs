using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.Diagnostics;
using BLTools;
using Advantage.Data.Provider;

namespace BLTools.ADS {
  public class TBase<T> where T : TAdsDatabase, new() {

    #region Public properties
    public T CurrentDatabase {
      get;
      set;
    }
    #endregion Public properties

    #region Constructor(s)
    public TBase() {
      CurrentDatabase = new T();
    }

    public TBase(T newDatabase) : this() {
      CurrentDatabase = newDatabase;
    }
    #endregion Constructor(s)

    #region Public methods
    public override string ToString() {
      return this.ToString(false);
    }
    public virtual string ToString(bool deep) {
      StringBuilder RetVal = new StringBuilder();
      RetVal.Append(this.GetType().ToString());
      return RetVal.ToString();
    }

    #endregion Public methods

  }
  
}
