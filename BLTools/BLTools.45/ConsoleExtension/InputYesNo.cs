using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BLTools;

namespace BLTools.ConsoleExtension {
  static public partial class ConsoleExtension {

    /// <summary>
    /// Display a question on the console, and wait for a Yes or No answer. Both Yes and No value can be customized. A default value can be specified.
    /// </summary>
    /// <param name="question">The question to display</param>
    /// <param name="defaultValue">The default value if &lt;enter&gt; is pressed (default=Y)</param>
    /// <param name="yesValue">The value for Yes (default=Y)</param>
    /// <param name="noValue">The value for No (default=N)</param>
    /// <returns>Either Yes or No</returns>
    static public EInputYesNo InputYesNo(string question = "", EInputYesNo defaultValue = EInputYesNo.Yes, char yesValue = 'Y', char noValue = 'N') {
      const char CR = '\r';
      List<char> AllowedAnsers = new List<char>() { yesValue.ToString().ToUpper()[0], noValue.ToString().ToUpper()[0], CR };

      Console.Write(string.Format("{0} ({1}) ", question, string.Format("{0}/{1}", defaultValue == EInputYesNo.Yes ? yesValue.ToString().ToUpper() : yesValue.ToString().ToLower(), defaultValue == EInputYesNo.Yes ? noValue.ToString().ToLower() : noValue.ToString().ToUpper())));
      char Key;
      do {
        Key = Console.ReadKey(true).KeyChar.ToString().ToUpper()[0];
      } while (!AllowedAnsers.Contains(Key));

      if (Key == CR) {
        if (defaultValue == EInputYesNo.Yes) {
          Console.WriteLine(yesValue.ToString().ToUpper());
          return EInputYesNo.Yes;
        } else {
          Console.WriteLine(noValue.ToString().ToUpper());
          return EInputYesNo.No;
        }
      } else if (Key == yesValue) {
        Console.WriteLine(Key.ToString().ToUpper());
        return EInputYesNo.Yes;
      } else {
        Console.WriteLine(noValue.ToString().ToUpper());
        return EInputYesNo.No;
      }

    }
  }
}
