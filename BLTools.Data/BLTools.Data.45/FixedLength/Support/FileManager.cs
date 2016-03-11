using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace StockRic {
  public class DataFileFixedLength {
    #region Public properties
    public string Name { get; set; }
    public int RecLen { get; private set; }
    public FileSchema Schema { get; set; }
    #endregion Public properties

    #region Constructor(s)
    public DataFileFixedLength( string name, int recLen) : this (name, recLen, null) {
    }
    public DataFileFixedLength( string name, int recLen, FileSchema schema ) {
      Name = name;
      RecLen = recLen;
      Schema = schema;
    }
    #endregion Constructor(s)

    #region Private variables
    private FileStream _InputStream;
    private StreamReader _InputStreamReader;
    private BufferedStream _InputBufferedStream;
    private FileStream _OutputStream;
    private StreamWriter _OutputStreamWriter;
    private BufferedStream _OutputBufferedStream;
    private bool _IsOpened;
    private OpenMode _FileOpenMode;
    #endregion Private variables

    #region Enums
    public enum OpenMode { Read, Create }; 
    #endregion Enums

    public void Open(OpenMode mode) {
      switch (mode) {
        case OpenMode.Read:
          _InputStream = new FileStream(Name, FileMode.Open, FileAccess.Read, FileShare.Read, RecLen);
          _InputBufferedStream = new BufferedStream(_InputStream, RecLen * 10);
          _InputStreamReader = new StreamReader(_InputBufferedStream);
          _FileOpenMode = OpenMode.Read;
          break;
        case OpenMode.Create:
          _OutputStream = new FileStream(Name, FileMode.Create, FileAccess.Write, FileShare.Read);
          _OutputBufferedStream = new BufferedStream(_OutputStream);
          _OutputStreamWriter = new StreamWriter(_OutputBufferedStream);
          _FileOpenMode = OpenMode.Create;
          break;
      }
      _IsOpened = true;
    }

    public void Close() {
      switch (_FileOpenMode) {
        case OpenMode.Read:
          if (_InputStreamReader != null) {
            _InputStreamReader.Close();
          }
          if (_InputBufferedStream != null) {
            _InputBufferedStream.Close();
          }
          if (_InputStream != null) {
            _InputStream.Close();
          }
          break;
        case OpenMode.Create:
          if (_OutputStreamWriter != null) {
            _OutputStreamWriter.Close();
          }
          if (_OutputBufferedStream != null) {
            _OutputBufferedStream.Close();
          }
          if (_OutputStream != null) {
            _OutputStream.Close();
          }
          break;
      }
      _IsOpened = false;
    }

    //public abstract void FillSchema();

    public void CopyTo(string OutputFile) {
    }

    public void Read() {
      if ( !_IsOpened ) {
        throw new ApplicationException("Error: you must open the file before reading data");
      }
    }
    public void Read(int recordNumber) {
    }

  }
}
