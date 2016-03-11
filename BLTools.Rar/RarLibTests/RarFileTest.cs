using RarLib;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

namespace RarLibTests {

  [TestClass()]
  public class RarFileTest {

    private TestContext testContextInstance;

    /// <summary>
    ///Gets or sets the test context which provides
    ///information about and functionality for the current test run.
    ///</summary>
    public TestContext TestContext {
      get {
        return testContextInstance;
      }
      set {
        testContextInstance = value;
      }
    }

    #region Additional test attributes
    // 
    //You can use the following additional attributes as you write your tests:
    //
    //Use ClassInitialize to run code before running the first test in the class
    //[ClassInitialize()]
    //public static void MyClassInitialize(TestContext testContext)
    //{
    //}
    //
    //Use ClassCleanup to run code after all tests in a class have run
    //[ClassCleanup()]
    //public static void MyClassCleanup()
    //{
    //}
    //
    //Use TestInitialize to run code before running each test
    //[TestInitialize()]
    //public void MyTestInitialize()
    //{
    //}
    //
    //Use TestCleanup to run code after each test has run
    //[TestCleanup()]
    //public void MyTestCleanup()
    //{
    //}
    //
    #endregion

    #region Tests for constructors
    [TestMethod()]
    public void RarFile_ConstructorEmpty_AllElementsAreInitialized() {
      TRarFile TestValue = new TRarFile();
      Assert.AreEqual(TestValue.Name, "");
      Assert.AreEqual(TestValue.Pathname, "");
      Assert.AreEqual(TestValue.FullName, "");
      Assert.AreEqual(TestValue.LastResult, "");
      Assert.AreEqual(TestValue.ExitCode, -1);
      Assert.IsNull(TestValue.Files);
    }

    [TestMethod()]
    public void RarFile_ConstructorNameOnly_AllElementsAreInitialized() {
      string TestFilename = "myrarfile.rar";
      TRarFile TestValue = new TRarFile(TestFilename);
      Assert.AreEqual(TestValue.Name, TestFilename);
      Assert.AreEqual(TestValue.Pathname, Environment.CurrentDirectory);
      Assert.AreEqual(TestValue.FullName, Path.Combine(Environment.CurrentDirectory, TestFilename));
      Assert.AreEqual(TestValue.LastResult, "");
      Assert.AreEqual(TestValue.ExitCode, -1);
      Assert.IsNull(TestValue.Files);
    }

    [TestMethod()]
    public void RarFile_ConstructorNameAndPath_AllElementsAreInitialized() {
      string TestFilename = "myrarfile.rar";
      string TestPathname = "c:\\testfolder";
      TRarFile TestValue = new TRarFile(Path.Combine(TestPathname, TestFilename));
      Assert.AreEqual(TestValue.Name, TestFilename);
      Assert.AreEqual(TestValue.Pathname, TestPathname);
      Assert.AreEqual(TestValue.FullName, Path.Combine(TestPathname, TestFilename));
      Assert.AreEqual(TestValue.LastResult, "");
      Assert.AreEqual(TestValue.ExitCode, -1);
      Assert.IsNull(TestValue.Files);
    }
    #endregion Tests for constructors

    [TestMethod()]
    public void RarFile_AddSingleFileReadBack_DataAreOK() {
      string TestFilename = "myrarfile.rar";
      string TestFilenameText = "mytextfile.txt";
      string TestPathname = Path.GetTempPath();
      TRarFile TestValue = new TRarFile(Path.Combine(TestPathname, TestFilename));
      Assert.AreEqual(TestValue.Name, TestFilename);
      Assert.AreEqual(TestValue.Pathname, TestPathname);
      Assert.AreEqual(TestValue.FullName, Path.Combine(TestPathname, TestFilename));
      Assert.AreEqual(TestValue.LastResult, "");
      Assert.AreEqual(TestValue.ExitCode, -1);
      Assert.IsNull(TestValue.Files);

      File.WriteAllText(Path.Combine(TestPathname, TestFilenameText), "blabla");
      TestValue.AddFile(Path.Combine(TestPathname, TestFilenameText),TRarIncludePath.RelativePath);

      TestValue = new TRarFile(Path.Combine(TestPathname, TestFilename));
      Assert.IsNotNull(TestValue.Files);
      Assert.AreEqual(TestValue.Files[0].Name, TestFilenameText);

      File.Delete(Path.Combine(TestPathname, TestFilenameText));
      File.Delete(Path.Combine(TestPathname, TestFilename));
    }

    [TestMethod()]
    [ExpectedException(typeof(ApplicationException))]
    public void RarFile_AddSingleFileMissing_Exception() {
      string TestFilename = "myrarfile.rar";
      string TestFilenameText = "c:\\mytextfile.txt";
      string TestPathname = Path.GetTempPath();
      TRarFile TestValue = new TRarFile(Path.Combine(TestPathname, TestFilename));
      Assert.AreEqual(TestValue.Name, TestFilename);
      Assert.AreEqual(TestValue.Pathname, TestPathname);
      Assert.AreEqual(TestValue.FullName, Path.Combine(TestPathname, TestFilename));
      Assert.AreEqual(TestValue.LastResult, "");
      Assert.AreEqual(TestValue.ExitCode, -1);
      Assert.IsNull(TestValue.Files);

      TestValue.AddFile(Path.Combine(TestPathname, TestFilenameText), TRarIncludePath.RelativePath);
    }

    [TestMethod()]
    public void RarFile_AddFilesReadBack_DataAreOK() {
      string TestFilename = "myrarfile.rar";
      string TestFilenameText1 = "mytextfile1.txt";
      string TestFilenameText2 = "mytextfile2.txt";
      string TestPathname = Path.GetTempPath();

      TRarFile TestValue = new TRarFile(Path.Combine(TestPathname, TestFilename));
      Assert.AreEqual(TestValue.Name, TestFilename);
      Assert.AreEqual(TestValue.Pathname, TestPathname);
      Assert.AreEqual(TestValue.FullName, Path.Combine(TestPathname, TestFilename));
      Assert.AreEqual(TestValue.LastResult, "");
      Assert.AreEqual(TestValue.ExitCode, -1);
      Assert.IsNull(TestValue.Files);

      File.WriteAllText(Path.Combine(TestPathname, TestFilenameText1), "blabla");
      File.WriteAllText(Path.Combine(TestPathname, TestFilenameText2), "blublu");
      TestValue.AddFiles(new string[] { Path.Combine(TestPathname, TestFilenameText1), Path.Combine(TestPathname, TestFilenameText2) }, TRarIncludePath.RelativePath);

      TestValue = new TRarFile(Path.Combine(TestPathname, TestFilename));
      Assert.IsNotNull(TestValue.Files);
      Assert.AreEqual(TestValue.Files.Count, 2);
      Assert.AreEqual(TestValue.Files[0].Name, TestFilenameText1);
      Assert.AreEqual(TestValue.Files[1].Name, TestFilenameText2);

      File.Delete(Path.Combine(TestPathname, TestFilenameText1));
      File.Delete(Path.Combine(TestPathname, TestFilenameText2));
      File.Delete(Path.Combine(TestPathname, TestFilename));
    }

    [TestMethod()]
    [ExpectedException(typeof(ApplicationException))]
    public void RarFile_AddFilesOneMissing_Exception() {
      string TestFilename = "myrarfile.rar";
      string TestFilenameText1 = "mytextfile1.txt";
      string TestFilenameText2 = "mytextfile2.txt";
      string TestPathname = Path.GetTempPath();

      TRarFile TestValue = new TRarFile(Path.Combine(TestPathname, TestFilename));
      Assert.AreEqual(TestValue.Name, TestFilename);
      Assert.AreEqual(TestValue.Pathname, TestPathname);
      Assert.AreEqual(TestValue.FullName, Path.Combine(TestPathname, TestFilename));
      Assert.AreEqual(TestValue.LastResult, "");
      Assert.AreEqual(TestValue.ExitCode, -1);
      Assert.IsNull(TestValue.Files);

      File.WriteAllText(Path.Combine(TestPathname, TestFilenameText1), "blabla");
      TestValue.AddFiles(new string[] { Path.Combine(TestPathname, TestFilenameText1), Path.Combine(TestPathname, TestFilenameText2) }, TRarIncludePath.RelativePath);
    }
  }
}
