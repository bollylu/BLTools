using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using BLTools.DataModels;

namespace UnitTest2015 {
  [TestClass]
  public class TCountryTest {

    [TestMethod, TestCategory("ComplexType")]
    public void TestCountry_ConstructorEmpty_DataInitialized() {
      TCountry Country = new TCountry();
      Assert.AreEqual<TCountry.EAvailableCode>(TCountry.EAvailableCode.Unknown, Country.Code);
      Assert.AreEqual<string>("Inconnu", Country.Name);
      Assert.AreEqual<string>("Inconnu", Country.GetName());
      Assert.AreEqual<string>("Inconnu", Country.GetName(EDescriptionLanguage.French));
      Assert.AreEqual<string>("Unknown", Country.GetName(EDescriptionLanguage.English));
    }

    [TestMethod, TestCategory("ComplexType")]
    public void TestCountry_ConstructorBE_DataInitialized() {
      TCountry Country = new TCountry(TCountry.EAvailableCode.BE);
      Assert.AreEqual<TCountry.EAvailableCode>(TCountry.EAvailableCode.BE, Country.Code);
      Assert.AreEqual<string>("Belgique", Country.Name);
      Assert.AreEqual<string>("Belgique", Country.GetName());
      Assert.AreEqual<string>("Belgique", Country.GetName(EDescriptionLanguage.French));
      Assert.AreEqual<string>("Belgium", Country.GetName(EDescriptionLanguage.English));
    }

    [TestMethod, TestCategory("ComplexType")]
    public void TestCountry_ConstructorUS_DataInitialized() {
      TCountry Country = new TCountry(TCountry.EAvailableCode.US);
      Assert.AreEqual<TCountry.EAvailableCode>(TCountry.EAvailableCode.US, Country.Code);
      Assert.AreEqual<string>("Etats-unis", Country.Name);
      Assert.AreEqual<string>("Etats-unis", Country.GetName());
      Assert.AreEqual<string>("Etats-unis", Country.GetName(EDescriptionLanguage.French));
      Assert.AreEqual<string>("United-states", Country.GetName(EDescriptionLanguage.English));
    }

  }
}
