using System;
using System.Collections.Generic;
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
using BLTools.Google.Apis.Direction;

namespace GoogleDirectionTest {
  /// <summary>
  /// Interaction logic for MainWindow.xaml
  /// </summary>
  public partial class MainWindow : Window {

    DirectionsQuery Query;

    public MainWindow() {
      InitializeComponent();
    }

    private async void btnStart_Click(object sender, RoutedEventArgs e) {
      List<string> WayPoints = new List<string>();
      if (txtWayPoint1.Text != "") {
        WayPoints.Add(txtWayPoint1.Text);
      }
      if (txtWayPoint2.Text != "") {
        WayPoints.Add(txtWayPoint2.Text);
      }
      if (txtWayPoint3.Text != "") {
        WayPoints.Add(txtWayPoint3.Text);
      }
      if (txtWayPoint4.Text != "") {
        WayPoints.Add(txtWayPoint4.Text);
      }
      if (txtWayPoint5.Text != "") {
        WayPoints.Add(txtWayPoint5.Text);
      }
      if (txtWayPoint6.Text != "") {
        WayPoints.Add(txtWayPoint6.Text);
      }
      if (txtWayPoint7.Text != "") {
        WayPoints.Add(txtWayPoint7.Text);
      }
      if (txtWayPoint8.Text != "") {
        WayPoints.Add(txtWayPoint8.Text);
      }
      if (txtWayPoint9.Text != "") {
        WayPoints.Add(txtWayPoint9.Text);
      }
      Query = new DirectionsQuery(new DirectionsQueryParameters(txtOrigin.Text, txtDestination.Text, WayPoints));
      Trace.WriteLine(Query.ToString());
      Query.OnQueryCompleted += Query_OnQueryCompleted;
      await Query.ExecuteAsync();
    }

    void Query_OnQueryCompleted(object sender, EventArgs e) {
      Query.OnQueryCompleted -= Query_OnQueryCompleted;
      Trace.WriteLine(Query.ToString());
      StringBuilder Text = new StringBuilder(Query.Response.Status.ToString());
      Text.AppendLine();
      Text.AppendLine(string.Format("Origin : {0}", Query.Response.Route.Legs.First().StartAddress));

      int i = 0;
      foreach (DirectionsResponseLeg LegItem in Query.Response.Route.Legs) {
        Text.AppendLine(string.Format("  Section {0} : {1} => {2} ({3})", i++, LegItem.StartAddress, LegItem.EndAddress, LegItem.Distance.Text));
      }

      Text.AppendLine(string.Format("Destination : {0}", Query.Response.Route.Legs.Last().EndAddress));
      Text.AppendLine(string.Format("Distance totale: {0}", Query.Response.CompleteDistanceText));
      txtResult.Text = Text.ToString();
    }
  }
}
