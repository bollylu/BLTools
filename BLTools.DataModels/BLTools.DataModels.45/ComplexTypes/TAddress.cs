using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLTools.DataModels {
  public class TAddress {
    public string Address1 { get; set; }
    public string Address2 { get; set; }
    public string ZipCode { get; set; }
    public string City { get; set; }
    public TCountry Country { get; set; }

    public TAddress() {
      Country = new TCountry();
    }

    public TAddress(TAddress address) {
      Address1 = address.Address1;
      Address2 = address.Address2;
      ZipCode = address.ZipCode;
      City = address.City;
      Country = new TCountry(address.Country);
    }

    public string FullAddress {
      get {
        return string.Format("{0} - {1} {2}", Address1, ZipCode, City);
      }
    }
  } 
}
