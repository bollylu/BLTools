using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using BLTools.DataModels;

namespace UnitTest2015 {
  [TestClass]
  public class TPersonTitleTest {

    [TestMethod, TestCategory("ComplexType")]
    public void TestPersonTitle_ConstructorEmpty_DataInitialized() {
      TPersonTitle PersonTitle = new TPersonTitle();
      Assert.AreEqual<TPersonTitle.EAvailableCode>(TPersonTitle.EAvailableCode.Unknown, PersonTitle.Code);
      Assert.AreEqual<string>("Inconnu", PersonTitle.GetName());
      Assert.AreEqual<string>("Inconnu", PersonTitle.GetName(EDescriptionLanguage.French));
      Assert.AreEqual<string>("Unknown", PersonTitle.GetName(EDescriptionLanguage.English));
    }

    [TestMethod, TestCategory("ComplexType")]
    public void TestPersonTitle_ConstructorMme_DataInitialized() {
      TPersonTitle PersonTitle = new TPersonTitle(TPersonTitle.EAvailableCode.Mme);
      Assert.AreEqual<TPersonTitle.EAvailableCode>(TPersonTitle.EAvailableCode.Mme, PersonTitle.Code);
      Assert.AreEqual<string>("Madame", PersonTitle.GetName());
      Assert.AreEqual<string>("Madame", PersonTitle.GetName(EDescriptionLanguage.French));
      Assert.AreEqual<string>("Madam", PersonTitle.GetName(EDescriptionLanguage.English));
    }

    [TestMethod, TestCategory("ComplexType")]
    public void TestPersonTitleAbbreviations_ConstructorMme_DataInitialized() {
      TPersonTitle PersonTitle = new TPersonTitle(TPersonTitle.EAvailableCode.Mme);
      Assert.AreEqual<TPersonTitle.EAvailableCode>(TPersonTitle.EAvailableCode.Mme, PersonTitle.Code);
      Assert.AreEqual<string>("Mme", PersonTitle.GetAbbreviation());
      Assert.AreEqual<string>("Mme", PersonTitle.GetAbbreviation(EDescriptionLanguage.French));
      Assert.AreEqual<string>("Miss", PersonTitle.GetAbbreviation(EDescriptionLanguage.English));
    }
  }
}
