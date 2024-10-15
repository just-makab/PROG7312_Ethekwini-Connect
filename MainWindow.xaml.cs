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
            InitializeTooltips();
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
            //Open Events & Anouncments Page
            EventsAndAnnouncementsWindow eventsWindow = new EventsAndAnnouncementsWindow();
            eventsWindow.Show();
            this.Close();
        }

        private void ServiceButton_Click(object sender, RoutedEventArgs e)
        {
            //Service status coming soon
            MessageBox.Show("Service Request Status page is coming soon!", "Coming Soon", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void InitializeTooltips()
        {
            //tooltips for buttons
            ServiceButton.ToolTip = "This page is not available but will be soon :)";
            EventsButton.ToolTip = "This page is not available but will be soon :)";
        }

        private void BugButton_Click(object sender, RoutedEventArgs e)
        {
            //Feedback Form
            BugReportWindow bugReportWindow = new BugReportWindow();
            bugReportWindow.Show();
        }

        private void SettingsButton_Click(object sender, RoutedEventArgs e)
        {
            //Settings Page
            SettingsWindow SettingsWindow = new SettingsWindow();
            SettingsWindow.Show();
            this.Close ();
        }
    }
}