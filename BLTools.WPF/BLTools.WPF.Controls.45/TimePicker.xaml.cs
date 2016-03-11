using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
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
using BLTools.WPF;

namespace BLTools.WPF.Controls {
  /// <summary>
  /// Interaction logic for TimePicker.xaml
  /// </summary>
  public partial class TimePicker : UserControl, IEnablers {

    /// <summary>
    /// Possible values for time step
    /// </summary>
    public enum ETimeStep {
      Manual,
      QuartHour,
      HalfHour,
      Hour
    }

    #region Dependency properties
    public static readonly DependencyProperty IsMandatoryProperty = DependencyProperty.Register("IsMandatory", typeof(bool), typeof(TimePicker), new FrameworkPropertyMetadata(true, FrameworkPropertyMetadataOptions.AffectsRender));
    public bool IsMandatory {
      get {
        return (bool)GetValue(IsMandatoryProperty);
      }
      set {
        SetValue(IsMandatoryProperty, value);
      }
    }

    public static readonly DependencyProperty ValueProperty = DependencyProperty.Register("Value", typeof(DateTime), typeof(TimePicker));
    public DateTime Value {
      get {
        return (DateTime)GetValue(ValueProperty);
      }
      set {
        SetValue(ValueProperty, value);
      }
    }

    public static readonly DependencyProperty MinValueProperty = DependencyProperty.Register("MinValue", typeof(DateTime), typeof(TimePicker), new PropertyMetadata(DateTime.MinValue));
    public DateTime MinValue {
      get {
        return (DateTime)GetValue(MinValueProperty);
      }
      set {
        SetValue(MinValueProperty, value);
      }
    }

    public static readonly DependencyProperty MaxValueProperty = DependencyProperty.Register("MaxValue", typeof(DateTime), typeof(TimePicker), new PropertyMetadata(DateTime.MinValue));
    public DateTime MaxValue {
      get {
        return (DateTime)GetValue(MaxValueProperty);
      }
      set {
        SetValue(MaxValueProperty, value);
      }
    }

    public static readonly DependencyProperty StepProperty = DependencyProperty.Register("Step", typeof(ETimeStep), typeof(TimePicker), new FrameworkPropertyMetadata(ETimeStep.Hour));
    public ETimeStep Step {
      get {
        return (ETimeStep)GetValue(StepProperty);
      }
      set {
        SetValue(StepProperty, value);
      }
    }

    public static readonly DependencyProperty ManualProperty = DependencyProperty.Register("Manual", typeof(bool), typeof(TimePicker), new FrameworkPropertyMetadata(true));
    public bool Manual {
      get {
        return (bool)GetValue(ManualProperty);
      }
      set {
        SetValue(ManualProperty, value);
      }
    }
    public static readonly DependencyProperty TimeSelectorProperty = DependencyProperty.Register("TimeSelector", typeof(bool), typeof(TimePicker), new FrameworkPropertyMetadata(false));
    public bool TimeSelector {
      get {
        return (bool)GetValue(TimeSelectorProperty);
      }
      set {
        SetValue(TimeSelectorProperty, value);
      }
    }
    #endregion Dependency properties

    private bool _Error = false;
    public bool HasError {
      get {
        return _Error;
      }
    }

    public readonly ObservableCollection<DateTime> Items;

    private bool DisplayTimeCombo = false;

    public event EventHandler OnApplyEnablers;

    private Visibility cbTimeVisibility {
      get {
        return DisplayTimeCombo ? Visibility.Visible : Visibility.Collapsed;
      }
    }
    private Brush OriginalTxtTimeBorderBrush;
    private Brush txtTimeBorderBrush {
      get {
        return _Error ? Brushes.Red : OriginalTxtTimeBorderBrush;
      }
    }
    private Thickness txtTimeBorderThickness {
      get {
        return _Error ? new Thickness(1)  : new Thickness(0);
      }
    }

    public void ApplyEnablers() {
      cbTime.Visibility = cbTimeVisibility;
      txtTime.BorderBrush = txtTimeBorderBrush;
      txtTime.BorderThickness = txtTimeBorderThickness;
      if (OnApplyEnablers!=null) {
        OnApplyEnablers(this, EventArgs.Empty);
      }
    }

    /// <summary>
    /// Create a Time picker control
    /// </summary>
    public TimePicker() {
      Items = new ObservableCollection<DateTime>();
      InitializeComponent();
      _Error = false;
      
    }

    private void UserControl_Initialized(object sender, EventArgs e) {
      cbTime.ItemsSource = Items;
      txtTime.Text = Value.ToString("HH:mm");
      OriginalTxtTimeBorderBrush = txtTime.BorderBrush;
    }

    private void UserControl_Loaded(object sender, RoutedEventArgs e) {

      DateTime StartValue;
      if (MinValue == DateTime.MinValue) {
        StartValue = DateTime.MinValue.Time();
      } else {
        StartValue = MinValue.Time();
      }

      DateTime StopValue;
      if (MaxValue == DateTime.MinValue) {
        StopValue = new DateTime(1, 1, 1, 23, 45, 0);
      } else {
        StopValue = MaxValue.Time();
      }

      //Trace.WriteLine(string.Format("Step={0}", Step.ToString()));
      switch (Step) {
        default:
        case ETimeStep.Hour:
          for (DateTime Current = StartValue; Current < StopValue; Current = Current.AddHours(1)) {
            Items.Add(Current);
          }
          break;
        case ETimeStep.HalfHour:
          for (DateTime Current = StartValue; Current < StopValue; Current = Current.AddMinutes(30)) {
            Items.Add(Current);
          }
          break;
        case ETimeStep.QuartHour:
          for (DateTime Current = StartValue.Time(); Current.Time() < StopValue.Time(); Current = Current.AddMinutes(15).Time()) {
            Items.Add(Current);
          }
          break;
      }

      cbTime.SelectedItem = Items.FirstOrDefault(x => x.Time() == Value.Time());
      ApplyEnablers();
      
      
    }

    private void cbTime_SelectionChanged(object sender, SelectionChangedEventArgs e) {
      Value = (DateTime)cbTime.SelectedItem;
      txtTime.Text = Value.ToString("HH:mm");
      DisplayTimeCombo = false;
      _Error = false;
      ApplyEnablers();
    }

    private void Button_Click(object sender, RoutedEventArgs e) {
      DisplayTimeCombo = true;
      ApplyEnablers();
    }

    private void txtTime_LostFocus(object sender, RoutedEventArgs e) {

      if (string.IsNullOrWhiteSpace(txtTime.Text)) {
        if (IsMandatory) {
          _Error = true;
        } else {
          _Error = false;
        }
        ApplyEnablers();
        return;
      }

      DateTime TempTime;
      if (!DateTime.TryParse(txtTime.Text, out TempTime)) {
        _Error = true;
        MessageBox.Show("Invalid time, please correct it", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
      } else {
        _Error = false;
        Value = TempTime;
      }
      ApplyEnablers();
    }

  }
}
