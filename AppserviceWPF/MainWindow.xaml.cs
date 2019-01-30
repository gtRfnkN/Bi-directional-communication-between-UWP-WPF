using System.Diagnostics;
using System.Windows;
using Windows.Storage;

namespace AppserviceWPF
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            string deviceUUID = ApplicationData.Current.LocalSettings.Values["uuid"] as string;
            string app_config = ApplicationData.Current.LocalSettings.Values["config"] as string;

            UUID_Text.Text = $"UUID: {deviceUUID}";
            Config_Text.Text = $"App Config: {app_config}";
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Debug.WriteLine("WPF -- button clicked");

            // TODO: send back stuff to UWP
        }
    }
}
