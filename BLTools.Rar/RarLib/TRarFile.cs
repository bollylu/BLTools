using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Linq;
using System.Text.RegularExpressions;

namespace RarLib {

  public class TRarFile {

    #region Public properties
    public string Name { get; private set; }
    public string Pathname { get; private set; }
    public string FullName { get { return Path.Combine(Pathname ?? "", Name ?? ""); } }
    public List<TRarElement> Files { get; set; }
    public string LastResult { get; private set; }
    public int ExitCode { get; private set; }
    public AutoResetEvent JobDone { get; private set; }
    #endregion Public properties

    #region Constructors
    public TRarFile() {
      Name = "";
      Pathname = "";
      Files = new List<TRarElement>();
      LastResult = "";
      ExitCode = -1;
    }

    public TRarFile(string fullname, AutoResetEvent isJobDone = null)
      : this() {
      Name = Path.GetFileName(fullname);
      Pathname = Path.GetDirectoryName(fullname);
      if (File.Exists(this.FullName)) {
        _loadContent();
      }
      JobDone = isJobDone;
    }
    #endregion Constructors

    #region Converters
    public override string ToString() {
      StringBuilder RetVal = new StringBuilder();
      foreach (TRarElement RarElementItem in Files) {
        RetVal.AppendLine(RarElementItem.ToString());
      }
      return RetVal.ToString();
    }
    #endregion Converters

    #region Public methods
    #region Add files and folders
    public void AddFile(string filename, TRarIncludePath includePath = TRarIncludePath.RelativePath, TRarCompressionLevel compressionLevel = TRarCompressionLevel.Normal) {
      #region Validate parameters
      if (!File.Exists(filename)) {
        string ErrorMessage = string.Format("Unable to add file {0} to archive {1} : Missing file or access denied", filename, Name);
        Trace.WriteLine(ErrorMessage);
        throw new ApplicationException(ErrorMessage);
      }
      #endregion Validate parameters

      Trace.WriteLine(string.Format("Adding file {0} to archive {1}", filename, this.FullName));
      TRarCommand RarCommand = new TRarCommand(TRarAction.AddFile, FullName, filename);
      RarCommand.CompressionLevel = compressionLevel;
      RarCommand.IncludePath = includePath;
      using (TRarProcess RarProcess = TRarProcess.Exec(RarCommand)) {
        ExitCode = RarProcess.ExitCode;
        LastResult = RarProcess.Output;
      }
      _loadContent();
    }

    public void AddFileAsync(string filename, TRarIncludePath includePath = TRarIncludePath.RelativePath, TRarCompressionLevel compressionLevel = TRarCompressionLevel.Normal) {
      #region Validate parameters
      if (!File.Exists(filename)) {
        string ErrorMessage = string.Format("Unable to add file {0} to archive {1} : Missing file or access denied", filename, Name);
        Trace.WriteLine(ErrorMessage);
        throw new ApplicationException(ErrorMessage);
      }
      #endregion Validate parameters

      Trace.WriteLine(string.Format("Adding async file {0} to archive {1}", filename, this.FullName));
      TRarCommand RarCommand = new TRarCommand(TRarAction.AddFile, FullName, filename);
      RarCommand.CompressionLevel = compressionLevel;
      RarCommand.IncludePath = includePath;
      TRarProcess RarProcess = new TRarProcess(RarCommand);
      RarProcess.OnRarCompleted += RarProcess_OnRarCompleted;
      RarProcess.OnOutputReceived += RarProcess_OnOutputReceived;
      Task RarTask = Task.Factory.StartNew(() => RarProcess.Exec());
    }

    

    public void AddFiles(IEnumerable<string> filenames, TRarIncludePath includePath = TRarIncludePath.RelativePath, TRarCompressionLevel compressionLevel = TRarCompressionLevel.Normal) {
      #region Validate parameters
      foreach (string Filename in filenames) {
        if (!File.Exists(Filename)) {
          string ErrorMessage = string.Format("Unable to add file {0} to archive {1} : Missing file or access denied", Filename, Name);
          Trace.WriteLine(ErrorMessage);
          throw new ApplicationException(ErrorMessage);
        }
      }
      #endregion Validate parameters

      #region Debugging
      Trace.WriteLine(string.Format("Adding files to archive {0}", this.FullName));
      Trace.Indent();
      foreach (string FileItem in filenames) {
        Trace.WriteLine(string.Format("File=\"{0}\"", FileItem));
      }
      Trace.Unindent();
      #endregion Debugging
      TRarCommand RarCommand = new TRarCommand(TRarAction.AddFiles, FullName, filenames);
      RarCommand.CompressionLevel = compressionLevel;
      RarCommand.IncludePath = includePath;
      using (TRarProcess RarProcess = TRarProcess.Exec(RarCommand)) {
        ExitCode = RarProcess.ExitCode;
        LastResult = RarProcess.Output;
      }
      _loadContent();
    }

    public void AddFilesAsync(IEnumerable<string> filenames, TRarIncludePath includePath = TRarIncludePath.RelativePath, TRarCompressionLevel compressionLevel = TRarCompressionLevel.Normal) {
      #region Validate parameters
      foreach (string Filename in filenames) {
        if (!File.Exists(Filename)) {
          string ErrorMessage = string.Format("Unable to add file {0} to archive {1} : Missing file or access denied", Filename, Name);
          Trace.WriteLine(ErrorMessage);
          throw new ApplicationException(ErrorMessage);
        }
      }
      #endregion Validate parameters

      #region Debugging
      Trace.WriteLine(string.Format("Adding files to archive {0}", this.FullName));
      Trace.Indent();
      foreach (string FileItem in filenames) {
        Trace.WriteLine(string.Format("File=\"{0}\"", FileItem));
      }
      Trace.Unindent();
      #endregion Debugging
      TRarCommand RarCommand = new TRarCommand(TRarAction.AddFiles, FullName, filenames);
      RarCommand.CompressionLevel = compressionLevel;
      RarCommand.IncludePath = includePath;
      TRarProcess RarProcess = new TRarProcess(RarCommand);
      RarProcess.OnRarCompleted += RarProcess_OnRarCompleted;
      RarProcess.OnOutputReceived += RarProcess_OnOutputReceived;
      RarProcess.OnErrorReceived += RarProcess_OnErrorReceived;
      Trace.WriteLine(Thread.CurrentThread.ManagedThreadId);
      Task RarTask = Task.Factory.StartNew(() => {
        Trace.WriteLine(Thread.CurrentThread.ManagedThreadId);
        RarProcess.Exec();
      });
    }

    

    public void AddFolder(string foldername, bool recurseSubFolders = true, TRarIncludePath includePath = TRarIncludePath.RelativePath, TRarCompressionLevel compressionLevel = TRarCompressionLevel.Normal) {
      Trace.WriteLine(string.Format("Adding folder \"{0}\" to archive {1}", foldername, this.FullName));
      TRarCommand RarCommand = new TRarCommand(TRarAction.AddFolder, FullName, foldername);
      RarCommand.IncludePath = includePath;
      RarCommand.RecurseFolders = recurseSubFolders;
      RarCommand.CompressionLevel = compressionLevel;
      using (TRarProcess RarProcess = TRarProcess.Exec(RarCommand)) {
        ExitCode = RarProcess.ExitCode;
        LastResult = RarProcess.Output;
      }
      _loadContent();
    }

    public void AddFolders(IEnumerable<string> foldernames, bool recurseSubFolders = true, TRarIncludePath includePath = TRarIncludePath.RelativePath, TRarCompressionLevel compressionLevel = TRarCompressionLevel.Normal) {
      #region Debugging
      Trace.WriteLine(string.Format("Adding folders to archive {0}", this.FullName));
      Trace.Indent();
      foreach (string FolderItem in foldernames) {
        Trace.WriteLine(string.Format("Folder=\"{0}\"", FolderItem));
      }
      Trace.Unindent();
      #endregion Debugging
      TRarCommand RarCommand = new TRarCommand(TRarAction.AddFolders, FullName, foldernames);
      RarCommand.IncludePath = includePath;
      RarCommand.RecurseFolders = recurseSubFolders;
      RarCommand.CompressionLevel = compressionLevel;
      using (TRarProcess RarProcess = TRarProcess.Exec(RarCommand)) {
        ExitCode = RarProcess.ExitCode;
        LastResult = RarProcess.Output;
      }
      _loadContent();
    }
    #endregion Add files and folders

    #region Extract
    public byte[] Extract(string filename) {
      Trace.WriteLine(string.Format("Extracting file {0} from archive {1}", filename, this.FullName));

      TRarCommand RarCommand = new TRarCommand(TRarAction.ExtractFile, FullName, filename);
      RarCommand.ExtractPath = Path.GetTempPath();
      RarCommand.Overwrite = true;
      string TempFilename = Path.Combine(RarCommand.ExtractPath, filename);

      using (TRarProcess RarProcess = TRarProcess.Exec(RarCommand)) {
        ExitCode = RarProcess.ExitCode;
        LastResult = RarProcess.Output;
      }
      byte[] RetVal = null;
      try {
        RetVal = File.ReadAllBytes(TempFilename);
        File.SetAttributes(TempFilename, FileAttributes.Normal);
        File.Delete(TempFilename);
      } catch (IOException ex) {
        Trace.WriteLine(string.Format("Error while extracting file {0} to byte[] : {1}", filename, ex.Message));
      }
      return RetVal;
    }

    public void Extract(string filename, string extractPathname, bool overwrite = false) {
      Trace.WriteLine(string.Format("Extracting file {0} from archive {1} to path {2}", filename, this.FullName, extractPathname));

      TRarCommand RarCommand = new TRarCommand(TRarAction.ExtractFile, FullName, filename);
      RarCommand.ExtractPath = extractPathname;
      RarCommand.Overwrite = true;
      using (TRarProcess RarProcess = TRarProcess.Exec(RarCommand)) {
        ExitCode = RarProcess.ExitCode;
        LastResult = RarProcess.Output;
      }
      Trace.WriteLine("Done.");
    }

    public void ExtractAll(string extractPathname, bool overwrite = false) {
      Trace.WriteLine(string.Format("Extracting everything from archive {0} to path {1}", FullName, extractPathname));
      TRarCommand RarCommand = new TRarCommand(TRarAction.ExtractAll, FullName);
      RarCommand.ExtractPath = extractPathname;
      RarCommand.Overwrite = true;
      using (TRarProcess RarProcess = TRarProcess.Exec(RarCommand)) {
        ExitCode = RarProcess.ExitCode;
        LastResult = RarProcess.Output;
      }
      Trace.WriteLine("Done.");
    }
    #endregion Extract

    #region Comment
    public void AddComment(string comment) {
      TRarCommand RarCommand = new TRarCommand(TRarAction.AddComment, FullName);
      RarCommand.Comment = comment;
      using (TRarProcess RarProcess = TRarProcess.Exec(RarCommand)) {
        ExitCode = RarProcess.ExitCode;
        LastResult = RarProcess.Output;
      }
    }
    #endregion Comment

    #region SFX
    public void ConvertToSfx(bool keepOriginal = true) {
      TRarCommand RarCommand = new TRarCommand(TRarAction.ConvertToSfx, FullName);
      RarCommand.Overwrite = true;
      RarCommand.KeepOriginal = keepOriginal;
      using (TRarProcess RarProcess = TRarProcess.Exec(RarCommand)) {
        ExitCode = RarProcess.ExitCode;
        LastResult = RarProcess.Output;
        if (RarProcess.ExitCode == 0 && !keepOriginal) {
          try {
            File.Delete(FullName);
          } catch (Exception ex) {
            Trace.WriteLine(string.Format("Unable to delete original RAR file ({0}) after conversion to SFX : {1}", FullName, ex.Message));
          }
        }
      }
    }
    #endregion SFX

    #region Delete files
    public void DeleteFile(string filename) {
      #region Validate parameters
      if (!Files.Exists(f => f.Name.ToLower() == filename.ToLower())) {
        string ErrorMessage = string.Format("Unable to delete file \"{0}\" from archive \"{1}\" : Missing file", filename, Name);
        Trace.WriteLine(ErrorMessage);
        throw new ApplicationException(ErrorMessage);
      }
      #endregion Validate parameters

      Trace.WriteLine(string.Format("Deleting file {0} from archive {1}", filename, FullName));
      TRarCommand RarCommand = new TRarCommand(TRarAction.DeleteFile, FullName, filename);
      using (TRarProcess RarProcess = TRarProcess.Exec(RarCommand)) {
        ExitCode = RarProcess.ExitCode;
        LastResult = RarProcess.Output;
      }
      _loadContent();
    }
    public void DeleteFiles(IEnumerable<string> filenames) {
      #region Validate parameters
      foreach (string FileItem in filenames) {
        if (!Files.Exists(f => f.Name.ToLower() == FileItem.ToLower())) {
          string ErrorMessage = string.Format("Unable to delete file \"{0}\" from archive \"{1}\" : Missing file", FileItem, Name);
          Trace.WriteLine(ErrorMessage);
          throw new ApplicationException(ErrorMessage);
        }
      }
      #endregion Validate parameters

      #region Debugging
      Trace.WriteLine(string.Format("Deleting files from archive \"{0}\"", FullName));
      Trace.Indent();
      foreach (string FileItem in filenames) {
        Trace.WriteLine(string.Format("File=\"{0}\"", FileItem));
      }
      Trace.Unindent();
      #endregion Debugging

      TRarCommand RarCommand = new TRarCommand(TRarAction.DeleteFiles, FullName, filenames);
      using (TRarProcess RarProcess = TRarProcess.Exec(RarCommand)) {
        ExitCode = RarProcess.ExitCode;
        LastResult = RarProcess.Output;
      }
      _loadContent();
    }
    #endregion Delete files
    #endregion Public methods

    #region Private methods
    private void RarProcess_OnOutputReceived(object sender, TRarProcess.StringEventArgs e) {
      Trace.WriteLine(string.Format("OutputReceived: {0}", e.Text));
    }

    private void RarProcess_OnRarCompleted(object sender, TRarProcess.RarCompletedEventArgs e) {
      Trace.WriteLine("Rar completed.");
      Trace.WriteLine(Thread.CurrentThread.ManagedThreadId);
      TRarProcess RarProcess = sender as TRarProcess;
      RarProcess.OnRarCompleted -= RarProcess_OnRarCompleted;
      RarProcess.OnOutputReceived -= RarProcess_OnOutputReceived;
      RarProcess.OnErrorReceived -= RarProcess_OnErrorReceived;
      ExitCode = RarProcess.ExitCode;
      LastResult = RarProcess.Output;
      Trace.WriteLine("Loading content back");
      _loadContent();
      if (JobDone != null) {
        JobDone.Set();
      }
    }
    void RarProcess_OnErrorReceived(object sender, TRarProcess.StringEventArgs e) {
      Trace.WriteLine(string.Format("ErrorReceived: {0}", e.Text));
    }
    private void _loadContent() {
      TRarCommand RarCommand = new TRarCommand(TRarAction.List, FullName);
      List<string> DataLines;
      using (TRarProcess RarProcess = TRarProcess.Exec(RarCommand)) {
        Trace.WriteLine(RarProcess.Output);
        Trace.WriteLine(RarProcess.Error);
        DataLines = RarProcess
          .OutputLines.Where(x => x != "")
          .SkipWhile(x => !x.StartsWith("---"))
          .Skip(1)
          .TakeWhile(x => !x.StartsWith("---"))
          .ToList();
      }
      Files.Clear();
      int i = 0;
      while (i * 3 < DataLines.Count) {
        List<string> DataItem = DataLines
          .Skip(i * 3)
          .Take(3)
          .ToList();
        Files.Add(new TRarElement(DataItem[0].Trim(), DataItem[1]));
        i++;
      }
    }
    #endregion Private methods

    #region Events
    //public event EventHandler<RarCompletedEventArgs> OnRarCompleted;
    //public class RarCompletedEventArgs : EventArgs {
    //  public int ExitCode;
    //  public List<string> Answer;
    //  public RarCompletedEventArgs(int exitCode, IEnumerable<string> answer) {
    //    ExitCode = exitCode;
    //    Answer = new List<string>();
    //    Answer.AddRange(answer);
    //  }
    //  public RarCompletedEventArgs(int exitCode, string answer) {
    //    ExitCode = exitCode;
    //    Answer = new List<string>();
    //    Answer.Add(answer);
    //  }
    //}
    #endregion Events

  }
}
