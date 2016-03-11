using Microsoft.Xrm.Sdk.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLTools.CRM2011 {
  public static class FilterExpressionHelper {

    public static FilterExpression Between(string fieldname, DateTime start, DateTime end) {
      FilterExpression RetVal = new FilterExpression(LogicalOperator.And);
      RetVal.AddCondition(new ConditionExpression(fieldname, ConditionOperator.GreaterEqual, start.ToUniversalTime()));
      RetVal.AddCondition(new ConditionExpression(fieldname, ConditionOperator.LessEqual, end.ToUniversalTime()));
      return RetVal;
    }

  }
}
