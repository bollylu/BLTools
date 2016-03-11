using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Threading;

namespace BLTools.WPF {
  /// <summary>
  /// Extensions for DispatcherObject
  /// </summary>
  public static class DispatcherExtensions {

    #region Action
    /// <summary>
    /// Execute an action on the UI thread, whatever the current thread.
    /// <example>
    /// MyWindow.Dispatch(()=> { btnOk.IsEnabled = true; } );
    /// </example>
    /// </summary>
    /// <param name="source">A DispatcherObject object</param>
    /// <param name="action">The action to execute on the UI thread</param>
    public static void Dispatch(this DispatcherObject source, Action action) {
      if (source.Dispatcher.CheckAccess()) {
        action();
      } else {
        source.Dispatcher.Invoke(action);
      }
    }

    /// <summary>
    /// Execute an action on the UI thread, whatever the current thread. Allows the priority to be specified.
    /// <example>
    /// MyWindow.Dispatch(()=> { btnOk.IsEnabled = true; }, DispatcherPriority.Background);
    /// </example>
    /// </summary>
    /// <param name="source">A DispatcherObject object</param>
    /// <param name="action">The action to execute on the UI thread</param>
    /// <param name="priority">The priority</param>
    public static void Dispatch(this DispatcherObject source, Action action, DispatcherPriority priority) {
      if (source.Dispatcher.CheckAccess()) {
        action();
      } else {
        source.Dispatcher.Invoke(action, priority);
      }
    }

    /// <summary>
    /// Execute aysnchronously an action on the UI thread, whatever the current thread.
    /// </summary>
    /// <param name="source">A DispatcherObject object</param>
    /// <param name="action">The action to execute on the UI thread</param>
    public static void DispatchAsync(this DispatcherObject source, Action action) {
      if (source.Dispatcher.CheckAccess()) {
        action();
      } else {
        source.Dispatcher.BeginInvoke(action);
      }
    }

    /// <summary>
    /// Execute aysnchronously an action on the UI thread, whatever the current thread. Allows the priority to be specified.
    /// </summary>
    /// <param name="source">A DispatcherObject object</param>
    /// <param name="action">The action to execute on the UI thread</param>
    /// <param name="priority">The priority</param>
    public static void DispatchAsync(this DispatcherObject source, Action action, DispatcherPriority priority) {
      if (source.Dispatcher.CheckAccess()) {
        action();
      } else {
        source.Dispatcher.BeginInvoke(action, priority);
      }
    } 
    #endregion Action

    #region Function
    /// <summary>
    /// Execute a function on the UI thread, whatever the current thread. The return type is the generic value.
    /// </summary>
    /// <typeparam name="T">The type of the return value</typeparam>
    /// <param name="source">A DispatcherObject object</param>
    /// <param name="function">The function to execute on the UI thread</param>
    /// <returns>The result value of the function</returns>
    public static T Dispatch<T>(this DispatcherObject source, Func<T> function) {
      if (source.Dispatcher.CheckAccess()) {
        return function();
      }
      return (T)source.Dispatcher.Invoke(function);
    }

    /// <summary>
    /// Execute a function on the UI thread, whatever the current thread. The return type is the generic value.
    /// Allows the priority to be specified.
    /// </summary>
    /// <typeparam name="T">The type of the return value</typeparam>
    /// <param name="source">A DispatcherObject object</param>
    /// <param name="function">The function to execute on the UI thread</param>
    /// <param name="priority">The priority</param>
    /// <returns>The result value of the function</returns>
    public static T Dispatch<T>(this DispatcherObject source, Func<T> function, DispatcherPriority priority) {
      if (source.Dispatcher.CheckAccess()) {
        return function();
      }
      return (T)source.Dispatcher.Invoke(function, priority);
    } 
    #endregion Function

  }

}
