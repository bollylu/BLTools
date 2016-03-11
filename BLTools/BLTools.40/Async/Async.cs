using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLTools.Async {
  public class Async {

    #region Simple actions, 1 to 3 parameters
    public static Task ExecuteAsync(Action action) {
      if (action == null) {
        throw new ArgumentNullException("action", "Missing action to execute");
      }
      return Task.Factory.StartNew(action);
    }
    public static Task ExecuteAsync<T1>(Action<T1> action, T1 p1) {
      if (action == null) {
        throw new ArgumentNullException("action", "Missing action to execute");
      }
      return Task.Factory.StartNew(() => action(p1));
    }
    public static Task ExecuteAsync<T1, T2>(Action<T1, T2> action, T1 p1, T2 p2) {
      if (action == null) {
        throw new ArgumentNullException("action", "Missing action to execute");
      }
      return Task.Factory.StartNew(() => action(p1, p2));
    }
    public static Task ExecuteAsync<T1, T2, T3>(Action<T1, T2, T3> action, T1 p1, T2 p2, T3 p3) {
      if (action == null) {
        throw new ArgumentNullException("action", "Missing action to execute");
      }
      return Task.Factory.StartNew(() => action(p1, p2, p3));
    }
    #endregion Simple actions, 1 to 3 parameters

    #region Action, then action, then ...
    public static Task ExecuteAsyncInOrder(params Action[] actions) {
      return ExecuteAsyncInOrder(new List<Action>(actions));
    }
    public static Task ExecuteAsyncInOrder(IEnumerable<Action> actions) {
      if (actions == null) {
        throw new ArgumentNullException("actions", "Missing list of actions to execute");
      }
      Task RunningTask = Task.Factory.StartNew(actions.First(), TaskCreationOptions.LongRunning);

      for (int i = 1; i < actions.Count(); i++) {
        Action NextAction = actions.ElementAt(i);
        RunningTask = RunningTask.ContinueWith((x) => NextAction());
      }
      return RunningTask;
    }
    #endregion Action, then action, then ...

    #region Functions, 0 to 3 parameters
    public static Task<T> ExecuteAsync<T>(Func<T> func) {
      #region Validate parameters
      if (func == null) {
        throw new ArgumentNullException("func", "Missing function to execute");
      }
      #endregion Validate parameters
      return Task.Factory.StartNew(() => func());
    }
    public static Task<T> ExecuteAsync<T, P1>(Func<P1, T> func, P1 p1) {
      #region Validate parameters
      if (func == null) {
        throw new ArgumentNullException("func", "Missing function to execute");
      }
      #endregion Validate parameters
      return Task.Factory.StartNew<T>(() => func(p1));
    }
    public static Task<T> ExecuteAsync<T, P1, P2>(Func<P1, P2, T> func, P1 p1, P2 p2) {
      #region Validate parameters
      if (func == null) {
        throw new ArgumentNullException("func", "Missing function to execute");
      }
      #endregion Validate parameters
      return Task.Factory.StartNew<T>(() => func(p1, p2));
    }
    public static Task<T> ExecuteAsync<T, P1, P2, P3>(Func<P1, P2, P3, T> func, P1 p1, P2 p2, P3 p3) {
      #region Validate parameters
      if (func == null) {
        throw new ArgumentNullException("func", "Missing function to execute");
      }
      #endregion Validate parameters
      return Task.Factory.StartNew<T>(() => func(p1, p2, p3));
    }
    #endregion Functions, 0 to 3 parameters


    public static void ReadTextFile(string filename, EventHandler<StringEventArgs> eventHandler) {
      #region Validate parameters
      if (string.IsNullOrWhiteSpace(filename)) {
        throw new ArgumentException("missing filename", "filename");
      }
      #endregion Validate parameters

      string RetVal = "";

      Action ReadFile = new Action(() => { RetVal = File.ReadAllText(filename); });
      Action Event = new Action(() => { if (eventHandler != null) { eventHandler(null, new StringEventArgs(RetVal)); } });
      Task ReadTask = ExecuteAsyncInOrder(ReadFile, Event);

    }



  }
}
