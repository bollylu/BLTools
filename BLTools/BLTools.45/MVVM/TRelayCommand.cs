﻿using System;
using System.Diagnostics;
using System.Globalization;
using System.Windows.Input;

namespace BLTools.MVVM {
  public class TRelayCommand : ICommand {
    private readonly Action _ExecuteAction;
    protected Predicate<object> _CanExecute;

    #region Constructor(s)
    public TRelayCommand() {
      _CanExecute = (x) => { return true; };
    }
    public TRelayCommand(Action executeAction) : this() {
      _ExecuteAction = executeAction;
    }
    public TRelayCommand(Action executeAction, Predicate<object> canExecute) {
      _ExecuteAction = executeAction;
      _CanExecute = canExecute;
    }
    #endregion Constructor(s)

    [DebuggerStepThrough]
    public virtual bool CanExecute(object parameter) {
      if ( _CanExecute == null ) {
        return true;
      }
      return _CanExecute(parameter);
    }

    public event EventHandler CanExecuteChanged {
      add { CommandManager.RequerySuggested += value; }
      remove { CommandManager.RequerySuggested -= value; }
    }

    public virtual void Execute(object parameter) {
      NotifyCanExecuteChanged();
      _ExecuteAction();
      NotifyCanExecuteChanged();
    }

    protected void NotifyCanExecuteChanged() {
      CommandManager.InvalidateRequerySuggested();
    }
  }

  public class TRelayCommand<T> : TRelayCommand {

    private readonly Action<T> _ExecuteAction;

    public TRelayCommand(Action<T> executeAction) : base() {
      _ExecuteAction = executeAction;
    }

    public TRelayCommand(Action<T> executeAction, Predicate<object> canExecute) : base() {
      _ExecuteAction = executeAction;
      _CanExecute = canExecute;
    }

    public override void Execute(object parameter) {
      NotifyCanExecuteChanged();
      _ExecuteAction((T)BLTools.BLConverter.BLConvert<T>(parameter, CultureInfo.CurrentCulture, default(T)));
      NotifyCanExecuteChanged();
    }
  }

}
