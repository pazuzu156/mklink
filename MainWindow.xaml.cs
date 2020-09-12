using System;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using WinForms = System.Windows.Forms;

namespace mklink
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow() => InitializeComponent();

        private void BtnCreate_Click(object sender, RoutedEventArgs e) {
          if (tbFrom.Text.Equals("") || tbTo.Text.Equals("")) {
            MessageBox.Show("You need to supply the paths!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
          } else {
            var psi = new ProcessStartInfo {
              UseShellExecute = false,
              FileName = "C:\\Windows\\System32\\cmd.exe",
              RedirectStandardError = true,
              Verb = "runas",
              Arguments = $"/c mklink {((bool)rbDir.IsChecked ? "/d" : "")} {tbTo.Text} {tbFrom.Text}"
            };

            try {
              var proc = Process.Start(psi);
              var er = string.Empty;

              try {
                er = proc.StandardError.ReadToEnd();
              } catch (Exception ex) {
                er = ex.Message;
              }

              if (!er.Equals("")) {
                MessageBox.Show(er);
              } else {
                var msg = MessageBox.Show("Link has been created! Would you like to view it?",
                                          "MKLink",
                                          MessageBoxButton.YesNo,
                                          MessageBoxImage.Question);

                if (msg == MessageBoxResult.Yes) {
                  var s = tbTo.Text.Split('\\').Last();
                  var l = tbTo.Text.Replace(s, "");

                  Process.Start(l);
                }
              }
            } catch {}
          }
        }
        private void BtnBrowseOrigin_Click(object sender, RoutedEventArgs e) {
          if ((bool)rbFile.IsChecked) {
            using (var ofd = new WinForms.OpenFileDialog()) {
              if (ofd.ShowDialog() == WinForms.DialogResult.OK) {
                tbFrom.Text = ofd.FileName;
              }
            }
          } else {
            using (var fbd = new WinForms.FolderBrowserDialog()) {
              if (fbd.ShowDialog() == WinForms.DialogResult.OK) {
                tbFrom.Text = fbd.SelectedPath;
              }
            }
          }
        }
        private void BtnBrowseTarget_Click(object sender, RoutedEventArgs e) {
          WinForms.SaveFileDialog sfd = new WinForms.SaveFileDialog {
            Filter = "Windows Shortcut|*.lnk"
          };

          if (!tbFrom.Text.Equals("")) {
            string l = tbFrom.Text.Split('\\').Last();
            sfd.FileName = l;
          }

          if (sfd.ShowDialog() == WinForms.DialogResult.OK) {
            string l = sfd.FileName.Replace(".lnk", "");
            tbTo.Text = l;
          }
        }
    }
}
