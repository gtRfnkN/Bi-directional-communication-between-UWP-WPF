using System;
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

        int counter = 0;

        public MainWindow()
        {
            InitializeComponent();

            string deviceUUID = ApplicationData.Current.LocalSettings.Values["uuid"] as string;
            string app_config = ApplicationData.Current.LocalSettings.Values["config"] as string;

            UUID_Text.Text = $"UUID: {deviceUUID}";
            Config_Text.Text = $"App Config: {app_config}";

            InitializeAppServiceConnection();
        }

        private async void InitializeAppServiceConnection()
        {
            // create AppService connection
            connection = new AppServiceConnection();
            connection.AppServiceName = "CustomInteropService";
            connection.PackageFamilyName = Package.Current.Id.FamilyName;

            // add listeners for messages and connection close
            connection.RequestReceived += Connection_RequestReceived;
            connection.ServiceClosed += Connection_ServiceClosed;

            // check if the connection was established successfully
            AppServiceConnectionStatus status = await connection.OpenAsync();
            if (status != AppServiceConnectionStatus.Success)
            {
                ShutdownWPF();
            }
        }

        /// <summary>
        /// Handles the event when the desktop process receives a request from the UWP app
        /// </summary>
        private async void Connection_RequestReceived(AppServiceConnection sender, AppServiceRequestReceivedEventArgs args)
        {
            string key = args.Request.Message["KEY"] as string;

            ValueSet response = new ValueSet();
            response.Add("result", $"Got value: {key} #{++counter}");

            await args.Request.SendResponseAsync(response);
        }

        /// <summary>
        /// Handles the event when the app service connection is closed
        /// </summary>
        private void Connection_ServiceClosed(AppServiceConnection sender, AppServiceClosedEventArgs args)
        {
            ShutdownWPF();
        }

        private void ShutdownWPF()
        {
            Dispatcher.BeginInvoke(DispatcherPriority.Normal, new Action(() =>
            {
                Application.Current.Shutdown();
            }));
        }

        private async void SendDataToUWPForResponse()
        {
            if (connection == null)
            {
                return;
            }

            // create random values to send to UWP for sum
            var random = new Random();
            int value1 = random.Next(0, 99);
            int value2 = random.Next(0, 99);

            // create value set from values
            ValueSet request = new ValueSet();
            request.Add("V1", value1);
            request.Add("V2", value2);

            // send data to UWP and wait for response
            AppServiceResponse response = await connection.SendMessageAsync(request);

            if (response.Status == AppServiceResponseStatus.Success)
            {
                var result = response.Message["RESULT"];
                Result_Text.Text = $"Result: {result.ToString()}";
            }
            else
            {
                Result_Text.Text = $"Connection failed: {response.Status}";
            }
        }


        private void Button_Click(object sender, RoutedEventArgs e)
        {
            SendDataToUWPForResponse();
        }
    }
}
