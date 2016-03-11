using RarLib;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace RarLibTests
{
    
    
    /// <summary>
    ///This is a test class for RarElementTest and is intended
    ///to contain all RarElementTest Unit Tests
    ///</summary>
  [TestClass()]
  public class RarElementTest {


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


    [TestMethod()]
    public void RarElement_ConstructorEmpty_AllElementsAreInitialized() {
      TRarElement TestValue = new TRarElement();
      Assert.AreEqual(TestValue.Name, "");
      Assert.AreEqual(TestValue.Pathname, "");
      Assert.AreEqual(TestValue.CompressionRatio, 0);
    }

    [TestMethod()]
    public void RarElement_ConstructorNameOnly_AllElementsAreInitialized() {
      TRarElement TestValue = new TRarElement("filename");
      Assert.AreEqual(TestValue.Name, "filename");
      Assert.AreEqual(TestValue.Pathname, "");
      Assert.AreEqual(TestValue.CompressionRatio, 0);
    }

    [TestMethod()]
    public void RarElement_ConstructorNameAndPath_AllElementsAreInitialized() {
      TRarElement TestValue = new TRarElement("filename", "c:\\test");
      Assert.AreEqual(TestValue.Name, "filename");
      Assert.AreEqual(TestValue.Pathname, "c:\\test");
      Assert.AreEqual(TestValue.CompressionRatio, 0);
    }

    
  }
}
