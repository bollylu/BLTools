using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BLTools;
using System.Xml.Linq;

namespace BLTools.Reports {
  public class TOperator : IToXml {

    internal const string TAG_THIS_ELEMENT = "Operator";
    internal const string TAG_ATTRIBUTE_ID = "Id";
    internal const string TAG_ATTRIBUTE_NAME = "Name";
    internal const string TAG_ATTRIBUTE_EMAIL = "Email";
    internal const string TAG_ELEMENT_DESCRIPTION = "Description";


    #region Public properties
    public string Name { get; set; }
    public string Id { get; set; }
    public string Description { get; set; }
    public string Email { get; set; }
    #endregion Public properties

    #region Constructor(s)
    public TOperator() {
      Id = "";
      Name = "";
      Description = "";
      Email = "";
    }

    public TOperator(string id = "", string name = "", string description = "", string email = "")
      : this() {
      Id = id;
      Name = name;
      Description = description;
      Email = email;
    }

    public TOperator(TOperator otherOperator)
      : this() {
      Id = otherOperator.Id;
      Name = otherOperator.Name;
      Description = otherOperator.Description;
      Email = otherOperator.Email;
    }

    public TOperator(XElement xmlOperator)
      : this() {
      Id = xmlOperator.SafeReadAttribute<string>(TAG_ATTRIBUTE_ID);
      Name = xmlOperator.SafeReadAttribute<string>(TAG_ATTRIBUTE_NAME);
      Description = xmlOperator.SafeReadElementValue<string>(TAG_ELEMENT_DESCRIPTION);
      Email = xmlOperator.SafeReadElementValue<string>(TAG_ATTRIBUTE_EMAIL);
    }
    #endregion Constructor(s)

    #region Converters
    public override string ToString() {
      StringBuilder RetVal = new StringBuilder();
      RetVal.AppendFormat("Operator: Id=\"{0}\"", Id);
      RetVal.AppendFormat(", Name=\"{0}\"", Name);
      RetVal.AppendFormat(", Description=\"{0}\"", Description);
      RetVal.AppendFormat(", Email=\"{0}\"", Email);
      return RetVal.ToString();
    }

    public XElement ToXml() {
      XElement RetVal = new XElement(TAG_THIS_ELEMENT);
      RetVal.SetAttributeValue(TAG_ATTRIBUTE_ID, Id);
      RetVal.SetAttributeValue(TAG_ATTRIBUTE_NAME, Name);
      RetVal.SetElementValue(TAG_THIS_ELEMENT, Description);
      RetVal.SetElementValue(TAG_ATTRIBUTE_EMAIL, Email);
      return RetVal;
    }
    #endregion Converters
  }
}
