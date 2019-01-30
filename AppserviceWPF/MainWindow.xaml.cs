using System.Diagnostics;
using System.Windows;

namespace AppserviceWPF
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Debug.WriteLine("WPF -- button clicked");

            // TODO: send back stuff to UWP
        }
    }
}
