using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLTools.DataModels {
  /// <summary>
  /// Describes all the components of a full name
  /// </summary>
  public class TPersonName {
    /// <summary>
    /// First name
    /// </summary>
    public string FirstName { get; set; }
    /// <summary>
    /// Middle name if any
    /// </summary>
    public string MiddleName { get; set; }
    /// <summary>
    /// Last name
    /// </summary>
    public string LastName { get; set; }
    /// <summary>
    /// Title
    /// </summary>
    public TPersonTitle Title { get; set; }
    /// <summary>
    /// Returns the full name based on other components
    /// </summary>
    public string FullName {
      get {
        return string.Format("{0}{1}{2}",
          string.IsNullOrWhiteSpace(FirstName) ? "" : FirstName,
          string.IsNullOrWhiteSpace(MiddleName) ? "" : string.Format(" ({0})", MiddleName),
          string.IsNullOrWhiteSpace(LastName) ? "" : " " + LastName);
      }
    }

    #region Constructor(s)
    /// <summary>
    /// Empty constructor
    /// </summary>
    public TPersonName() {
      FirstName = "";
      MiddleName = "";
      LastName = "";
      Title = new TPersonTitle();
    }

    /// <summary>
    /// Build an TPersonName from first and last name
    /// </summary>
    /// <param name="firstName">First name</param>
    /// <param name="lastName">Last name</param>
    public TPersonName(string firstName, string lastName = "")
      : this() {
      FirstName = firstName;
      LastName = lastName;
    }
    public TPersonName(TPersonName person) {
      FirstName = person.FirstName;
      MiddleName = person.MiddleName;
      LastName = person.LastName;
      Title = new TPersonTitle(person.Title);
    }
    #endregion Constructor(s)


    /// <summary>
    /// Return a string version without details
    /// </summary>
    /// <returns>A string v ersion of the object</returns>
    public override string ToString() {
      return ToString(false);
    }

    /// <summary>
    /// Return a string version with possible details
    /// </summary>
    /// <param name="withDetails">true if details requested, false otherwise</param>
    /// <param name="languageId">id of the language used for the details</param>
    /// <returns>A string v ersion of the object</returns>
    public string ToString(bool withDetails, EDescriptionLanguage languageId = EDescriptionLanguage.French) {
      StringBuilder RetVal = new StringBuilder();
      if (!withDetails) {
        RetVal.Append(FullName);
      } else {
        RetVal.AppendFormat("{0} {1}", Title.GetName(languageId), FullName);
      }
      return RetVal.ToString();
    }
  }
}
