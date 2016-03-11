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
using BLTools.Async;
using BLTools.WPF;
using BLTools;
using System.Diagnostics;
using System.Threading;

namespace WpfApplication1 {
  /// <summary>
  /// Interaction logic for MainWindow.xaml
  /// </summary>
  public partial class MainWindow : Window {
    public MainWindow() {
      InitializeComponent();
    }

    private bool Process1Running = false;

    private bool btnStart1Enabler {
      get {
        return !Process1Running;
      }
    }

    private void ApplyEnablers() {
      this.Dispatch(() => {
        btnStart.IsEnabled = btnStart1Enabler;
      }, System.Windows.Threading.DispatcherPriority.Send);
    }

    private void btnStart_Click(object sender, RoutedEventArgs e) {

      Action<int, int> CountFromTo = new Action<int, int>((start, stop) => {
        for (int i = start; i < stop; i++) {
          Trace.WriteLine(string.Format("       {0}", i));
        }
      });

      Action DisplayText = new Action(() => {
        Process1Running = true;
        ApplyEnablers();
        for (int i = 1; i < 5; i++) {
          Trace.WriteLine(DateTime.Now.ToYMDHMS());
          Thread.Sleep(1000);
        }
        Thread.Sleep(4000);
        Process1Running = false;
        ApplyEnablers();
      });

      Action DisplayTextError = new Action(() => {
        throw new ApplicationException("error");
      });

      Async.ExecuteAsyncInOrder(new List<Action>() { () => { Process1Running = true; ApplyEnablers(); }, DisplayText, DisplayText, () => { Process1Running = false; ApplyEnablers(); } });

      Async.ExecuteAsyncInOrder(DisplayText, DisplayText);

      //Async.ExecuteAsync<int, int>(CountFromTo, 100, 900);

      //Async.ExecuteAsync<int, int>((start, stop) => {
      //  for (int i = start; i < stop; i++) {
      //    Trace.WriteLine(string.Format("       {0}", i));
      //  }
      //  this.Dispatch(() => txtSum1.Text = "Done.");
      //}, 100, 900);

      //TaskSum1.ContinueWith((x) => DisplayResults2(x), TaskContinuationOptions.NotOnCanceled | TaskContinuationOptions.NotOnFaulted);

      //Task<int> TaskSum2 = Async.ExecuteAsync<int>(() => {
      //  int i;
      //  int LocalSum = 0;
      //  for (i = 1; i < 1000; i++) {
      //    Trace.WriteLine(string.Format("-------{0}", i));
      //    LocalSum += i;
      //  }
      //  return LocalSum;
      //});
      //TaskSum2.ContinueWith((x) => DisplayResults2(x), TaskContinuationOptions.NotOnCanceled | TaskContinuationOptions.NotOnFaulted);

    }

    private void btnStart2_Click(object sender, RoutedEventArgs e) {
      Async.ReadTextFile("c:\\logs\\rotatelog.log", DisplayTextFile);
    }

    private void DisplayResults2(Task<int> sum) {
      this.Dispatch(() => {
        txtSum1.Text += sum.Result.ToString();
      });
    }

    private void DisplayTextFile(object sender, StringEventArgs e) {
      this.Dispatch(() => {
        txtSum1.Text += e.Value;
      });
    }

  }
}
