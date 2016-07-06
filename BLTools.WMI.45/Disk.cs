using System;
using System.Diagnostics;

namespace BLTools.WMI {
  
  public class Disk {

    private string _disk;
    
    public Disk(char disk) {
      _disk = new string(new char[] {disk, ':'});
    }
    public Disk(string disk) {
      _disk = disk;
    }

    public long TotalDiskSpaceMBytes {
      get {
        WMI oWMI = new WMI();
        return oWMI.GetDiskSize(_disk);
      }
    }

    public long FreeDiskSpaceMBytes {
      get {
        WMI oWMI = new WMI();
        return oWMI.GetFreeDiskSpace(_disk);
      }
    }

    public bool IsDiskSpaceLow(long lowLimitMBytes) {
      WMI oWMI = new WMI();
      long _FreeDiskSpaceMBytes = oWMI.GetFreeDiskSpace(_disk);
      if ( (_FreeDiskSpaceMBytes) <= lowLimitMBytes ) {
        Trace.WriteLine(string.Format("Disk space on drive {0} is low : {1} MB", _disk, _FreeDiskSpaceMBytes), Severity.Warning);
        return true;
      } else {
        return false;
      }
    }

  }
}
