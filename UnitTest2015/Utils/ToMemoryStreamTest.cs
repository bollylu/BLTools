using System;
using BLTools;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace UnitTest2015.Utils {
  [TestClass]
  public class MemoryStreamTest {

    #region --- String to stream --------------------------------------------
    [TestMethod(), TestCategory("String to MemoryStream")]
    public void ToStream_EmptyString_ResultStream() {
      string SourceValue = "";
      MemoryStream Result = SourceValue.ToStream();
      Assert.IsNotNull(Result);
      using ( TextReader Reader = new StreamReader(Result, Encoding.UTF8) ) {
        Assert.AreEqual(Result.Length, 0);
      }
    }

    [TestMethod(), TestCategory("String to MemoryStream")]
    public void ToStream_EmptyStringWithEncoding_ResultStream() {
      string SourceValue = "";
      MemoryStream Result = SourceValue.ToStream(Encoding.UTF8);
      Assert.IsNotNull(Result);
      using ( TextReader Reader = new StreamReader(Result, Encoding.UTF8) ) {
        Assert.AreEqual(Result.Length, 0);
      }
    }

    [TestMethod(), TestCategory("String to MemoryStream")]
    public void ToStream_ValidString_ResultStreamOk() {
      string SourceValue = "123ABCéèà";
      MemoryStream TempStream = SourceValue.ToStream();
      Assert.IsNotNull(TempStream);
      using ( TextReader Reader = new StreamReader(TempStream, Encoding.UTF8) ) {
        string Result = Reader.ReadToEnd();
        Assert.AreEqual(SourceValue, Result);
      }
    }

    [TestMethod(), TestCategory("String to MemoryStream")]
    public void ToStream_ValidStringWithEncoding_ResultStreamOk() {
      string SourceValue = "123ABCéèà";
      MemoryStream TempStream = SourceValue.ToStream(Encoding.UTF8);
      Assert.IsNotNull(TempStream);
      using ( TextReader Reader = new StreamReader(TempStream, Encoding.UTF8) ) {
        string Result = Reader.ReadToEnd();
        Assert.AreEqual(SourceValue, Result);
      }
    }

    [TestMethod(), TestCategory("String to MemoryStream")]
    public void ToStream_ValidStringWithWrongEncoding_ResultStreamNotOk() {
      string SourceValue = "123ABCéèà";
      MemoryStream TempStream = SourceValue.ToStream(Encoding.Default);
      Assert.IsNotNull(TempStream);
      using ( TextReader Reader = new StreamReader(TempStream, Encoding.UTF8) ) {
        string Result = Reader.ReadToEnd();
        Assert.AreNotEqual(SourceValue, Result);
      }
    }


    [TestMethod(), TestCategory("String to MemoryStream")]
    public async Task ToStreamAsync_EmptyString_ResultStream() {
      string SourceValue = "";
      MemoryStream Result = await SourceValue.ToStreamAsync();
      Assert.IsNotNull(Result);
      using ( TextReader Reader = new StreamReader(Result, Encoding.UTF8) ) {
        Assert.AreEqual(Result.Length, 0);
      }
    }

    [TestMethod(), TestCategory("String to MemoryStream")]
    public async Task ToStreamAsync_EmptyStringWithEncoding_ResultStream() {
      string SourceValue = "";
      MemoryStream Result = await SourceValue.ToStreamAsync(Encoding.UTF8);
      Assert.IsNotNull(Result);
      using ( TextReader Reader = new StreamReader(Result, Encoding.UTF8) ) {
        Assert.AreEqual(Result.Length, 0);
      }
    }

    [TestMethod(), TestCategory("String to MemoryStream")]
    public async Task ToStreamAsync_ValidString_ResultStreamOk() {
      string SourceValue = "123ABCéèà";
      MemoryStream TempStream = await SourceValue.ToStreamAsync();
      Assert.IsNotNull(TempStream);
      using ( TextReader Reader = new StreamReader(TempStream, Encoding.UTF8) ) {
        string Result = await Reader.ReadToEndAsync();
        Assert.AreEqual(SourceValue, Result);
      }
    }

    [TestMethod(), TestCategory("String to MemoryStream")]
    public async Task ToStreamAsync_ValidStringWithEncoding_ResultStreamOk() {
      string SourceValue = "123ABCéèà";
      MemoryStream TempStream = await SourceValue.ToStreamAsync(Encoding.UTF8);
      Assert.IsNotNull(TempStream);
      using ( TextReader Reader = new StreamReader(TempStream, Encoding.UTF8) ) {
        string Result = await Reader.ReadToEndAsync();
        Assert.AreEqual(SourceValue, Result);
      }
    }

    [TestMethod(), TestCategory("String to MemoryStream")]
    public async Task ToStreamAsync_ValidStringWithWrongEncoding_ResultStreamNotOk() {
      string SourceValue = "123ABCéèà";
      MemoryStream TempStream = await SourceValue.ToStreamAsync(Encoding.Default);
      Assert.IsNotNull(TempStream);
      using ( TextReader Reader = new StreamReader(TempStream, Encoding.UTF8) ) {
        string Result = await Reader.ReadToEndAsync();
        Assert.AreNotEqual(SourceValue, Result);
      }
    }

    #endregion --- String to stream --------------------------------------------

    #region --- Bytes to stream --------------------------------------------
    [TestMethod(), TestCategory("IEnumerable<byte> to MemoryStream")]
    public void ToStream_EmptyByteArray_ResultStreamOkButEmpty() {
      byte[] SourceValue = new byte[] { };
      MemoryStream Result = SourceValue.ToStream();
      Assert.IsNotNull(Result);
      using ( BinaryReader Reader = new BinaryReader(Result) ) {
        Assert.AreEqual(Result.Length, 0);
      }
    }

    [TestMethod(), TestCategory("IEnumerable<byte> to MemoryStream")]
    public void ToStream_EmptyListOfBytes_ResultStreamOkButEmpty() {
      List<byte> SourceValue = new List<byte>();
      MemoryStream Result = SourceValue.ToStream();
      Assert.IsNotNull(Result);
      using ( BinaryReader Reader = new BinaryReader(Result) ) {
        Assert.AreEqual(Result.Length, 0);
      }
    }

    [TestMethod(), TestCategory("IEnumerable<byte> to MemoryStream")]
    public void ToStream_ValidByteArray_ResultStreamOk() {
      byte[] SourceValue = new byte[] { 0x64, 0x65, 0x66 };
      MemoryStream Result = SourceValue.ToStream();
      Assert.IsNotNull(Result);
      using ( BinaryReader Reader = new BinaryReader(Result) ) {
        Assert.AreEqual(Reader.BaseStream.Length, SourceValue.Length);
        byte[] CheckData = Reader.ReadBytes(SourceValue.Length);
        Assert.AreEqual(CheckData.Length, SourceValue.Length);
        Assert.AreEqual(CheckData.ToHexString(), SourceValue.ToHexString());
      }
    }

    [TestMethod(), TestCategory("IEnumerable<byte> to MemoryStream")]
    public void ToStream_ValidListOfBytes_ResultStreamOk() {
      List<byte> SourceValue = new List<byte>() { 0x64, 0x65, 0x66 };
      MemoryStream Result = SourceValue.ToStream();
      Assert.IsNotNull(Result);
      using ( BinaryReader Reader = new BinaryReader(Result) ) {
        Assert.AreEqual(Reader.BaseStream.Length, SourceValue.Count);
        byte[] CheckData = Reader.ReadBytes(SourceValue.Count);
        Assert.AreEqual(CheckData.Length, SourceValue.Count);
        Assert.AreEqual(CheckData.ToHexString(), SourceValue.ToArray().ToHexString());
      }
    }

    [TestMethod(), TestCategory("IEnumerable<byte> to MemoryStream")]
    public async Task ToStreamAsync_EmptyByteArray_ResultStreamOkButEmpty() {
      byte[] SourceValue = new byte[] { };
      MemoryStream Result = await SourceValue.ToStreamAsync();
      Assert.IsNotNull(Result);
      using ( BinaryReader Reader = new BinaryReader(Result) ) {
        Assert.AreEqual(Result.Length, 0);
      }
    }

    [TestMethod(), TestCategory("IEnumerable<byte> to MemoryStream")]
    public async Task ToStreamAsync_EmptyListOfBytes_ResultStreamOkButEmpty() {
      List<byte> SourceValue = new List<byte>();
      MemoryStream Result = await SourceValue.ToStreamAsync();
      Assert.IsNotNull(Result);
      using ( BinaryReader Reader = new BinaryReader(Result) ) {
        Assert.AreEqual(Result.Length, 0);
      }
    }

    [TestMethod(), TestCategory("IEnumerable<byte> to MemoryStream")]
    public async Task ToStreamAsync_ValidByteArray_ResultStreamOk() {
      byte[] SourceValue = new byte[] { 0x64, 0x65, 0x66 };
      MemoryStream Result = await SourceValue.ToStreamAsync();
      Assert.IsNotNull(Result);
      using ( BinaryReader Reader = new BinaryReader(Result) ) {
        Assert.AreEqual(Reader.BaseStream.Length, SourceValue.Length);
        byte[] CheckData = Reader.ReadBytes(SourceValue.Length);
        Assert.AreEqual(CheckData.Length, SourceValue.Length);
        Assert.AreEqual(CheckData.ToHexString(), SourceValue.ToHexString());
      }
    }

    [TestMethod(), TestCategory("IEnumerable<byte> to MemoryStream")]
    public async Task ToStreamAsync_ValidListOfBytes_ResultStreamOk() {
      List<byte> SourceValue = new List<byte>() { 0x64, 0x65, 0x66 };
      MemoryStream Result = await SourceValue.ToStreamAsync();
      Assert.IsNotNull(Result);
      using ( BinaryReader Reader = new BinaryReader(Result) ) {
        Assert.AreEqual(Reader.BaseStream.Length, SourceValue.Count);
        byte[] CheckData = Reader.ReadBytes(SourceValue.Count);
        Assert.AreEqual(CheckData.Length, SourceValue.Count);
        Assert.AreEqual(CheckData.ToHexString(), SourceValue.ToArray().ToHexString());
      }
    }
    #endregion --- Bytes to stream --------------------------------------------
  }
}
