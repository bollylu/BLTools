using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLTools.CRM2011 {
  interface IGroupedOrder : IList<IOrder>, ICrmEntity, IError, IFatalError {
     string GroupedOrderNumber { get; set; }
  }
}
