using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BLTools;
using System.Xml.Linq;
using System.Diagnostics;
using System.IO;

namespace BLTools.Reports {
  public class TOperatorCollection : List<TOperator>, IToXml {

    internal const string TAG_THIS_ELEMENT = "Operators";

    public string StorageName { get; set; }

    #region Constructor(s)
    public TOperatorCollection() { }

    public TOperatorCollection(TOperatorCollection operators) {
      foreach (TOperator OperatorItem in operators) {
        Add(new TOperator(OperatorItem));
      }
    }

    public TOperatorCollection(IEnumerable<TOperator> operators) {
      foreach (TOperator OperatorItem in operators) {
        Add(new TOperator(OperatorItem));
      }
    }

    public TOperatorCollection(IEnumerable<XElement> operators) {
      foreach (XElement OperatorItem in operators) {
        Add(new TOperator(OperatorItem));
      }
    }

    public TOperatorCollection(XElement operators) {
      if (operators != null && operators.Elements().Any(e => e.Name == "operator")) {
        foreach (XElement OperatorItem in operators.Elements("operator")) {
          Add(new TOperator(OperatorItem));
        }
      }
    }
    #endregion Constructor(s)

    #region Converters
    public override string ToString() {
      StringBuilder RetVal = new StringBuilder();
      foreach (TOperator OperatorItem in this) {
        RetVal.AppendLine(OperatorItem.ToString());
      }
      return RetVal.ToString();
    }

    public XElement ToXml() {
      XElement RetVal = new XElement("operators");
      foreach (TOperator OperatorItem in this) {
        RetVal.Add(OperatorItem.ToXml());
      }
      return RetVal;
    }
    #endregion Converters

    #region Xml Save/Read
    public void SaveXml() {
      SaveXml(StorageName);
    }
    public void SaveXml(Encoding encoding) {
      SaveXml(StorageName, encoding);
    }
    public void SaveXml(string filename) {
      SaveXml(filename, Encoding.Default);
    }
    public void SaveXml(string filename, Encoding encoding) {
      #region Validate parameters
      if (string.IsNullOrWhiteSpace(filename)) {
        string Msg = "Unable to save data to an empty filename";
        Trace.WriteLine(Msg);
        throw new ArgumentException("filename", Msg);
      }
      #endregion Validate parameters
      XDocument XmlFile = new XDocument();
      XmlFile.Declaration = new XDeclaration("1.0", encoding.EncodingName, "yes");
      XmlFile.Add(new XElement("Root"));
      XmlFile.Element("Root").Add(this.ToXml());
      try {
        TextWriter XmlWriter = new StreamWriter(filename, false, encoding);
        XmlFile.Save(XmlWriter);
      } catch (Exception ex) {
        Trace.WriteLine(string.Format("Error: unable to save the operator list to \"{0}\" : {1}", filename, ex.Message));
      }
    }
    
    public TOperatorCollection ReadXml() {
      TOperatorCollection RetVal = TOperatorCollection.ReadXml(StorageName);
      this.Clear();
      foreach (TOperator OperatorItem in RetVal) {
        this.Add(new TOperator(OperatorItem));
      }
      return RetVal;
    }
    #endregion Xml Save/Read

    public static TOperatorCollection ReadXml(string filename) {
      #region Validate parameters
      if (string.IsNullOrWhiteSpace(filename)) {
        string Msg = "Unable to read data from an empty filename";
        Trace.WriteLine(Msg);
        throw new ArgumentException("filename", Msg);
      }
      if (!File.Exists(filename)) {
        string Msg = string.Format("Unable to read data from \"{0}\": filename is missing or access is denied", filename);
        Trace.WriteLine(Msg);
        throw new ArgumentException("filename", Msg);
      }
      #endregion Validate parameters

      XDocument OperatorFile;
      try {
        OperatorFile = XDocument.Load(filename);
        XElement Operators = OperatorFile.Root.Element(TAG_THIS_ELEMENT);
        TOperatorCollection RetVal = new TOperatorCollection(Operators);
        return RetVal;
      } catch (Exception ex) {
        string Msg = string.Format("Error: unable to load the file content : {0} : {1}", filename, ex.Message);
        Trace.WriteLine(Msg);
        throw new ApplicationException(Msg);
      }

    }
  }
}
