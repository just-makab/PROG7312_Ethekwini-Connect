using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace PROG7312_Ethekwini_Connect
{
    /// <summary>
    /// Interaction logic for BugReportWindow.xaml
    /// </summary>
    public partial class BugReportWindow : Window
    {
        public BugReportWindow()
        {
            InitializeComponent();
        }

        private List<string> feedbackList = new List<string>();


        private void FeedbackTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            UpdateProgressBar();
        }

        private void ReportTypeComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateProgressBar();
        }

        private void UpdateProgressBar()
        {
            int filledFields = 0;

            // Check if FeedbackTextBox is filled
            if (!string.IsNullOrEmpty(FeedbackTextBox.Text)) filledFields++;

            // Check if ReportTypeComboBox has a selected item
            if (ReportTypeComboBox.SelectedItem != null) filledFields++;

            double progress = (filledFields / 2.0) * 100; // Adjust based on number of fields
            ProgressBar.Value = progress;

            ProgressBar.Foreground = new SolidColorBrush(InterpolateColor(Colors.Red, Colors.Green, progress / 100));

            // Display percentage below progress bar
            ProgressPercentageTextBlock.Text = $"{progress}% completed";
        }

        private Color InterpolateColor(Color startColor, Color endColor, double factor)
        {
            byte r = (byte)((endColor.R - startColor.R) * factor + startColor.R);
            byte g = (byte)((endColor.G - startColor.G) * factor + startColor.G);
            byte b = (byte)((endColor.B - startColor.B) * factor + startColor.B);

            return Color.FromRgb(r, g, b);
        }


        private void SubmitFeedback_Click(object sender, RoutedEventArgs e)
        {
            // Validate that feedback is not empty
            if (string.IsNullOrEmpty(FeedbackTextBox.Text))
            {
                MessageBox.Show("Feedback cannot be empty.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // Save inputs to list
            feedbackList.Add(FeedbackTextBox.Text);
            feedbackList.Add(ReportTypeComboBox.SelectedItem?.ToString() ?? "No Category Selected");

            MessageBox.Show("Thank you for your feedback!", "Feedback Submitted", MessageBoxButton.OK, MessageBoxImage.Information);

            this.Close();
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
