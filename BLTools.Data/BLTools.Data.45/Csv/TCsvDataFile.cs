using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BLTools.Data;

namespace BLTools.Data.Csv {
  public class TCsvDataFile : List<TCsvRecord>, IDisposable {

    #region Public properties
    public string Name { get; set; }
    public Type RecordType { get; set; }
    public bool HasHeaders { get; set; }
    public Encoding DataEncoding { get; set; }
    #endregion Public properties

    #region Private variables
    private FileStream _Stream;
    private BufferedStream _BufferedStream;
    private TextReader _StreamReader;
    private TextWriter _StreamWriter;
    private bool _IsOpened;
    private EOpenMode _FileOpenMode;
    #endregion Private variables

    #region Constructor(s)
    public TCsvDataFile()
      : base() {
      Name = "";
      RecordType = null;
      HasHeaders = true;
      DataEncoding = Encoding.Default;
    }

    public TCsvDataFile(string name)
      : this() {
      Name = name;
    }
    public TCsvDataFile(string name, Encoding encoding)
      : this(name) {
      DataEncoding = encoding;
    }

    public TCsvDataFile(TCsvDataFile csvDataFile)
      : this() {
      Name = csvDataFile.Name;
    }

    public void Dispose() {
      if (_IsOpened) {
        Close();
      }
    }
    #endregion Constructor(s)

    public void Init() {
      if (string.IsNullOrWhiteSpace(Name)) {
        return;
      }
      if (File.Exists(Name)) {
        try {
          File.Delete(Name);
        } catch (Exception ex) {
          Trace.WriteLine(string.Format("Unable to delete file \"{0}\" : {1}", Name, ex.Message));
        }
      }
    }

    public void Open(EOpenMode mode) {
      if (_IsOpened) {
        return;
      }
      try {
        switch (mode) {
          case EOpenMode.Read:
            _Stream = new FileStream(Name, FileMode.Open, FileAccess.Read, FileShare.Read);
            _BufferedStream = new BufferedStream(_Stream);
            _StreamReader = new StreamReader(_BufferedStream, DataEncoding);
            _FileOpenMode = EOpenMode.Read;
            break;
          case EOpenMode.Create:
            _Stream = new FileStream(Name, FileMode.Create, FileAccess.Write, FileShare.ReadWrite);
            _BufferedStream = new BufferedStream(_Stream);
            _StreamWriter = new StreamWriter(_BufferedStream, DataEncoding);
            _FileOpenMode = EOpenMode.Create;
            break;
        }
        _IsOpened = true;
      } catch (Exception ex) {
        Trace.WriteLine(string.Format("Error opening file : {0}", ex.Message));
        _IsOpened = false;
      }
    }

    public void Close() {
      if (!_IsOpened) {
        return;
      }
      if (_IsOpened) {
        switch (_FileOpenMode) {
          case EOpenMode.Read:
            if (_StreamReader != null) {
              _StreamReader.Close();
            }

            break;
          case EOpenMode.Create:
            if (_StreamWriter != null) {
              _StreamWriter.Close();
            }
            break;
        }
        if (_BufferedStream != null) {
          _BufferedStream.Close();
        }
        if (_Stream != null) {
          _Stream.Close();
        }
        _IsOpened = false;
      }
    }

    public virtual void AppendRecord(TCsvRecord data) {
      if (data == null) {
        Trace.WriteLine("Error: record to write is null");
        return;
      }
      if (data.GetType().Name != this.RecordType.Name) {
        Trace.WriteLine("Error while appending record to file : record type does not match");
        return;
      }
      if (!_IsOpened) {
        Trace.WriteLine("Error : trying to write to file while it is closed");
        return;
      }
      _StreamWriter.WriteLine(data.ToCsv());
    }

    public virtual void Save() {
      #region Validate parameters
      if (string.IsNullOrWhiteSpace(Name)) {
        Trace.WriteLine("Error : Unable to save file : filename is missing");
        return;
      }
      #endregion Validate parameters
      try {
        Open(EOpenMode.Create);
        if (HasHeaders && this.Count > 0) {
          _StreamWriter.WriteLine(this.First().ToCsvHeader());
        }
        foreach (TCsvRecord RecordItem in this) {
          _StreamWriter.WriteLine(RecordItem.ToCsv());
        }
      } catch (Exception ex) {
        Trace.WriteLine(string.Format("Error while saving data to file {0} : {1}", Name, ex.Message));
      } finally {
        Close();
      }
    }

    public virtual void Read<T>() where T : TCsvRecord, new() {
      #region Validate parameters
      if (string.IsNullOrWhiteSpace(Name)) {
        Trace.WriteLine("Unable to read file : filename is missing", Severity.Error);
        return;
      }
      #endregion Validate parameters
      try {
        Open(EOpenMode.Read);
        if (HasHeaders) {
          _StreamReader.ReadLine();
        }
        for (string CsvRow = _StreamReader.ReadLine(); CsvRow != null; CsvRow = _StreamReader.ReadLine()) {
          T NewRecord = new T();
          Add(NewRecord.FromCsv(CsvRow));
        }
      } catch (Exception ex) {
        Trace.WriteLine(string.Format("Error while reading data from file {0} : {1}", Name, ex.Message));
      } finally {
        Close();
      }
    }
  }
}
