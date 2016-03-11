using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using BLTools.DataModels;

namespace UnitTest2015 {
  [TestClass]
  public class TPersonNameTest {

    [TestMethod, TestCategory("ComplexType")]
    public void TestPersonName_ConstructorEmpty_DataInitialized() {
      TPersonName PersonName = new TPersonName();
      Assert.AreEqual<TPersonTitle.EAvailableCode>(TPersonTitle.EAvailableCode.Unknown, PersonName.Title.Code);
      Assert.AreEqual<string>("", PersonName.FirstName);
      Assert.AreEqual<string>("", PersonName.MiddleName);
      Assert.AreEqual<string>("", PersonName.LastName);
      Assert.AreEqual<string>("", PersonName.FullName);
    }

    [TestMethod, TestCategory("ComplexType")]
    public void TestPersonName_ConstructorNotEmpty_DataInitialized() {
      TPersonName PersonName = new TPersonName("Emile", "Dupont");
      Assert.AreEqual<TPersonTitle.EAvailableCode>(TPersonTitle.EAvailableCode.Unknown, PersonName.Title.Code);
      Assert.AreEqual<string>("Emile", PersonName.FirstName);
      Assert.AreEqual<string>("", PersonName.MiddleName);
      Assert.AreEqual<string>("Dupont", PersonName.LastName);
      Assert.AreEqual<string>("Emile Dupont", PersonName.FullName);
    }

    [TestMethod, TestCategory("ComplexType")]
    public void TestPersonName_ConstructorNotEmptyDataAdded_DataInitialized() {
      TPersonName PersonName = new TPersonName("Emile", "Dupont");
      PersonName.MiddleName = "Jr";
      PersonName.Title = new TPersonTitle(TPersonTitle.EAvailableCode.M);
      Assert.AreEqual<TPersonTitle.EAvailableCode>(TPersonTitle.EAvailableCode.M, PersonName.Title.Code);
      Assert.AreEqual<string>("Emile", PersonName.FirstName);
      Assert.AreEqual<string>("Jr", PersonName.MiddleName);
      Assert.AreEqual<string>("Dupont", PersonName.LastName);
      Assert.AreEqual<string>("Emile (Jr) Dupont", PersonName.FullName);
    }

  }
}
