using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLTools.ConsoleExtension {
  /// <summary>
  /// Extensions to the console
  /// </summary>
  public static partial class ConsoleExtension {
    /// <summary>
    /// Display a message to the console and wait for an answer. Return the entered value converted to requested type or default value for this type in case of convert error
    /// </summary>
    /// <typeparam name="T">Requested type of the return value</typeparam>
    /// <param name="questionMessage">Message to display on the console</param>
    /// <param name="optionFlags">Validation option for the answer (mandatory, alpha, ...)</param>
    /// <param name="errorMessage">Message to display in case of error</param>
    /// <returns>Entered value converted to the requested type or default for this type in case of convert error</returns>
    static public T Input<T>(string questionMessage = "", EInputValidation optionFlags = EInputValidation.Unknown, string errorMessage = "") {
      bool IsOk = true;
      string AnswerAsString = "";
      do {

        if (optionFlags.HasFlag(EInputValidation.Mandatory)) {
          Console.Write(string.Format("{0}{1}", "* ", questionMessage));
        } else {
          Console.Write(questionMessage);
        }

        AnswerAsString = Console.ReadLine();

        if (optionFlags.HasFlag(EInputValidation.Mandatory) && AnswerAsString == "") {
          IsOk = false;
        } else if (optionFlags.HasFlag(EInputValidation.IsNumeric) && !AnswerAsString.IsNumeric()) {
          IsOk = false;
        } else if (optionFlags.HasFlag(EInputValidation.IsAlpha) && !AnswerAsString.IsAlpha()) {
          IsOk = false;
        } else if (optionFlags.HasFlag(EInputValidation.IsAlphaNumeric) && !AnswerAsString.IsAlphaNumeric()) {
          IsOk = false;
        } else if (optionFlags.HasFlag(EInputValidation.IsAlphaNumericAndSpacesAndDashes) && !AnswerAsString.IsAlphaNumericOrBlankOrDashes()) {
          IsOk = false;
        } else {
          IsOk = true;
        }

        if (!IsOk) {
          Console.WriteLine(errorMessage);
        }

      } while (!IsOk);

      return BLConverter.BLConvert<T>(AnswerAsString, System.Globalization.CultureInfo.CurrentCulture, default(T));
    }
  }
}
