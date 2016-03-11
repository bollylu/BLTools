using BLTools;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Xml.Linq;
using System.Globalization;

namespace UnitTest2015 {

  /// <summary>
  ///This is a test class for XElementExtensionTest and is intended
  ///to contain all XElementExtensionTest Unit Tests
  ///</summary>
  [TestClass()]
  public class XElementExtensionTest {


    private TestContext testContextInstance;
    private XElement testXElement;

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
      testXElement = new XElement("test");
      testXElement.SetAttributeValue("name", "testXElement");
      testXElement.SetAttributeValue("numeroint", "133625");
      testXElement.SetAttributeValue("numerodoubleus", "133.625");
      testXElement.SetAttributeValue("numerodoublefr", "133,625");
      testXElement.SetAttributeValue("badnumero", "13.36.25");
      testXElement.SetAttributeValue("badnumero2", "13,36,25");
      testXElement.SetAttributeValue("datefr", "28/04/1966");
      testXElement.SetAttributeValue("dateus", "04-28-1966");
      testXElement.SetAttributeValue("dateansi", "1966-04-28");
      testXElement.SetElementValue("subelement", "commentaires");
    }
    //
    //Use TestCleanup to run code after each test has run
    //[TestCleanup()]
    //public void MyTestCleanup()
    //{
    //}
    //
    #endregion


    #region Tests for SafeReadAttribute
    #region String
    /// <summary>
    ///A test for SafeReadAttribute
    ///</summary>
    [TestMethod(), TestCategory("Xml"), TestCategory("Linq")]
    public void SafeReadAttribute_ReadString_ResultValid() {
      string name = "name";
      string defaultValue = "";
      string expected = "testXElement";
      string actual = testXElement.SafeReadAttribute<string>(name, defaultValue);
      Assert.AreEqual(expected, actual);
    }

    /// <summary>
    ///A test for SafeReadAttribute
    ///</summary>
    [TestMethod(), TestCategory("Xml"), TestCategory("Linq")]
    public void SafeReadAttribute_ReadString_ResultFromDefault() {
      string name = "xname";
      string defaultValue = "this is the default value";
      string expected = "this is the default value";
      string actual = testXElement.SafeReadAttribute<string>(name, defaultValue);
      Assert.AreEqual(expected, actual);
    } 
    #endregion String

    #region Int
    /// <summary>
    ///A test for SafeReadAttribute
    ///</summary>
    [TestMethod(), TestCategory("Xml"), TestCategory("Linq")]
    public void SafeReadAttribute_ReadInt_ResultValid() {
      string name = "numeroint";
      int defaultValue = 0;
      int expected = 133625;
      int actual = testXElement.SafeReadAttribute<int>(name, defaultValue);
      Assert.AreEqual(expected, actual);
    }

    /// <summary>
    ///A test for SafeReadAttribute
    ///</summary>
    [TestMethod(), TestCategory("Xml"), TestCategory("Linq")]
    public void SafeReadAttribute_ReadInt_ResultFromDefault() {
      string name = "xnumero";
      int defaultValue = 13;
      int expected = 13;
      int actual = testXElement.SafeReadAttribute<int>(name, defaultValue);
      Assert.AreEqual(expected, actual);
    } 
    #endregion Int

    #region Double
    /// <summary>
    ///A test for SafeReadAttribute
    ///</summary>
    [TestMethod(), TestCategory("Xml"), TestCategory("Linq")]
    public void SafeReadAttribute_ReadDoubleFR_ResultValid() {
      string name = "numerodoublefr";
      double defaultValue = 0;
      double expected = 133.625;
      double actual = testXElement.SafeReadAttribute<double>(name, defaultValue, CultureInfo.GetCultureInfo("fr-fr"));
      Assert.AreEqual(expected, actual);
    }

    /// <summary>
    ///A test for SafeReadAttribute
    ///</summary>
    [TestMethod(), TestCategory("Xml"), TestCategory("Linq")]
    public void SafeReadAttribute_ReadDoubleUS_ResultValid() {
      string name = "numerodoubleus";
      double defaultValue = 0;
      double expected = 133.625;
      double actual = testXElement.SafeReadAttribute<double>(name, defaultValue, CultureInfo.GetCultureInfo("en-us"));
      Assert.AreEqual(expected, actual);
    }

    /// <summary>
    ///A test for SafeReadAttribute
    ///</summary>
    [TestMethod(), TestCategory("Xml"), TestCategory("Linq")]
    public void SafeReadAttribute_ReadDouble_ResultFromDefault() {
      string name = "badnumero";
      double defaultValue = 17D;
      double expected = 17D;
      double actual = testXElement.SafeReadAttribute<double>(name, defaultValue);
      Assert.AreEqual(expected, actual);
    }

    /// <summary>
    ///A test for SafeReadAttribute
    ///</summary>
    [TestMethod(), TestCategory("Xml"), TestCategory("Linq")]
    public void SafeReadAttribute_ReadDouble2_ResultFromDefault() {
      string name = "badnumero2";
      double defaultValue = 17D;
      double expected = 17D;
      double actual = testXElement.SafeReadAttribute<double>(name, defaultValue);
      Assert.AreEqual(expected, actual);
    } 
    #endregion Double

    #region Float
    /// <summary>
    ///A test for SafeReadAttribute
    ///</summary>
    [TestMethod(), TestCategory("Xml"), TestCategory("Linq")]
    public void SafeReadAttribute_ReadFloatFR_ResultValid() {
      string name = "numerodoublefr";
      float defaultValue = 0;
      float expected = 133.625F;
      float actual = testXElement.SafeReadAttribute<float>(name, defaultValue, CultureInfo.GetCultureInfo("fr-fr"));
      Assert.AreEqual(expected, actual);
    }

    /// <summary>
    ///A test for SafeReadAttribute
    ///</summary>
    [TestMethod(), TestCategory("Xml"), TestCategory("Linq")]
    public void SafeReadAttribute_ReadFloatUS_ResultValid() {
      string name = "numerodoubleus";
      float defaultValue = 0;
      float expected = 133.625F;
      float actual = testXElement.SafeReadAttribute<float>(name, defaultValue, CultureInfo.GetCultureInfo("en-us"));
      Assert.AreEqual(expected, actual);
    }

    /// <summary>
    ///A test for SafeReadAttribute
    ///</summary>
    [TestMethod(), TestCategory("Xml"), TestCategory("Linq")]
    public void SafeReadAttribute_ReadFloat_ResultFromDefault() {
      string name = "badnumero";
      float defaultValue = 17F;
      float expected = 17F;
      float actual = testXElement.SafeReadAttribute<float>(name, defaultValue);
      Assert.AreEqual(expected, actual);
    }

    /// <summary>
    ///A test for SafeReadAttribute
    ///</summary>
    [TestMethod(), TestCategory("Xml"), TestCategory("Linq")]
    public void SafeReadAttribute_ReadFloat2_ResultFromDefault() {
      string name = "badnumero2";
      float defaultValue = 17F;
      float expected = 17F;
      float actual = testXElement.SafeReadAttribute<float>(name, defaultValue);
      Assert.AreEqual(expected, actual);
    }  
    #endregion Float

    #region DateTime
    /// <summary>
    ///A test for SafeReadAttribute
    ///</summary>
    [TestMethod(), TestCategory("Xml"), TestCategory("Linq")]
    public void SafeReadAttribute_ReadDateTimeFR_ResultValid() {
      string name = "datefr";
      DateTime defaultValue = DateTime.MinValue;
      DateTime expected = new DateTime(1966,4,28);
      DateTime actual = testXElement.SafeReadAttribute<DateTime>(name, defaultValue, CultureInfo.GetCultureInfo("fr-fr"));
      Assert.AreEqual(expected, actual);
    }

    /// <summary>
    ///A test for SafeReadAttribute
    ///</summary>
    [TestMethod(), TestCategory("Xml"), TestCategory("Linq")]
    public void SafeReadAttribute_ReadDateTimeUS_ResultValid() {
      string name = "dateus";
      DateTime defaultValue = DateTime.MinValue;
      DateTime expected = new DateTime(1966, 4, 28);
      DateTime actual = testXElement.SafeReadAttribute<DateTime>(name, defaultValue, CultureInfo.GetCultureInfo("en-us"));
      Assert.AreEqual(expected, actual);
    }

    /// <summary>
    ///A test for SafeReadAttribute
    ///</summary>
    [TestMethod(), TestCategory("Xml"), TestCategory("Linq")]
    public void SafeReadAttribute_ReadDateTimeAnsi_ResultValid() {
      string name = "dateansi";
      DateTime defaultValue = DateTime.MinValue;
      DateTime expected = new DateTime(1966, 4, 28);
      DateTime actual = testXElement.SafeReadAttribute<DateTime>(name, defaultValue, CultureInfo.GetCultureInfo(""));
      Assert.AreEqual(expected, actual);
    }

    /// <summary>
    ///A test for SafeReadAttribute
    ///</summary>
    [TestMethod(), TestCategory("Xml"), TestCategory("Linq")]
    public void SafeReadAttribute_ReadDateTime_ResultFromDefault() {
      string name = "baddate";
      DateTime defaultValue = DateTime.MinValue;
      DateTime expected = DateTime.MinValue;
      DateTime actual = testXElement.SafeReadAttribute<DateTime>(name, defaultValue);
      Assert.AreEqual(expected, actual);
    }

    #endregion DateTime
    #endregion Tests for SafeReadAttribute


    #region Tests for SafeReadElementValue
    /// <summary>
    ///A test for SafeReadElementValue
    ///</summary>
    [TestMethod(), TestCategory("Xml"), TestCategory("Linq")]
    public void SafeReadElementValue_ReadString_ResultValid() {
      string name = "subelement";
      string defaultValue = "";
      string expected = "commentaires";
      string actual = testXElement.SafeReadElementValue<string>(name, defaultValue);
      Assert.AreEqual(expected, actual);
    } 
    #endregion Tests for SafeReadElementValue

    #region Tests for SafeReadElement
    /// <summary>
    ///A test for SafeReadElementValue
    ///</summary>
    [TestMethod(), TestCategory("Xml"), TestCategory("Linq")]
    public void SafeReadElement_ReadExisting_ResultValid() {
      string name = "subelement";
      XElement expected = new XElement("subelement");
      expected.SetValue("commentaires");
      XElement actual = testXElement.SafeReadElement(name);
      Assert.AreEqual(expected.ToString(), actual.ToString());
    }

    /// <summary>
    ///A test for SafeReadElementValue
    ///</summary>
    [TestMethod(), TestCategory("Xml"), TestCategory("Linq")]
    public void SafeReadElement_ReadNotExisting_ResultInvalid() {
      string name = "ssubelement";
      XElement expected = new XElement("ssubelement");
      XElement actual = testXElement.SafeReadElement(name);
      Assert.AreEqual(expected.ToString(), actual.ToString());
    }
    #endregion Tests for SafeReadElement
  }
}
