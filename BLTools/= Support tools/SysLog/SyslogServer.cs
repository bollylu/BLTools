using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Collections;
using System.Diagnostics;
using BLTools;

namespace SysLog {
  public class SyslogServer {

    private class _UdpState {
      public UdpClient Client { get; set; }
      public IPEndPoint EndPoint { get; set; }
      public _UdpState( UdpClient client, IPEndPoint endPoint ) {
        Client = client;
        EndPoint = endPoint;
      }
    }

    #region Private variables
    private Thread _ThreadAsyncListen;
    private bool _StopListen;
    private ManualResetEvent _WaitingForData;
    private int _PortNumber;
    private UdpClient _UdpClient;
    private IPEndPoint _RemoteEndPoint;

    private object _LockSync = new object();
    private AsyncCallback _GotDataCallback;
    private _UdpState _UdpReceptionState;
    #endregion Private variables

    #region Public properties
    public string Filename { get; set; }
    public Queue<SyslogRecord> Items { get; set; }
    public int Port { get; set; }
    public SyslogSourceCollection SyslogSources { get; set; }
    #endregion  Public properties

    #region Constructor(s)
    public SyslogServer()
      : this(514, new SyslogSourceCollection()) {
    }
    public SyslogServer( SyslogSourceCollection syslogSources )
      : this(514, syslogSources) {
    }
    public SyslogServer( int portNumber, SyslogSourceCollection syslogSources ) {
      _PortNumber = portNumber;
      Items = new Queue<SyslogRecord>(64);
      SyslogSources = syslogSources;
      _WaitingForData = new ManualResetEvent(true);
    }
    #endregion Constructor(s)

    public event EventHandler DataReceived;

    #region Public methods
    public void Listen() {
      ListenMethod();
    }
    public void ListenAsync() {
      _GotDataCallback = new AsyncCallback(GotData);
      _StopListen = false;
      _WaitingForData = new ManualResetEvent(true);
      _WaitingForData.Reset();
      _UdpClient = new UdpClient(_PortNumber);
      _UdpClient.Client.ReceiveBufferSize = 1500;
      _ThreadAsyncListen = new Thread(new ThreadStart(ListenMethod));
      _ThreadAsyncListen.Name = "Listener thread";
      _ThreadAsyncListen.Start();

    }
    public void AbortListen() {
      _StopListen = true;
      if ( _ThreadAsyncListen != null && _ThreadAsyncListen.IsAlive ) {
        _ThreadAsyncListen.Abort();
        _ThreadAsyncListen.Join();
      }
      if ( _UdpClient != null ) {
        _UdpClient.Close();
      }
    }
    public SyslogRecord GetData() {
      SyslogRecord RetVal = null;
      lock ( _LockSync ) {
        if ( Items.Count > 0 ) {
          RetVal = Items.Dequeue();
        }
      }
      return RetVal;
    }
    #endregion Public methods

    private void ListenMethod() {
      while ( !_StopListen ) {
        try {
          //Trace.WriteLine("Looping in listenmethod");
          _UdpReceptionState = new _UdpState(_UdpClient, _RemoteEndPoint);
          IAsyncResult StartReceive = _UdpClient.BeginReceive(_GotDataCallback, _UdpReceptionState);
          //Trace.WriteLine("Wait for data being read...");
          _WaitingForData.Reset();
          _WaitingForData.WaitOne();
        } catch ( ThreadAbortException ) {
          Trace.WriteLine("Thread execution ended.");
          _StopListen = true;
        } catch ( Exception ex ) {
          Trace.WriteLine(string.Format("Exception during ListenMethod : {0}", ex.Message));
        }
      }
      Trace.WriteLine("Leaving listenmethod");
      _UdpClient.Close();
    }

    private void GotData( IAsyncResult result ) {
      try {
        //Trace.WriteLine("Entering GotData");
        UdpClient CurrentUdpClient = ((_UdpState)result.AsyncState).Client as UdpClient;
        string StringReceivedData = Encoding.ASCII.GetString(CurrentUdpClient.EndReceive(result, ref _RemoteEndPoint));

        Trace.WriteLine(StringReceivedData);
        SyslogSource Source = SyslogSources.Find(s => s.Client.Address.ToString() == _RemoteEndPoint.Address.ToString());
        SyslogRecord.SyslogType TypeOfLog;
        if ( Source != null ) {
          TypeOfLog = Source.ClientType;
        } else {
          TypeOfLog = SyslogRecord.SyslogType.RFC3164;
        }
        SyslogRecord NewRecord = new SyslogRecord(StringReceivedData, _RemoteEndPoint.Address, TypeOfLog);
        lock ( _LockSync ) {
          Items.Enqueue(NewRecord);
        }
        
      } catch ( Exception ex ) {
        Trace.WriteLine(string.Format("Exception during GotData : {0}", ex.Message));
      }
      //Trace.WriteLine("Authorizing next read");
      _WaitingForData.Set();
      if ( DataReceived != null ) {
        DataReceived.Invoke(this, EventArgs.Empty);
      }
      //Trace.WriteLine("Leaving gotdata");
    }
  }
}
