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
using System.Security.Cryptography;
using System.Windows.Forms;
using System.IO;
using BLTools.Encryption;

namespace GenerateKeyPair {
  /// <summary>
  /// Interaction logic for Window1.xaml
  /// </summary>
  public partial class Window1 : Window {

    private string _PublicKey;
    private string _PrivateKey;

    public Window1() {
      InitializeComponent();
    }

    private void btnGenerate_Click(object sender, RoutedEventArgs e) {
      using (RSACryptoServiceProvider TempRSACSP = new RSACryptoServiceProvider(int.Parse((string)(cbKeyLength.SelectionBoxItem)))) {
        _PublicKey = TempRSACSP.ToXmlString(false);
        _PrivateKey = TempRSACSP.ToXmlString(true);
      }
      chkEncrypt.IsChecked = false;
      txtPublicKey.Text = _PublicKey;
      txtPrivateKey.Text = _PrivateKey;
      btnNext.IsEnabled = true;
      tabKeysave.IsEnabled = true;
    }

    private void Window_Loaded(object sender, RoutedEventArgs e) {
      btnNext.IsEnabled = false;
      tabKeysave.IsEnabled = false;
    }

    private void btnNext_Click(object sender, RoutedEventArgs e) {
      _BuildContainers();
      _AllowSave();
      tabKeysave.Focus();
    }
    private void btnPrevious_Click(object sender, RoutedEventArgs e) {
      tabKeygen.Focus();
    }

    private void chkEncrypt_Checked(object sender, RoutedEventArgs e) {
      //txtPublicKey.Text = _PublicKey.EncryptTo3DESBase64(new TSymmetricSecurityKey(TSymmetricSecurityKey.KeyType.Default), Encoding.Default);
      //txtPrivateKey.Text = _PrivateKey.EncryptTo3DESBase64(new TSymmetricSecurityKey(TSymmetricSecurityKey.KeyType.Default), Encoding.Default);
      _BuildContainers();
    }
    private void chkEncrypt_Unchecked(object sender, RoutedEventArgs e) {
      txtPrivateKey.Text = _PrivateKey;
      txtPublicKey.Text = _PublicKey;
      _BuildContainers();
    }

    private void txtPrefix_TextChanged(object sender, TextChangedEventArgs e) {
      _BuildContainers();
    }
    private void btnGetStorageLocation_Click(object sender, RoutedEventArgs e) {
      FolderBrowserDialog FBD = new FolderBrowserDialog();
      FBD.SelectedPath = Environment.CurrentDirectory;
      if (FBD.ShowDialog() ==System.Windows.Forms.DialogResult.OK) {
        txtStorageLocation.Text = FBD.SelectedPath;
      }
      _AllowSave();
    }
    private void txtStorageLocation_TextChanged(object sender, TextChangedEventArgs e) {
      _AllowSave();
    }

    private void btnQuit_Click(object sender, RoutedEventArgs e) {
      System.Windows.Application.Current.Shutdown();
    }

    private void btnSavePrivate_Click(object sender, RoutedEventArgs e) {
      _SavePrivateKey();
    }
    private void btnSavePublic_Click(object sender, RoutedEventArgs e) {
      _SavePublicKey();
    }
    private void btnSavePrivatePublic_Click(object sender, RoutedEventArgs e) {
      _SavePrivateKey();
      _SavePublicKey();
    }

    private void _BuildContainers() {
      txtPrivateContainer.Text = string.Format("{0}-pvt.{1}", txtPrefix.Text, chkEncrypt.IsChecked == true ? "blcrypt" : "blkey");
      txtPublicContainer.Text = string.Format("{0}-pub.{1}", txtPrefix.Text, chkEncrypt.IsChecked == true ? "blcrypt" : "blkey");
    }
    private void _AllowSave() {
      if (Directory.Exists(txtStorageLocation.Text)) {
        btnSavePrivate.IsEnabled = true;
        btnSavePublic.IsEnabled = true;
        btnSavePrivatePublic.IsEnabled = true;
      } else {
        btnSavePrivate.IsEnabled = false;
        btnSavePublic.IsEnabled = false;
        btnSavePrivatePublic.IsEnabled = false;
      }
    }
    private void _SavePrivateKey() {
      string Destination = System.IO.Path.Combine(txtStorageLocation.Text, txtPrivateContainer.Text);
      try {
        File.WriteAllText(Destination, txtPrivateKey.Text);
        System.Windows.MessageBox.Show(string.Format("Private key successfully saved at : {0}", Destination));
      } catch (Exception ex) {
        System.Windows.MessageBox.Show(string.Format("Unable to save the private key at \"{0}\" : {1}", Destination, ex.Message));
      }
    }
    private void _SavePublicKey() {
      string Destination = System.IO.Path.Combine(txtStorageLocation.Text, txtPublicContainer.Text);
      try {
        File.WriteAllText(Destination, txtPublicKey.Text);
        System.Windows.MessageBox.Show(string.Format("Public key successfully saved at : {0}", Destination));
      } catch (Exception ex) {
        System.Windows.MessageBox.Show(string.Format("Unable to save the public key at \"{0}\" : {1}", Destination, ex.Message));
      }
    }

    private void tabKeysave_GotFocus(object sender, RoutedEventArgs e) {
      _BuildContainers();
      _AllowSave();
    }

    
  }
}
