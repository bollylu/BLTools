using BLTools;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Diagnostics;

namespace UnitTest2015 {


  /// <summary>
  ///This is a test class for ArgElementTest and is intended
  ///to contain all ArgElementTest Unit Tests
  ///</summary>
  [TestClass()]
  public class DateTimeExtensionTest {


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
    ///A test for DateTime extension
    ///</summary>
    [TestMethod(), TestCategory("DateTime")]
    public void DateTimeToYMD_ResultOK() {
      DateTime Source = new DateTime(2015, 04, 28);
      string target = "2015-04-28";
      Assert.IsTrue(target == Source.ToYMD());
    }

    /// <summary>
    ///A test for DateTime extension
    ///</summary>
    [TestMethod(), TestCategory("DateTime")]
    public void DateTimeToYMDHMS_ResultOK() {
      DateTime Source = new DateTime(2015, 04, 28, 18, 6, 30);
      string target = "2015-04-28 18:06:30";
      Assert.IsTrue(target == Source.ToYMDHMS());
    }

    /// <summary>
    ///A test for DateTime extension
    ///</summary>
    [TestMethod(), TestCategory("DateTime")]
    public void DateTimeToDMY_ResultOK() {
      DateTime Source = new DateTime(2015, 04, 28);
      string target = "28/04/2015";
      Assert.IsTrue(target == Source.ToDMY());
    }

    /// <summary>
    ///A test for DateTime extension
    ///</summary>
    [TestMethod(), TestCategory("DateTime")]
    public void DateTimeToDMYHMS_ResultOK() {
      DateTime Source = new DateTime(2015, 04, 28, 18, 6, 30);
      string target = "28/04/2015 18:06:30";
      Assert.IsTrue(target == Source.ToDMYHMS());
    }

    /// <summary>
    ///A test for DateTime extension
    ///</summary>
    [TestMethod(), TestCategory("DateTime")]
    public void DateTimeToHMS_ResultOK() {
      DateTime Source = new DateTime(2015, 04, 28, 18, 6, 30);
      string target = "18:06:30";
      Assert.IsTrue(target == Source.ToHMS());
    }

    /// <summary>
    ///A test for DateTime extension
    ///</summary>
    [TestMethod(), TestCategory("DateTime")]
    public void DateTimeFromUTC_ResultOK() {
      DateTime Source = new DateTime(2015, 04, 28, 18, 6, 30);
      DateTime target = Source.ToLocalTime();
      Assert.IsTrue(target == Source.FromUTC());
    }

    /// <summary>
    ///A test for DateTime extension
    ///</summary>
    [TestMethod(), TestCategory("DateTime")]
    public void DateTimeEmptyDateAsDash_ResultOK() {
      DateTime Source = DateTime.MinValue;
      string target = "-";
      Assert.IsTrue(target == Source.EmptyDateAsDash());
    }

    /// <summary>
    ///A test for DateTime extension
    ///</summary>
    [TestMethod(), TestCategory("DateTime")]
    public void DateTimeEmptyDateAsBlank_ResultOK() {
      DateTime Source = DateTime.MinValue;
      string target = "";
      Assert.IsTrue(target == Source.EmptyDateAsBlank());
    }

    /// <summary>
    ///A test for DateTime extension
    ///</summary>
    [TestMethod(), TestCategory("DateTime")]
    public void DateTimeEmptyDateAs_CustomValue_ResultOK() {
      DateTime Source = DateTime.MinValue;
      string target = "../../....";
      Assert.IsTrue(target == Source.EmptyDateAs("../../...."));
    }

    /// <summary>
    ///A test for DateTime extension
    ///</summary>
    [TestMethod(), TestCategory("DateTime")]
    public void DateTime_TimeConsistent_ResultOK() {
      DateTime Source = new DateTime(2015, 3, 10, 10, 33, 0);
      DateTime target = DateTime.MinValue.Add(new TimeSpan(10, 33, 0));
      Trace.WriteLine(target.ToString());
      Assert.IsTrue(target == Source.Time());
    }

  }
}
