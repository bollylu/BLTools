using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLTools.DataModels {
  public class TLanguage {

    #region Public properties
    public enum EAvailableLanguage {
      Unknown,
      FR,
      DE,
      NL,
      UK,
      SP
    }

    static readonly public Dictionary<EAvailableLanguage, List<string>> AvailableLanguage = new Dictionary<EAvailableLanguage, List<string>>() {
      {EAvailableLanguage.Unknown, new List<string>() {"Inconnu", "Unknown", "???"}},
      {EAvailableLanguage.FR, new List<string>() {"Français", "French", "Frans"}},
      {EAvailableLanguage.NL, new List<string>() {"Néerlandais", "Dutch", "Nederlands"}},
      {EAvailableLanguage.DE, new List<string>() {"Allemand", "German", "Duits"}},
      {EAvailableLanguage.UK, new List<string>() {"Anglais", "English", "Engels"}},
      {EAvailableLanguage.SP, new List<string>() {"Espagnol", "Spanish", "Spans"}}
    };

    public EAvailableLanguage Code { get; set; }
    public string Name { get { return AvailableLanguage[Code].ElementAt((int)EDescriptionLanguage.French); } }
    #endregion Public properties

    #region Constructor(s)
    public TLanguage() {
      Code = EAvailableLanguage.Unknown;
    }

    public TLanguage(EAvailableLanguage countryCode) {
      Code = countryCode;
    }

    public TLanguage(TLanguage language) {
      Code = language.Code;
    }
    #endregion Constructor(s)

    #region Public methods
    public string GetName(EDescriptionLanguage languageId = EDescriptionLanguage.French) {
      return AvailableLanguage[Code].ElementAt((int)languageId);
    }
    #endregion Public methods

    #region Converters
    public override string ToString() {
      return ToString(false);
    }

    public string ToString(bool withDetails, EDescriptionLanguage languageId = EDescriptionLanguage.French) {
      StringBuilder RetVal = new StringBuilder();
      RetVal.Append(Code);
      if (withDetails) {
        RetVal.AppendFormat(" ({0})", GetName(languageId));
      }
      return RetVal.ToString();
    }
    #endregion Converters

  }

}
