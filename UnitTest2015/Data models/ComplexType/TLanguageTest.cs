using System;
using BLTools.DataModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTest2015 {
  [TestClass]
  public class TLanguageTest {

    [TestMethod, TestCategory("ComplexType")]
    public void TestLanguage_ConstructorEmpty_DataInitialized() {
      TLanguage Language = new TLanguage();
      Assert.AreEqual<TLanguage.EAvailableLanguage>(TLanguage.EAvailableLanguage.Unknown, Language.Code);
      Assert.AreEqual<string>("Inconnu", Language.Name);
      Assert.AreEqual<string>("Inconnu", Language.GetName());
      Assert.AreEqual<string>("Inconnu", Language.GetName(EDescriptionLanguage.French));
      Assert.AreEqual<string>("Unknown", Language.GetName(EDescriptionLanguage.English));
    }

    [TestMethod, TestCategory("ComplexType")]
    public void TestLanguage_ConstructorFrench_DataInitialized() {
      TLanguage Language = new TLanguage(TLanguage.EAvailableLanguage.FR);
      Assert.AreEqual<TLanguage.EAvailableLanguage>(TLanguage.EAvailableLanguage.FR, Language.Code);
      Assert.AreEqual<string>("Français", Language.Name);
      Assert.AreEqual<string>("Français", Language.GetName());
      Assert.AreEqual<string>("Français", Language.GetName(EDescriptionLanguage.French));
      Assert.AreEqual<string>("French", Language.GetName(EDescriptionLanguage.English));
    }

    [TestMethod, TestCategory("ComplexType")]
    public void TestLanguage_ConstructorEnglish_DataInitialized() {
      TLanguage Language = new TLanguage(TLanguage.EAvailableLanguage.UK);
      Assert.AreEqual<TLanguage.EAvailableLanguage>(TLanguage.EAvailableLanguage.UK, Language.Code);
      Assert.AreEqual<string>("Anglais", Language.Name);
      Assert.AreEqual<string>("Anglais", Language.GetName());
      Assert.AreEqual<string>("Anglais", Language.GetName(EDescriptionLanguage.French));
      Assert.AreEqual<string>("English", Language.GetName(EDescriptionLanguage.English));
    }
  }
}
