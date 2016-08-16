using BLTools;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace UnitTest2015 {


  /// <summary>
  ///This is a test class for ArgElementTest and is intended
  ///to contain all ArgElementTest Unit Tests
  ///</summary>
  [TestClass()]
  public class ArgElementTest {


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


    /// <summary>
    ///A test for ArgElement Constructor
    ///</summary>
    [TestMethod(), TestCategory("SplitArgs")]
    public void ArgElementConstructorTest() {
      int id = 0; 
      string name = "first";
      string value = "first value";
      ArgElement target = new ArgElement(id, name, value);
      Assert.IsTrue(target.Id == 0, "Id should be 0");
      Assert.IsTrue(target.Name == name, "Name should be {0}", name);
      Assert.IsTrue(target.Value == value, "Value should be {0}", value);
    }

    /// <summary>
    ///A test for Id
    ///</summary>
    [TestMethod(), TestCategory("SplitArgs")]
    public void IdTest() {
      int id = 0;
      string name = "first";
      string value = "first value";
      ArgElement target = new ArgElement(id, name, value);
      Assert.AreEqual(0, target.Id);
    }

    /// <summary>
    ///A test for Name
    ///</summary>
    [TestMethod(), TestCategory("SplitArgs")]
    public void NameTest() {
      int id = 0;
      string name = "first";
      string value = "first value";
      ArgElement target = new ArgElement(id, name, value);
      Assert.AreEqual(name, target.Name);
    }

    /// <summary>
    ///A test for Value
    ///</summary>
    [TestMethod(), TestCategory("SplitArgs")]
    public void ValueTest() {
      int id = 0;
      string name = "first";
      string value = "first value";
      ArgElement target = new ArgElement(id, name, value);
      Assert.AreEqual(value, target.Value);
    }
  }
}
