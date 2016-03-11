using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using BLTools.Text;

namespace BLTools.Debugging {
  /// <summary>
  /// Helper for obtaining runtime infos
  /// </summary>
  public static partial class ApplicationInfo {

    /// <summary>
    /// Generate a banner with startup information and send it to Trace
    /// </summary>
    public static void ApplicationStart() {
      Trace.IndentSize = 2;
      Trace.AutoFlush = true;
      try {
        AssemblyName StartupApplication = Assembly.GetEntryAssembly().GetName();
        string NameLine = string.Format("Startup of {0}, {1}, {2}", StartupApplication.Name, StartupApplication.ProcessorArchitecture, StartupApplication.Version.ToString());
        string ContextLine = string.Format(".NET Runtime version: {0}", Assembly.GetEntryAssembly().ImageRuntimeVersion);
        string BLToolsLine = string.Format("BLTools version: {0}", Assembly.GetExecutingAssembly().GetName().Version.ToString());
        string CompleteString = string.Join("\r\n", NameLine, ContextLine, BLToolsLine);
        Trace.WriteLine(TextBox.BuildFixedWidth(CompleteString, 80, TextBox.StringAlignmentEnum.Left));
      } catch { }
      ApplicationInfo.TraceRuntimeInfo();
    }
    /// <summary>
    /// Generate a banner to confirm that application is stopping and send it to Trace
    /// </summary>
    public static void ApplicationStop() {
      try {
        AssemblyName StartupApplication = Assembly.GetEntryAssembly().GetName();
        string NameLine = string.Format("{0} is stopping", StartupApplication.Name);
        Trace.WriteLine(TextBox.BuildFixedWidth(NameLine, 80, TextBox.StringAlignmentEnum.Left));
        Trace.Flush();
      } catch { }
    }

    /// <summary>
    /// Send a banner with runtime info to Trace
    /// </summary>
    public static void TraceRuntimeInfo() {
      try {
        Trace.WriteLine(BuildRuntimeInfo());
      } catch { }
    }
    
    /// <summary>
    /// Builds a banner with runtime infos
    /// </summary>
    /// <returns></returns>
    public static string BuildRuntimeInfo() {
      StringBuilder RetVal = new StringBuilder();
      RetVal.AppendLine(string.Format("Working folder = \"{0}\"", Environment.CurrentDirectory));
      RetVal.AppendLine(string.Format("Operating system = \"{0}\"", Environment.OSVersion.VersionString));
      RetVal.AppendLine(string.Format("Current user = \"{0}\"", Environment.UserName));
      RetVal.AppendLine(string.Format("Current domain user = \"{0}\"", Environment.UserDomainName));
      RetVal.AppendLine(string.Format("Is 64 bits OS = {0}", Environment.Is64BitOperatingSystem));
      RetVal.Append(string.Format("Is current user administrator = {0}", IsCurrentUserAdmin()));
      return TextBox.BuildDynamic(RetVal.ToString(),0, TextBox.StringAlignmentEnum.Left);

    }

  }
}
