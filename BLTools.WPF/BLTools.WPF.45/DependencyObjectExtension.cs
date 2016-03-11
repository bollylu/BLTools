using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace BLTools.WPF {
  /// <summary>
  /// Extensions for DependencyObject
  /// </summary>
  public static class DependencyObjectExtension {

    /// <summary>
    /// Finds a DependencyObject of type T in the visual tree
    /// </summary>
    /// <typeparam name="T">The type of the DependencyObject to find</typeparam>
    /// <param name="obj">The DependencyObject where to start the search</param>
    /// <returns>The found DependencyObject or null when not found</returns>
    public static T FindVisualChild<T>(this DependencyObject obj) where T : DependencyObject {
      for (int i = 0; i < VisualTreeHelper.GetChildrenCount(obj); i++) {

        DependencyObject Child = VisualTreeHelper.GetChild(obj, i);

        if (Child != null && Child is T) {
          return (T)Child;
        } else {
          T ChildOfChild = Child.FindVisualChild<T>();
          if (ChildOfChild != null) {
            return ChildOfChild;
          }
        }
      }
      return null;
    }

    /// <summary>
    /// Find a list of DependencyObject in the visual tree
    /// </summary>
    /// <typeparam name="T">The type of the DependencyObject to find</typeparam>
    /// <param name="obj">The DependencyObject where to start the search</param>
    /// <returns>A IEnumerable of DependencyObject of type T (empty when not found)</returns>
    public static IEnumerable<T> FindVisualChilds<T>(this DependencyObject obj) where T : DependencyObject {

      for (int i = 0; i < VisualTreeHelper.GetChildrenCount(obj); i++) {

        DependencyObject Child = VisualTreeHelper.GetChild(obj, i);

        if (Child != null && Child is T) {
          yield return (T)Child;
        } else {
          foreach (DependencyObject ChildItem in Child.FindVisualChilds<T>()) {
            if (ChildItem != null) {
              yield return (T)ChildItem;
            }
          }
        }
      }
      yield break;
    }

  }
}
