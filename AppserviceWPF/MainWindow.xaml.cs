using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Threading;
using Windows.ApplicationModel;
using Windows.ApplicationModel.AppService;
using Windows.Foundation.Collections;
using Windows.Storage;

namespace AppserviceWPF
{
    public partial class MainWindow : Window
    {
        private AppServiceConnection connection = null;

        public MainWindow()
        {
            InitializeComponent();

            string deviceUUID = ApplicationData.Current.LocalSettings.Values["uuid"] as string;
            string app_config = ApplicationData.Current.LocalSettings.Values["config"] as string;

            UUID_Text.Text = $"UUID: {deviceUUID}";
            Config_Text.Text = $"App Config: {app_config}";

            InitializeAppServiceConnection();
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            Debug.WriteLine("WPF -- button clicked");

            // ask the UWP to calculate 1 + 2
            ValueSet request = new ValueSet();
            request.Add("D1", 1);
            request.Add("D2", 2);
            AppServiceResponse response = await connection.SendMessageAsync(request);

            if (response.Status == AppServiceResponseStatus.Success)
            {
                foreach (string key in response.Message.Keys)
                {
                    double result = (double)response.Message["RESULT"];
                    Result_Text.Text = $"Result: {result.ToString()}";
                }
            }
            else
            {
                Result_Text.Text = $"Connection failed: {response.Status}";
            }
        }

        private async void InitializeAppServiceConnection()
        {
            connection = new AppServiceConnection();
            connection.AppServiceName = "CustomInteropService";
            connection.PackageFamilyName = Package.Current.Id.FamilyName;
            connection.RequestReceived += Connection_RequestReceived;
            connection.ServiceClosed += Connection_ServiceClosed;

            AppServiceConnectionStatus status = await connection.OpenAsync();
            if (status == AppServiceConnectionStatus.Success)
            {
                Error_Text.Text = $"Error: none";
                this.IsEnabled = true;
            } else
            {
                Error_Text.Text = $"Error: {status.ToString()}";
                this.IsEnabled = false;
            }
        }

        /// <summary>
        /// Handles the event when the app service connection is closed
        /// </summary>
        private void Connection_ServiceClosed(AppServiceConnection sender, AppServiceClosedEventArgs args)
        {
            // connection to the UWP lost, so we shut down the desktop process
            Dispatcher.BeginInvoke(DispatcherPriority.Normal, new Action(() =>
            {
                Application.Current.Shutdown();
            }));
        }

        /// <summary>
        /// Handles the event when the desktop process receives a request from the UWP app
        /// </summary>
        private async void Connection_RequestReceived(AppServiceConnection sender, AppServiceRequestReceivedEventArgs args)
        {
            string key = args.Request.Message["KEY"] as string;

            ValueSet response = new ValueSet();
            response.Add("result", $"Okay value: {key}");

            await args.Request.SendResponseAsync(response);
        }
    }
}
