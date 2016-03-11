using Microsoft.Crm.Sdk.Messages;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Messages;
using Microsoft.Xrm.Sdk.Metadata;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLTools.CRM2011 {
  public class TOptionSetGlobal : Dictionary<int, string> {

    public static TCrmDatabase DefaultDatabase;

    public string Name { get; protected set; }
    public string DisplayName { get; protected set; }
    public Guid Id { get; protected set; }
    public bool IsGlobal { get; protected set; }

    #region Constructor(s)
    public TOptionSetGlobal() {
      Name = "";
      DisplayName = "";
    }
    public TOptionSetGlobal(string name, string displayName="") : this() {
      Name = name;
      DisplayName = displayName;
    }
    #endregion Constructor(s)

    public override string ToString() {
      StringBuilder RetVal = new StringBuilder();
      RetVal.Append(string.Format("{0} - {1}", Name, DisplayName));
      RetVal.Append(string.Format("{0}", IsGlobal ? " (G)" : ""));
      RetVal.AppendLine();
      foreach (KeyValuePair<int, string> OptionSetItem in this) {
        RetVal.AppendLine(string.Format("  {0}, {1}", OptionSetItem.Key, OptionSetItem.Value));
      }
      return RetVal.ToString();
    }

    public int GetKeyValue(string textValue) {
      if (this.Count == 0) {
        return -1;
      }
      foreach (KeyValuePair<int, string> Item in this) {
        if (Item.Value.ToLower() == textValue.ToLower()) {
          return Item.Key;
        }
      }
      return -1;
    }

    public string GetStringValue(int keyValue) {
      if (this.Count == 0) {
        return "";
      }
      foreach (KeyValuePair<int, string> Item in this) {
        if (Item.Key == keyValue) {
          return Item.Value;
        }
      }
      return "";
    }

    public static List<TOptionSetGlobal> ReadAllGlobal() {
      if (DefaultDatabase == null) {
        return null;
      }

      List<TOptionSetGlobal> RetVal = new List<TOptionSetGlobal>();

      RetrieveAllOptionSetsRequest Request = new RetrieveAllOptionSetsRequest();
      RetrieveAllOptionSetsResponse Response = (RetrieveAllOptionSetsResponse)DefaultDatabase.OSP.Execute(Request);

      if (Response.OptionSetMetadata.Count() == 0) {
        return RetVal;
      }

      foreach (OptionSetMetadataBase OptionSetMetadataItem in Response.OptionSetMetadata) {
        TOptionSetGlobal NewOptionSet;
        switch (OptionSetMetadataItem.OptionSetType) {
          case OptionSetType.Picklist:
            NewOptionSet = new TOptionSetGlobal(OptionSetMetadataItem.Name, OptionSetMetadataItem.DisplayName.UserLocalizedLabel == null ? "" : OptionSetMetadataItem.DisplayName.UserLocalizedLabel.Label);
            foreach (OptionMetadata OptionMetadataItem in ((OptionSetMetadata)OptionSetMetadataItem).Options) {
              NewOptionSet.Add(OptionMetadataItem.Value.Value , OptionMetadataItem.Label.UserLocalizedLabel.Label.ToString());
              NewOptionSet.Id = OptionSetMetadataItem.MetadataId.Value;
              NewOptionSet.IsGlobal = OptionSetMetadataItem.IsGlobal.Value;
            }
            RetVal.Add(NewOptionSet);
            break;
          case OptionSetType.Boolean:
            NewOptionSet = new TOptionSetGlobal(OptionSetMetadataItem.Name, OptionSetMetadataItem.DisplayName.UserLocalizedLabel == null ? "" : OptionSetMetadataItem.DisplayName.UserLocalizedLabel.Label);
            BooleanOptionSetMetadata BooleanOptionSetMetadataItem = (BooleanOptionSetMetadata)OptionSetMetadataItem;
            NewOptionSet.Add(BooleanOptionSetMetadataItem.TrueOption.Value.Value , BooleanOptionSetMetadataItem.TrueOption.Label.UserLocalizedLabel.Label);
            NewOptionSet.Add(BooleanOptionSetMetadataItem.FalseOption.Value.Value, BooleanOptionSetMetadataItem.FalseOption.Label.UserLocalizedLabel.Label);
            NewOptionSet.Id = OptionSetMetadataItem.MetadataId.Value;
            NewOptionSet.IsGlobal = OptionSetMetadataItem.IsGlobal.Value;
            RetVal.Add(NewOptionSet);
            break;
          default:
            break;
        }
      }
      return RetVal;
    }
  }
}
