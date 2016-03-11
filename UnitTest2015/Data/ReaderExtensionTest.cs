using BLTools;
using BLTools.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Data;

namespace UnitTest2015 {

  /// <summary>
  ///This is a test class for ReaderExtensionTest and is intended
  ///to contain all ReaderExtensionTest Unit Tests
  ///</summary>
  [TestClass()]
  public class ReaderExtensionTest {

    private TestContext testContextInstance;
    private DataTable Data;

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
    [TestInitialize()]
    public void MyTestInitialize() {
      Data = new DataTable();
      Data.Columns.Add("ID", typeof(int));
      Data.Columns.Add("Name", typeof(string));
      Data.Columns.Add("Price", typeof(double));
      Data.Columns.Add("Date", typeof(DateTime));

      Data.Rows.Add(new object[] { 1, "Banane", 28.36, new DateTime(1966,4,28) });
      Data.Rows.Add(new object[] { DBNull.Value, "Ananas", 38.36, DBNull.Value });
      Data.Rows.Add(new object[] { 3, "Pomme", DBNull.Value, new DateTime(1968,2,29) });
      Data.Rows.Add(new object[] { 4, DBNull.Value, 29.466, new DateTime(1969,7,31) });
    }
    //
    //Use TestCleanup to run code after each test has run
    //[TestCleanup()]
    //public void MyTestCleanup()
    //{
    //}
    //
    #endregion


    /// <summary>
    ///A test for SafeRead
    ///</summary>
    [TestMethod(), TestCategory("Data"), TestCategory("DataReader")]
    public void SafeRead_ReadString_ResultOK() {
      string actual;
      using ( IDataReader reader = Data.CreateDataReader() ) {
        reader.Read();
        actual = reader.SafeRead<string>("Name", "");
      }
      Assert.AreEqual("Banane", actual);
    }

    /// <summary>
    ///A test for SafeRead
    ///</summary>
    [TestMethod(), TestCategory("Data"), TestCategory("DataReader")]
    public void SafeRead_ReadInt_ResultOK() {
      int actual;
      using ( IDataReader reader = Data.CreateDataReader() ) {
        reader.Read();
        actual = reader.SafeRead<int>("ID", 0);
      }
      Assert.AreEqual(1, actual);
    }

    /// <summary>
    ///A test for SafeRead
    ///</summary>
    [TestMethod(), TestCategory("Data"), TestCategory("DataReader")]
    public void SafeRead_ReadFloat_ResultOK() {
      float actual;
      using ( IDataReader reader = Data.CreateDataReader() ) {
        reader.Read();
        actual = reader.SafeRead<float>("Price", 0F);
      }
      Assert.AreEqual(28.36F, actual);
    }

    /// <summary>
    ///A test for SafeRead
    ///</summary>
    [TestMethod(), TestCategory("Data"), TestCategory("DataReader")]
    public void SafeRead_ReadDouble_ResultOK() {
      double actual;
      using ( IDataReader reader = Data.CreateDataReader() ) {
        reader.Read();
        actual = reader.SafeRead<double>("Price", 0D);
      }
      Assert.AreEqual(28.36D, actual);
    }

    /// <summary>
    ///A test for SafeRead
    ///</summary>
    [TestMethod(), TestCategory("Data"), TestCategory("DataReader")]
    public void SafeRead_ReadDateTime_ResultOK() {
      DateTime actual;
      using ( IDataReader reader = Data.CreateDataReader() ) {
        reader.Read();
        actual = reader.SafeRead<DateTime>("Date", DateTime.MinValue);
      }
      Assert.AreEqual(new DateTime(1966,4,28), actual);
    }

    /// <summary>
    ///A test for SafeRead
    ///</summary>
    [TestMethod(), TestCategory("Data"), TestCategory("DataReader")]
    public void SafeRead_ReadString_ResultDefault() {
      string actual;
      using ( IDataReader reader = Data.CreateDataReader() ) {
        reader.Read();
        reader.Read();
        reader.Read();
        reader.Read();
        actual = reader.SafeRead<string>("Name", "missing");
      }
      Assert.AreEqual("missing", actual);
    }

    /// <summary>
    ///A test for SafeRead
    ///</summary>
    [TestMethod(), TestCategory("Data"), TestCategory("DataReader")]
    public void SafeRead_ReadInt_ResultDefault() {
      int actual;
      using ( IDataReader reader = Data.CreateDataReader() ) {
        reader.Read();
        reader.Read();
        actual = reader.SafeRead<int>("ID", -1);
      }
      Assert.AreEqual(-1, actual);
    }

    /// <summary>
    ///A test for SafeRead
    ///</summary>
    [TestMethod(), TestCategory("Data"), TestCategory("DataReader")]
    public void SafeRead_ReadFloat_ResultDefault() {
      float actual;
      using ( IDataReader reader = Data.CreateDataReader() ) {
        reader.Read();
        reader.Read();
        reader.Read();
        actual = reader.SafeRead<float>("Price", -1F);
      }
      Assert.AreEqual(-1F, actual);
    }

    /// <summary>
    ///A test for SafeRead
    ///</summary>
    [TestMethod(), TestCategory("Data"), TestCategory("DataReader")]
    public void SafeRead_ReadDouble_ResultDefault() {
      double actual;
      using ( IDataReader reader = Data.CreateDataReader() ) {
        reader.Read();
        reader.Read();
        reader.Read();
        actual = reader.SafeRead<double>("Price", -1D);
      }
      Assert.AreEqual(-1D, actual);
    }

    /// <summary>
    ///A test for SafeRead
    ///</summary>
    [TestMethod(), TestCategory("Data"), TestCategory("DataReader")]
    public void SafeRead_ReadDateTimeDBNull_ResultDefault() {
      DateTime actual;
      using ( IDataReader reader = Data.CreateDataReader() ) {
        reader.Read();
        reader.Read();
        actual = reader.SafeRead<DateTime>("Date", DateTime.MinValue);
      }
      Assert.AreEqual(DateTime.MinValue, actual);
    }

  }
}
