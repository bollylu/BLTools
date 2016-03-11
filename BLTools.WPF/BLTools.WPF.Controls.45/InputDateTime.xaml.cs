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

  public partial class InputDateTime : UserControl, IEnablers {

    private bool _Error = false;
    public bool HasError {
      get {
        if (IsDateMandatory && DateValue == DateTime.MinValue) {
          return true;
        }
        return _Error || InputValueTimeTop.HasError;
      }
    }

    public event EventHandler OnApplyEnablers;

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
      if (OnApplyEnablers!=null) {
        OnApplyEnablers(this, EventArgs.Empty);
      }
    }
    #endregion Enablers

    #region Dependency properties
    public static readonly DependencyProperty HeaderProperty = DependencyProperty.Register("Header", typeof(string), typeof(InputDateTime));
    public string Header {
      get {
        return (string)GetValue(HeaderProperty);
      }
      set {
        SetValue(HeaderProperty, value);
      }
    }

    public static readonly DependencyProperty DateValueProperty = DependencyProperty.Register("DateValue", typeof(DateTime), typeof(InputDateTime));
    public DateTime DateValue {
      get {
        return (DateTime)GetValue(DateValueProperty);
      }
      set {
        SetValue(DateValueProperty, value);
      }
    }
    public static readonly DependencyProperty IsDateMandatoryProperty = DependencyProperty.Register("IsDateMandatory", typeof(bool), typeof(InputDateTime), new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.AffectsRender));
    public bool IsDateMandatory {
      get {
        return (bool)GetValue(IsDateMandatoryProperty);
      }
      set {
        SetValue(IsDateMandatoryProperty, value);
      }
    }

    public static readonly DependencyProperty TimeValueProperty = DependencyProperty.Register("TimeValue", typeof(DateTime), typeof(InputDateTime));
    public DateTime TimeValue {
      get {
        return (DateTime)GetValue(DateValueProperty);
      }
      set {
        SetValue(DateValueProperty, value);
      }
    }

    public static readonly DependencyProperty IsTimeMandatoryProperty = DependencyProperty.Register("IsTimeMandatory", typeof(bool), typeof(InputDateTime), new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.AffectsRender));
    public bool IsTimeMandatory {
      get {
        return (bool)GetValue(IsTimeMandatoryProperty);
      }
      set {
        SetValue(IsTimeMandatoryProperty, value);
      }
    }

    public static readonly DependencyProperty IsReadOnlyProperty = DependencyProperty.Register("IsReadOnly", typeof(bool), typeof(InputDateTime), new FrameworkPropertyMetadata(false));
    public bool IsReadOnly {
      get {
        return (bool)GetValue(IsReadOnlyProperty);
      }
      set {
        SetValue(IsReadOnlyProperty, value);
      }
    }

    public static readonly DependencyProperty ErrorMessageProperty = DependencyProperty.Register("ErrorMessage", typeof(string), typeof(InputDateTime));
    public string ErrorMessage {
      get {
        return (string)GetValue(ErrorMessageProperty);
      }
      set {
        SetValue(ErrorMessageProperty, value);
      }
    }

    public static readonly DependencyProperty FieldWidthProperty = DependencyProperty.Register("FieldWidth", typeof(GridLength), typeof(InputDateTime), new FrameworkPropertyMetadata(new GridLength(1, GridUnitType.Star), FrameworkPropertyMetadataOptions.Inherits | FrameworkPropertyMetadataOptions.AffectsMeasure));
    public GridLength FieldWidth {
      get {
        return (GridLength)GetValue(FieldWidthProperty);
      }
      set {
        SetValue(FieldWidthProperty, value);
      }
    }

    public static readonly DependencyProperty LabelPositionProperty = DependencyProperty.Register("LabelPosition", typeof(EInputTextLabelPosition), typeof(InputDateTime), new FrameworkPropertyMetadata(EInputTextLabelPosition.LEFT));
    public EInputTextLabelPosition LabelPosition {
      get {
        return (EInputTextLabelPosition)GetValue(LabelPositionProperty);
      }
      set {
        SetValue(LabelPositionProperty, value);
      }
    }

    public static readonly DependencyProperty DateVisibilityProperty = DependencyProperty.Register("DateVisibility", typeof(Visibility), typeof(InputDateTime), new FrameworkPropertyMetadata(Visibility.Visible));
    public Visibility DateVisibility {
      get {
        return (Visibility)GetValue(DateVisibilityProperty);
      }
      set {
        SetValue(DateVisibilityProperty, value);
      }
    }

    public static readonly DependencyProperty TimeVisibilityProperty = DependencyProperty.Register("TimeVisibility", typeof(Visibility), typeof(InputDateTime), new FrameworkPropertyMetadata(Visibility.Collapsed));
    public Visibility TimeVisibility {
      get {
        return (Visibility)GetValue(TimeVisibilityProperty);
      }
      set {
        SetValue(TimeVisibilityProperty, value);
      }
    }

    public static readonly DependencyProperty MinValueProperty = DependencyProperty.Register("MinValue", typeof(DateTime), typeof(InputDateTime), new PropertyMetadata(DateTime.MinValue));
    public DateTime MinValue {
      get {
        return (DateTime)GetValue(MinValueProperty);
      }
      set {
        SetValue(MinValueProperty, value);
      }
    }

    public static readonly DependencyProperty MaxValueProperty = DependencyProperty.Register("MaxValue", typeof(DateTime), typeof(InputDateTime), new PropertyMetadata(DateTime.MinValue));
    public DateTime MaxValue {
      get {
        return (DateTime)GetValue(MaxValueProperty);
      }
      set {
        SetValue(MaxValueProperty, value);
      }
    }

    public static readonly DependencyProperty StepProperty = DependencyProperty.Register("Step", typeof(TimePicker.ETimeStep), typeof(InputDateTime), new FrameworkPropertyMetadata(TimePicker.ETimeStep.Hour));
    public TimePicker.ETimeStep Step {
      get {
        return (TimePicker.ETimeStep)GetValue(StepProperty);
      }
      set {
        SetValue(StepProperty, value);
      }
    }

    public static readonly DependencyProperty TimeSelectorProperty = DependencyProperty.Register("TimeSelector", typeof(bool), typeof(InputDateTime), new FrameworkPropertyMetadata(false));

    

    public bool TimeSelector {
      get {
        return (bool)GetValue(TimeSelectorProperty);
      }
      set {
        SetValue(TimeSelectorProperty, value);
      }
    }

    #endregion Dependency properties

    #region Constructor(s)
    /// <summary>
    /// Constructor of the InputDateTime user control
    /// </summary>
    public InputDateTime() {
      InitializeComponent();
    }

    private void UserControl_Loaded(object sender, RoutedEventArgs e) {
      ApplyEnablers();
    }

    private void UserControl_Initialized(object sender, EventArgs e) {
      InputValueTimeTop.OnApplyEnablers += ((o, ev) => { ApplyEnablers(); });

      TextBlock LabelTextBlock = this.FindVisualChild<TextBlock>();
      if (LabelTextBlock != null) {
        LabelTextBlock.TextWrapping = TextWrapping.WrapWithOverflow;
      }

      if (IsTimeMandatory) {
        InputLabel.Content = string.Format("{0} *", InputLabel.Content);
      }
    }
    #endregion Constructor(s)
  }
}
