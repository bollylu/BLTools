using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLTools.ConsoleExtension {
  static public partial class ConsoleExtension {

    /// <summary>
    /// Allow a user at the console to select a response from a list
    /// </summary>
    /// <param name="possibleValues">Dictionnary of the possible values</param>
    /// <param name="title">Title of the list</param>
    /// <param name="question">Prompt at the bottom of the list</param>
    /// <param name="errorMessage">Message to display in case of erroneous value</param>
    /// <returns>The key of the selected dictionnary item</returns>
    static public int InputList(Dictionary<int, string> possibleValues, string title = "", string question = "", string errorMessage = "") {
      bool IsOk = true;
      int Answer = -1;
      do {
        Console.WriteLine(string.Format("[--{0}--]", title));

        foreach (KeyValuePair<int, string> ValueItem in possibleValues) {
          Console.WriteLine(string.Format("  {0}. {1}", ValueItem.Key, ValueItem.Value));
        }
        Answer = Input<int>(question, EInputValidation.Unknown);

        if (possibleValues.ContainsKey(Answer)) {
          IsOk = true;
        } else {
          Console.WriteLine(errorMessage);
          IsOk = false;
        }

      } while (!IsOk);

      return Answer;
    }

    /// <summary>
    /// Allow a user at the console to select a response from a list
    /// </summary>
    /// <param name="items">List of the possible values</param>
    /// <param name="title">Title of the list</param>
    /// <param name="question">Prompt at the bottom of the list</param>
    /// <param name="errorMessage">Message to display in case of erroneous value</param>
    /// <returns>The key of the selected item</returns>
    static public int InputList(IEnumerable<string> items, string title = "", string question = "", string errorMessage = "") {
      Dictionary<int, string> DictionaryItems = new Dictionary<int, string>();
      int i = 1;
      foreach (string ItemItem in items) {
        DictionaryItems.Add(i++, ItemItem);
      }
      return InputList(DictionaryItems, title, question, errorMessage);
    }

    

  }
}
