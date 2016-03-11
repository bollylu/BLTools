using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using BLTools;

namespace BLTools.WPF.Controls {
  /// <summary>
  /// Follow steps 1a or 1b and then 2 to use this custom control in a XAML file.
  ///
  /// Step 1a) Using this custom control in a XAML file that exists in the current project.
  /// Add this XmlNamespace attribute to the root element of the markup file where it is 
  /// to be used:
  ///
  ///     xmlns:MyNamespace="clr-namespace:BLTools.WPF.Controls"
  ///
  ///
  /// Step 1b) Using this custom control in a XAML file that exists in a different project.
  /// Add this XmlNamespace attribute to the root element of the markup file where it is 
  /// to be used:
  ///
  ///     xmlns:MyNamespace="clr-namespace:BLTools.WPF.Controls;assembly=BLTools.WPF.Controls"
  ///
  /// You will also need to add a project reference from the project where the XAML file lives
  /// to this project and Rebuild to avoid compilation errors:
  ///
  ///     Right click on the target project in the Solution Explorer and
  ///     "Add Reference"->"Projects"->[Select this project]
  ///
  ///
  /// Step 2)
  /// Go ahead and use your control in the XAML file.
  ///
  ///     
  ///
  /// </summary>
  /// 

  public class InputForm : ContentControl, IDisposable {

    #region Dependency properties
    public static readonly RoutedEvent ClickOkEvent = EventManager.RegisterRoutedEvent("ClickOK", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(InputForm));
    public event RoutedEventHandler ClickOk {
      add { AddHandler(ClickOkEvent, value); }
      remove { RemoveHandler(ClickOkEvent, value); }
    }

    public static readonly RoutedEvent ClickCancelEvent = EventManager.RegisterRoutedEvent("ClickCancel", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(InputForm));
    public event RoutedEventHandler ClickCancel {
      add { AddHandler(ClickCancelEvent, value); }
      remove { RemoveHandler(ClickCancelEvent, value); }
    }

    public static readonly DependencyProperty InputFormTypeProperty = DependencyProperty.Register("InputFormType", typeof(string), typeof(InputForm));
    public string InputFormType {
      get {
        return (string)GetValue(InputFormTypeProperty);
      }
      set {
        SetValue(InputFormTypeProperty, value);
      }

    }

    public static readonly DependencyProperty HeaderProperty = DependencyProperty.Register("Header", typeof(string), typeof(InputForm));
    public string Header {
      get {
        return (string)GetValue(HeaderProperty);
      }
      set {
        SetValue(HeaderProperty, value);
      }
    }

    public static readonly DependencyProperty HeaderVisibilityProperty = DependencyProperty.Register("HeaderVisibility", typeof(Visibility), typeof(InputForm));
    public Visibility HeaderVisibility {
      get {
        return (Visibility)GetValue(HeaderVisibilityProperty);
      }
      set {
        SetValue(HeaderVisibilityProperty, value);
      }
    }

    public static readonly DependencyProperty FieldWidthProperty = DependencyProperty.Register("FieldWidth", typeof(GridLength), typeof(InputForm), new FrameworkPropertyMetadata(new GridLength(1, GridUnitType.Star), FrameworkPropertyMetadataOptions.Inherits | FrameworkPropertyMetadataOptions.AffectsMeasure));
    public GridLength FieldWidth {
      get {
        return (GridLength)GetValue(FieldWidthProperty);
      }
      set {
        SetValue(FieldWidthProperty, value);
      }
    }
    #endregion Dependency properties

    public Button btnOk {
      get {
        return GetTemplateChild("btnOk") as Button;
      }
    }

    public Button btnCancel {
      get {
        return GetTemplateChild("btnCancel") as Button;
      }
    }

    #region Constructor(s)
    static InputForm() {
      DefaultStyleKeyProperty.OverrideMetadata(typeof(InputForm), new FrameworkPropertyMetadata(typeof(InputForm), FrameworkPropertyMetadataOptions.Inherits));
    }
    public void Dispose() {
      Button ClickOkButton = GetTemplateChild("btnOk") as Button;
      Button ClickCancelButton = GetTemplateChild("btnCancel") as Button;

      if (ClickOkButton != null) {
        ClickOkButton.Click -= ClickOkButton_Click;
      }
      if (ClickCancelButton != null) {
        ClickCancelButton.Click -= ClickCancelButton_Click;
      }
    } 
    #endregion Constructor(s)

    public override void OnApplyTemplate() {
      base.OnApplyTemplate();

      Button ClickOkButton = GetTemplateChild("btnOk") as Button;
      Button ClickCancelButton = GetTemplateChild("btnCancel") as Button;

      if (ClickOkButton != null) {
        ClickOkButton.Click += ClickOkButton_Click;
      }
      if (ClickCancelButton != null) {
        ClickCancelButton.Click += ClickCancelButton_Click;
      }

    }

    void ClickOkButton_Click(object sender, RoutedEventArgs e) {
      DependencyObject Presenter = this.GetTemplateChild("ContentPlaceHolder");

      foreach (InputText InputTextItemItem in Presenter.FindVisualChilds<InputText>()) {
        #region Check for mandatory fields
        if (InputTextItemItem.IsMandatory && InputTextItemItem.Value == "") {
          if (!string.IsNullOrWhiteSpace(InputTextItemItem.ErrorMessage)) {
            MessageBox.Show(InputTextItemItem.ErrorMessage, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            InputTextItemItem.Focus();
            return;
          } else {
            MessageBox.Show(string.Format("Error: data is missing from a mandatory field : {0}", InputTextItemItem.Header));
            return;
          }
        } 
        #endregion Check for mandatory fields

        #region Check for numeric fields
        if (InputTextItemItem.IsNumeric && !InputTextItemItem.Value.IsNumeric()) {
          if (!string.IsNullOrWhiteSpace(InputTextItemItem.ErrorMessage)) {
            MessageBox.Show(InputTextItemItem.ErrorMessage, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            InputTextItemItem.Focus();
            return;
          } else {
            MessageBox.Show(string.Format("Error: data must only contains numeric characters : {0}", InputTextItemItem.Header));
            return;
          }
        } 
        #endregion Check for numeric fields
      }

      foreach (InputPassword InputPasswordItem in Presenter.FindVisualChilds<InputPassword>()) {
        #region Check for mandatory fields
        if (InputPasswordItem.IsMandatory && InputPasswordItem.Value == "".ConvertToSecureString()) {
          if (!string.IsNullOrWhiteSpace(InputPasswordItem.ErrorMessage)) {
            MessageBox.Show(InputPasswordItem.ErrorMessage, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            InputPasswordItem.Focus();
            return;
          } else {
            MessageBox.Show(string.Format("Error: data is missing from a mandatory field : {0}", InputPasswordItem.Header));
            return;
          }
        }
        #endregion Check for mandatory fields

      }

      RaiseEvent(new RoutedEventArgs(ClickOkEvent));
    }

    void ClickCancelButton_Click(object sender, RoutedEventArgs e) {
      RaiseEvent(new RoutedEventArgs(ClickCancelEvent));
    }



    
  }
}
