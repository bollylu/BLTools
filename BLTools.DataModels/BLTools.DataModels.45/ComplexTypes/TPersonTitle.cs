using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLTools.DataModels {
  public class TPersonTitle {

    public enum EAvailableCode {
      Unknown,
      M,
      Mme
    }

    public EAvailableCode Code { get; set; }

    static readonly public Dictionary<EAvailableCode, List<string>> AvailableTitles = new Dictionary<EAvailableCode, List<string>>() {
      {EAvailableCode.Unknown, new List<string>() {"Inconnu", "Unknown", "???"}},
      {EAvailableCode.M, new List<string>() {"Monsieur", "Sir", "Meneer"}},
      {EAvailableCode.Mme, new List<string>() {"Madame", "Madam", "Mevrouw"}}
    };

    static readonly public Dictionary<EAvailableCode, List<string>> AvailableTitleAbbreviations = new Dictionary<EAvailableCode, List<string>>() {
      {EAvailableCode.Unknown, new List<string>() {"???", "???", "???"}},
      {EAvailableCode.M, new List<string>() {"M.", "Mr", "M."}},
      {EAvailableCode.Mme, new List<string>() {"Mme", "Miss", "Mv"}}
    };

    public string Name { get { return AvailableTitles[Code].ElementAt((int)EDescriptionLanguage.French); } } 

   public TPersonTitle() {
      Code = EAvailableCode.Unknown;
    }

   public TPersonTitle(EAvailableCode titleCode) {
      Code = titleCode;
    } 

    public TPersonTitle(TPersonTitle title) {
      Code = title.Code;
    }


    public string GetName(EDescriptionLanguage languageId = EDescriptionLanguage.French) {
      return AvailableTitles[Code].ElementAt((int)languageId);
    }
    public string GetAbbreviation(EDescriptionLanguage languageId = EDescriptionLanguage.French) {
      return AvailableTitleAbbreviations[Code].ElementAt((int)languageId);
    }

    public override string ToString() {
      return ToString(false);
    }

    public string ToString(bool withDetails, EDescriptionLanguage languageId = EDescriptionLanguage.French) {
      StringBuilder RetVal = new StringBuilder();
      if (!withDetails) {
        RetVal.Append(Code);
      } else {
        RetVal.AppendFormat("{0} - {1} - {2}", Code, GetAbbreviation(languageId), GetName(languageId));
      }
      return RetVal.ToString();
    }
  }
}
