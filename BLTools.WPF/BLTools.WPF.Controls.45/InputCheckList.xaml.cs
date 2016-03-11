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

  public partial class InputCheckList : UserControl {

    #region Dependency properties
    public static readonly DependencyProperty HeaderProperty = DependencyProperty.Register("Header", typeof(string), typeof(InputCheckList));
    public string Header {
      get {
        return (string)GetValue(HeaderProperty);
      }
      set {
        SetValue(HeaderProperty, value);
      }
    }

    public static readonly DependencyProperty ValueProperty = DependencyProperty.Register("Value", typeof(string), typeof(InputCheckList));
    public string Value {
      get {
        return (string)GetValue(ValueProperty);
      }
      set {
        SetValue(ValueProperty, value);
      }
    }

    public static readonly DependencyProperty IsMandatoryProperty = DependencyProperty.Register("IsMandatory", typeof(bool), typeof(InputCheckList), new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.AffectsRender));
    public bool IsMandatory {
      get {
        return (bool)GetValue(IsMandatoryProperty);
      }
      set {
        SetValue(IsMandatoryProperty, value);
      }
    }

    public static readonly DependencyProperty IsReadOnlyProperty = DependencyProperty.Register("IsReadOnly", typeof(bool), typeof(InputCheckList));
    public bool IsReadOnly {
      get {
        return (bool)GetValue(IsReadOnlyProperty);
      }
      set {
        SetValue(IsReadOnlyProperty, value);
      }
    }

    public static readonly DependencyProperty ErrorMessageProperty = DependencyProperty.Register("ErrorMessage", typeof(string), typeof(InputCheckList));
    public string ErrorMessage {
      get {
        return (string)GetValue(ErrorMessageProperty);
      }
      set {
        SetValue(ErrorMessageProperty, value);
      }
    }

    public static readonly DependencyProperty FieldWidthProperty = DependencyProperty.Register("FieldWidth", typeof(GridLength), typeof(InputCheckList), new FrameworkPropertyMetadata(new GridLength(1, GridUnitType.Star), FrameworkPropertyMetadataOptions.Inherits | FrameworkPropertyMetadataOptions.AffectsMeasure));
    public GridLength FieldWidth {
      get {
        return (GridLength)GetValue(FieldWidthProperty);
      }
      set {
        SetValue(FieldWidthProperty, value);
      }
    }

    public static readonly DependencyProperty LabelPositionProperty = DependencyProperty.Register("LabelPosition", typeof(EInputTextLabelPosition), typeof(InputCheckList), new FrameworkPropertyMetadata(EInputTextLabelPosition.LEFT));
    public EInputTextLabelPosition LabelPosition {
      get {
        return (EInputTextLabelPosition)GetValue(LabelPositionProperty);
      }
      set {
        SetValue(LabelPositionProperty, value);
      }
    }

    public static readonly DependencyProperty ItemsProperty = DependencyProperty.Register("Items", typeof(IEnumerable<object>), typeof(InputCheckList));
    public IEnumerable<object> Items {
      get {
        return (IEnumerable<object>)GetValue(LabelPositionProperty);
      }
      set {
        SetValue(LabelPositionProperty, value);
      }
    }
    #endregion Dependency properties

    public InputCheckList() {
      InitializeComponent();
    }

    private void UserControl_Loaded(object sender, RoutedEventArgs e) {
      

    }

    private void UserControl_Initialized(object sender, EventArgs e) {
      switch (LabelPosition) {
        case EInputTextLabelPosition.TOP:
          LabelOnTop.Visibility = Visibility.Visible;
          LabelOnLeft.Visibility = Visibility.Collapsed;
          break;
        case EInputTextLabelPosition.LEFT:
        default:
          LabelOnTop.Visibility = Visibility.Collapsed;
          LabelOnLeft.Visibility = Visibility.Visible;
          break;
      }
      TextBlock LabelTextBlock = this.FindVisualChild<TextBlock>();
      if (LabelTextBlock != null) {
        LabelTextBlock.TextWrapping = TextWrapping.WrapWithOverflow;
      }
      if (IsMandatory) {
        InputLabel.Content = string.Format("{0} *", InputLabel.Content);
      }
    }
  }
}
