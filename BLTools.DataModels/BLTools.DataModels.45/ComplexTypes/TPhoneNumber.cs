using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BLTools;

namespace BLTools.DataModels {
  /// <summary>
  /// describes a phone number with all components
  /// </summary>
  public class TPhoneNumber {
    #region Public properties
    /// <summary>
    /// The international component of the phone number, validated against a list of valid prefixes
    /// </summary>
    public int InternationalCode {
      get { return _InternationalCode; }
      set {
        if (!AvailableInternationalCodes.ContainsKey(value)) {
          string Msg = string.Format("Attempt to assign unavailable international phone prefix : {0}", value);
          Trace.WriteLine(Msg, Severity.Error);
          throw new ArgumentException(Msg, "InternationalCode");
        }
        _InternationalCode = value;
      }
    }
    private int _InternationalCode;
    /// <summary>
    /// The zone prefix
    /// </summary>
    public int Prefix { get; set; }
    /// <summary>
    /// The number itself
    /// </summary>
    public string Number { get; set; }
    /// <summary>
    /// A possible extension
    /// </summary>
    public string Extension { get; set; }
    /// <summary>
    /// A composition of other components to return the full number
    /// </summary>
    public string FullNumber {
      get {
        return string.Format("{0}{1}{2}{3}",
          InternationalCode == 0 ? "" : (string.Format("+{0}", InternationalCode)),
          Prefix == 0 ? "" : string.Format("({0})", Prefix),
          Number ?? "",
          Extension ?? ""
        );
      }
    }
    /// <summary>
    /// The dictionnary of available international prefixes
    /// </summary>
    static readonly public Dictionary<int, List<string>> AvailableInternationalCodes = new Dictionary<int, List<string>>() {
      //TODO: fill the list from official international codes
      {0, new List<string>() {"Inconnu", "Unknown", "???"}},
      {32, new List<string>() {"Belgique", "Belgium", "Belgïe"}},
      {33, new List<string>() {"France", "France", "France"}},
      {31, new List<string>() {"Pays-Bas", "Netherlands", "Nederland"}},
      {49, new List<string>() {"Allemangne", "Germany", "Duitsland"}},
      {44, new List<string>() {"Royaume-Uni", "United Kingdom", "UK"}},
      {34, new List<string>() {"Espagne", "Spain", "Spanje"}},
      {352, new List<string>() {"Luxembourg", "Luxembourg", "Luxemburg"}}
    };
    #endregion Public properties

    #region Constructor(s)
    /// <summary>
    /// Empty constructor
    /// </summary>
    public TPhoneNumber() {
      InternationalCode = 0;
      Prefix = 0;
      Number = "";
      Extension = "";
    }

    /// <summary>
    /// Builds a phone number from its components
    /// </summary>
    /// <param name="internationalCode">The international prefix (see the dictionnary)</param>
    /// <param name="prefix">The zone prefix</param>
    /// <param name="number">The number itself</param>
    /// <param name="extension">The optional extension</param>
    public TPhoneNumber(int internationalCode, int prefix, string number, string extension = "")
      : this() {
      InternationalCode = internationalCode;
      Prefix = prefix;
      Number = number;
      Extension = extension;
    }

    public TPhoneNumber(string number) : this() {
      Number = number;
    }

    public TPhoneNumber(TPhoneNumber number) {
      InternationalCode = number.InternationalCode;
      Prefix = number.Prefix;
      Number = number.Number;
      Extension = number.Extension;
    }
    #endregion Constructor(s)

    #region Converters
    /// <summary>
    /// Gets a string version of the object without details
    /// </summary>
    /// <returns>A string version of the object</returns>
    public override string ToString() {
      return ToString(false);
    }

    /// <summary>
    /// Gets a string version of the object with optional details
    /// </summary>
    /// <param name="withDetails">true for details, false otherwise</param>
    /// <param name="languageId">the id of the language used for details</param>
    /// <returns>A string version of the object with optional details</returns>
    public string ToString(bool withDetails, EDescriptionLanguage languageId = EDescriptionLanguage.French) {
      StringBuilder RetVal = new StringBuilder();
      RetVal.Append(FullNumber);
      if (withDetails) {
        RetVal.AppendFormat(" ({0})", GetInternationalCountryName(languageId));
      }
      return RetVal.ToString();
    }
    #endregion Converters

    #region Public methods
    /// <summary>
    /// Gets the name of country of the prefix
    /// </summary>
    /// <param name="languageId">The id of the language to return the country name</param>
    /// <returns>The name of the country in international prefix</returns>
    public string GetInternationalCountryName(EDescriptionLanguage languageId = EDescriptionLanguage.French) {
      return AvailableInternationalCodes[InternationalCode].ElementAt((int)languageId);
    }
    #endregion Public methods
  }
}
