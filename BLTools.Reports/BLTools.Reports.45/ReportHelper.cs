using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CaratFileManagementLib;

namespace CaratManagementReports {
  public class ReportHelper {
    public static string FullBox(string message) {
      StringBuilder RetVal = new StringBuilder();
      string Border = new string('-', message.Length + 4);
      RetVal.AppendLine(Border);
      RetVal.AppendFormat("| {0} |\r\n", message);
      RetVal.AppendLine(Border);
      return RetVal.ToString();
    }
    public static string OpenBox(string message) {
      StringBuilder RetVal = new StringBuilder();
      string Border = new string('-', message.Length);
      RetVal.AppendLine(Border);
      RetVal.AppendLine(message);
      RetVal.AppendLine(Border);
      return RetVal.ToString();
    }

    public static string DisplayStatistics(TCaratProjectCollection projects) {
      StringBuilder RetVal = new StringBuilder();
      RetVal.AppendLine("Statistics");
      RetVal.AppendLine("----------");
      RetVal.AppendFormat("Total of projects : {0}\r\n", projects.Count);
      RetVal.AppendFormat("Total of valid files : {0}\r\n", projects.Sum(p => p.ValidCaratFilesCount));
      RetVal.AppendFormat("Total of invalid files : {0}\r\n", projects.Sum(p => p.InvalidCaratFilesCount));
      RetVal.AppendFormat("Total of duplicate files : {0}\r\n", projects.Sum(p => p.DupesCount));
      return RetVal.ToString();
    }
  }
}
