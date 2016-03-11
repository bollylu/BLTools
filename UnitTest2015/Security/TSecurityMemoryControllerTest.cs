using BLTools.Security.Authorization;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace UnitTest2015 {
    
    
    /// <summary>
    ///This is a test class for TSecurityMemoryControllerTest and is intended
    ///to contain all TSecurityMemoryControllerTest Unit Tests
    ///</summary>
  [TestClass()]
  public class TSecurityMemoryControllerTest {


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

    [TestMethod(), TestCategory("SecurityController")]
    public void TSecurityMemoryController_ConstructorWithName_AllOK() {
      string Name = "test.xml";
      TSecurityController Target = new TSecurityController(new TSecurityStorageXml(Name));
      Assert.AreEqual(Name, Target.Storage.Name);
      Assert.AreEqual(0, Target.Storage.Users.Count);
      Assert.AreEqual(0, Target.Storage.Groups.Count);
      Assert.AreEqual(0, Target.LoggedInUsersCount);
    }

    [TestMethod(), TestCategory("SecurityController")]
    public void TSecurityMemoryController_UnknonwUserLogonAttempt_LogonFailed() {
      string Name = "test.xml";
      TSecurityController Target = new TSecurityController(new TSecurityStorageXml(Name));

      TSecurityUser User = new TSecurityUser("test");

      Assert.IsFalse(Target.LogOn(User));
    }

    [TestMethod(), TestCategory("SecurityController")]
    public void TSecurityMemoryController_UserLogonAttemptBadPassword_LogonFailed() {
      string Name = "test.xml";
      TSecurityController Target = new TSecurityController(new TSecurityStorageXml(Name));

      TSecurityUser User = new TSecurityUser("test", "pass");
      Target.Storage.Users.Add(User);

      Assert.IsFalse(Target.LogOn(User));
    }

    [TestMethod(), TestCategory("SecurityController")]
    public void TSecurityMemoryController_UserLogonAttempt_LogonOK() {
      string Name = "test.xml";
      TSecurityController Target = new TSecurityController(new TSecurityStorageXml(Name));

      TSecurityUser User = new TSecurityUser("test", "pass");
      Target.Storage.Users.Add(User);

      Assert.IsTrue(Target.LogOn(User, "pass"));
    }

    [TestMethod(), TestCategory("SecurityController")]
    public void TSecurityMemoryController_UserLogonAttemptAlreadyLoggedon_LogonOK() {
      string Name = "test.xml";
      TSecurityController Target = new TSecurityController(new TSecurityStorageXml(Name));

      TSecurityUser User = new TSecurityUser("test", "pass");
      Target.Storage.Users.Add(User);
      Target.LogOn(User, "pass");
      Assert.IsTrue(Target.LogOn(User, "pass"));
    }

    [TestMethod(), TestCategory("SecurityController")]
    public void TSecurityMemoryController_TestifLoggedUser_UserIsLoggedon() {
      string Name = "test.xml";
      TSecurityController Target = new TSecurityController(new TSecurityStorageXml(Name));

      TSecurityUser User = new TSecurityUser("test");
      Target.Storage.Users.Add(User);
      Target.LogOn(User);
      Assert.IsTrue(Target.IsLoggedOn(User));
    }

    [TestMethod(), TestCategory("SecurityController")]
    public void TSecurityMemoryController_TestifLoggedUser_UserIsNotLoggedon() {
      string Name = "test.xml";
      TSecurityController Target = new TSecurityController(new TSecurityStorageXml(Name));

      TSecurityUser User = new TSecurityUser("test");
      TSecurityUser User2 = new TSecurityUser("test2");
      Target.Storage.Users.Add(User);
      Target.LogOn(User);
      Assert.IsFalse(Target.IsLoggedOn(User2));
    }

    [TestMethod(), TestCategory("SecurityController")]
    public void TSecurityMemoryController_UserLogoff_LogoffOK() {
      string Name = "test.xml";
      TSecurityController Target = new TSecurityController(new TSecurityStorageXml(Name));

      TSecurityUser User = new TSecurityUser("test", "pass");
      Target.Storage.Users.Add(User);
      Target.LogOn(User, "pass");
      Assert.IsTrue(Target.LogOn(User, "pass"));
      Assert.IsTrue(Target.IsLoggedOn(User));
      Target.LogOff(User);
      Assert.IsFalse(Target.IsLoggedOn(User));
    }
  }
}
