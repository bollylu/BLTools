#pragma warning disable 1591
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLTools.FileManagement {
  /// <summary>
  /// Describe extensively executable files at the OS level
  /// </summary>
  public class ExtendedFileVersionInfo {

    #region Enums
    /// <summary>
    /// Kind of executable file
    /// </summary>
    public enum ExecutableTypeEnum : int {
      /// <summary>
      /// Unknown
      /// </summary>
      Unknown = -1,
      /// <summary>
      /// MS-Dos 
      /// </summary>
      Dos = 0,
      /// <summary>
      /// 32 bits
      /// </summary>
      x86 = 1,
      /// <summary>
      /// 64 bits
      /// </summary>
      x64 = 2,
      /// <summary>
      /// IL language
      /// </summary>
      AnyCpu = 3
    }
    /// <summary>
    /// Kind of machine that could execute the process
    /// </summary>
    public enum MachineFamilyEnum : int {
      /// <summary>
      /// Unknown
      /// </summary>
      Unknown = -1,
      /// <summary>
      /// Intel 32 bits
      /// </summary>
      Intelx86 = 0x14C,     // Intel x386
      /// <summary>
      /// Mips R3000
      /// </summary>
      MipsR3000 = 0x162,
      /// <summary>
      /// Mips R4000
      /// </summary>
      MipsR4000 = 0x166,
      /// <summary>
      /// Dec Alpha
      /// </summary>
      DecAlphaAXP = 0x183,
      /// <summary>
      /// Intel Itanium
      /// </summary>
      IntelIA64 = 0x200,    // Itanium
      /// <summary>
      /// Intel 64 bits (non-itanium)
      /// </summary>
      Intelx64 = 0x8664     // Intel EMT64 or AMD64
    }
    [Flags]
    public enum PE_CharacteristicsEnum : int {
      RelocStripped = 0x0001,
      ExecutableImage = 0x0002,
      LineNumbersStripped = 0x0004,
      LocalSymbolsStripped = 0x0008,
      TrimLocalSet = 0x0010,
      CanHandleAddressLarger2Gb = 0x0020,
      BytesReversed = 0x0080,
      X86Machine = 0x0100,
      DebuggingInfoStripped = 0x0200,
      RemovableMediaSwap = 0x0400,
      NetSwap = 0x0800,
      SystemFile = 0x1000,
      Dll = 0x2000,
      UniProcessorOnly = 0x4000,
      HighBytesReversed = 0x8000
    }
    public enum SubSystemEnum : int {
      Unknown = -1,
      Console = 0,
      Native = 2,
      GUI = 3
    }
    [Flags]
    public enum PE_RVA_CLR_FlagsEnum : int {
      ILOnly = 0x0001,
      X86Only = 0x0002
    }
    #endregion Enums

    #region Internal types
    internal class PE_Header_Type {
      internal Int16 PE_Machine;
      internal Int16 PE_NumberOfSections;
      internal Int32 PE_Timestamp;
      internal Int32 PE_PointerToSymbols;
      internal Int32 PE_NumberOfSymbols;
      internal Int16 PE_SizeOfOptionalHeader;
      internal Int16 PE_Characteristics;
    };
    internal class PE_RvaSize_Type {
      internal Int32 VirtualAddress;
      internal Int32 Size;
      public PE_RvaSize_Type(Int32 virtualAddress, Int32 size) {
        VirtualAddress = virtualAddress;
        Size = size;
      }
      public override string ToString() {
        StringBuilder RetVal = new StringBuilder();
        RetVal.AppendFormat("RVA={0}, Size={1}", VirtualAddress.ToString("X8"), Size.ToString("X8"));
        return RetVal.ToString();
      }
    }
    internal class PE_DataDirectories_Type {
      internal PE_RvaSize_Type ExportTable { get; set; }
      internal PE_RvaSize_Type ImportTable { get; set; }
      internal PE_RvaSize_Type Win32ResourceTable { get; set; }
      internal PE_RvaSize_Type ExceptionTable { get; set; }
      internal PE_RvaSize_Type CertificateTable { get; set; }
      internal PE_RvaSize_Type BaseRelocationTable { get; set; }
      internal PE_RvaSize_Type DebugTable { get; set; }
      internal PE_RvaSize_Type CopyrightTable { get; set; }
      internal PE_RvaSize_Type MipsGlobalPtr { get; set; }
      internal PE_RvaSize_Type TLS { get; set; }
      internal PE_RvaSize_Type LoadConfig { get; set; }
      internal PE_RvaSize_Type BoundImport { get; set; }
      internal PE_RvaSize_Type IAT { get; set; }
      internal PE_RvaSize_Type DelayImportDescriptor { get; set; }
      internal PE_RvaSize_Type ClrHeader { get; set; }
      internal PE_RvaSize_Type Reserved { get; set; }

      public override string ToString() {
        StringBuilder RetVal = new StringBuilder();

        RetVal.AppendFormat("Export Table : {0}\n", ExportTable.ToString());
        RetVal.AppendFormat("Import Table : {0}\n", ImportTable.ToString());
        RetVal.AppendFormat("Win32 resource Table : {0}\n", Win32ResourceTable.ToString());
        RetVal.AppendFormat("Exception Table : {0}\n", ExceptionTable.ToString());
        RetVal.AppendFormat("Certificate Table : {0}\n", CertificateTable.ToString());
        RetVal.AppendFormat("Base relocation Table : {0}\n", BaseRelocationTable.ToString());
        RetVal.AppendFormat("Debug Table : {0}\n", DebugTable.ToString());
        RetVal.AppendFormat("Copyright Table : {0}\n", CopyrightTable.ToString());
        RetVal.AppendFormat("Mips global pointer : {0}\n", MipsGlobalPtr.ToString());
        RetVal.AppendFormat("TLS : {0}\n", TLS.ToString());
        RetVal.AppendFormat("Load config : {0}\n", LoadConfig.ToString());
        RetVal.AppendFormat("Bound import : {0}\n", BoundImport.ToString());
        RetVal.AppendFormat("IAT : {0}\n", IAT.ToString());
        RetVal.AppendFormat("Delay import descriptor : {0}\n", DelayImportDescriptor.ToString());
        RetVal.AppendFormat("CLR header : {0}\n", ClrHeader.ToString());
        RetVal.AppendFormat("Reserved : {0}\n", Reserved.ToString());

        return RetVal.ToString();
      }
    }
    internal class PE_OptionalHeader_Type {
      internal Int16 Magic;
      internal byte MajorLinkerVersion;
      internal byte MinorLinkerVersion;
      internal Int32 SizeOfCode;
      internal Int32 SizeOfInitializedData;
      internal Int32 SizeOfUnitializedData;
      internal Int32 AddressOfEntryPoint;
      internal Int32 BaseOfCode;
      internal Int32 BaseOfData;
      internal Int32 ImageBase;
      internal Int32 SectionAlignment;
      internal Int32 FileAlignment;
      internal Int16 MajorOperatingSystemVersion;
      internal Int16 MinorOperatingSystemVersion;
      internal Int16 MajorImageVersion;
      internal Int16 MinorImageVersion;
      internal Int16 MajorSubSystemVersion;
      internal Int16 MinorSubSystemVersion;
      internal Int32 W32VersionValue;
      internal Int32 SizeOfImage;
      internal Int32 SizeOfHeaders;
      internal Int32 Checksum;
      internal Int16 SubSystem;
      internal Int16 DllCharacteristics;
      internal Int32 SizeOfStackReserve;
      internal Int32 SizeOfStackCommit;
      internal Int32 SizeOfHeapReserve;
      internal Int32 SizeOfHeapCommit;
      internal Int32 LoaderFlags;
      internal Int32 NumberOfRvaAndSizes;
      internal PE_DataDirectories_Type DataDirectories;
      internal PE_OptionalHeader_Type() {
        Magic = 0;
        MajorLinkerVersion = 0;
        MinorLinkerVersion = 0;
        SizeOfCode = 0;
        SizeOfInitializedData = 0;
        SizeOfUnitializedData = 0;
        AddressOfEntryPoint = 0;
        BaseOfCode = 0;
        BaseOfData = 0;
        ImageBase = 0;
        SectionAlignment = 0;
        FileAlignment = 0;
        MajorOperatingSystemVersion = 0;
        MinorOperatingSystemVersion = 0;
        MajorImageVersion = 0;
        MinorImageVersion = 0;
        MajorSubSystemVersion = 0;
        MinorSubSystemVersion = 0;
        W32VersionValue = 0;
        SizeOfImage = 0;
        SizeOfHeaders = 0;
        Checksum = 0;
        SubSystem = 0;
        DllCharacteristics = 0;
        SizeOfStackReserve = 0;
        SizeOfStackCommit = 0;
        SizeOfHeapReserve = 0;
        SizeOfHeapCommit = 0;
        LoaderFlags = 0;
        NumberOfRvaAndSizes = 0;
        DataDirectories = new PE_DataDirectories_Type();
      }
    }
    internal class PE_RVA_CLR_Type {
      internal UInt16 MajorRuntimeVersion;
      internal UInt16 MinorRuntimeVersion;
      internal PE_RvaSize_Type MetaData;
      internal PE_RVA_CLR_FlagsEnum Flags;
      internal Int32 EntryPointToken;
      internal PE_RvaSize_Type Resources;
      internal PE_RvaSize_Type StrongNameSignature;
      internal PE_RvaSize_Type CodeManagerTable;
      internal PE_RvaSize_Type VTableFixups;
      internal PE_RvaSize_Type ExportAddressTableJumps;
      internal PE_RvaSize_Type ManagerNativeHeader;
    }
    #endregion Internal types

    #region Public properties
    public FileVersionInfo BasicFileVersionInfo {
      get;
      private set;
    }
    public ExecutableTypeEnum ExecutableType {
      get;
      private set;
    }
    public Version TargetDotNet { get; private set; }
    public MachineFamilyEnum TargetMachine { get; private set; }
    public DateTime DateCreated {
      get;
      private set;
    }
    public PE_CharacteristicsEnum Characteristics {
      get;
      private set;
    }
    public Version PE_LinkerVersion {
      get;
      private set;
    }
    public Version PE_OperatingSystemVersion {
      get;
      private set;
    }
    public Version PE_ImageVersion {
      get;
      private set;
    }
    public Version PE_SubSystemVersion {
      get;
      private set;
    }
    public SubSystemEnum Subsystem {
      get;
      private set;
    }
    #endregion Public properties

    #region Constructor(s)
    public ExtendedFileVersionInfo(string fullFilename) {
      BasicFileVersionInfo = FileVersionInfo.GetVersionInfo(fullFilename);
      ExecutableType = ExecutableTypeEnum.Unknown;
      TargetMachine = MachineFamilyEnum.Unknown;
      TargetDotNet = new Version();
      List<string> ExecutableTypes = new List<string>() { ".exe", ".dll" };
      if (ExecutableTypes.Contains(Path.GetExtension(fullFilename).ToLower())) {
        _GetExecutableFilePE(fullFilename);
      }
    }
    #endregion Constructor(s)

    #region Private methods
    private void _GetExecutableFilePE(string fullFilename) {

      const Int16 PE_Offset = 60;
      Int32 Start_PE_Header;
      PE_Header_Type PE_Header;
      PE_OptionalHeader_Type PE_OptionalHeader;
      PE_RVA_CLR_Type PE_Rva_Clr;

      using (FileStream ExeFile = File.OpenRead(fullFilename)) {
        using (BinaryReader ExeReader = new BinaryReader(ExeFile)) {

          ExeFile.Seek(PE_Offset, SeekOrigin.Begin);
          Start_PE_Header = ExeReader.ReadInt32();

          ExeFile.Seek(Start_PE_Header, SeekOrigin.Begin);
          string PE_Signature = new string(ExeReader.ReadChars(4));
          if (PE_Signature != "PE\0\0") {
            ExecutableType = ExecutableTypeEnum.Dos;
            return;
          }

          PE_Header = Read_PE_Header(ExeReader);
          PE_OptionalHeader = Read_PE_OptionalHeader(ExeReader);
          //Console.WriteLine(PE_OptionalHeader.DataDirectories.ToString());
          //ConsoleExtension.Pause();
          ExeFile.Seek(PE_OptionalHeader.DataDirectories.ClrHeader.VirtualAddress % 0x2000 + 512, SeekOrigin.Begin);
          PE_Rva_Clr = Read_PE_Rva_Clr(ExeReader);

          TargetDotNet = new Version(PE_Rva_Clr.MajorRuntimeVersion, PE_Rva_Clr.MinorRuntimeVersion);
          try {
            TargetMachine = (MachineFamilyEnum)PE_Header.PE_Machine;
          } catch {
            TargetMachine = MachineFamilyEnum.Unknown;
          }

          DateCreated = new DateTime(1970, 1, 1).AddSeconds(PE_Header.PE_Timestamp);
          Characteristics = (PE_CharacteristicsEnum)(int)PE_Header.PE_Characteristics;
          PE_LinkerVersion = new Version(PE_OptionalHeader.MajorLinkerVersion, PE_OptionalHeader.MinorLinkerVersion);
          PE_OperatingSystemVersion = new Version(PE_OptionalHeader.MajorOperatingSystemVersion, PE_OptionalHeader.MinorOperatingSystemVersion);
          PE_ImageVersion = new Version(PE_OptionalHeader.MajorImageVersion, PE_OptionalHeader.MinorImageVersion);
          PE_SubSystemVersion = new Version(PE_OptionalHeader.MajorSubSystemVersion, PE_OptionalHeader.MinorSubSystemVersion);
          Subsystem = (SubSystemEnum)(int)PE_OptionalHeader.SubSystem;

          switch (PE_OptionalHeader.Magic) {
            case 0x10B:
              if ((PE_Rva_Clr.Flags & PE_RVA_CLR_FlagsEnum.ILOnly) != 0) {
                ExecutableType = ExecutableTypeEnum.AnyCpu;
              } else {
                ExecutableType = ExecutableTypeEnum.x86;
              }
              break;
            case 0x20B:
              ExecutableType = ExecutableTypeEnum.x64;
              break;

          }
        }

      }
    }

    private PE_RVA_CLR_Type Read_PE_Rva_Clr(BinaryReader ExeReader) {
      PE_RVA_CLR_Type RetVal = new PE_RVA_CLR_Type();
      RetVal.MajorRuntimeVersion = ExeReader.ReadUInt16();
      RetVal.MinorRuntimeVersion = ExeReader.ReadUInt16();
      RetVal.MetaData = new PE_RvaSize_Type(ExeReader.ReadInt32(), ExeReader.ReadInt32());
      RetVal.Flags = (PE_RVA_CLR_FlagsEnum)(int)ExeReader.ReadInt32();
      RetVal.EntryPointToken = ExeReader.ReadInt32();
      RetVal.Resources = new PE_RvaSize_Type(ExeReader.ReadInt32(), ExeReader.ReadInt32());
      RetVal.StrongNameSignature = new PE_RvaSize_Type(ExeReader.ReadInt32(), ExeReader.ReadInt32());
      RetVal.CodeManagerTable = new PE_RvaSize_Type(ExeReader.ReadInt32(), ExeReader.ReadInt32());
      RetVal.VTableFixups = new PE_RvaSize_Type(ExeReader.ReadInt32(), ExeReader.ReadInt32());
      RetVal.ExportAddressTableJumps = new PE_RvaSize_Type(ExeReader.ReadInt32(), ExeReader.ReadInt32());
      RetVal.ManagerNativeHeader = new PE_RvaSize_Type(ExeReader.ReadInt32(), ExeReader.ReadInt32());
      return RetVal;
    }

    private PE_Header_Type Read_PE_Header(BinaryReader ExeReader) {
      PE_Header_Type PE_Header;
      PE_Header = new PE_Header_Type();
      PE_Header.PE_Machine = ExeReader.ReadInt16();
      PE_Header.PE_NumberOfSections = ExeReader.ReadInt16();
      PE_Header.PE_Timestamp = ExeReader.ReadInt32();
      PE_Header.PE_PointerToSymbols = ExeReader.ReadInt32();
      PE_Header.PE_NumberOfSymbols = ExeReader.ReadInt32();
      PE_Header.PE_SizeOfOptionalHeader = ExeReader.ReadInt16();
      PE_Header.PE_Characteristics = ExeReader.ReadInt16();
      return PE_Header;
    }
    private PE_OptionalHeader_Type Read_PE_OptionalHeader(BinaryReader ExeReader) {
      PE_OptionalHeader_Type PE_OptionalHeader;
      PE_OptionalHeader = new PE_OptionalHeader_Type();
      PE_OptionalHeader.Magic = ExeReader.ReadInt16();
      PE_OptionalHeader.MajorLinkerVersion = ExeReader.ReadByte();
      PE_OptionalHeader.MinorLinkerVersion = ExeReader.ReadByte();
      PE_OptionalHeader.SizeOfCode = ExeReader.ReadInt32();
      PE_OptionalHeader.SizeOfInitializedData = ExeReader.ReadInt32();
      PE_OptionalHeader.SizeOfUnitializedData = ExeReader.ReadInt32();
      PE_OptionalHeader.AddressOfEntryPoint = ExeReader.ReadInt32();
      PE_OptionalHeader.BaseOfCode = ExeReader.ReadInt32();
      PE_OptionalHeader.BaseOfData = ExeReader.ReadInt32();
      PE_OptionalHeader.ImageBase = ExeReader.ReadInt32();
      PE_OptionalHeader.SectionAlignment = ExeReader.ReadInt32();
      PE_OptionalHeader.FileAlignment = ExeReader.ReadInt32();
      PE_OptionalHeader.MajorOperatingSystemVersion = ExeReader.ReadInt16();
      PE_OptionalHeader.MinorOperatingSystemVersion = ExeReader.ReadInt16();
      PE_OptionalHeader.MinorImageVersion = ExeReader.ReadInt16();
      PE_OptionalHeader.MinorImageVersion = ExeReader.ReadInt16();
      PE_OptionalHeader.MajorSubSystemVersion = ExeReader.ReadInt16();
      PE_OptionalHeader.MinorSubSystemVersion = ExeReader.ReadInt16();
      PE_OptionalHeader.W32VersionValue = ExeReader.ReadInt32();
      PE_OptionalHeader.SizeOfImage = ExeReader.ReadInt32();
      PE_OptionalHeader.SizeOfHeaders = ExeReader.ReadInt32();
      PE_OptionalHeader.Checksum = ExeReader.ReadInt16();
      PE_OptionalHeader.SubSystem = ExeReader.ReadInt16();
      PE_OptionalHeader.DllCharacteristics = ExeReader.ReadInt16();
      PE_OptionalHeader.SizeOfStackReserve = ExeReader.ReadInt32();
      PE_OptionalHeader.SizeOfStackCommit = ExeReader.ReadInt32();
      PE_OptionalHeader.SizeOfHeapReserve = ExeReader.ReadInt32();
      PE_OptionalHeader.SizeOfHeapCommit = ExeReader.ReadInt32();
      PE_OptionalHeader.LoaderFlags = ExeReader.ReadInt32();
      PE_OptionalHeader.DataDirectories = Read_PE_DataDirectories(ExeReader);
      return PE_OptionalHeader;
    }
    private PE_DataDirectories_Type Read_PE_DataDirectories(BinaryReader ExeReader) {
      PE_DataDirectories_Type RetVal = new PE_DataDirectories_Type();
      RetVal.ExportTable = new PE_RvaSize_Type(ExeReader.ReadInt32(), ExeReader.ReadInt32());
      RetVal.ImportTable = new PE_RvaSize_Type(ExeReader.ReadInt32(), ExeReader.ReadInt32());
      RetVal.Win32ResourceTable = new PE_RvaSize_Type(ExeReader.ReadInt32(), ExeReader.ReadInt32());
      RetVal.ExceptionTable = new PE_RvaSize_Type(ExeReader.ReadInt32(), ExeReader.ReadInt32());
      RetVal.CertificateTable = new PE_RvaSize_Type(ExeReader.ReadInt32(), ExeReader.ReadInt32());
      RetVal.BaseRelocationTable = new PE_RvaSize_Type(ExeReader.ReadInt32(), ExeReader.ReadInt32());
      RetVal.DebugTable = new PE_RvaSize_Type(ExeReader.ReadInt32(), ExeReader.ReadInt32());
      RetVal.CopyrightTable = new PE_RvaSize_Type(ExeReader.ReadInt32(), ExeReader.ReadInt32());
      RetVal.MipsGlobalPtr = new PE_RvaSize_Type(ExeReader.ReadInt32(), ExeReader.ReadInt32());
      RetVal.TLS = new PE_RvaSize_Type(ExeReader.ReadInt32(), ExeReader.ReadInt32());
      RetVal.LoadConfig = new PE_RvaSize_Type(ExeReader.ReadInt32(), ExeReader.ReadInt32());
      RetVal.BoundImport = new PE_RvaSize_Type(ExeReader.ReadInt32(), ExeReader.ReadInt32());
      RetVal.IAT = new PE_RvaSize_Type(ExeReader.ReadInt32(), ExeReader.ReadInt32());
      RetVal.DelayImportDescriptor = new PE_RvaSize_Type(ExeReader.ReadInt32(), ExeReader.ReadInt32());
      RetVal.ClrHeader = new PE_RvaSize_Type(ExeReader.ReadInt32(), ExeReader.ReadInt32());
      RetVal.Reserved = new PE_RvaSize_Type(ExeReader.ReadInt32(), ExeReader.ReadInt32());
      return RetVal;
    }
    #endregion Private methods
  }
}
#pragma warning restore 1591