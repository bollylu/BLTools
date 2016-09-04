﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using BLTools;
using System.Diagnostics;

namespace BLTools.MVVM {
  /// <summary>
  /// Base class for a new MVVM class
  /// </summary>
  public class MVVMBase : INotifyPropertyChanged {

    /// <summary>
    /// Minimum level for tracing. If under the level, the callback is skipped
    /// </summary>
    public static ErrorLevel MinTraceLevel = ErrorLevel.Info;

    #region === Constructor(s) ====================================================================
    static MVVMBase() {
      MinTraceLevel = ErrorLevel.Warning;
    }
    #endregion === Constructor(s) =================================================================

    #region === INotifyPropertyChanged ============================================================
    public event PropertyChangedEventHandler PropertyChanged;
    protected void NotifyPropertyChanged(string propertyName) {
      if (PropertyChanged != null) {
        PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
      }
    }

    public static event PropertyChangedEventHandler GlobalPropertyChanged;
    protected static void GlobalNotifyPropertyChanged(string propertyName) {
      if (GlobalPropertyChanged != null) {
        GlobalPropertyChanged(null, new PropertyChangedEventArgs(propertyName));
      }
    }
    #endregion === INotifyPropertyChanged =========================================================

    /// <summary>
    /// Indicates when an operation is in progress
    /// </summary>
    public virtual bool WorkInProgress {
      get {
        return _WorkInProgress;
      }
      set {
        if (value != _WorkInProgress) {
          _WorkInProgress = value;
          NotifyPropertyChanged(nameof(WorkInProgress));
          GlobalNotifyPropertyChanged(nameof(WorkInProgress));
        }
      }
    }
    /// <summary>
    /// Indicates when an operation is in progress
    /// </summary>
    protected bool _WorkInProgress;

    #region === Events ============================================================================
    /// <summary>
    /// Indicates a change in operation status. Transmit a string.
    /// </summary>
    public static event EventHandler<StringEventArgs> OnExecutionStatus;

    /// <summary>
    /// Request progress bar initialisation. Provides the maximum value
    /// </summary>
    public static event EventHandler<IntEventArgs> OnInitProgressBar;
    /// <summary>
    /// Indicates progress bar change. Provides new current value
    /// </summary>
    public static event EventHandler<IntEventArgs> OnProgressBarNewValue;

    /// <summary>
    /// Indicates a change in operation progress. Provides a message and optionally a integer value
    /// </summary>
    public static event EventHandler<IntAndMessageEventArgs> OnExecutionProgress;
    /// <summary>
    /// Indicates that an operation is completed. Provides a bool to reflect the operation success and optionally a message
    /// </summary>
    public static event EventHandler<BoolAndMessageEventArgs> OnProgressCompleted;
    /// <summary>
    /// Indicates an error in operation progress. Provides a message and an errorlevel
    /// </summary>
    public static event EventHandler<IntAndMessageEventArgs> OnExecutionError;
    #endregion === Events =========================================================================

    #region --- Progress bar ----------------------------------------------------------------------
    /// <summary>
    /// Notify that a progress bar is to be reinitialised
    /// </summary>
    /// <param name="maxValue">Maximum value of the bar</param>
    protected virtual void NotifyInitProgressBar(int maxValue) {
      if (OnInitProgressBar == null) {
        return;
      }
      OnInitProgressBar(this, new IntEventArgs(maxValue));
    }

    /// <summary>
    /// Notify the progress bar of a new current value
    /// </summary>
    /// <param name="value">The current value</param>
    protected virtual void NotifyProgressBarNewValue(int value) {
      if (OnProgressBarNewValue == null) {
        return;
      }
      OnProgressBarNewValue(this, new IntEventArgs(value));
    }

    /// <summary>
    /// Notify a progress bar of a job completion, with optional message and status
    /// </summary>
    /// <param name="message">The optional message</param>
    /// <param name="status">The optional status (true/false)</param>
    protected virtual void NotifyProgressBarCompleted(string message = "", bool status = true) {
      if (OnProgressCompleted == null) {
        return;
      }
      OnProgressCompleted(this, new BoolAndMessageEventArgs(status, message));
    }
    #endregion --- Progress bar ------------------------------------------------------------------

    #region --- Execution status ------------------------------------------------------------------
    /// <summary>
    /// Sends an empty execution status to clear it
    /// </summary>
    protected virtual void ClearExecutionStatus() {
      if (OnExecutionStatus == null) {
        return;
      }
      OnExecutionStatus(this, new StringEventArgs(""));
    }

    /// <summary>
    /// Sends an execution status message
    /// </summary>
    /// <param name="statusMessage">The message</param>
    protected virtual void NotifyExecutionStatus(string statusMessage = "") {
      if (OnExecutionStatus == null) {
        return;
      }
      OnExecutionStatus(this, new StringEventArgs(statusMessage));
    }
    #endregion --- Execution status ---------------------------------------------------------------

    #region --- Execution progress ----------------------------------------------------------------
    /// <summary>
    /// Sends an empty execution progress message to clear it
    /// </summary>
    protected virtual void ClearExecutionProgress() {
      if (OnExecutionProgress == null) {
        return;
      }
      OnExecutionProgress(this, new IntAndMessageEventArgs(0, ""));
    }

    /// <summary>
    /// Sends a message for progress
    /// </summary>
    /// <param name="message">The message</param>
    /// <param name="errorlevel">The optional errorlevel (will be filtered by MinTraceLevel)</param>
    protected virtual void NotifyExecutionProgress(string message = "", ErrorLevel errorlevel = ErrorLevel.Info) {
      if (errorlevel < MinTraceLevel || OnExecutionProgress == null) {
        return;
      }
      NotifyExecutionProgress(message, 0, errorlevel);
    }

    /// <summary>
    /// Sends a message and an integer to indicate progress
    /// </summary>
    /// <param name="message">The message</param>
    /// <param name="progress">The integer</param>
    /// <param name="errorlevel">The optional errorlevel (will be filtered by MinTraceLevel)</param>
    protected virtual void NotifyExecutionProgress(string message, int progress, ErrorLevel errorlevel = ErrorLevel.Info) {
      if (errorlevel < MinTraceLevel || OnExecutionProgress == null) {
        return;
      }
      OnExecutionProgress(this, new IntAndMessageEventArgs(progress, message));
    }
    #endregion --- Execution progress -------------------------------------------------------------

    #region --- Execution error -------------------------------------------------------------------
    /// <summary>
    /// Sends an message to indicate an error
    /// </summary>
    /// <param name="message">The message</param>
    /// <param name="errorlevel">The optional errorlevel (will be filtered by MinTraceLevel)</param>
    protected virtual void NotifyExecutionError(string message = "", ErrorLevel errorlevel = ErrorLevel.Warning) {
      if (message == "" || errorlevel < MinTraceLevel || OnExecutionError == null) {
        return;
      }
      OnExecutionError(this, new IntAndMessageEventArgs((int)errorlevel, message));
    }
    #endregion --- Execution error ----------------------------------------------------------------


  }
}