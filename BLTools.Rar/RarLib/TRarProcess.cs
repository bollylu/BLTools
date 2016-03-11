using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RarLib {

  internal class TRarProcess : IDisposable {
    #region Public properties
    /// <summary>
    /// Location of rar.exe
    /// </summary>
    public static string RarExe;

    public TRarCommand RarCommand { get; set; }
    public string Parameters { get; set; }

    public string Input { get; set; }
    public string Output {
      get {
        lock (SyncOutput) {
          return TempOutput.ToString();
        }
      }
    }
    public string[] OutputLines {
      get {
        lock (SyncOutput) {
          return TempOutput.ToString().Split(Environment.NewLine.ToCharArray());
        }
      }
    }
    public string Error {
      get {
        lock (SyncError) {
          return TempError.ToString();
        }
      }
    }
    public string[] ErrorLines {
      get {
        lock (SyncError) {
          return TempError.ToString().Split(Environment.NewLine.ToCharArray());
        }
      }
    }
    public int ExitCode { get; private set; }
    #endregion Public properties

    #region Private variables
    private Process Rar;
    private StringBuilder TempOutput;
    private object SyncOutput = new object();
    private StringBuilder TempError;
    private object SyncError = new object();
    #endregion Private variables

    #region Constructor(s)
    /// <summary>
    /// Pre-initialization constructor : locate rar.exe
    /// If unable to auto-locate, you can set the public static string RarExe.
    /// </summary>
    static TRarProcess() {
      List<string> SearchPath = new List<string>() {
        Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles), 
        Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86) 
      };
      foreach (string PathItem in SearchPath) {
        Trace.WriteLine(string.Format("Looking in {0}", PathItem));
        string FoundPath = Directory.GetFiles(PathItem, "rar.exe", SearchOption.AllDirectories).FirstOrDefault();
        if (!string.IsNullOrWhiteSpace(FoundPath)) {
          RarExe = FoundPath;
          Trace.WriteLine(string.Format("Found : {0}", RarExe));
          break;
        }
      }
    }

    /// <summary>
    /// Builds an empty TRarProcess. You will need to add parameters either manually or through the Exec method
    /// </summary>
    public TRarProcess() {
      Parameters = "";
      Input = "";
      TempOutput = new StringBuilder();
      TempError = new StringBuilder();
    }
    public TRarProcess(TRarCommand rarCommand)
      : this() {
        RarCommand = new TRarCommand(rarCommand);
    }
    
    public void Dispose() {
      if (Rar != null && !Rar.HasExited) {
        Trace.WriteLine("Cleanup rar.exe");
        Rar.Kill();
        Rar.Dispose();
      }
    }

    #endregion Constructor(s)

    #region Public methods
    public TRarProcess Exec() {
      #region Validate parameters
      if (string.IsNullOrWhiteSpace(RarExe)) {
        const string ErrorMsg = "Error: missing sub-process RAR.EXE in path, please verify.";
        Trace.WriteLine(ErrorMsg);
        throw new ApplicationException(ErrorMsg);
      }
      #endregion Validate parameters

      Rar = new Process();

      Parameters = RarCommand.Generate();
      Trace.WriteLine(string.Format("Rar parameters = {0}", Parameters));
      Rar.StartInfo = new ProcessStartInfo(RarExe, Parameters);
      Rar.StartInfo.UseShellExecute = false;
      Rar.StartInfo.RedirectStandardInput = (RarCommand.InputParameters != null && RarCommand.InputParameters.Count > 0);
      Rar.StartInfo.RedirectStandardOutput = true;
      Rar.StartInfo.RedirectStandardError = true;
      Rar.StartInfo.CreateNoWindow = true;

      Rar.OutputDataReceived += new DataReceivedEventHandler(Rar_OutputDataReceived);
      Rar.ErrorDataReceived += new DataReceivedEventHandler(Rar_ErrorDataReceived);

      try {
        Rar.Start();
        Rar.PriorityClass = ProcessPriorityClass.BelowNormal;
        Rar.BeginOutputReadLine();

        if (RarCommand.InputParameters != null && RarCommand.InputParameters.Count > 0) {
          foreach (string InputItem in RarCommand.InputParameters) {
            Rar.StandardInput.WriteLine(InputItem);
          }
          Rar.StandardInput.Close();
        }

        Rar.WaitForExit();

        Rar.OutputDataReceived -= new DataReceivedEventHandler(Rar_OutputDataReceived);
        Rar.ErrorDataReceived -= new DataReceivedEventHandler(Rar_ErrorDataReceived);

        ExitCode = Rar.ExitCode;
        if (OnRarCompleted != null) {
          OnRarCompleted(this, new RarCompletedEventArgs(ExitCode, Output));
        }

      } catch (Exception ex) {
        Trace.WriteLine(string.Format("Unable to execute RAR command : {0} : {1}", Parameters, ex.Message));
        Trace.WriteLine(string.Format("Error code from sub-process is {0}", Rar.ExitCode));
        return this;
      }

      return this;
    }
    #endregion Public methods

    public static TRarProcess Exec(TRarCommand rarCommand) {
      return (new TRarProcess(rarCommand)).Exec();
    }

    #region Private methods
    private void Rar_ErrorDataReceived(object sender, DataReceivedEventArgs e) {
      if (e.Data != null) {
        lock (SyncError) {
          TempError.AppendLine(e.Data);
        }
        if (OnErrorReceived != null) {
          OnErrorReceived(this, new StringEventArgs(e.Data));
        }
      }
    }
    private void Rar_OutputDataReceived(object sender, DataReceivedEventArgs e) {
      if (e.Data != null) {
        lock (SyncOutput) {
          TempOutput.AppendLine(e.Data);
        }
        if (OnOutputReceived != null) {
          OnOutputReceived(this, new StringEventArgs(e.Data));
        }
      }
    }
    #endregion Private methods

    #region Events
    public event EventHandler<RarCompletedEventArgs> OnRarCompleted;
    public class RarCompletedEventArgs : EventArgs {
      public int ExitCode;
      public List<string> Answer;
      public RarCompletedEventArgs(int exitCode, IEnumerable<string> answer) {
        ExitCode = exitCode;
        Answer = new List<string>();
        Answer.AddRange(answer);
      }
      public RarCompletedEventArgs(int exitCode, string answer) {
        ExitCode = exitCode;
        Answer = new List<string>();
        Answer.Add(answer);
      }
    }
    public event EventHandler<StringEventArgs> OnOutputReceived;
    public event EventHandler<StringEventArgs> OnErrorReceived;
    public class StringEventArgs : EventArgs {
      public string Text;
      public StringEventArgs(string text) {
        Text = text;
      }
    }
    #endregion Events

    
  }
}
