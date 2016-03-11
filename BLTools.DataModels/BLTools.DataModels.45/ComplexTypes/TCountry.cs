using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLTools.DataModels {
  public class TCountry {
    #region Public properties
    public enum EAvailableCode {
      Unknown,
      BE,
      FR,
      DE,
      NL,
      US,
      UK,
      SP
    }

    static readonly public Dictionary<EAvailableCode, List<string>> AvailableCountries = new Dictionary<EAvailableCode, List<string>>() {
      {EAvailableCode.Unknown, new List<string>() {"Inconnu", "Unknown", "???"}},
      {EAvailableCode.BE, new List<string>() {"Belgique", "Belgium", "Belgïe"}},
      {EAvailableCode.FR, new List<string>() {"France", "France", "France"}},
      {EAvailableCode.NL, new List<string>() {"Pays-Bas", "Netherlands", "Nederland"}},
      {EAvailableCode.DE, new List<string>() {"Allemangne", "Germany", "Duitsland"}},
      {EAvailableCode.UK, new List<string>() {"Royaume-Uni", "United Kingdom", "UK"}},
      {EAvailableCode.US, new List<string>() {"Etats-unis", "United-states", "Verenigde Staten"}},
      {EAvailableCode.SP, new List<string>() {"Espagne", "Spain", "Spanje"}}
    };

    public EAvailableCode Code { get; set; }
    public string Name { get { return AvailableCountries[Code].ElementAt((int)EDescriptionLanguage.French); } } 
    #endregion Public properties

    #region Constructor(s)
    public TCountry() {
      Code = EAvailableCode.Unknown;
    }

    public TCountry(EAvailableCode countryCode) {
      Code = countryCode;
    } 

    public TCountry(TCountry country) {
      Code = country.Code;
    }
    #endregion Constructor(s)

    #region Public methods
    public string GetName(EDescriptionLanguage languageId = EDescriptionLanguage.French) {
      return AvailableCountries[Code].ElementAt((int)languageId);
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
