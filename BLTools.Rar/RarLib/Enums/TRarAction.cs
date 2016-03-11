using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RarLib {
  public enum TRarAction {
    Unknown,
    AddFile,
    AddFiles,
    AddFolder,
    AddFolders,
    ExtractFile,
    ExtractAll,
    AddComment,
    ConvertToSfx,
    List,
    DeleteFile,
    DeleteFiles
  }
}
