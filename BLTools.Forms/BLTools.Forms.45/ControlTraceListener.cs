using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Windows.Forms;

namespace BLTools.Forms {
  public class ControlTraceListener : DefaultTraceListener {

    #region Public properties
    public Control TraceControl { get; set; }
    public string TraceControlProperty { get; set; }
    #endregion Public properties

    #region Private variables
    private DelegateChangeProperty _ChangeProperty;
    #endregion Private variables

    #region Constructor(s)
    public ControlTraceListener( Control control ) {
      TraceControl = control;
      TraceControlProperty = "Text";
      base.Name = "";
      base.TraceOutputOptions = TraceOptions.Timestamp;
      _ChangeProperty = new DelegateChangeProperty(ChangeProperty);
    }
    public ControlTraceListener( Control control, string name ) {
      TraceControl = control;
      TraceControlProperty = "Text";
      base.Name = name;
      _ChangeProperty = new DelegateChangeProperty(ChangeProperty);
    }
    public ControlTraceListener( Control control, string property, string name ) {
      TraceControl = control;
      TraceControlProperty = property;
      base.Name = name;
      base.TraceOutputOptions = TraceOptions.Timestamp;
      _ChangeProperty = new DelegateChangeProperty(ChangeProperty);
    }
    #endregion Constructor(s)

    #region Public methods
    public override void WriteLine( string message ) {
      WriteLine(message, "");
    }
    public override void WriteLine( string message, string category ) {
      if ( TraceControl.InvokeRequired ) {
        TraceControl.BeginInvoke(_ChangeProperty, new object[] { message, category });
      } else {
        ChangeProperty(message, category);
      }
    }
    #endregion Public methods

    #region Private methods

    private delegate void DelegateChangeProperty( string message, string category );
    private void ChangeProperty( string message, string category ) {
      lock ( TraceControl ) {
        if ( TraceControlProperty == "Text" ) {
          if ( TraceControl is RichTextBox ) {
            ((RichTextBox)TraceControl).AppendText(new string(' ', base.IndentLevel * base.IndentSize));
            ((RichTextBox)TraceControl).AppendText(message + Environment.NewLine);
            ((RichTextBox)TraceControl).ScrollToCaret();
            ((RichTextBox)TraceControl).Invalidate();
            ((RichTextBox)TraceControl).Update();
            return;
          }
          if ( TraceControl is TextBox ) {
            ((TextBox)TraceControl).AppendText(new string(' ', base.IndentLevel * base.IndentSize));
            ((TextBox)TraceControl).AppendText(message + Environment.NewLine);
            ((TextBox)TraceControl).ScrollToCaret();
            ((TextBox)TraceControl).Invalidate();
            ((TextBox)TraceControl).Update();
            return;
          }
        }
      }
    }
    #endregion Private methods
  }
}
