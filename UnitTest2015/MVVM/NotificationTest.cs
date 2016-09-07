using BLTools;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Security;
using BLTools.MVVM;

namespace UnitTest2015 {


  /// <summary>
  ///This is a test class for StringExtensionTest and is intended
  ///to contain all NotificationTest Unit Tests
  ///</summary>
  [TestClass()]
  public class NotificationTest {

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

    #region NotifyProgress

    public class TestNotify : MVVMBase {
      public void Execute() {
        NotifyExecutionProgress("Doing action...");
        NotifyExecutionCompleted("Job done", true);
      }
    }

    [TestMethod(), TestCategory("MVVM")]
    public void ExecutionProgress_HookThroughBaseClass_ResultOK() {
      string ProgressMessage = "";
      string EndMessage = "";
      bool EndStatus = false;
      string Emitter = "";
      MVVMBase.MinTraceLevel = ErrorLevel.Info;
      EventHandler<IntAndMessageEventArgs> ExP = (o, e) => ProgressMessage = e.Message;
      EventHandler<BoolAndMessageEventArgs> ExC = (o, e) => { EndMessage = e.Message; EndStatus = e.Result; Emitter = o.GetType().Name; };
      MVVMBase.OnExecutionProgress += ExP;
      MVVMBase.OnExecutionCompleted += ExC;
      TestNotify Test = new TestNotify();
      Test.Execute();
      MVVMBase.OnExecutionProgress -= ExP;
      MVVMBase.OnExecutionCompleted -= ExC;
      Assert.AreEqual(ProgressMessage, "Doing action...");
      Assert.AreEqual(EndMessage, "Job done");
      Assert.AreEqual(EndStatus, true);
    }

    [TestMethod(), TestCategory("MVVM")]
    public void ExecutionProgress_HookThroughDerivedClass_ResultOK() {
      string Result = "";
      TestNotify Test = new TestNotify();
      TestNotify.MinTraceLevel = ErrorLevel.Info;
      TestNotify.OnExecutionProgress += (o, e) => Result = e.Message;
      Test.Execute();
      TestNotify.OnExecutionProgress -= (o, e) => Result = e.Message;
      Assert.AreEqual(Result, "Doing action...");
    }

    [TestMethod(), TestCategory("MVVM")]
    public void ExecutionProgress_HookThroughDerivedClassUnhookFromBaseClass_ResultOK() {
      string Result = "";
      TestNotify Test = new TestNotify();
      TestNotify.MinTraceLevel = ErrorLevel.Info;
      TestNotify.OnExecutionProgress += (o, e) => Result = e.Message;
      Test.Execute();
      MVVMBase.OnExecutionProgress -= (o, e) => Result = e.Message;
      Assert.AreEqual(Result, "Doing action...");
    }

    #endregion NotifyProgress



  }
}
