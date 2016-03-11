using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLTools.DataModels {
  /// <summary>
  /// Describes a person and its contact info
  /// </summary>
  public class TContactPerson {
    /// <summary>
    /// Unique identifier
    /// </summary>
    public string Id { get; set; }
    /// <summary>
    /// Full name component
    /// </summary>
    public TPersonName Name { get; set; }
    /// <summary>
    /// Full address component
    /// </summary>
    public TAddress Address { get; set; }
    /// <summary>
    /// Phone number 1
    /// </summary>
    public TPhoneNumber Phone1 { get; set; }
    /// <summary>
    /// Phone number 2
    /// </summary>
    public TPhoneNumber Phone2 { get; set; }
    /// <summary>
    /// Fax number
    /// </summary>
    public TPhoneNumber Fax { get; set; }
    /// <summary>
    /// Email address 1
    /// </summary>
    public string Email1 { get; set; }
    /// <summary>
    /// Email address 2
    /// </summary>
    public string Email2 { get; set; }
    /// <summary>
    /// Language
    /// </summary>
    public TLanguage Language { get; set; }

    public TContactPerson() {
      Name = new TPersonName();
      Address = new TAddress();
      Phone1 = new TPhoneNumber();
      Phone2 = new TPhoneNumber();
      Fax = new TPhoneNumber();
      Language = new TLanguage();
    }

    public TContactPerson(string firstname, string lastname) : this() {
      Name.FirstName = firstname;
      Name.LastName = lastname;
    }

    public TContactPerson(TContactPerson contact) {
      Id = contact.Id;
      Name = new TPersonName(contact.Name);
      Address = new TAddress(contact.Address);
      Phone1 = new TPhoneNumber(contact.Phone1);
      Phone2 = new TPhoneNumber(contact.Phone2);
      Fax = new TPhoneNumber(contact.Fax);
      Email1 = contact.Email1;
      Email2 = contact.Email2;
    }
  }
}
