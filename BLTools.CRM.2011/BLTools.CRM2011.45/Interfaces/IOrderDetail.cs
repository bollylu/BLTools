using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLTools.CRM2011 {
  public interface IOrderDetail : ICrmEntity, IError, IFatalError {
    int LineNumber { get; set; }
    IProduct Product { get; set; }
    int QuantityOrdered { get; set; }
    int QuantityDelivered { get; set; }
    int QuantityBackordered { get; set; }
    decimal DiscountPercentage { get; }
    decimal UnitPriceWithoutDiscount { get; }
    decimal UnitPrice { get; }
  }
}
