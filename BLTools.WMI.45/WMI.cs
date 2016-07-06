using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Management;
using System.Collections;
using System.Globalization;

namespace BLTools.WMI {
  public class WMI {

    #region Public properties
    public string OSVersion {
      get {
        return WMI_GetValue<string>("Win32_OperatingSystem", "Caption", "");
      }
    }

    public string OSServicePackNumVersion {
      get {
        return WMI_GetValue<UInt16>("Win32_OperatingSystem", "ServicePackMajorVersion", 0).ToString()
          + "."
          + WMI_GetValue<UInt16>("Win32_OperatingSystem", "ServicePackMinorVersion", 0).ToString();
      }
    }

    public string OSNumVersion {
      get {
        return WMI_GetValue<string>("Win32_OperatingSystem", "Version", "");
      }
    }

    public string OSServicePack {
      get {
        return WMI_GetValue<string>("Win32_OperatingSystem", "CSDVersion", "");
      }
    }

    public UInt64 OSAvailablePhysicalMemory {
      get {
        return WMI_GetValue<UInt64>("TotalVisibleMemorySize", "Win32_OperatingSystem", 0);
      }
    }

    public UInt64 OSFreeMemory {
      get {
        UInt64 FreePhysicalMemory = WMI_GetValue<UInt64>("FreePhysicalMemory", "Win32_OperatingSystem", 0);
        UInt64 FreeVirtualMemory = WMI_GetValue<UInt64>("FreeVirtualMemory", "Win32_OperatingSystem", 0);
        return FreePhysicalMemory + FreeVirtualMemory;
      }
    }

    public string GetScreenResolution {
      get {
        StringBuilder RetVal = new StringBuilder();
        try {
          WqlObjectQuery oQuery = new WqlObjectQuery("SELECT Caption, ScreenHeight, ScreenWidth FROM Win32_DesktopMonitor");
          ManagementObjectSearcher oSearcher = new ManagementObjectSearcher(oQuery);
          ManagementObjectCollection oResults = oSearcher.Get();
          //Trace.WriteLine("Get Enumerator");
          IEnumerator oEnum = oResults.GetEnumerator();
          //Trace.WriteLine("Reset Enumerator");
          oEnum.Reset();
          //Trace.WriteLine("Move next Enumerator");
          oEnum.MoveNext();
          //Trace.WriteLine("Get current value");
          ManagementObject oResult = (ManagementObject)oEnum.Current;
          Trace.WriteLine(oResult);

          RetVal.Append(oResult.Properties["caption"].Value.ToString());
          RetVal.AppendFormat(" [{0}x{1}]", ((UInt32)oResult.Properties["ScreenWidth"].Value).ToString(), ((UInt32)oResult.Properties["ScreenHeight"].Value).ToString());
        } catch (Exception ex) {
          Trace.WriteLine(string.Format("Error during GetScreenResolution : {0}", ex.Message));
        }
        return RetVal.ToString();
      }
    }
    #endregion Public properties

    #region public methods
    public List<string> GetParallelPortList() {
      List<string> RetVal = new List<string>();
      try {
        WqlObjectQuery oQuery = new WqlObjectQuery("SELECT caption FROM Win32_ParallelPort");
        ManagementObjectSearcher oSearcher = new ManagementObjectSearcher(oQuery);
        foreach (ManagementObject oResult in oSearcher.Get()) {
          RetVal.Add(oResult.Properties["caption"].Value.ToString());
        }
      } catch (Exception ex) {
        Trace.WriteLine(string.Format("Error during WMI request for parallel ports : {0}", ex.Message));
      }
      return RetVal;
    }

    public List<string> GetUsbControllerList() {
      List<string> RetVal = new List<string>();
      WqlObjectQuery oQuery = new WqlObjectQuery("SELECT caption FROM Win32_USBController");
      ManagementObjectSearcher oSearcher = new ManagementObjectSearcher(oQuery);
      foreach (ManagementObject oResult in oSearcher.Get()) {
        RetVal.Add(oResult.Properties["caption"].Value.ToString());
      }
      return RetVal;
    }

    public List<string> GetSerialPortList() {
      List<string> RetVal = new List<string>();
      WqlObjectQuery oQuery = new WqlObjectQuery("SELECT caption FROM Win32_SerialPort");
      ManagementObjectSearcher oSearcher = new ManagementObjectSearcher(oQuery);
      foreach (ManagementObject ManagementObjectItem in oSearcher.Get()) {
        RetVal.Add(ManagementObjectItem.Properties["caption"].Value.ToString());
      }
      return RetVal;
    }

    public List<string> GetEthernetNetworkControllerList() {
      List<string> RetVal = new List<string>();
      WqlObjectQuery oQuery = new WqlObjectQuery("SELECT * FROM Win32_NetworkAdapter WHERE (AdapterTypeId=0)");
      ManagementObjectSearcher oSearcher = new ManagementObjectSearcher(oQuery);
      try {
        foreach (ManagementObject ManagementObjectItem in oSearcher.Get()) {
          PropertyDataCollection oPropertyDataCollection = ManagementObjectItem.Properties;
          if (oPropertyDataCollection["name"].Value.ToString().ToLower().IndexOf("miniport") < 0 && oPropertyDataCollection["caption"].Value.ToString().ToLower().IndexOf("miniport") < 0) {
            StringBuilder sbTemp = new StringBuilder();
            sbTemp.Append(oPropertyDataCollection["caption"].Value.ToString());
            sbTemp.AppendFormat(" [{0}]", oPropertyDataCollection["MACAddress"].Value.ToString());
            RetVal.Add(sbTemp.ToString());
          }
        }
      } catch (Exception ex) {
        Trace.WriteLine(string.Format("Unable to obtain list of NetworkAdapters from WMI : {0}", ex.Message));
        RetVal = new List<string>();
      }
      return RetVal;
    }

    public List<string> GetProcessorList() {
      List<string> RetVal = new List<string>();
      WqlObjectQuery oQuery = new WqlObjectQuery("SELECT caption FROM Win32_Processor");
      ManagementObjectSearcher oSearcher = new ManagementObjectSearcher(oQuery);
      foreach (ManagementObject oResult in oSearcher.Get()) {
        RetVal.Add(oResult.Properties["caption"].Value.ToString().Trim());
      }
      return RetVal;
    }

    public List<string> GetDvdList() {
      List<string> RetVal = new List<string>();
      WqlObjectQuery oQuery = new WqlObjectQuery("SELECT caption FROM Win32_CDRomDrive");
      ManagementObjectSearcher oSearcher = new ManagementObjectSearcher(oQuery);
      foreach (ManagementObject oResult in oSearcher.Get()) {
        RetVal.Add(oResult.Properties["caption"].Value.ToString());
      }
      return RetVal;
    }

    public List<string> GetNetConnectionStatus() {
      List<string> RetVal = new List<string>();
      WqlObjectQuery oQuery = new WqlObjectQuery("SELECT * FROM Win32_NetworkAdapter WHERE (AdapterTypeId=0)");
      ManagementObjectSearcher oSearcher = new ManagementObjectSearcher(oQuery);
      foreach (ManagementObject oResult in oSearcher.Get()) {
        PropertyDataCollection oPropertyDataCollection = oResult.Properties;
        if (oPropertyDataCollection["name"].Value.ToString().ToLower().IndexOf("miniport") < 0) {
          StringBuilder sbTemp = new StringBuilder();
          sbTemp.Append(oPropertyDataCollection["caption"].Value.ToString());
          sbTemp.AppendFormat(" [{0}]", oPropertyDataCollection["NetConnectionStatus"].Value);
          RetVal.Add(sbTemp.ToString());
        }
      }
      return RetVal;
    }

    public long GetFreeDiskSpace(string disk) {
      disk = disk.ToUpper();
      if (!disk.EndsWith(":")) {
        disk += ":";
      }
      try {
        ulong FreeSpace = System.Convert.ToUInt64(WMI_GetValue("FreeSpace", "Win32_LogicalDisk", string.Format("DeviceID='{0}'", disk)));
        return ((long)((double)FreeSpace) / 1024 / 1024);
      } catch {
        return (long)0;
      }
    }

    public long GetDiskSize(string disk) {
      disk = disk.ToUpper();
      if (!disk.EndsWith(":")) {
        disk += ":";
      }
      try {
        ulong Size = System.Convert.ToUInt64(WMI_GetValue("Size", "Win32_LogicalDisk", string.Format("DeviceID='{0}'", disk)));
        return ((long)((double)Size) / 1024 / 1024);
      } catch {
        return (long)0;
      }
    }
    #endregion public methods

    #region private methods
    private object WMI_GetValue(string queryField, string fromTable, string whereClause = "") {
      object Retval = null;
      WqlObjectQuery oQuery = new WqlObjectQuery("SELECT " + queryField + " FROM " + fromTable);
      if (whereClause.Length != 0) {
        oQuery.QueryString += " WHERE " + whereClause;
      }
      ManagementObjectSearcher oSearcher = new ManagementObjectSearcher(oQuery);
      try {
        foreach (ManagementObject oResult in oSearcher.Get()) {
          Retval = oResult[queryField];
        }
      } catch (Exception ex) {
        Trace.WriteLine(string.Format("Error accessing the WMI : {0} : {1}", ex.Message, ex.InnerException));
        return null;
      }
      return Retval;
    }

    private T WMI_GetValue<T>(string table, string queryField, T defaultValue, string whereClause = "") {
      T Retval = defaultValue;
      WqlObjectQuery oQuery = new WqlObjectQuery("SELECT " + queryField + " FROM " + table);
      if (whereClause.Length != 0) {
        oQuery.QueryString += " WHERE " + whereClause;
      }
      ManagementObjectSearcher oSearcher = new ManagementObjectSearcher(oQuery);
      try {
        foreach (ManagementObject oResult in oSearcher.Get()) {
          Retval = BLConverter.BLConvert<T>(oResult[queryField], CultureInfo.CurrentCulture, defaultValue);
        }
      } catch (Exception ex) {
        Trace.WriteLine(string.Format("Error accessing the WMI : {0} : {1}", ex.Message, ex.InnerException));
        return defaultValue;
      }
      return Retval;
    }
    #endregion private methods
  }
}
