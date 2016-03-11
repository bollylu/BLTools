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

namespace BLTools.WPF.Controls {

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

    public static readonly DependencyProperty MaxLinesProperty = DependencyProperty.Register("MaxLines", typeof(int), typeof(InputText), new PropertyMetadata(1));
    public int MaxLines {
      get {
        return (int)GetValue(MaxLinesProperty);
      }
      set {
        SetValue(MaxLinesProperty, value);
        //InputValue.MaxLines = value;
      }
    }

    public static readonly DependencyProperty MinLinesProperty = DependencyProperty.Register("MinLines", typeof(int), typeof(InputText), new PropertyMetadata(1));
    public int MinLines {
      get {
        return (int)GetValue(MinLinesProperty);
      }
      set {
        SetValue(MinLinesProperty, value);
      }
    }

    public static readonly DependencyProperty IsMandatoryProperty = DependencyProperty.Register("IsMandatory", typeof(bool), typeof(InputText), new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.AffectsRender));
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

    public static readonly DependencyProperty FieldWidthProperty = DependencyProperty.Register("FieldWidth", typeof(GridLength), typeof(InputText), new FrameworkPropertyMetadata(new GridLength(1, GridUnitType.Star), FrameworkPropertyMetadataOptions.Inherits | FrameworkPropertyMetadataOptions.AffectsMeasure));
    public GridLength FieldWidth {
      get {
        return (GridLength)GetValue(FieldWidthProperty);
      }
      set {
        SetValue(FieldWidthProperty, value);
      }
    }

    public static readonly DependencyProperty IsNumericProperty = DependencyProperty.Register("IsNumeric", typeof(bool), typeof(InputText), new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.Inherits | FrameworkPropertyMetadataOptions.AffectsMeasure));
    public bool IsNumeric {
      get {
        return (bool)GetValue(IsNumericProperty);
      }
      set {
        SetValue(IsNumericProperty, value);
      }
    }

    public static readonly DependencyProperty IsDateTimeProperty = DependencyProperty.Register("IsDateTime", typeof(bool), typeof(InputText), new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.Inherits | FrameworkPropertyMetadataOptions.AffectsMeasure));
    public bool IsDateTime {
      get {
        return (bool)GetValue(IsDateTimeProperty);
      }
      set {
        SetValue(IsDateTimeProperty, value);
      }
    }

    public static readonly DependencyProperty LabelPositionProperty = DependencyProperty.Register("LabelPosition", typeof(EInputTextLabelPosition), typeof(InputText), new FrameworkPropertyMetadata(EInputTextLabelPosition.LEFT));
    public EInputTextLabelPosition LabelPosition {
      get {
        return (EInputTextLabelPosition)GetValue(LabelPositionProperty);
      }
      set {
        SetValue(LabelPositionProperty, value);
      }
    }

    public static readonly DependencyProperty MaxCharsProperty = DependencyProperty.Register("MaxChars", typeof(int), typeof(InputText), new FrameworkPropertyMetadata(0));
    public int MaxChars {
      get {
        return (int)GetValue(MaxCharsProperty);
      }
      set {
        SetValue(MaxCharsProperty, value);
      }
    }

    public static readonly DependencyProperty IsPasswordProperty = DependencyProperty.Register("IsPassword", typeof(bool), typeof(InputText), new FrameworkPropertyMetadata(false));
    public bool IsPassword {
      get {
        return (bool)GetValue(IsPasswordProperty);
      }
      set {
        SetValue(IsPasswordProperty, value);
      }
    }
    #endregion Dependency properties

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

    public InputText() {
      InitializeComponent();
    }

    private void UserControl_Loaded(object sender, RoutedEventArgs e) {
      ApplyEnablers();
    }
    private void UserControl_Initialized(object sender, EventArgs e) {
      TextBlock LabelTextBlock = this.FindVisualChild<TextBlock>();
      if (LabelTextBlock != null) {
        LabelTextBlock.TextWrapping = TextWrapping.WrapWithOverflow;
      }

      if (IsMandatory) {
        InputLabel.Text = string.Format("{0} *", InputLabel.Text);
        InputLabelTop.Content = string.Format("{0} *", InputLabelTop.Content);
      }

      if (IsNumeric) {
        InputValue.HorizontalContentAlignment = System.Windows.HorizontalAlignment.Right;
      }
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
      if (e.Key == Key.Return && CurrentTextbox.LineCount >= MaxLines) {
        e.Handled = true;
        return;
      }
    }

    
  }
}
