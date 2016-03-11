using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
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

namespace BLTools.WPF.Controls {

  public partial class InputPassword : UserControl {

    #region Dependency properties
    public static readonly DependencyProperty HeaderProperty = DependencyProperty.Register("Header", typeof(string), typeof(InputPassword));
    public string Header {
      get {
        return (string)GetValue(HeaderProperty);
      }
      set {
        SetValue(HeaderProperty, value);
      }
    }

    public static readonly DependencyProperty ValueProperty = DependencyProperty.Register("Value", typeof(SecureString), typeof(InputPassword));
    public SecureString Value {
      get {
        return (SecureString)GetValue(ValueProperty);
      }
      set {
        SetValue(ValueProperty, value);
      }
    }

    public static readonly DependencyProperty IsMandatoryProperty = DependencyProperty.Register("IsMandatory", typeof(bool), typeof(InputPassword), new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.AffectsRender));
    public bool IsMandatory {
      get {
        return (bool)GetValue(IsMandatoryProperty);
      }
      set {
        SetValue(IsMandatoryProperty, value);
      }
    }


    public static readonly DependencyProperty ErrorMessageProperty = DependencyProperty.Register("ErrorMessage", typeof(string), typeof(InputPassword));
    public string ErrorMessage {
      get {
        return (string)GetValue(ErrorMessageProperty);
      }
      set {
        SetValue(ErrorMessageProperty, value);
      }
    }

    public static readonly DependencyProperty FieldWidthProperty = DependencyProperty.Register("FieldWidth", typeof(GridLength), typeof(InputPassword), new FrameworkPropertyMetadata(new GridLength(1, GridUnitType.Star), FrameworkPropertyMetadataOptions.Inherits | FrameworkPropertyMetadataOptions.AffectsMeasure));
    public GridLength FieldWidth {
      get {
        return (GridLength)GetValue(FieldWidthProperty);
      }
      set {
        SetValue(FieldWidthProperty, value);
      }
    }

    public static readonly DependencyProperty LabelPositionProperty = DependencyProperty.Register("LabelPosition", typeof(EInputTextLabelPosition), typeof(InputPassword), new FrameworkPropertyMetadata(EInputTextLabelPosition.LEFT));
    public EInputTextLabelPosition LabelPosition {
      get {
        return (EInputTextLabelPosition)GetValue(LabelPositionProperty);
      }
      set {
        SetValue(LabelPositionProperty, value);
      }
    }

    public static readonly DependencyProperty MaxCharsProperty = DependencyProperty.Register("MaxChars", typeof(int), typeof(InputPassword), new FrameworkPropertyMetadata(0));
    public int MaxChars {
      get {
        return (int)GetValue(MaxCharsProperty);
      }
      set {
        SetValue(MaxCharsProperty, value);
      }
    }
    #endregion Dependency properties

    #region Enablers
    private Visibility LabelOnLeftVisibility {
      get {
        if (LabelPosition == EInputTextLabelPosition.LEFT) {
          return Visibility.Visible;
        }
        return Visibility.Collapsed;
      }
    }
    private Visibility LabelOnTopVisibility {
      get {
        if (LabelPosition == EInputTextLabelPosition.TOP) {
          return Visibility.Visible;
        }
        return Visibility.Collapsed;
      }
    }
    public void ApplyEnablers() {
      LabelOnLeft.Visibility = LabelOnLeftVisibility;
      LabelOnTop.Visibility = LabelOnTopVisibility;
    }  
    #endregion Enablers

    /// <summary>
    /// Constructor for InputPassword
    /// </summary>
    public InputPassword() {
      InitializeComponent();
    }
    private void UserControl_Initialized(object sender, EventArgs e) {
      TextBlock LabelTextBlock = this.FindVisualChild<TextBlock>();
      if (LabelTextBlock != null) {
        LabelTextBlock.TextWrapping = TextWrapping.WrapWithOverflow;
      }
      if (IsMandatory) {
        InputLabel.Content = string.Format("{0} *", InputLabel.Content);
        InputLabelTop.Content = string.Format("{0} *", InputLabelTop.Content);
      }
      InputValue.Password = Value.ConvertToUnsecureString();
    }

    private void UserControl_Loaded(object sender, RoutedEventArgs e) {
      ApplyEnablers();
    }

    private static Key[] AllowedKeysWhenAtSizeLimit = new Key[] { Key.Back, Key.Delete, Key.Left };
    private void InputValue_PreviewKeyDown(object sender, KeyEventArgs e) {
      TextBox CurrentTextbox = sender as TextBox;
      if (MaxChars > 0 && CurrentTextbox.Text.Length >= MaxChars) {
        if (!AllowedKeysWhenAtSizeLimit.Contains(e.Key)) {
          e.Handled = true;
          return;
        }
      }
    }

    private void InputValue_PasswordChanged(object sender, RoutedEventArgs e) {
      switch (LabelPosition) {
        case EInputTextLabelPosition.TOP:
        Value = InputValueTop.Password.ConvertToSecureString();
        break;
        case EInputTextLabelPosition.LEFT:
        default:
        Value = InputValue.Password.ConvertToSecureString();
        break;
      }
    }

    
  }
}
