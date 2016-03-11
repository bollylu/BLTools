using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BLTools.DataModels;

namespace BLTools.CRM2011 {
  public interface IContact : ICrmEntity, IFatalError, IError {
    TContactPerson ContactPerson { get; }
    ISystemUser SystemUser { get; }
  }
}
