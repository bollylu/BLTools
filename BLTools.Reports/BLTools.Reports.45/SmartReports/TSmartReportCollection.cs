using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Printing;
using BLTools.Reports;

namespace CaratManagementReports.SmartReports {
  public class TSmartReportCollection : List<TSmartReport> {

    //public ReportDestinationEnum GetDestination(string report) {
    //  return ((TCaratReportAttribute)(SmartReport.AvailableReports[report]).GetCustomAttributes(typeof(TCaratReportAttribute), true)[0]).Destination;
    //}

    //public ReportTypeEnum GetReportType(string report) {
    //  return ((TCaratReportAttribute)(SmartReport.AvailableReports[report]).GetCustomAttributes(typeof(TCaratReportAttribute), true)[0]).ReportType;
    //}

    //public PageOrientation GetPageOrientation(string report) {
    //  return ((TCaratReportAttribute)(SmartReport.AvailableReports[report]).GetCustomAttributes(typeof(TCaratReportAttribute), true)[0]).Orientation;
    //}

    //public string GetDescrption(string report) {
    //  return ((TCaratReportAttribute)(SmartReport.AvailableReports[report]).GetCustomAttributes(typeof(TCaratReportAttribute), true)[0]).Description;
    //}

    //public Dictionary<string, string> ToListBox() {
    //  Dictionary<string, string> RetVal = new Dictionary<string, string>();
    //  foreach (string KeyItem in this.Keys) {
    //    TCaratReportAttribute Attribute = (TCaratReportAttribute)this[KeyItem].GetCustomAttributes(typeof(TCaratReportAttribute), true)[0];
    //    RetVal.Add(KeyItem, string.Format("({0}-{1}) - {2}", Attribute.ReportType, Attribute.Destination, Attribute.Description));
    //  }
    //  return RetVal;
    //}

  }

  
}


