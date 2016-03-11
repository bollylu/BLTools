using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLTools.Google.Apis.Direction {
  public enum EDirectionsResponseStatus {
    Unknown,
    OK,
    NOT_FOUND,
    MAX_WAYPOINTS_EXCEEDED,
    INVALID_REQUEST,
    OVER_QUERY_LIMIT,
    REQUEST_DENIED,
    ZERO_RESULTS
  }
}
