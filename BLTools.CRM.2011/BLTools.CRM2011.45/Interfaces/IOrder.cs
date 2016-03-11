using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLTools.CRM2011 {
  public interface IOrder : ICrmEntity, IError, IFatalError {
    ISystemUser SystemUser { get; set; }
    IContact Customer { get; set; }
    IList<IOrderDetail> Details { get; }
    string OrderNumber { get; }
  }
}
