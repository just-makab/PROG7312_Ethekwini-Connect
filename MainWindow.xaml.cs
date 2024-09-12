using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace PROG7312_Ethekwini_Connect
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

        private void ReportIssues_Click(object sender, RoutedEventArgs e)
        {
            // Open the Report Issues form
           ReportIssuesWindow reportWindow = new ReportIssuesWindow();
           reportWindow.Show();
           this.Close();
        }

        private void EventsButton_Click(object sender, RoutedEventArgs e)
        {
            //Events page Coming soon
            MessageBox.Show("Local Events and Announcements page is coming soon!", "Coming Soon", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void ServiceButton_Click(object sender, RoutedEventArgs e)
        {
            //Service status coming soon
            MessageBox.Show("Service Request Status page is coming soon!", "Coming Soon", MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }
}