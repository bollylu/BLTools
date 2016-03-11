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

namespace BLTools.WPF.Test {
  /// <summary>
  /// Interaction logic for MainWindow.xaml
  /// </summary>
  public partial class MainWindow : Window {
    public MainWindow() {
      InitializeComponent();
    }

    private void Window_Loaded(object sender, RoutedEventArgs e) {
      
      
      
    }

    void MyForm_ClickCancel(object sender, RoutedEventArgs e) {
      MessageBox.Show("Cancel form");
      Application.Current.Shutdown();
    }

    void MyForm_ClickOk(object sender, RoutedEventArgs e) {
      MessageBox.Show(txtPassword.Value.ConvertToUnsecureString());
    }

    

  }
}
