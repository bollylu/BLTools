using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Windows.Forms;


namespace StockRic {
  public class Export {
    public string SourceFile { get; set; }
    public string DestinationFile { get; set; }

    private FileStream _InputStream;
    private StreamReader _InputStreamReader;
    private BufferedStream _InputBufferedStream;
    private FileStream _OutputStream;
    private StreamWriter _OutputStreamWriter;
    private BufferedStream _OutputBufferedStream;

    public Export( string sourceFile, string destinationFile ) {
      SourceFile = sourceFile;
      DestinationFile = destinationFile;
    }

    public bool Execute() {
      return Execute(null);
    }
    public bool Execute(ProgressBar progressInfo) {

      try {
        _InputStream = new FileStream(SourceFile, FileMode.Open, FileAccess.Read, FileShare.Read, 701);
        _InputBufferedStream = new BufferedStream(_InputStream, 7010);
        _InputStreamReader = new StreamReader(_InputBufferedStream);

        _OutputStream = new FileStream(DestinationFile, FileMode.Create, FileAccess.Write, FileShare.Read);
        _OutputBufferedStream = new BufferedStream(_OutputStream);
        _OutputStreamWriter = new StreamWriter(_OutputBufferedStream);

        string BufferInput;

        while ( !_InputStreamReader.EndOfStream ) {

          BufferInput = _InputStreamReader.ReadLine();
          StringBuilder BufferOutput = new StringBuilder(200);
          // découpage buffer
          BufferOutput.Append(Filler(2));
          BufferOutput.Append(BufferInput.Substring(3, 10));
          BufferOutput.Append(BufferInput.Substring(71, 20));
          BufferOutput.Append(BufferInput.Substring(91, 20));
          BufferOutput.Append(Filler(6));
          BufferOutput.Append(BufferInput.Substring(175, 1));
          BufferOutput.Append(BufferInput.Substring(173, 1));
          BufferOutput.Append(Filler(1));
          BufferOutput.Append(BufferInput.Substring(16, 1));
          BufferOutput.Append(BufferInput.Substring(169, 4));
          BufferOutput.Append(Filler(6));
          BufferOutput.Append(BufferInput.Substring(13, 3));
          BufferOutput.Append(Filler(5));
          BufferOutput.Append(Filler(1, '0'));
          BufferOutput.Append(BufferInput.Substring(17, 9));
          BufferOutput.Append(Filler(1, '0'));
          BufferOutput.Append(BufferInput.Substring(49, 9));
          BufferOutput.Append(BufferInput.Substring(201, 10));
          BufferOutput.Append(BufferInput.Substring(211, 10));
          BufferOutput.Append(Filler(80));

          //ecriture dans output
          _OutputStreamWriter.Write(BufferOutput);
          _OutputStreamWriter.WriteLine();

          if ( progressInfo != null ) {
            progressInfo.PerformStep();
          }

        }
        return true;

      } catch {
        return false;

      } finally {
        if ( _InputStreamReader != null ) {
          _InputStreamReader.Close();
        }
        if ( _InputBufferedStream != null ) {
          _InputBufferedStream.Close();
        }
        if ( _InputStream != null ) {
          _InputStream.Close();
        }

        if (_OutputStreamWriter != null) {
          _OutputStreamWriter.Close();
        }
        if (_OutputBufferedStream != null) {
          _OutputBufferedStream.Close();
        }
        if (_OutputStream != null) {
          _OutputStream.Close();
        }
      }
    }

    private static string Filler( int length ) {
      return Filler(length, ' ');
    }
    private static string Filler( int length, char filler ) {
      return new string(filler, length);
    }
  }


}
