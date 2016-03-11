using System;
using System.Management;
using System.IO;
using BLTools;
using System.Diagnostics;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text;
using System.Management.Instrumentation;
using System.Collections.ObjectModel;

namespace BLTools.Hardware {

  public class MachineComponents {

    #region Public properties
    public string CurrentUser { get; private set; }
    public string CurrentDomain { get; private set; }
    public string CurrentDirectory { get; private set; }
    public string OSVersion { get; private set; }
    public string OSServicePack { get; private set; }
    public UInt64 OSAvailablePhysicalMemory { get; private set; }

    public List<string> ParallelPorts { get; private set; }
    public List<string> SerialPorts { get; private set; }
    public List<string> UsbControllers { get; private set; }
    public List<string> EthernetAdapters { get; private set; }
    public List<string> CDRoms { get; private set; }
    public List<string> Processors { get; private set; }

    public string ScreenResolution { get; private set; }

    #endregion Public properties

    #region Constructors
    public MachineComponents() {

      WMI oWMI = new WMI();
      Trace.WriteLine("Getting parallel ports list...");
      ParallelPorts = oWMI.GetParallelPortList();
      Trace.WriteLine("Getting serial ports list...");
      SerialPorts = oWMI.GetSerialPortList();
      Trace.WriteLine("Getting USB controllers list...");
      UsbControllers = oWMI.GetUsbControllerList();
      Trace.WriteLine("Getting Ethernet controllers list...");
      EthernetAdapters = oWMI.GetEthernetNetworkControllerList();
      Trace.WriteLine("Getting CD/DVD list...");
      CDRoms = oWMI.GetDvdList();
      Trace.WriteLine("Getting processors list...");
      Processors = oWMI.GetProcessorList();
      Trace.WriteLine("Getting environment infos ...");
      CurrentUser = Environment.UserName;
      CurrentDomain = Environment.UserDomainName;
      CurrentDirectory = Environment.CurrentDirectory;
      Trace.WriteLine("Getting OS version ...");
      OSVersion = oWMI.OSVersion;
      Trace.WriteLine("Getting OS service pack ...");
      OSServicePack = oWMI.OSServicePack;
      Trace.WriteLine("Getting available memory ...");
      OSAvailablePhysicalMemory = oWMI.OSAvailablePhysicalMemory;
      Trace.WriteLine("Getting screen infos ...");
      ScreenResolution = oWMI.GetScreenResolution;
    }
    #endregion Constructors

    #region Public methods
    public string Report() {
      WMI oWMI = new WMI();
      StringBuilder sbTemp = new StringBuilder();
      sbTemp.AppendFormat("Userid = {0}\n", CurrentUser);
      sbTemp.AppendFormat("Domain = {0}\n", CurrentDomain);
      sbTemp.AppendFormat("Execution directory = {0}\n", CurrentDirectory);
      sbTemp.AppendFormat("OS Version & service pack = {0} {1}\n", OSVersion, OSServicePack);
      sbTemp.AppendFormat("OS Version & service pack = {0} {1}\n", oWMI.OSNumVersion, oWMI.OSServicePackNumVersion);
      sbTemp.AppendFormat("OS Available physical memory = {0} KB\n", OSAvailablePhysicalMemory.ToString("#,##0"));
      if (ParallelPorts.Count > 0) {
        sbTemp.AppendFormat("Parallel ports detected : {0}\n", ParallelPorts.Count);
        sbTemp.AppendFormat("  {0}\n", string.Join("\n  ", ParallelPorts.ToArray()));
      } else {
        sbTemp.Append("No parallel port detected.");
      }
      if (SerialPorts.Count > 0) {
        sbTemp.AppendFormat("Serial ports detected : {0}\n", SerialPorts.Count);
        sbTemp.AppendFormat("  {0}\n", string.Join("\n  ", SerialPorts.ToArray()));
      } else {
        sbTemp.Append("No serial port detected.");
      }
      if (UsbControllers.Count > 0) {
        sbTemp.AppendFormat("USB controllers detected : {0}\n", UsbControllers.Count);
        sbTemp.AppendFormat("  {0}\n", string.Join("\n  ", UsbControllers.ToArray()));
      } else {
        sbTemp.Append("No USB controller detected.");
      }
      if (EthernetAdapters.Count>0) {
      sbTemp.AppendFormat("Ethernet adapters detected : {0}\n", EthernetAdapters.Count);
      sbTemp.AppendFormat("  {0}\n", string.Join("\n  ", EthernetAdapters.ToArray()));
      } else {
        sbTemp.Append("No ethernet adapter detected.");
      }
      if (CDRoms.Count > 0) {
        sbTemp.AppendFormat("CD/DVD detected : {0}\n", CDRoms.Count);
        sbTemp.AppendFormat("  {0}\n", string.Join("\n  ", CDRoms.ToArray()));
      } else {
        sbTemp.Append("No CD/DVD detected.");
      }
      if (Processors.Count > 0) {
        sbTemp.AppendFormat("Processors detected : {0}\n", Processors.Count);
        sbTemp.AppendFormat("  {0}\n", string.Join("\n  ", Processors.ToArray()));
      } else {
        sbTemp.Append("No processor detected.");
      }
      sbTemp.AppendFormat("Current screen characteristics : {0}", ScreenResolution);
      return sbTemp.ToString();
    }
    #endregion Public methods

  }



}
