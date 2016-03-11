using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace BLTools.WPF {

  public partial class InputText : UserControl {

    #region Dependency properties
    public static readonly DependencyProperty HeaderProperty = DependencyProperty.Register("Header", typeof(string), typeof(InputText));
    public string Header {
      get {
        return (string)GetValue(HeaderProperty);
      }
      set {
        SetValue(HeaderProperty, value);
      }
    }

    public static readonly DependencyProperty ValueProperty = DependencyProperty.Register("Value", typeof(string), typeof(InputText));
    public string Value {
      get {
        return (string)GetValue(ValueProperty);
      }
      set {
        SetValue(ValueProperty, value);
      }
    }

    public static readonly DependencyProperty IsMandatoryProperty = DependencyProperty.Register("IsMandatory", typeof(bool), typeof(InputText));
    public bool IsMandatory {
      get {
        return (bool)GetValue(IsMandatoryProperty);
      }
      set {
        SetValue(IsMandatoryProperty, value);
      }
    }

    public static readonly DependencyProperty IsReadOnlyProperty = DependencyProperty.Register("IsReadOnly", typeof(bool), typeof(InputText));
    public bool IsReadOnly {
      get {
        return (bool)GetValue(IsReadOnlyProperty);
      }
      set {
        SetValue(IsReadOnlyProperty, value);
      }
    }

    public static readonly DependencyProperty ErrorMessageProperty = DependencyProperty.Register("ErrorMessage", typeof(string), typeof(InputText));
    public string ErrorMessage {
      get {
        return (string)GetValue(ErrorMessageProperty);
      }
      set {
        SetValue(ErrorMessageProperty, value);
      }
    }
    #endregion Dependency properties




    
    public InputText() {
      InitializeComponent();
    }
  }
}
