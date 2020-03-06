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
        public MainWindow()
        {
            InitializeComponent();
        }

        private void BtnCreate_Click(object sender, RoutedEventArgs e)
        {
            if (tbFrom.Text.Equals("") || tbTo.Text.Equals("")) {
                MessageBox.Show("You need to supply the paths!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            } else {
                ProcessStartInfo psi = new ProcessStartInfo {
                    UseShellExecute = false,
                    FileName = @"C:\Windows\System32\cmd.exe",
                    RedirectStandardError = true,
                    Verb = "runas"
                };

                if ((bool)rbDir.IsChecked) {
                    psi.Arguments = string.Format("/c mklink /d \"{0}\" \"{1}\"", tbTo.Text, tbFrom.Text);
                } else if ((bool)rbFile.IsChecked) {
                    psi.Arguments = string.Format("/c mklink \"{0}\" \"{1}\"", tbTo.Text, tbFrom.Text);
                }

                try {
                    Process p = Process.Start(psi);
                    string er = "";

                    try {
                        er = p.StandardError.ReadToEnd();
                    } catch (Exception ex) {
                        er = ex.Message;
                    }

                    if (!er.Equals("")) {
                        MessageBox.Show(er);
                    } else {
                        string m = "Link has been created! Would you like to view it?";

                        if (MessageBox.Show(m, "MKLink", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes) {
                            string s = tbTo.Text.Split('\\').Last();
                            string l = tbTo.Text.Replace(s, "");

                            Process.Start(l);
                        }
                    }
                } catch (Exception) { }
            }
        }

        private void BtnBrowseOrigin_Click(object sender, RoutedEventArgs e)
        {
            if ((bool)rbFile.IsChecked) {
                WinForms.OpenFileDialog ofd = new WinForms.OpenFileDialog();

                if (ofd.ShowDialog() == WinForms.DialogResult.OK) {
                    tbFrom.Text = ofd.FileName;
                }
            } else {
                WinForms.FolderBrowserDialog fbd = new WinForms.FolderBrowserDialog();

                if (fbd.ShowDialog() == WinForms.DialogResult.OK) {
                    tbFrom.Text = fbd.SelectedPath;
                }
            }
        }

        private void BtnBrowseTarget_Click(object sender, RoutedEventArgs e)
        {
            WinForms.SaveFileDialog sfd = new WinForms.SaveFileDialog {
                Filter = "Windows Shortcut|*.lnk"
            };

            if (!tbFrom.Text.Equals("")) {
                string l = tbFrom.Text.Split('\\').Last();
                sfd.FileName = l;
            }

            if (sfd.ShowDialog() == WinForms.DialogResult.OK) {
                string lnk = sfd.FileName.Replace(".lnk", "");
                tbTo.Text = lnk;
            }
        }
    }
}
