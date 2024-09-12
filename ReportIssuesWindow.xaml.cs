using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
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
            try
            {
                // Ensure the file path is correctly formatted and the image exists
                string imagePath = @"C:\PROG7312-POE\PROG7312-Ethekwini Connect\No Image.jpg";

                if (File.Exists(imagePath))
                {
                    BitmapImage image = new BitmapImage();
                    image.BeginInit();
                    image.UriSource = new Uri(imagePath);
                    image.EndInit();
                    ImagePreview.Source = image;
                }
                else
                {
                    MessageBox.Show("Default image not found.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Image Load Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void AttachMedia_Click(object sender, RoutedEventArgs e)
        {
            // Upload photo
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Image files (*.jpg, *.jpeg, *.png)|*.jpg;*.jpeg;*.png";
            if (openFileDialog.ShowDialog() == true)
            {
                try
                {
                    BitmapImage image = new BitmapImage(new Uri(openFileDialog.FileName));
                    ImagePreview.Source = image;
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

                // Update the DataGrid
                ReportDataGrid.ItemsSource = null; // Refresh
                ReportDataGrid.ItemsSource = reports;

                MessageBox.Show("Thank you for your report! Please provide feedback on the reporting function.", "Feedback", MessageBoxButton.OK, MessageBoxImage.Information);
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

        private void UpdateProgressBar()
        {
            int filledFields = 0;
            if (!string.IsNullOrEmpty(Title.Text)) filledFields++;
            if (!string.IsNullOrEmpty(LocationTextBox.Text)) filledFields++;
            if (CategoryComboBox.SelectedItem != null) filledFields++;
            if (!string.IsNullOrEmpty(new TextRange(DescriptionRichTextBox.Document.ContentStart, DescriptionRichTextBox.Document.ContentEnd).Text)) filledFields++;
            if (ImagePreview.Source != null) filledFields++;

            double progress = (filledFields / 5.0) * 100;
            ProgressBar.Value = progress;

            // Display percentage below progress bar
            ProgressPercentageTextBlock.Text = $"{progress}% completed";
        }


        private bool ValidateFields()
        {
            // Check if all fields are filled
            return !string.IsNullOrEmpty(Title.Text) &&
                   !string.IsNullOrEmpty(LocationTextBox.Text) &&
                   CategoryComboBox.SelectedItem != null &&
                   !string.IsNullOrEmpty(new TextRange(DescriptionRichTextBox.Document.ContentStart, DescriptionRichTextBox.Document.ContentEnd).Text);
        }

        private void ClearForm()
        {
            // Clear all fields
            Title.Clear();
            LocationTextBox.Clear();
            CategoryComboBox.SelectedIndex = -1;
            DescriptionRichTextBox.Document.Blocks.Clear();
            //ProgressBar.Value = 0;
        }

        private void InitializeTooltips()
        {
            // Example tooltips for buttons
            AttachMedia.ToolTip = "Attach any relevant media to your report.";
            Submit.ToolTip = "Submit your report once all fields are filled.";
            Back.ToolTip = "Return to the main menu.";
        }
    }

    public class Report
    {
        public string Title { get; set; }
        public string Location { get; set; }
        public string Category { get; set; }
        public string Description { get; set; }
        public string ImagePath { get; set; }
    }
}
