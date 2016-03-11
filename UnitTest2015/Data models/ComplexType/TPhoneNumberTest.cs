using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using BLTools.DataModels;
using System.Linq;

namespace UnitTest2015 {
  [TestClass]
  public class TPhoneNumberTest {

    [TestMethod, TestCategory("ComplexType")]
    public void TestPhoneNumber_ConstructorEmpty_DataInitialized() {
      TPhoneNumber PhoneNumber = new TPhoneNumber();
      Assert.IsTrue(TPhoneNumber.AvailableInternationalCodes.ContainsKey(PhoneNumber.InternationalCode));
      Assert.AreEqual<int>(0, PhoneNumber.Prefix);
      Assert.AreEqual<string>("", PhoneNumber.Number);
      Assert.AreEqual<string>("", PhoneNumber.Extension);
    }

    [TestMethod, TestCategory("ComplexType")]
    public void TestPhoneNumber_ConstructorBelgium_DataInitialized() {
      TPhoneNumber PhoneNumber = new TPhoneNumber(32, 0, "");
      Assert.IsTrue(TPhoneNumber.AvailableInternationalCodes.ContainsKey(PhoneNumber.InternationalCode));
      Assert.AreEqual<int>(0, PhoneNumber.Prefix);
      Assert.AreEqual<string>("", PhoneNumber.Number);
      Assert.AreEqual<string>("", PhoneNumber.Extension);
    }

    [TestMethod, TestCategory("ComplexType")]
    public void TestPhoneNumber_ConstructorBelgiumPrefixNumber_DataInitialized() {
      TPhoneNumber PhoneNumber = new TPhoneNumber(32, 474, "960084");
      Assert.IsTrue(TPhoneNumber.AvailableInternationalCodes.ContainsKey(PhoneNumber.InternationalCode));
      Assert.AreEqual<int>(474, PhoneNumber.Prefix);
      Assert.AreEqual<string>("960084", PhoneNumber.Number);
      Assert.AreEqual<string>("", PhoneNumber.Extension);
    }

    [TestMethod, TestCategory("ComplexType")]
    public void TestPhoneNumber_ConstructorBelgiumPrefixNumberExtension_DataInitialized() {
      TPhoneNumber PhoneNumber = new TPhoneNumber(32, 474, "960084", "18");
      Assert.IsTrue(TPhoneNumber.AvailableInternationalCodes.ContainsKey(PhoneNumber.InternationalCode));
      Assert.AreEqual<int>(474, PhoneNumber.Prefix);
      Assert.AreEqual<string>("960084", PhoneNumber.Number);
      Assert.AreEqual<string>("18", PhoneNumber.Extension);
    }

    [TestMethod, TestCategory("ComplexType")]
    public void TestPhoneNumber_ConstructorBelgiumPrefixNumberExtension_FullNumberIsOk() {
      TPhoneNumber PhoneNumber = new TPhoneNumber(32, 474, "960084");
      Assert.AreEqual<string>("+32(474)960084", PhoneNumber.FullNumber);
    }

    [TestMethod, TestCategory("ComplexType")]
    [ExpectedException(typeof(ArgumentException))]
    public void TestPhoneNumber_InternationalCodeIsWrong_Exception() {
      TPhoneNumber PhoneNumber = new TPhoneNumber();
      PhoneNumber.InternationalCode = 999;
    }

    [TestMethod, TestCategory("ComplexType")]
    public void TestPhoneNumber_InternationalCodeIsGood_AssignationOk() {
      TPhoneNumber PhoneNumber = new TPhoneNumber();
      PhoneNumber.InternationalCode = 49;
      Assert.AreEqual<string>("Germany", PhoneNumber.GetInternationalCountryName(EDescriptionLanguage.English));
    }
  }
}
