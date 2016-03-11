using BLTools;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Security;
using System.Text;

namespace UnitTest2015 {


  /// <summary>
  ///This is a test class for StringBuilderExtension and is intended
  ///to contain all StringExtensionTest Unit Tests
  ///</summary>
  [TestClass()]
  public class StringBuilderExtensionTest {

    #region Test context
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
    #endregion Test context

    #region Truncate
    [TestMethod(), TestCategory("StringBuilder")]
    public void StringBuilderExtension_Truncate_ResultOK() {
      StringBuilder source = new StringBuilder("A brown fox jumps over a lazy dog");
      int length = 4;
      string expected = "A brown fox jumps over a lazy";
      StringBuilder actual = source.Truncate(length);
      Assert.AreEqual(expected.ToString(), actual.ToString());
    }
    #endregion Truncate

    #region Trim
    [TestMethod(), TestCategory("StringBuilder")]
    public void StringBuilderExtension_Trim_ResultOK() {
      StringBuilder source = new StringBuilder("A brown fox jumps over a lazy dog  ");
      string expected = "A brown fox jumps over a lazy dog";
      StringBuilder actual = source.Trim();
      Assert.AreEqual(expected.ToString(), actual.ToString());
    }

    [TestMethod(), TestCategory("StringBuilder")]
    public void StringBuilderExtension_TrimLeft_ResultOK() {
      StringBuilder source = new StringBuilder("  A brown fox jumps over a lazy dog  ");
      string expected = "A brown fox jumps over a lazy dog  ";
      StringBuilder actual = source.TrimLeft();
      Assert.AreEqual(expected.ToString(), actual.ToString());
    }

    [TestMethod(), TestCategory("StringBuilder")]
    public void StringBuilderExtension_TrimAll_ResultOK() {
      StringBuilder source = new StringBuilder("  A brown fox jumps over a lazy dog  ");
      string expected = "A brown fox jumps over a lazy dog";
      StringBuilder actual = source.TrimAll();
      Assert.AreEqual(expected.ToString(), actual.ToString());
    }

    [TestMethod(), TestCategory("StringBuilder")]
    public void StringBuilderExtension_TrimWithChars_ResultOK() {
      StringBuilder source = new StringBuilder("A brown fox jumps over a lazy dog *+");
      string expected = "A brown fox jumps over a lazy dog";
      StringBuilder actual = source.Trim('*', '+', ' ');
      Assert.AreEqual(expected.ToString(), actual.ToString());
    }

    [TestMethod(), TestCategory("StringBuilder")]
    public void StringBuilderExtension_TrimLeftWithChars_ResultOK() {
      StringBuilder source = new StringBuilder("===  A brown fox jumps over a lazy dog  ");
      string expected = "A brown fox jumps over a lazy dog  ";
      StringBuilder actual = source.TrimLeft('=', ' ');
      Assert.AreEqual(expected.ToString(), actual.ToString());
    }

    [TestMethod(), TestCategory("StringBuilder")]
    public void StringBuilderExtension_TrimAllWithChars_ResultOK() {
      StringBuilder source = new StringBuilder("*** A brown fox jumps over a lazy dog ***");
      string expected = "A brown fox jumps over a lazy dog";
      StringBuilder actual = source.TrimAll(' ', '*');
      Assert.AreEqual(expected.ToString(), actual.ToString());
    }
    #endregion Trim

  }
}
