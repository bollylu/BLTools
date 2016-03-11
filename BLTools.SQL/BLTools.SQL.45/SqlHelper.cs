using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.SqlServer.Management.Smo;

namespace BLTools.SQL {
  public static class SqlHelper {

    public static DataType TextToSqlDataType(string textDataType, int length=0) {
      DataType RetVal = new DataType();
      switch (textDataType.ToLower()) {
        case "nvarchar":
          RetVal = DataType.NVarChar(length);
          break;
        case "nvarcharmax":
          RetVal = DataType.NVarCharMax;
          break;
        case "int":
          RetVal = DataType.Int;
          break;
        case "datetime":
          RetVal = DataType.DateTime;
          break;
        case "bool":
        case "bit":
          RetVal = DataType.Bit;
          break;
        case "float":
          RetVal = DataType.Float;
          break;
        case "real":
          RetVal = DataType.Real;
          break;
        case "varchar":
          RetVal = DataType.VarChar(length);
          break;
        case "varcharmax":
          RetVal = DataType.VarCharMax;
          break;
        case "char":
          RetVal = DataType.Char(length);
          break;
        case "binary":
          RetVal = DataType.Binary(length);
          break;
        case "long":
        case "bigint":
          RetVal = DataType.BigInt;
          break;
        case "money":
          RetVal = DataType.Money;
          break;
        case "image":
          RetVal = DataType.Image;
          break;
      }
      return RetVal;
    }

  }
}
