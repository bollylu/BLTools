using BLTools.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLTools.CRM2011 {
  public interface ISystemUser : ICrmEntity {
    TPersonName Name { get; }
  }
}
