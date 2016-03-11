using Microsoft.Xrm.Sdk.Messages;
using Microsoft.Xrm.Sdk.Metadata;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLTools.CRM2011 {
  public class TOptionSet : Dictionary<int, string> {

    public static TCrmDatabase DefaultDatabase;

    public string EntityName { get; private set; }
    public string AttributeName { get; private set; }
    public string LogicalName {get; private set;}

    #region Constructor(s)
    public TOptionSet() {
      EntityName = "";
      AttributeName = "";
      LogicalName = "";
    }
    public TOptionSet(string entityName, string attributeName, string logicalName="") {
      EntityName = entityName;
      AttributeName = attributeName;
      LogicalName = logicalName;
      Load();
    }

    public TOptionSet(TOptionSet optionSet) {
      EntityName = optionSet.EntityName;
      AttributeName = optionSet.AttributeName;
      LogicalName = optionSet.LogicalName;
      foreach (KeyValuePair<int, string> Item in optionSet) {
        this.Add(Item.Key, Item.Value);
      }
    }

    public TOptionSet(IEnumerable<KeyValuePair<int, string>> items, string entityName = "", string attributeName = "", string logicalName ="") {
      EntityName = entityName;
      AttributeName = attributeName;
      LogicalName = logicalName;
      foreach (KeyValuePair<int, string> Item in items) {
        this.Add(Item.Key, Item.Value);
      }
    }
    #endregion Constructor(s)

    public override string ToString() {
      StringBuilder RetVal = new StringBuilder();
      foreach (KeyValuePair<int, string> OptionSetItem in this) {
        RetVal.AppendLine(string.Format("{0}, {1}", OptionSetItem.Key, OptionSetItem.Value));
      }
      return RetVal.ToString();
    }

    public virtual void Load() {
      RetrieveAttributeRequest Request = new RetrieveAttributeRequest() {
        EntityLogicalName = EntityName,
        LogicalName = AttributeName,
        RetrieveAsIfPublished = true
      };

      RetrieveAttributeResponse Response;
      try {
        Response = (RetrieveAttributeResponse)DefaultDatabase.OSP.Execute(Request);
      } catch (Exception ex) {
        Trace.WriteLine(string.Format("Unable to load optionset : Entity name = {0} - Attribute name = {1} : {2}", EntityName, AttributeName, ex.Message));
        return;
      }
      OptionMetadata[] OptionSetArray = null;

      #region Determine which type of metadata
      switch (Response.AttributeMetadata.GetType().Name) {

        case "PicklistAttributeMetadata":
          PicklistAttributeMetadata _PicklistAttributeMetadata = Response.AttributeMetadata as PicklistAttributeMetadata;
          if (_PicklistAttributeMetadata != null) {
            OptionSetArray = _PicklistAttributeMetadata.OptionSet.Options.ToArray();
          }
          break;

        case "StatusAttributeMetadata":
          StatusAttributeMetadata _StatusAttributeMetadata = Response.AttributeMetadata as StatusAttributeMetadata;
          if (_StatusAttributeMetadata != null) {
            OptionSetArray = _StatusAttributeMetadata.OptionSet.Options.ToArray();
          }
          break;

        case "StateAttributeMetadata":
          StateAttributeMetadata _StateAttributeMetadata = Response.AttributeMetadata as StateAttributeMetadata;
          if (_StateAttributeMetadata != null) {
            OptionSetArray = _StateAttributeMetadata.OptionSet.Options.ToArray();
          }
          break;

        case "EntityNameAttributeMetadata":
          EntityNameAttributeMetadata _EntityNameAttributeMetadata = Response.AttributeMetadata as EntityNameAttributeMetadata;
          if (_EntityNameAttributeMetadata != null) {
            OptionSetArray = _EntityNameAttributeMetadata.OptionSet.Options.ToArray();
          }
          break;

      }
      #endregion Determine which type of metadata

      if (OptionSetArray != null) {
        Dictionary<int, string> RetVal = new Dictionary<int, string>();
        foreach (OptionMetadata OptionSetItem in OptionSetArray) {
          if (OptionSetItem.Value != null) {
            this.Add((int)OptionSetItem.Value, OptionSetItem.Label.UserLocalizedLabel.Label);
          }
        }
      }
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

    public static List<TOptionSet> ReadAllGlobal() {
      if (DefaultDatabase == null) {
        return null;
      }

      List<TOptionSet> RetVal = new List<TOptionSet>();

      RetrieveAllOptionSetsRequest Request = new RetrieveAllOptionSetsRequest();
      RetrieveAllOptionSetsResponse Response = (RetrieveAllOptionSetsResponse)DefaultDatabase.OSP.Execute(Request);

      if (Response.OptionSetMetadata.Count() == 0) {
        return RetVal;
      }

      foreach (OptionSetMetadataBase OptionSetMetadataItem in Response.OptionSetMetadata) {
        switch (OptionSetMetadataItem.OptionSetType) {
          case OptionSetType.Picklist:
            TOptionSet NewOptionSet = new TOptionSet("", OptionSetMetadataItem.Name, OptionSetMetadataItem.Name);
            foreach (OptionMetadata OptionMetadataItem in ((OptionSetMetadata)OptionSetMetadataItem).Options) {
              NewOptionSet.Add(OptionMetadataItem.Value.Value , OptionMetadataItem.Label.UserLocalizedLabel.Label.ToString());
            }
            RetVal.Add(NewOptionSet);
            break;
          case OptionSetType.Boolean:
            break;
          default:
            break;
        }
      }
      return RetVal;
    }
  }
}
