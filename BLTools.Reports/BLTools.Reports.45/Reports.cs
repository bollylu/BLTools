using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using BLTools;
using CaratFileManagementLib;

namespace CaratManagementReports {
  public class Reports {

    public static TCaratReportAttributeCollection AvailableReports { get; private set; }

    public static string LastResult { get; private set; }

    static Reports() {
      AvailableReports = new TCaratReportAttributeCollection();

      //AddReports(typeof(CaratManagementReports.ReportTable));
      //AddReports(typeof(CaratManagementReports.ReportDetails));
      //AddReports(typeof(CaratManagementReports.ReportHtmlTable));

    }

    private static void AddReports(Type T) {
      foreach (MethodInfo MethodItem in T.GetMethods()) {
        object[] Attributes = MethodItem.GetCustomAttributes(typeof(TCaratReportAttribute), true);
        if (Attributes.Length > 0) {
          TCaratReportAttribute CurrentReportMethodAttribute = Attributes[0] as TCaratReportAttribute;
          AvailableReports.Add(string.Format("{0}.{1}", MethodItem.DeclaringType.Name, MethodItem.Name), new TCaratReportAttribute(CurrentReportMethodAttribute));
        }
      }
    }

    public static string DisplayAvailableReports() {
      return AvailableReports.ToString();
    }

    public static bool IsReportAvailable(string report) {
      return (AvailableReports.ContainsKey(report.ToLower()));
    }

    public static string ExecuteReport(string report, TCaratProjectCollection projects, string title="") {
      string Namespace = typeof(Reports).Namespace;
      string DeclaringType = report.Left(report.IndexOf('.'));
      Type T = Type.GetType(string.Format("{0}.{1}", Namespace, DeclaringType), false, true);
      string ReportMethod = report.Substring(report.IndexOf('.') + 1);
      MethodInfo[] MethodInfos = T.GetMethods();
      MethodInfo ReportMethodInfo = MethodInfos.Where(m => m.Name.ToLower() == ReportMethod.ToLower()).First();
      string Title = title != "" ? title : AvailableReports[report].Description;
      LastResult = (string)ReportMethodInfo.Invoke(null, new object[] { projects, Title });
      return LastResult;
    }

  }




}
