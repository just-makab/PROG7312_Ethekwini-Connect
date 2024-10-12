using System.Windows;

/***************************************************************************************
*    Title: C# Tutorial - Advance Custom MessageBox (Yes or No)
*    Author: Blade Programming
*    URL: https://youtu.be/oAvx12mx7zs?si=BvnUtzUGVQ3ADR0_
***************************************************************************************/


namespace PROG7312_Ethekwini_Connect
{
    /// <summary>
    /// Interaction logic for SettingsWindow.xaml
    /// </summary>
    public partial class SettingsWindow : Window
    {
        public SettingsWindow()
        {
            InitializeComponent();
            FeedbackPromptCheckBox.IsChecked = Properties.Settings.Default.DisableFeedbackPrompt;
        }
        private void ClearReports_Click(object sender, RoutedEventArgs e)
        {
            // Confirm before clearing all reports
            var result = MessageBox.Show("Are you sure you want to clear all reports?", "Confirm", MessageBoxButton.YesNo, MessageBoxImage.Warning);

            if (result == MessageBoxResult.Yes)
            {
                ReportManager.Reports.Clear();
                // Refresh the DataGrid or other UI elements if needed
                MessageBox.Show("All reports have been cleared.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void SaveSettings_Click(object sender, RoutedEventArgs e)
        {
            // Save the settings
            Properties.Settings.Default.DisableFeedbackPrompt = FeedbackPromptCheckBox.IsChecked == true;
            Properties.Settings.Default.Save();
            MessageBox.Show("Settings have been saved.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            this.Close();
        }
    }
}
