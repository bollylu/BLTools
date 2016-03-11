using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLTools.CRM2011 {
  public interface IProduct : ICrmEntity {
    string Name { get; }
    string ProductNumber { get; }
    bool IsActive { get; }
    string Description { get; }
    
  }
}
