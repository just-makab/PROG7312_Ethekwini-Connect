using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace PROG7312_Ethekwini_Connect
{
    /// <summary>
    /// Interaction logic for ServiceRequestStatus.xaml
    /// </summary>
    public partial class ServiceRequestStatus : Window
    {

        private List<ServiceRequest> userRequests = new List<ServiceRequest>();
        private List<ServiceRequest> filteredList = new List<ServiceRequest>();

        public ServiceRequestStatus()
        {
            InitializeComponent();

            userRequests = new List<ServiceRequest>
            {
               // Service Requests
                new ServiceRequest { RequestID = 1, Title = "Damaged water pipes", RequestedDate = DateTime.Now.AddDays(-2), Status = "Complete", Priority = 1 },
                new ServiceRequest { RequestID = 2, Title = "Fix pothole", RequestedDate = DateTime.Now.AddDays(-5), Status = "Canceled", Priority = 3 },
                new ServiceRequest { RequestID = 3, Title = "Water outage", RequestedDate = DateTime.Now.AddDays(-1), Status = "Pending", Priority = 2 },
                new ServiceRequest { RequestID = 4, Title = "Power outage", RequestedDate = DateTime.Now, Status = "Pending", Priority = 1 },
                new ServiceRequest { RequestID = 5, Title = "Flooded school", RequestedDate = DateTime.Now.AddDays(-3), Status = "In Progress", Priority = 2 },
                new ServiceRequest { RequestID = 6, Title = "Paint school", RequestedDate = DateTime.Now.AddDays(-7), Status = "Complete", Priority = 5 },
                new ServiceRequest { RequestID = 7, Title = "Blocked drainage", RequestedDate = DateTime.Now.AddDays(-10), Status = "Complete", Priority = 2 },
                new ServiceRequest { RequestID = 8, Title = "Streetlights malfunctioning", RequestedDate = DateTime.Now.AddDays(-4), Status = "Cancelled", Priority = 3 },
                new ServiceRequest { RequestID = 9, Title = "Tree blocking road", RequestedDate = DateTime.Now.AddDays(-6), Status = "In Progress", Priority = 1 },
                new ServiceRequest { RequestID = 10, Title = "Graffiti removal", RequestedDate = DateTime.Now.AddDays(-8), Status = "Complete", Priority = 4 },
            };
            filteredList = userRequests.ToList();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            resultsListView.ItemsSource = userRequests;
            DrawGraph(userRequests);
        }


        private class MinHeap
        {
            private List<ServiceRequest> _heap = new List<ServiceRequest>();

            private int Parent(int index) => (index - 1) / 2;
            private int LeftChild(int index) => 2 * index + 1;
            private int RightChild(int index) => 2 * index + 2;

            private void Swap(int
            index1, int index2)
            {
                (_heap[index1], _heap[index2]) = (_heap[index2], _heap[index1]);
            }

            public void Insert(ServiceRequest request)
            {
                _heap.Add(request);
                HeapUp(_heap.Count - 1);
            }

            private void HeapUp(int index)
            {
                while (index > 0 && _heap[Parent(index)].Priority > _heap[index].Priority)
                {
                    Swap(index, Parent(index));
                    index = Parent(index);
                }
            }

            public ServiceRequest ExtractMin()
            {
                if (_heap.Count == 0) return null;

                ServiceRequest min = _heap[0];
                _heap[0] = _heap[_heap.Count - 1];
                _heap.RemoveAt(_heap.Count - 1);

                if (_heap.Count > 0)
                    HeapDown(0);

                return min;
            }

            private void HeapDown(int index)
            {
                int minIndex = index;
                int leftChildIndex = LeftChild(index);
                int rightChildIndex = RightChild(index);

                if (leftChildIndex < _heap.Count && _heap[leftChildIndex].Priority < _heap[minIndex].Priority)
                    minIndex = leftChildIndex;

                if (rightChildIndex < _heap.Count && _heap[rightChildIndex].Priority < _heap[minIndex].Priority)
                    minIndex = rightChildIndex;

                if (index != minIndex)
                {
                    Swap(index, minIndex);
                    HeapDown(minIndex);
                }
            }

            public List<ServiceRequest> GetAllRequests()
            {
                return new List<ServiceRequest>(_heap);
            }
        }

        private void DrawAxes()
        {
            if (graph == null || graph.ActualWidth <= 0 || graph.ActualHeight <= 0)
                return;

            graph.Children.Clear();

            // Draw Y-Axis
            Line yAxis = new Line
            {
                X1 = 0,
                Y1 = 0,
                X2 = 0,
                Y2 = graph.ActualHeight - 30,
                Stroke = Brushes.Black,
                StrokeThickness = 2
            };
            graph.Children.Add(yAxis);

            // Draw X-Axis
            Line xAxis = new Line
            {
                X1 = 0,
                Y1 = graph.ActualHeight - 30,
                X2 = graph.ActualWidth,
                Y2 = graph.ActualHeight - 30,
                Stroke = Brushes.Black,
                StrokeThickness = 2
            };
            graph.Children.Add(xAxis);
        }

        private void DrawGraph(List<ServiceRequest> requests = null)
        {
            if (graph == null || graph.ActualWidth <= 0 || graph.ActualHeight <= 0)
                return;

            graph.Children.Clear();
            DrawAxes();

            var sourceList = requests ?? filteredList;

            // Calculate status counts dynamically
            var statusCounts = sourceList.GroupBy(req => req.Status)
                                         .ToDictionary(g => g.Key, g => g.Count());

            if (statusCounts.Count == 0)
                return;

            // Ensure statuses are displayed in a specific order
            var orderedStatuses = new[] { "Complete", "In Progress", "Pending", "Cancelled" };
            var orderedStatusCounts = orderedStatuses.Select(status =>
                new KeyValuePair<string, int>(status, statusCounts.ContainsKey(status) ? statusCounts[status] : 0));

            // Calculate graph dimensions
            int maxValue = Math.Max(1, statusCounts.Values.DefaultIfEmpty(0).Max()); // Avoid divide-by-zero
            double graphHeight = graph.ActualHeight - 60; // Space for margins
            double yAxisScale = graphHeight / maxValue;

            // Draw Y-axis labels
            for (int i = 0; i <= 5; i++)
            {
                int value = (int)((i * maxValue) / 5);
                TextBlock label = new TextBlock
                {
                    Text = value.ToString(),
                    FontSize = 12
                };
                Canvas.SetLeft(label, 5); // Padding
                Canvas.SetTop(label, graph.ActualHeight - 30 - (i * graphHeight / 5));
                graph.Children.Add(label);
            }

            // Draw bars for each status
            double availableWidth = Math.Max(0, graph.ActualWidth - 60);
            double barWidth = Math.Max(20, availableWidth / (orderedStatusCounts.Count() * 2));
            double spacing = Math.Max(5, barWidth / 2);

            int index = 0;
            foreach (var kvp in orderedStatusCounts)
            {
                double barHeight = kvp.Value * yAxisScale;

                Rectangle bar = new Rectangle
                {
                    Width = barWidth,
                    Height = barHeight,
                    Fill = GetBarColor(kvp.Key),
                    Stroke = Brushes.Black,
                    StrokeThickness = 1
                };

                double leftPosition = spacing + (index * (barWidth + spacing)) + 30;
                Canvas.SetLeft(bar, leftPosition);
                Canvas.SetTop(bar, graph.ActualHeight - 30 - barHeight);
                graph.Children.Add(bar);

                TextBlock statusLabel = new TextBlock
                {
                    Text = kvp.Key,
                    FontSize = 10,
                    TextAlignment = TextAlignment.Center,
                    Width = barWidth
                };
                Canvas.SetLeft(statusLabel, leftPosition);
                Canvas.SetTop(statusLabel, graph.ActualHeight - 15);
                graph.Children.Add(statusLabel);

                if (kvp.Value > 0)
                {
                    TextBlock valueLabel = new TextBlock
                    {
                        Text = kvp.Value.ToString(),
                        FontSize = 10,
                        TextAlignment = TextAlignment.Center,
                        Width = barWidth
                    };
                    Canvas.SetLeft(valueLabel, leftPosition);
                    Canvas.SetTop(valueLabel, Math.Max(5, graph.ActualHeight - 35 - barHeight - 15));
                    graph.Children.Add(valueLabel);
                }

                index++;
            }
        }

        private Brush GetBarColor(string status)
        {
            switch (status)
            {
                case "Cancelled":
                    return new SolidColorBrush(Color.FromRgb(231, 76, 60)); // Red
                case "Pending":
                    return new SolidColorBrush(Color.FromRgb(255, 165, 0)); // Orange
                case "In Progress":
                    return new SolidColorBrush(Color.FromRgb(241, 196, 15)); // Yellow
                case "Complete":
                    return new SolidColorBrush(Color.FromRgb(46, 204, 113)); // Green
                default:
                    return Brushes.Gray;
            }
        }



        private void SortByPriority_Click(object sender, RoutedEventArgs e)
        {
            MinHeap heap = new MinHeap();
            foreach (var request in userRequests)
                heap.Insert(request);

            List<ServiceRequest> sortedByPriority = new List<ServiceRequest>();
            ServiceRequest nextRequest;
            while ((nextRequest = heap.ExtractMin()) != null)
                sortedByPriority.Add(nextRequest);

            resultsListView.ItemsSource = sortedByPriority;
            DrawGraph(sortedByPriority);
        }


        private void SortByDate_Click(object sender, RoutedEventArgs e)
        {
            var sortedByDate = userRequests.OrderBy(req => req.RequestedDate).ToList();
            resultsListView.ItemsSource = sortedByDate;
            DrawGraph(sortedByDate);
        }


        private void Clear_Click(object sender, RoutedEventArgs e)
        {
            SearchTextBox.Text = string.Empty;
            searchComboBox.SelectedIndex = 0;
            resultsListView.ItemsSource = userRequests;
            DrawGraph(userRequests);
        }


        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            string searchTerm = SearchTextBox.Text?.ToLower();
            ComboBoxItem selectedCategory = searchComboBox.SelectedItem as ComboBoxItem;
            string category = selectedCategory?.Content.ToString();

            if (string.IsNullOrEmpty(searchTerm) || string.IsNullOrEmpty(category) || category == "Search by")
            {
                MessageBox.Show("Please select a search category and enter a search term.");
                return;
            }

            switch (category)
            {
                case "Search by ID":
                    if (int.TryParse(searchTerm, out int id))
                        filteredList = userRequests.Where(r => r.RequestID == id).ToList();
                    break;
                case "Search by Title":
                    filteredList = userRequests.Where(r => r.Title.ToLower().Contains(searchTerm)).ToList();
                    break;
                case "Search by Status":
                    filteredList = userRequests.Where(r => r.Status.ToLower().Contains(searchTerm)).ToList();
                    break;
                case "Search by Priority":
                    if (int.TryParse(searchTerm, out int priority))
                        filteredList = userRequests.Where(r => r.Priority == priority).ToList();
                    break;
            }

            if (filteredList.Count == 0)
            {
                MessageBox.Show("No matching records found.");
                filteredList = userRequests;
            }

            // Refresh the graph and results
            resultsListView.ItemsSource = filteredList;
            DrawGraph();
        }


        // Go home
        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            this.Close();
        }
    }
}
