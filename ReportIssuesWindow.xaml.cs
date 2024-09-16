using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace PROG7312_Ethekwini_Connect
{
    public partial class ReportIssuesWindow : Window
    {
        private List<Report> reports = new List<Report>();

        public ReportIssuesWindow()
        {
            InitializeComponent();
            InitializeTooltips();
            ReportDataGrid.ItemsSource = ReportManager.Reports;

        }

        // Only update progress after new image is uploaded
        private void AttachMedia_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Image files (*.jpg, *.jpeg, *.png)|*.jpg;*.jpeg;*.png";

            if (openFileDialog.ShowDialog() == true)
            {
                try
                {
                    BitmapImage image = new BitmapImage(new Uri(openFileDialog.FileName));

                    // Check if the new image is different from the current one
                    if (ImagePreview.Source == null || (ImagePreview.Source as BitmapImage)?.UriSource.ToString() != image.UriSource.ToString())
                    {
                        ImagePreview.Source = image;
                        UpdateProgressBar();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "File Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }



        private void Submit_Click(object sender, RoutedEventArgs e)
        {
            if (ValidateFields())
            {
                var report = new Report
                {
                    Title = Title.Text,
                    Location = LocationTextBox.Text,
                    Category = (CategoryComboBox.SelectedItem as ComboBoxItem)?.Content.ToString(),
                    Description = new TextRange(DescriptionRichTextBox.Document.ContentStart, DescriptionRichTextBox.Document.ContentEnd).Text,
                    ImagePath = (ImagePreview.Source as BitmapImage)?.UriSource.ToString() // Store image path
                };

                reports.Add(report);

                ReportManager.Reports.Add(report);

                // Update the DataGrid
                ReportDataGrid.ItemsSource = null;
                ReportDataGrid.ItemsSource = reports;

                //Make Feedbacks Opitonal in settings
                if (!Properties.Settings.Default.DisableFeedbackPrompt)
                {
                    // Show success message and ask for feedback
                    var result = MessageBox.Show("Report added successfully. Would you like to give feedback on your experience?",
                                             "Feedback Request",
                                             MessageBoxButton.YesNo,
                                             MessageBoxImage.Question);

                    if (result == MessageBoxResult.Yes)
                    {
                        // Open the feedback window
                        BugReportWindow feedbackWindow = new BugReportWindow();
                        feedbackWindow.Show();
                    }

                }

                ClearForm();
                UpdateProgressBar(); // Update progress
            }
            else
            {
                MessageBox.Show("Please fill out all fields before submitting.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void BackToMainMenu_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            this.Close();
        }

        private void Title_TextChanged(object sender, TextChangedEventArgs e)
        {
            UpdateProgressBar();
        }

        private void LocationTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            UpdateProgressBar();
        }

        private void CategoryComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateProgressBar();
        }

        private void DescriptionRichTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            UpdateProgressBar();
        }

        private void ImagePreview_SourceUpdated(object sender, DataTransferEventArgs e)
        {
            UpdateProgressBar();
        }


        private void UpdateProgressBar()
        {
            int filledFields = 0;

            if (!string.IsNullOrEmpty(Title.Text)) filledFields++;
            if (!string.IsNullOrEmpty(LocationTextBox.Text)) filledFields++;
            if (CategoryComboBox.SelectedItem != null) filledFields++;
            if (!string.IsNullOrEmpty(new TextRange(DescriptionRichTextBox.Document.ContentStart, DescriptionRichTextBox.Document.ContentEnd).Text.Trim())) filledFields++;
            if (ImagePreview.Source !=  null) filledFields++;

            double progress = (filledFields / 5.0) * 100;
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


        private bool ValidateFields()
        {
            // Check if all fields are filled
            return !string.IsNullOrEmpty(Title.Text) &&
                   !string.IsNullOrEmpty(LocationTextBox.Text) &&
                   CategoryComboBox.SelectedItem != null &&
                   !string.IsNullOrWhiteSpace(new TextRange(DescriptionRichTextBox.Document.ContentStart, DescriptionRichTextBox.Document.ContentEnd).Text.Trim()) &&
                   ImagePreview.Source != null;
        }


        private void ClearForm()
        {
            // Clear all fields
            Title.Clear();
            LocationTextBox.Clear();
            CategoryComboBox.SelectedIndex = -1;
            DescriptionRichTextBox.Document.Blocks.Clear();

            ImagePreview.Source = null;
            ProgressBar.Value = 0;
            ProgressPercentageTextBlock.Text = "0% completed";
        }

        private void InitializeTooltips()
        {
            // Example tooltips for buttons
            AttachMedia.ToolTip = "Attach any relevant media to your report.";
            Submit.ToolTip = "Submit your report once all fields are filled.";
            Back.ToolTip = "Back to the main menu.";
        }
    }
}
