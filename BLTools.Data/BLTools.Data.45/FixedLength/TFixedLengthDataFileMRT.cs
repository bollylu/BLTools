using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Diagnostics;
using System.Reflection;

namespace BLTools.Data.FixedLength {

  /// <summary>
  /// Describes a file with fixed length records
  /// </summary>
  public abstract class TFixedLengthDataFileMRT : List<TFixedLengthRecord>, IDisposable {

    #region Public properties
    /// <summary>
    /// Full name of the data file
    /// </summary>
    public string Name { get; set; }
    /// <summary>
    /// Get the record length
    /// </summary>
    public int RecLen {
      get {
        if (RecordType == null) {
          return 1024;
        }

        if (RecordType.BaseType != typeof(TFixedLengthRecord)) {
          Trace.WriteLine("Record is not a TFixedLengthRecord", Severity.Error);
          return 1024;
        }

        object Instance = Activator.CreateInstance(RecordType);

        return (int)RecordType.GetProperty("RecLen").GetGetMethod().Invoke(Instance, null);
      }
    }
    /// <summary>
    /// The type of records in the data file
    /// </summary>
    public Type RecordType { get; set; }
    /// <summary>
    /// The encoding used to read/write the data
    /// </summary>
    public Encoding DataEncoding { get; set; }
    #endregion Public properties

    #region Constructor(s)
    public TFixedLengthDataFileMRT(string name)
      : base() {
      Name = name;
      RecordType = typeof(string);
      DataEncoding = Encoding.Default;
    }

    /// <summary>
    /// Flush, close then cleanup
    /// </summary>
    public void Dispose() {
      try {
        if (_IsOpened) {
          Close();
        }
      } catch { }
      if (_StreamReader != null) {
        _StreamReader.Dispose();
      }
      if (_StreamWriter != null) {
        _StreamWriter.Dispose();
      }
      if (_BufferedStream != null) {
        _BufferedStream.Dispose();
      }
      if (_Stream != null) {
        _Stream.Dispose();
      }
    }
    #endregion Constructor(s)

    /// <summary>
    /// Basic string representation of the data file
    /// </summary>
    /// <returns>A descrption of the data file</returns>
    public override string ToString() {
      StringBuilder RetVal = new StringBuilder();
      RetVal.AppendFormat("{0} : {1} record(s)", Name, this.Count());
      return RetVal.ToString();
    }

    #region Private variables
    protected FileStream _Stream;
    protected BufferedStream _BufferedStream;
    protected BinaryReader _StreamReader;
    protected BinaryWriter _StreamWriter;
    private bool _IsOpened;
    private EOpenMode _FileOpenMode;
    #endregion Private variables

    /// <summary>
    /// Delete the data file if it exists
    /// </summary>
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

    /// <summary>
    /// Open the file for future use
    /// </summary>
    /// <param name="mode">Specify the usage of the file</param>
    public void Open(EOpenMode mode) {
      if (_IsOpened) {
        return;
      }
      try {
        switch (mode) {
          case EOpenMode.Read:
            _Stream = new FileStream(Name, FileMode.Open, FileAccess.Read, FileShare.Read, RecLen);
            _BufferedStream = new BufferedStream(_Stream, RecLen * 10);
            _StreamReader = new BinaryReader(_BufferedStream, DataEncoding);
            _FileOpenMode = EOpenMode.Read;
            break;
          case EOpenMode.Create:
            _Stream = new FileStream(Name, FileMode.OpenOrCreate, FileAccess.Write, FileShare.ReadWrite, RecLen);
            _BufferedStream = new BufferedStream(_Stream);
            _StreamWriter = new BinaryWriter(_BufferedStream, DataEncoding);
            _FileOpenMode = EOpenMode.Create;
            break;
          case EOpenMode.Append:
            _Stream = new FileStream(Name, FileMode.Append, FileAccess.Write, FileShare.ReadWrite, RecLen);
            _BufferedStream = new BufferedStream(_Stream);
            _StreamWriter = new BinaryWriter(_BufferedStream, DataEncoding);
            _FileOpenMode = EOpenMode.Append;
            break;
        }
        _IsOpened = true;
      } catch (Exception ex) {
        Trace.WriteLine(string.Format("Error opening file : {0}", ex.Message));
        _IsOpened = false;
      }
    }

    /// <summary>
    /// Close the file (while flushing data when needed)
    /// </summary>
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
              _StreamWriter.Flush();
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

    //public virtual TFixedLengthRecord Select() {
    //  return new TFixedLengthRecord();
    //}

    //protected byte[] _read() {
    //  if (!_IsOpened) {
    //    throw new ApplicationException("Error: you must open the file before reading data");
    //  }
    //  try {
    //    if (_StreamReader.BaseStream.Position != _StreamReader.BaseStream.Length) {
    //      return _StreamReader.ReadBytes(RecLen);
    //    } else {
    //      return null;
    //    }
    //  } catch (Exception ex) {
    //    Trace.WriteLine(string.Format("Error reading a record from {0} : {1}", Name, ex.Message));
    //    return null;
    //  }
    //}

    /// <summary>
    /// Add a record at the end of the data file
    /// </summary>
    /// <param name="data">The record to add</param>
    public virtual void Append(TFixedLengthRecord data) {
      if (data == null) {
        Trace.WriteLine("Error: record to write is null");
        return;
      }
      //if (data.RecLen != RecLen) {
      //  Trace.WriteLine("Error while appending data to record : record length does not match");
      //  return;
      //}
      if (!_IsOpened) {
        Trace.WriteLine("Error : trying to write to file while it is closed");
        return;
      }
      this.Add(data);
      _StreamWriter.Seek(0, SeekOrigin.End);
      _StreamWriter.Write(data.ToRawData(DataEncoding));
    }

    /// <summary>
    /// Save the in memory content to data file
    /// </summary>
    public virtual void Save(bool appendMode = false) {
      #region Validate parameters
      if (string.IsNullOrWhiteSpace(Name)) {
        Trace.WriteLine("Error : Unable to save file : filename is missing");
        return;
      }
      #endregion Validate parameters
      try {
        if (appendMode) {
          Open(EOpenMode.Append);
        } else {
          Init();
          Open(EOpenMode.Create);
        }
        foreach (IFixedLengthRecord RecordItem in this) {
          _StreamWriter.Write(RecordItem.ToRawData(DataEncoding));
        }
      } catch (Exception ex) {
        Trace.WriteLine(string.Format("Error while saving data to file {0} : {1}", Name, ex.Message));
      } finally {
        Close();
      }
    }

    /// <summary>
    /// Read the data file content into memory
    /// </summary>
    public virtual void Read() {
      #region Validate parameters
      if (string.IsNullOrWhiteSpace(Name)) {
        Trace.WriteLine("Error : Unable to read file : filename is missing");
        return;
      }
      if (!File.Exists(Name)) {
        Trace.WriteLine("Error : Unable to read file : the file is missing or access is denied");
        return;
      }
      #endregion Validate parameters
      try {
        Open(EOpenMode.Read);
        byte[] Record;
        do {
          Record = _StreamReader.ReadBytes(RecLen);
          if (Record.Length == RecLen) {
            TFixedLengthRecord NewRecord = RecordType.GetConstructor(Type.EmptyTypes).Invoke(null) as TFixedLengthRecord;
            if (NewRecord != null) {
              NewRecord.FromRawData(Record);
              Add(NewRecord);
            }
          }
        } while (Record.Length == RecLen);
      } catch (Exception ex) {
        Trace.WriteLine(string.Format("Error while reading data from file {0} : {1}", Name, ex.Message));
      } finally {
        Close();
      }
    }

  }

}
