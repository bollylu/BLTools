using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RarLib {
  public class TRarCommand {

    #region Public properties
    public TRarAction RarAction { get; set; }
    public TRarCompressionLevel CompressionLevel { get; set; }
    public TRarIncludePath IncludePath { get; set; }
    public bool RecurseFolders { get; set; }
    public bool Overwrite { get; set; }
    public bool KeepOriginal { get; set; }
    public string TargetFilename { get; set; }
    public List<string> Files { get; set; }
    public List<string> Folders { get; set; }
    internal List<string> InputParameters { get; private set; }
    public string ExtractPath { get; set; }
    public string Comment { get; set; }
    #endregion Public properties

    #region Constructor(s)
    public TRarCommand() {
      RarAction = TRarAction.Unknown;
      CompressionLevel = TRarCompressionLevel.Normal;
      IncludePath = TRarIncludePath.RelativePath;
      RecurseFolders = true;
      Overwrite = true;
      KeepOriginal = true;
      TargetFilename = "";
      Files = new List<string>();
      Folders = new List<string>();
      InputParameters = new List<string>();
      ExtractPath = "";
    }

    public TRarCommand(TRarAction rarAction)
      : this() {
      RarAction = rarAction;
    }

    public TRarCommand(TRarAction rarAction, string targetFilename)
      : this(rarAction) {
      TargetFilename = targetFilename;
    }

    public TRarCommand(TRarAction rarAction, string targetFilename, string name)
      : this(rarAction, targetFilename) {
      switch (rarAction) {
        case TRarAction.AddFile:
        case TRarAction.ExtractFile:
        case TRarAction.DeleteFile:
          Files.Add(name);
          break;
        case TRarAction.AddFolder:
          Folders.Add(name);
          break;
      }
    }

    public TRarCommand(TRarAction rarAction, string targetFilename, IEnumerable<string> names)
      : this(rarAction, targetFilename) {
      switch (rarAction) {
        case TRarAction.AddFiles:
        case TRarAction.DeleteFiles:
          Files.AddRange(names);
          break;
        case TRarAction.AddFolders:
          Folders.AddRange(names);
          break;
      }

    }

    public TRarCommand(TRarCommand rarCommand) {
      RarAction = rarCommand.RarAction;
      CompressionLevel = rarCommand.CompressionLevel;
      IncludePath = rarCommand.IncludePath;
      RecurseFolders = rarCommand.RecurseFolders;
      Overwrite = rarCommand.Overwrite;
      KeepOriginal = rarCommand.KeepOriginal;
      TargetFilename = rarCommand.TargetFilename;
      Files = new List<string>(rarCommand.Files);
      Folders = new List<string>(rarCommand.Folders);
      InputParameters = new List<string>(rarCommand.InputParameters);
      ExtractPath = rarCommand.ExtractPath;
    }
    #endregion Constructor(s)

    #region Public methods
    public string Generate() {
      string RarCompression = string.Format("-m{0}", (int)CompressionLevel);
      string RarIncludePath = (IncludePath == TRarIncludePath.NoPath ? "-ep" : IncludePath == TRarIncludePath.RelativePath ? "-ep1" : "-ep2");
      string RarRecurseFolder = RecurseFolders ? "-r" : "";
      string RarOverwrite = Overwrite ? "-o+" : "-o-";

      switch (RarAction) {

        case TRarAction.Unknown:
        default:
          return "";

        case TRarAction.AddFile:
          return string.Format("a -idq {0} {1} \"{2}\" \"{3}\"", RarCompression, RarIncludePath, TargetFilename, Files.First());

        case TRarAction.AddFiles:
          InputParameters = new List<string>(Files);
          //return string.Format("a -idq {0} {1} \"{2}\" @", RarCompression, RarIncludePath, TargetFilename);
          return string.Format("a {0} {1} \"{2}\" @", RarCompression, RarIncludePath, TargetFilename);

        case TRarAction.List:
          //return string.Format("vt -idcdp \"{0}\"", TargetFilename);
          return string.Format("vt \"{0}\"", TargetFilename);

        case TRarAction.AddFolder:
          return string.Format("a -idq {0} {1} {2} \"{3}\" \"{4}\"", RarCompression, RarRecurseFolder, RarIncludePath, TargetFilename, Folders.First());

        case TRarAction.AddFolders:
          InputParameters = new List<string>(Folders);
          return string.Format("a -idq {0} {1} {2} \"{3}\" @", RarCompression, RarRecurseFolder, RarIncludePath, TargetFilename);

        case TRarAction.ExtractFile:
          return string.Format("e -idq {0} \"{1}\" \"{2}\" \"{3}\\\"", RarOverwrite, TargetFilename, Files.First(), ExtractPath);

        case TRarAction.ExtractAll:
          return string.Format("x -idq {0} \"{1}\" \"{2}\\\"", RarOverwrite, TargetFilename, ExtractPath);

        case TRarAction.ConvertToSfx:
          return string.Format("s -idq \"{0}\"", TargetFilename);

        case TRarAction.DeleteFile:
          return string.Format("d -idq \"{0}\" \"{1}\"", TargetFilename, Files.First());

        case TRarAction.DeleteFiles:
          InputParameters = new List<string>(Files);
          return string.Format("d -idq \"{0}\" @", TargetFilename);
      }

    }
    #endregion Public methods
  }
}
