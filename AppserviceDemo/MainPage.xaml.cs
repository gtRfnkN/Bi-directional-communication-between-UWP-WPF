using System;
using System.Diagnostics;
using Windows.ApplicationModel;
using Windows.Foundation.Metadata;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace AppserviceDemo
{
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            InitializeComponent();
        }

        private async void StartButton_Click(object sender, RoutedEventArgs e)
        {
            Debug.WriteLine("UWP -- start button clicked");

            if (ApiInformation.IsApiContractPresent("Windows.ApplicationModel.FullTrustAppContract", 1, 0))
            {
                // store command line parameters in local settings
                // so the Lancher can retrieve them and pass them on
                ApplicationData.Current.LocalSettings.Values["uuid"] = "UUID-67890-ghijk-12345-abcdef";
                ApplicationData.Current.LocalSettings.Values["config"] = "0-1-0-0-2-BC";

                await FullTrustProcessLauncher.LaunchFullTrustProcessForCurrentAppAsync("WPF");
            }
        }

        private void SendButton_Click(object sender, RoutedEventArgs e)
        {
            Debug.WriteLine("UWP -- send button clicked");

            // TODO: send data to WPF
        }
    }
}
