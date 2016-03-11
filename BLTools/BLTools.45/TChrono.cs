using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BLTools {
  public class TChrono : IDisposable {

    public DateTime StartTime { get; set; }
    public DateTime StopTime { get; set; }
    public TimeSpan ElapsedTime {
      get {
        return (DateTime.Now - StartTime);
      }
    }
    public TimeSpan ElapsedTimeMemory {
      get {
        if (_ElapsedTimeMemory == TimeSpan.Zero) {
          return ElapsedTime;
        }
        return _ElapsedTimeMemory;
      }
    }
    private TimeSpan _ElapsedTimeMemory;

    public TChrono() {
      StartTime = DateTime.Now;
      Reset();
    }

    public void Start() {
      StartTime = DateTime.Now;
    }
    public void Stop() {
      _ElapsedTimeMemory += ElapsedTime;
    }

    public void Reset() {
      _ElapsedTimeMemory = TimeSpan.Zero;
    }

    public void Dispose() {
    }
  }
}
