using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;

namespace BLTools.Web {
  public class TFtpClient {

    internal const int DEFAULT_MAX_RETRIES = 3;
    internal const int DEFAULT_DELAY_BETWEEN_RETRY = 2000;

    public static int MaxRetries {
      get {
        if (_MaxRetries == -1) {
          return DEFAULT_MAX_RETRIES;
        } else {
          return _MaxRetries;
        }
      }
      set {
        _MaxRetries = value;
      }
    }
    private static int _MaxRetries = -1;

    public static int DelayBetweenRetries {
      get {
        if (_DelayBetweenRetries == -1) {
          return DEFAULT_DELAY_BETWEEN_RETRY;
        } else {
          return _DelayBetweenRetries;
        }
      }
      set {
        _DelayBetweenRetries = value;
      }
    }
    private static int _DelayBetweenRetries = -1;

    public static bool IsDebug = false;

    #region Public properties
    public string HostName { get; set; }
    public int HostPort { get; set; }

    public string UserName { get; set; }
    public string Password { get; set; }

    public EConnectionType ConnectionType { get; set; }
    #endregion Public properties

    #region Constructor(s)
    public TFtpClient() {
      HostName = "";
      HostPort = 21;
      UserName = "";
      Password = "";
      ConnectionType = EConnectionType.Passive;
    }
    public TFtpClient(string hostName, string userName = "anonymous", string password = "")
      : this() {
      HostName = hostName;
      UserName = userName;
      Password = password;
    }
    #endregion Constructor(s)

    /// <summary>
    /// Transfer a file in binary mode to an FTP site, in the current distant folder. Optionally overwrite the file.
    /// </summary>
    /// <param name="localFilename">Name (and path) of the local file name</param>
    /// <param name="overwrite">True to overwrite any existing file with the same name on destination. False otherwise.</param>
    /// <returns>True if success, False otherwise</returns>
    public bool PutFile(string localFilename = "", bool overwrite = false) {
      return PutFile(localFilename, ETransferType.Binary, "", "", overwrite);
    }

    /// <summary>
    /// Transfer a file to an FTP site, in the current distant folder. Optionally specify transfer mode and overwrite the file.
    /// </summary>
    /// <param name="localFilename">Name (and path) of the local file name</param>
    /// <param name="transferType">The type of transfer (binary/text)</param>
    /// <param name="overwrite">True to overwrite any existing file with the same name on destination. False otherwise</param>
    /// <returns>True if success, False otherwise</returns>
    public bool PutFile(string localFilename = "", ETransferType transferType = ETransferType.Binary, bool overwrite = false) {
      return PutFile(localFilename, transferType, "", "", overwrite);
    }

    /// <summary>
    /// Transfer a file to an FTP site, in the current distant folder. Optionally specify transfer mode, distant ftp path and overwrite the file.
    /// </summary>
    /// <param name="localFilename">Name (and path) of the local file name</param>
    /// <param name="transferType">The type of transfer (binary/text)</param>
    /// <param name="ftpPath">The distant ftp path</param>
    /// <param name="overwrite">True to overwrite any existing file with the same name on destination. False otherwise</param>
    /// <returns>True if success, False otherwise</returns>
    public bool PutFile(string localFilename = "", ETransferType transferType = ETransferType.Binary, string ftpPath = "", bool overwrite = false) {
      return PutFile(localFilename, transferType, ftpPath, "", overwrite);
    }

    /// <summary>
    /// Transfer a file to an FTP site, in the current distant folder. Optionally specify transfer mode, distant ftp path, file new name and overwrite the file.
    /// </summary>
    /// <param name="localFilename">Name (and path) of the local file name</param>
    /// <param name="transferType">The type of transfer (binary/text)</param>
    /// <param name="ftpPath">The distant ftp path</param>
    /// <param name="ftpFilename">The new name of the file on the ftp site</param>
    /// <param name="overwrite">True to overwrite any existing file with the same name on destination. False otherwise</param>
    /// <returns>True if success, False otherwise</returns>
    public bool PutFile(string localFilename, ETransferType transferType, string ftpPath, string ftpFilename, bool overwrite) {
      #region Validate parameters
      string LocalSource = localFilename;
      if (string.IsNullOrWhiteSpace(ftpFilename)) {
        ftpFilename = localFilename;
      }
      string FtpDestination = string.Format("{0}/{1}", ftpPath.TrimEnd('/'), Path.GetFileName(ftpFilename));

      if (string.IsNullOrWhiteSpace(LocalSource)) {
        Trace.WriteLine("Unable to put file : filename is null or empty", Severity.Error);
        return false;
      }
      if (!File.Exists(LocalSource)) {
        Trace.WriteLine(string.Format("Unable to put file {0} : file is missing or access denied", LocalSource), Severity.Error);
        return false;
      }
      #endregion Validate parameters

      bool TransferOK = false;
      int Attempts = 0;
      do {
        Attempts++;
        if (Attempts > 1) {
          Thread.Sleep(DelayBetweenRetries);
        }
        Trace.WriteLineIf(IsDebug, string.Format("Transfer of {0} to {1}, attempt {2}", LocalSource, FtpDestination, Attempts));

        if (overwrite && FileExist(ftpPath, ftpFilename)) {
          Trace.WriteLineIf(IsDebug, "File already exists, delete it because overwrite is on");
          DeleteFile(ftpPath, ftpFilename);
        }

        FtpWebRequest Client = _FtpConnect(FtpDestination);
        Client.UseBinary = (transferType == ETransferType.Binary);
        Client.Method = WebRequestMethods.Ftp.UploadFile;

        #region Transfer
        if (transferType == ETransferType.Binary) {
          try {
            using (Stream UploadStream = Client.GetRequestStream()) {
              using (BinaryWriter UploadWriter = new BinaryWriter(UploadStream)) {
                using (Stream LocalStream = new FileStream(LocalSource, FileMode.Open)) {
                  using (BinaryReader LocalReader = new BinaryReader(LocalStream)) {

                    byte[] LocalContent = LocalReader.ReadBytes((int)LocalStream.Length);
                    UploadWriter.Write(LocalContent);

                  }
                }
                UploadStream.Flush();
                UploadStream.Close();
              }
              UploadStream.Flush();
              UploadStream.Close();
            }
            FtpWebResponse Response = Client.GetResponse() as FtpWebResponse;
            Trace.WriteLineIf(IsDebug, string.Format("{0} - {1}", Response.StatusCode, Response.StatusDescription));
            TransferOK = true;
          } catch (Exception ex) {
            Trace.WriteLine(string.Format("Error while transferring file {0} to {1} : {2}", LocalSource, FtpDestination, ex.Message), Severity.Error);
          }
        } else {
          try {
            using (Stream UploadStream = Client.GetRequestStream()) {
              using (TextWriter UploadWriter = new StreamWriter(UploadStream)) {
                string FileContent = File.ReadAllText(LocalSource);
                UploadWriter.Write(FileContent);
                UploadWriter.Flush();
                UploadWriter.Close();
              }
              UploadStream.Flush();
              UploadStream.Close();
            }
            FtpWebResponse Response = Client.GetResponse() as FtpWebResponse;
            Trace.WriteLineIf(IsDebug, string.Format("{0} - {1}", Response.StatusCode, Response.StatusDescription));
            TransferOK = true;
          } catch (Exception ex) {
            Trace.WriteLine(string.Format("Error while transferring file {0} to {1} : {2}", LocalSource, FtpDestination, ex.Message), Severity.Error);
          }
        }
        #endregion Transfer

      } while (!TransferOK && Attempts < MaxRetries);


      if (!TransferOK) {
        Trace.WriteLine("Aborting transfer, too many unsuccessful attempts", Severity.Error);
        return false;
      } else {
        return true;
      }


    }

    public bool GetFile(string ftpPath = "", string ftpFilename = "", ETransferType transferType = ETransferType.Binary, string localPath = ".", string localFilename = "") {
      #region Validate parameters
      if (ftpFilename == "") {
        Trace.WriteLine("Unable to get a file : filename is missing", Severity.Error);
        return false;
      }
      if (localFilename == "") {
        localFilename = ftpFilename;
      }
      #endregion Validate parameters

      string FtpSource = string.Format("{0}/{1}", ftpPath.TrimEnd('/'), ftpFilename.TrimStart('/'));
      string LocalDestination = Path.Combine(localPath, localFilename);

      string FileOnServer = List(ftpPath).Where(x => x == ftpFilename).FirstOrDefault();
      if (FileOnServer == null) {
        Trace.WriteLine(string.Format("Error : File to get in missing on FTP server : {0}", FtpSource), Severity.Error);
        return false;
      }

      FtpWebRequest Client;
      FtpWebResponse Response;

      int Attempt = 0;
      bool TransferOk = false;
      do {
        Attempt++;
        if (Attempt > 1) {
          Thread.Sleep(DelayBetweenRetries);
        }

        Trace.WriteLineIf(IsDebug, string.Format("Transfer of {0} to {1}, attempt {2}", FtpSource, LocalDestination, Attempt));

        Client = _FtpConnect(FtpSource);
        Client.UseBinary = transferType == ETransferType.Binary;
        Client.Method = WebRequestMethods.Ftp.DownloadFile;

        Response = Client.GetResponse() as FtpWebResponse;
        if (Response == null) {
          Trace.WriteLine("Error while getting file", Severity.Error);
          if (Attempt < MaxRetries) {
            continue;
          } else {
            break;
          }
        }

        if (transferType == ETransferType.Binary) {
          try {
            using (Stream ResponseStream = Response.GetResponseStream()) {
              using (Stream LocalStream = new FileStream(LocalDestination, FileMode.Create)) {

                byte[] FtpBuffer = new byte[1024];
                List<byte> FtpFileContent = new List<byte>();
                int BytesCount = 0;
                do {
                  BytesCount = ResponseStream.Read(FtpBuffer, 0, FtpBuffer.Length);
                  if (BytesCount > 0) {
                    FtpFileContent.AddRange(FtpBuffer.Take(BytesCount));
                  }
                } while (BytesCount > 0);

                LocalStream.Write(FtpFileContent.ToArray(), 0, FtpFileContent.Count());
                TransferOk = true;
              }
            }
          } catch (Exception ex) {
            Trace.WriteLine(string.Format("Error while transferring file {0} to {1} : {2}", FtpSource, LocalDestination, ex.Message), Severity.Error);
            if (Attempt < MaxRetries) {
              continue;
            } else {
              break;
            }
          }
        } else {
          try {
            using (Stream ResponseStream = Response.GetResponseStream()) {
              using (TextReader ResponseStreamReader = new StreamReader(ResponseStream)) {
                string FileContent = ResponseStreamReader.ReadToEnd();
                File.WriteAllText(LocalDestination, FileContent);
                TransferOk = true;
              }
            }
          } catch (Exception ex) {
            Trace.WriteLine(string.Format("Error while transferring file {0} to {1} : {2}", FtpSource, LocalDestination, ex.Message), Severity.Error);
            if (Attempt < MaxRetries) {
              continue;
            } else {
              break;
            }
          }
        }
        Trace.WriteLineIf(IsDebug, string.Format("{0} - {1}", Response.StatusCode, Response.StatusDescription));
      } while (Attempt < MaxRetries && !TransferOk);

      return TransferOk;

    }
    public bool MoveFileFromFtp(string ftpPath = "", string ftpFilename = "", ETransferType transferType = ETransferType.Binary, string localPath = ".", string localFilename = "") {
      string LocalDestination = Path.Combine(localPath, localFilename);

      if (!GetFile(ftpPath, ftpFilename, transferType, localPath, localFilename)) {
        Trace.WriteLine("Error : Incorrect file transfer : file is missing or invalid", Severity.Error);
        if (File.Exists(LocalDestination)) {
          Trace.WriteLineIf(IsDebug, string.Format("Cleanup of the local file"));
          File.Delete(LocalDestination);
        }
        return false;
      }

      if (File.Exists(LocalDestination)) { 
        return DeleteFile(ftpPath, ftpFilename);
      } else {
        return false;
      }

    }

    public bool DeleteFile(string ftpPath = "", string ftpFilename = "") {
      #region Validate parameters
      if (ftpFilename == "") {
        Trace.WriteLine("Unable to delete a file : filename is missing", Severity.Error);
        return false;
      }
      #endregion Validate parameters

      string FtpFile = string.Format("{0}/{1}", ftpPath, ftpFilename);
      Trace.WriteLineIf(IsDebug, string.Format("Delete of {0}", FtpFile));

      FtpWebRequest Client = _FtpConnect(FtpFile);
      Client.Method = WebRequestMethods.Ftp.DeleteFile;

      FtpWebResponse Response = Client.GetResponse() as FtpWebResponse;
      if (Response == null) {
        Trace.WriteLine("Error while deleting file", Severity.Error);
        return false;
      }

      Trace.WriteLine(string.Format("{0} - {1}", Response.StatusCode, Response.StatusDescription));
      return true;

    }

    public IEnumerable<TFtpFile> ListDetails(string folder) {

      FtpWebRequest Client = _FtpConnect(folder);
      Client.Method = WebRequestMethods.Ftp.ListDirectoryDetails;

      FtpWebResponse Response = Client.GetResponse() as FtpWebResponse;
      if (Response == null) {
        yield break;
      }
      using (Stream ResponseStream = Response.GetResponseStream()) {
        using (TextReader ResponseStreamReader = new StreamReader(ResponseStream)) {
          string DirectoryLine = null;
          do {
            DirectoryLine = ResponseStreamReader.ReadLine();
            if (DirectoryLine != null) {
              TFtpFile TempFile = new TFtpFile(DirectoryLine);
              if (TempFile.Name != "." && TempFile.Name != "..") {
                yield return TempFile;
              }
            }
          } while (DirectoryLine != null) ;
        }
      }
      yield break;
    }

    public IEnumerable<string> List(string folder = "") {

      Trace.WriteLineIf(IsDebug, string.Format("List content of \"{0}\"", folder));
      FtpWebRequest Client = _FtpConnect(folder);
      Client.Method = WebRequestMethods.Ftp.ListDirectory;
      Client.UseBinary = true;

      FtpWebResponse Response = Client.GetResponse() as FtpWebResponse;
      if (Response == null) {
        yield break;
      }
      try {
        if (IsDebug) {
          Trace.Indent();
        }
        using (Stream ResponseStream = Response.GetResponseStream()) {
          using (TextReader ResponseStreamReader = new StreamReader(ResponseStream)) {
            string DirectoryLine = null;
            do { 
              try { 
                DirectoryLine = ResponseStreamReader.ReadLine(); 
              } catch (Exception Exception) {
                Trace.WriteLineIf(IsDebug, string.Format("{0}", Exception.Message), Severity.Error);
              }
              if (DirectoryLine != null) {
                string RetVal = DirectoryLine.Split('/').LastOrDefault();
                if (RetVal != null) {
                  Trace.WriteLineIf(IsDebug, RetVal);
                  yield return RetVal;
                }
              }
            } while (DirectoryLine != null) ;
          }
        }
      } finally {
        if (IsDebug) {
          Trace.Unindent();
        }
      }
      yield break;
    }

    public bool FileExist(string ftpPath, string ftpFilename) {

      if (ftpPath == null) {
        ftpPath = "";
      }

      StringBuilder TraceLine = new StringBuilder(string.Format("Verifying if file {0} exists at {1} ... ", ftpFilename, ftpPath));
      try {
        foreach (string FilenameItem in List(ftpPath)) {
          if (FilenameItem.ToLower().EndsWith(ftpFilename.ToLower())) {
            TraceLine.Append("Yes");
            return true;
          }
        }
        TraceLine.Append("No");
        return false;
      } finally {
        Trace.WriteLineIf(IsDebug, TraceLine.ToString());
      }


    }

    #region Private methods
    private FtpWebRequest _FtpConnect(string folder = "") {
      FtpWebRequest RetVal;
      if (folder == "") {
        RetVal = FtpWebRequest.Create(string.Format("ftp://{0}", HostName)) as FtpWebRequest;
      } else {
        RetVal = FtpWebRequest.Create(string.Format("ftp://{0}/{1}", HostName, folder)) as FtpWebRequest;
      }
      RetVal.Credentials = new NetworkCredential(UserName, Password);
      RetVal.AuthenticationLevel = System.Net.Security.AuthenticationLevel.MutualAuthRequested;
      RetVal.UsePassive = (ConnectionType == EConnectionType.Passive);

      return RetVal;
    }

    private object _Execute(string command, params string[] arguments) {
      return null;
    }
    #endregion Private methods

  }
}
