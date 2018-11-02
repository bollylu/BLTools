using System;
using System.Threading.Tasks;

namespace BLTools.ConsoleExtension {
  static public partial class ConsoleExtension {
    /// <summary>
    /// Display "Press any key to continue..." on console, then wait for a key, possibly with a timeout in msec
    /// </summary>
    /// <param name="timeout">The timeout if no key is pressed</param>
    /// <param name="isAnimated">Will animation run during the pause</param>
    /// <param name="displayTimeout">Display the timeout counter during the pause</param>
    static public void Pause(double timeout = 0, bool isAnimated = false, bool displayTimeout = false) {
      Pause("Press any key to continue...", timeout, isAnimated, displayTimeout);
    }
    /// <summary>
    /// Display a message on console, then wait for a key, possibly with a timeout in msec
    /// </summary>
    /// <param name="message">The message to be displayed</param>
    /// <param name="timeout">The timeout if no key is pressed</param>
    /// <param name="isAnimated">Will animation run during the pause</param>
    /// <param name="displayTimeout">Display the timeout counter during the pause</param>
    static public async void Pause(string message, double timeout = 0, bool isAnimated = false, bool displayTimeout = false) {

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
          await Task.Delay(200);
        }
        if (Console.KeyAvailable) {
          Console.ReadKey(true);
        }
        Console.WriteLine();
      }

    }
  }
}
