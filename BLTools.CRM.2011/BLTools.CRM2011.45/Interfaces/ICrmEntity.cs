using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLTools.CRM2011 {
  public interface ICrmEntity {
    Guid Id { get; }
    DateTime CreatedOn { get; }
    DateTime ModifiedOn { get; }
  }
}
