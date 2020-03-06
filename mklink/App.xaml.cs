using System.Globalization;
using System.Reflection;
using System.Threading;
using System.Windows;

namespace mklink
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private Mutex m_Mutex;

        private MainWindow m_MainWindow;

        private void Application_Startup(object sender, StartupEventArgs e)
        {
            var ass = Assembly.GetExecutingAssembly();
            bool mutexCreated;
            var mutexName = string.Format(CultureInfo.InvariantCulture, "Local\\{{{0}}}{{{1}}}",
                ass.GetType().GUID, ass.GetName().Name);

            m_Mutex = new Mutex(true, mutexName, out mutexCreated);

            if (!mutexCreated) {
                m_Mutex = null;
                MessageBox.Show("You can only have one instance of mklink open at a time", "Multi-Instance",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                Shutdown();

                return;
            }

            m_MainWindow = new MainWindow();
            m_MainWindow.Show();
        }

        private void Application_Exit(object sender, ExitEventArgs e)
        {

        }
    }
}
