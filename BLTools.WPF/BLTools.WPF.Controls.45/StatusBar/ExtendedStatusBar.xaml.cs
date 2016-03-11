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
  /// Interaction logic for ExtendedStatusBar.xaml
  /// </summary>
  public partial class ExtendedStatusBar : UserControl {

    #region Dependency Property LeftText
    public string LeftText {
      get {
        return (string)GetValue(LeftTextProperty);
      }
      set {
        SetValue(LeftTextProperty, value);
      }
    }
    public static readonly DependencyProperty LeftTextProperty = DependencyProperty.Register("LeftText", typeof(string), typeof(ExtendedStatusBar));
    #endregion Dependency Property LeftText

    #region Dependency Property RightText
    public string RightText {
      get {
        return (string)GetValue(RightTextProperty);
      }
      set {
        SetValue(RightTextProperty, value);
      }
    }
    public static readonly DependencyProperty RightTextProperty = DependencyProperty.Register("RightText", typeof(string), typeof(ExtendedStatusBar));
    #endregion Dependency Property RightText

    #region Dependency Property ProgressBarMinValue
    public int ProgressBarMinValue {
      get {
        return (int)GetValue(ProgressBarMinValueProperty);
      }
      set {
        SetValue(ProgressBarMinValueProperty, value);
      }
    }
    public static readonly DependencyProperty ProgressBarMinValueProperty = DependencyProperty.Register("ProgressBarMinValue", typeof(int), typeof(ExtendedStatusBar));
    #endregion Dependency Property ProgressBarMinValue

    #region Dependency Property ProgressBarMaxValue
    public int ProgressBarMaxValue {
      get {
        return (int)GetValue(ProgressBarMaxValueProperty);
      }
      set {
        SetValue(ProgressBarMaxValueProperty, value);
      }
    }
    public static readonly DependencyProperty ProgressBarMaxValueProperty = DependencyProperty.Register("ProgressBarMaxValue", typeof(int), typeof(ExtendedStatusBar));
    #endregion Dependency Property ProgressBarMaxValue

    #region Dependency Property ProgressBarValue
    public int ProgressBarValue {
      get {
        return (int)GetValue(ProgressBarValueProperty);
      }
      set {
        SetValue(ProgressBarValueProperty, value);
      }
    }
    public static readonly DependencyProperty ProgressBarValueProperty = DependencyProperty.Register("ProgressBarValue", typeof(int), typeof(ExtendedStatusBar));
    #endregion Dependency Property ProgressBarValue

    #region Dependency Property ProgressBarVisibility
    public Visibility ProgressBarVisibility {
      get {
        return pbStatus.Visibility;
      }
      set {
        pbStatus.Visibility = value;
      }
    }

    #endregion Dependency Property ProgressBarVisibility

    #region Events
    public static event EventHandler<StatusEventArgs> NotifyStatusEventHandler;
    public static event EventHandler<ProgressBarEventArgs> InitProgressEventHandler;
    public static event EventHandler<IntEventArgs> NotifyProgressEventHandler;
    public static event EventHandler EnableProgressEventHandler;
    public static event EventHandler DisableProgressEventHandler; 
    #endregion Events

    #region Constructor(s)
    public ExtendedStatusBar() {
      InitializeComponent();
      DataContext = this;
    }
    #endregion Constructor(s)

    #region Public methods
    public void SetStatusLeft(string message) {
      txtStatusBarLeft.Text = message;
    }
    public void SetStatusRight(string message) {
      txtStatusBarRight.Text = message;
    }
    public void ClearStatusLeft() {
      txtStatusBarLeft.Text = "";
    }
    public void ClearStatusRight() {
      txtStatusBarRight.Text = "";
    }
    #endregion Public methods

    #region Static methods NotifyStatus
    public static void NotifyStatusLeft(string message) {
      if (NotifyStatusEventHandler != null) {
        NotifyStatusEventHandler(null, new StatusEventArgs(StatusEventArgs.StatusEventLocationEnum.Left, message));
      }
    }
    public static void NotifyStatusRight(string message) {
      if (NotifyStatusEventHandler != null) {
        NotifyStatusEventHandler(null, new StatusEventArgs(StatusEventArgs.StatusEventLocationEnum.Right, message));
      }
    }
    public static void NotifyClearStatusLeft() {
      if (NotifyStatusEventHandler != null) {
        NotifyStatusEventHandler(null, new StatusEventArgs(StatusEventArgs.StatusEventLocationEnum.Left, ""));
      }
    }
    public static void NotifyClearStatusRight() {
      if (NotifyStatusEventHandler != null) {
        NotifyStatusEventHandler(null, new StatusEventArgs(StatusEventArgs.StatusEventLocationEnum.Right, ""));
      }
    }
    #endregion Static methods NotifyStatus

    #region Static methods Progress
    public static void InitProgressBar(int minValue, int maxValue) {
      if (NotifyProgressEventHandler != null) {
        InitProgressEventHandler(null, new ProgressBarEventArgs(minValue, maxValue, minValue));
      }
    }
    public static void NotifyProgress(int currentValue) {
      if (NotifyProgressEventHandler != null) {
        NotifyProgressEventHandler(null, new IntEventArgs(currentValue));
      }
    }

    public static void EnableProgressBar() {
      if (EnableProgressEventHandler != null) {
        EnableProgressEventHandler(null, EventArgs.Empty);
      }
    }
    public static void DisableProgressBar() {
      if (DisableProgressEventHandler != null) {
        DisableProgressEventHandler(null, EventArgs.Empty);
      }
    } 
    #endregion Static methods Progress
  }
}
