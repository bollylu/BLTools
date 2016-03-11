using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLTools.ConsoleExtension {
  /// <summary>
  /// Possible values for validation of an input
  /// </summary>
  [Flags]
  public enum EInputValidation {
    /// <summary>
    /// Value is unknonw
    /// </summary>
    Unknown = 0,
    /// <summary>
    /// The request needs an answer in any case
    /// </summary>
    Mandatory=1,
    /// <summary>
    /// The request wait for a numeric only anwser
    /// </summary>
    IsNumeric=2,
    /// <summary>
    /// The request wait for a alpha only answer
    /// </summary>
    IsAlpha=4,
    /// <summary>
    /// The answer could be alpha or numeric
    /// </summary>
    IsAlphaNumeric=8,
    /// <summary>
    /// The answer can contain both alphanumeric, numeric plus dashes and spaces
    /// </summary>
    IsAlphaNumericAndSpacesAndDashes = 16
  }
}
