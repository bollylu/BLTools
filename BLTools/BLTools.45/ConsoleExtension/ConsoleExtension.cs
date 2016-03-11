using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace BLTools {
  static public class ConsoleExtension {
    /// <summary>
    /// Display a message on console, then wait for a key, possibly with a timeout in msec
    /// </summary>
    /// <param name="message">the message to be displayed</param>
    static public void Pause(double timeout = 0, bool isAnimated = false, bool displayTimeout = false) {
      Pause("Press any key to continue...", timeout, isAnimated, displayTimeout);
    }
    /// <summary>
    /// Display a message on console, then wait for a key, possibly with a timeout in msec
    /// </summary>
    /// <param name="message">the message to be displayed</param>
    static public void Pause(string message, double timeout = 0, bool isAnimated = false, bool displayTimeout = false) {
      char[] Roll = new char[] { '|', '/', '-', '\\' };
      Console.Write(message);
      int SaveCursorLeft = Console.CursorLeft;
      int SaveCursorTop = Console.CursorTop;
      if (timeout == 0) {
        Console.ReadKey(true);
      } else {
        DateTime StartTime = DateTime.Now;
        int i = 0;
        while (((DateTime.Now - StartTime) < (TimeSpan.FromMilliseconds(timeout)) && !Console.KeyAvailable)) {
          if (isAnimated) {
            Console.SetCursorPosition(SaveCursorLeft + 1, SaveCursorTop);
            Console.Write(Roll[i++ % 4]);
          }

          if (displayTimeout) {
            Console.SetCursorPosition(SaveCursorLeft + 3, SaveCursorTop);
            double ElapsedTime = (DateTime.Now - StartTime).TotalMilliseconds;
            Console.Write(TimeSpan.FromMilliseconds(timeout - ElapsedTime).ToString("hh\\:mm\\:ss\\:ff"));
          }
          Thread.Sleep(200);
        }
        if (Console.KeyAvailable) {
          Console.ReadKey(true);
        }
        Console.WriteLine();
      }
    }

    ///// <summary>
    ///// Display a message to the console and wait for an answer. Return the entered value converted to requested type or default value for this type in case of convert error
    ///// </summary>
    ///// <typeparam name="T">Requested type of the return value</typeparam>
    ///// <param name="message">Message to display on the console</param>
    ///// <returns>Entered value converted to the requested type or default for this type in case of convert error</returns>
    //static public T Input<T>(string message = "") {
    //  Console.Write(message);
    //  string AnswerAsString = Console.ReadLine();
    //  return BLConverter.BLConvert<T>(AnswerAsString, System.Globalization.CultureInfo.CurrentCulture, default(T));
    //}

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

    static public int InputList(IEnumerable<string> items, string title = "", string question = "", string errorMessage = "") {
      Dictionary<int, string> DictionaryItems = new Dictionary<int, string>();
      int i = 1;
      foreach (string ItemItem in items) {
        DictionaryItems.Add(i++, ItemItem);
      }
      return InputList(DictionaryItems, title, question, errorMessage);
    }

    static public bool InputYesNo(string question = "", string errorMessage = "") {
      bool Answer = true;

      return Answer;
    }

  }
}
