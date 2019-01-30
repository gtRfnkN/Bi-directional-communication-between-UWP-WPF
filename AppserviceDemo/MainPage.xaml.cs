using System;
using System.Diagnostics;
using Windows.ApplicationModel;
using Windows.Foundation.Metadata;
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
                await FullTrustProcessLauncher.LaunchFullTrustProcessForCurrentAppAsync();
            }
        }

        private void SendButton_Click(object sender, RoutedEventArgs e)
        {
            Debug.WriteLine("UWP -- send button clicked");

            // TODO: send data to WPF
        }
    }
}
