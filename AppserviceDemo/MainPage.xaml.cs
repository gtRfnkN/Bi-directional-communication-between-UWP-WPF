using System;
using System.Diagnostics;
using Windows.ApplicationModel;
using Windows.ApplicationModel.AppService;
using Windows.Foundation.Collections;
using Windows.Foundation.Metadata;
using Windows.Storage;
using Windows.UI.Core;
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
                App.AppServiceConnected += MainPage_AppServiceConnected;
                App.AppServiceDisconnected += MainPage_AppServiceDisconnected;

                // store command line parameters in local settings
                // so the Lancher can retrieve them and pass them on
                ApplicationData.Current.LocalSettings.Values["uuid"] = "UUID-67890-ghijk-12345-abcdef";
                ApplicationData.Current.LocalSettings.Values["config"] = "0-1-0-0-2-BC";

                await FullTrustProcessLauncher.LaunchFullTrustProcessForCurrentAppAsync("WPF");
            }
        }

        private async void SendButton_Click(object sender, RoutedEventArgs e)
        {
            Debug.WriteLine("UWP -- send button clicked");

            // display the response key/value pairs
            ResultText.Text = "loading...";

            ValueSet request = new ValueSet();
            request.Add("KEY", "Stuff requested");
            AppServiceResponse response = await App.Connection.SendMessageAsync(request);

            if (response.Status == AppServiceResponseStatus.Success)
            {
                foreach (string key in response.Message.Keys)
                {
                    ResultText.Text += key + " = " + response.Message[key] + "\r\n";
                }
            }
            else
            {
                ResultText.Text = $"Connection failed: {response.Status}";
            }
        }

        /// <summary>
        /// When the desktop process is connected, get ready to send/receive requests
        /// </summary>
        private async void MainPage_AppServiceConnected(object sender, AppServiceTriggerDetails e)
        {
            App.Connection.RequestReceived += AppServiceConnection_RequestReceived;
            await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                // enable UI to access  the connection
                SendButton.IsEnabled = true;
            });
        }

        /// <summary>
        /// When the desktop process is disconnected, reconnect if needed
        /// </summary>
        private async void MainPage_AppServiceDisconnected(object sender, EventArgs e)
        {
            await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                // disable UI to access the connection
                SendButton.IsEnabled = false;

                // ask user if they want to reconnect
                Reconnect();
            });
        }

        /// <summary>
        /// Handle calculation request from desktop process
        /// (dummy scenario to show that connection is bi-directional)
        /// </summary>
        private async void AppServiceConnection_RequestReceived(AppServiceConnection sender, AppServiceRequestReceivedEventArgs args)
        {
            double d1 = (double)args.Request.Message["D1"];
            double d2 = (double)args.Request.Message["D2"];
            double result = d1 + d2;

            ValueSet response = new ValueSet();
            response.Add("RESULT", result);
            await args.Request.SendResponseAsync(response);

            // log the request in the UI for demo purposes
            await this.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                ResultText.Text += string.Format("Request: {0} + {1} --> Response = {2}\r\n", d1, d2, result);
            });
        }

        /// <summary>
        /// Ask user if they want to reconnect to the desktop process
        /// </summary>
        private async void Reconnect()
        {
            await FullTrustProcessLauncher.LaunchFullTrustProcessForCurrentAppAsync();
        }
    }
}
