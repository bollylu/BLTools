using BLTools;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Net;

namespace UnitTest2015 {

  /// <summary>
  ///This is a test class for IPAddressExtensionTest and is intended
  ///to contain all IPAddressExtensionTest Unit Tests
  ///</summary>
  [TestClass()]
  public class IPAddressExtensionTest {


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

    /// <summary>
    ///A test for GetSubnet
    ///</summary>
    [TestMethod(), TestCategory("Network")]
    public void GetSubnet_NetmaskAsIPAddress_ResultOk() {
      IPAddress ipAddress = new IPAddress(new byte[] { 10, 100, 200, 28 });
      IPAddress netmask = new IPAddress(new byte[] { 255, 255, 255, 0 });
      IPAddress expected = new IPAddress(new byte[] { 10, 100, 200, 0 });
      IPAddress actual = ipAddress.GetSubnet(netmask);
      Assert.AreEqual(expected, actual);
    }

    /// <summary>
    ///A test for GetSubnet
    ///</summary>
    [TestMethod(), TestCategory("Network")]
    public void GetSubnet_NetmaskAsByteArray_ResultOk() {
      IPAddress ipAddress = new IPAddress(new byte[] { 10, 100, 200, 28 });
      byte[] netmask = new byte[] { 255, 255, 255, 0 };
      IPAddress expected = new IPAddress(new byte[] { 10, 100, 200, 0 });
      IPAddress actual = ipAddress.GetSubnet(netmask);
      Assert.AreEqual(expected, actual);
    }
  }
}
