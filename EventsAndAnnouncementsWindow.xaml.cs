using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

/***************************************************************************************
*    Title: A Step-by-Step Guide to a Modern and Easy Design Search Results | C# Windows Forms
*    Author: Coding Ideas
*   URL: https://www.youtube.com/watch?v=FfacqCa4Gc4
***************************************************************************************/

/***************************************************************************************
*    Title: The Dictionary Data Structure in C# in 10 Minutes or Less
*    Author: IAmTimCorey
*    URL: https://www.youtube.com/watch?v=R94JHIXdTk0
***************************************************************************************/

/***************************************************************************************
*    Title: Recommendation System Algorithm In C#
*    Author: Restack
*   URL: https://www.restack.io/p/recommendation-systems-answer-recommendation-algorithm-csharp-cat-ai
***************************************************************************************/


namespace PROG7312_Ethekwini_Connect
{
    /// <summary>
    /// Interaction logic for EventsAndAnnouncementsWindow.xaml
    /// </summary>
    public partial class EventsAndAnnouncementsWindow : Window
    {
        //Category selection count for recomendations
        private Dictionary<string, int> categorySelectionCount = new Dictionary<string, int>
        {
            { "Public Services", 0 },
            { "Community", 0 },
            { "Emergency Alerts", 0 },
            { "Health and Wellness", 0 }
        };

        private bool isInitialized = false;

        //Dictionary used to store each event by its Title for Title search
        private Dictionary<string, EventActivity> eventDictionary = new Dictionary<string, EventActivity>();
        //Hashset used for categories becuase it ensures each categpry is unique
        private HashSet<string> categorySet = new HashSet<string>();
        private SortedSet<DateTime> dateSet = new SortedSet<DateTime>();
        private SortedSet<EventActivity> eventQueue;
        //Prioriy queue and activites to manage recomendtion feature
        private List<EventActivity> activities;
        private PriorityQueue priorityQueue = new PriorityQueue();

        public EventsAndAnnouncementsWindow()
        {
            InitializeComponent();
            LoadPlaceholderData();
            resultsListView.MouseDoubleClick += ResultsListView_MouseDoubleClick;
            this.Loaded += EventsAndAnnouncementsWindow_Loaded;
        }

        private void LoadPlaceholderData()
        {   
             eventQueue = new SortedSet<EventActivity>
             {
                // Community Events
                new EventActivity { Title = "Neighborhood Watch Meeting", Date = DateTime.Now.AddDays(3), Category = "Community", Description = "Discuss safety measures with your neighbors." },
                new EventActivity { Title = "Park Beautification Day", Date = DateTime.Now.AddDays(4), Category = "Community", Description = "Join us to plant trees and flowers in the park." },
                new EventActivity { Title = "Local Charity Bake Sale", Date = DateTime.Now.AddDays(6), Category = "Community", Description = "Raise money for local schools with your baked goods." },
                new EventActivity { Title = "Town Hall Meeting", Date = DateTime.Now.AddDays(7), Category = "Community", Description = "Share your thoughts with local government representatives." },
                new EventActivity { Title = "Youth Sports Tournament", Date = DateTime.Now.AddDays(12), Category = "Community", Description = "Cheer on the young athletes in our community sports day." },

                // Health and Wellness Events
                new EventActivity { Title = "Mental Health Awareness Workshop", Date = DateTime.Now.AddDays(8), Category = "Health and Wellness", Description = "Learn how to manage stress and anxiety." },
                new EventActivity { Title = "Yoga in the Park", Date = DateTime.Now.AddDays(9), Category = "Health and Wellness", Description = "A relaxing outdoor yoga session for all levels." },
                new EventActivity { Title = "Blood Donation Camp", Date = DateTime.Now.AddDays(11), Category = "Health and Wellness", Description = "Donate blood and help save lives." },
                new EventActivity { Title = "Nutrition and Diet Seminar", Date = DateTime.Now.AddDays(14), Category = "Health and Wellness", Description = "Tips on maintaining a healthy diet and lifestyle." },
                new EventActivity { Title = "CPR Training Course", Date = DateTime.Now.AddDays(13), Category = "Health and Wellness", Description = "Learn how to perform CPR and save lives." },

                // Public Services Events
                new EventActivity { Title = "Public Transportation Info Session", Date = DateTime.Now.AddDays(2), Category = "Public Services", Description = "Learn about upcoming changes to public transport routes." },
                new EventActivity { Title = "Electricity Conservation Workshop", Date = DateTime.Now.AddDays(5), Category = "Public Services", Description = "Learn how to reduce electricity usage and save costs." },
                new EventActivity { Title = "Police and Community Partnership Forum", Date = DateTime.Now.AddDays(6), Category = "Public Services", Description = "Strengthen collaboration between law enforcement and the community." },
                new EventActivity { Title = "Water Supply Disruption Notice", Date = DateTime.Now.AddDays(1), Category = "Public Services", Description = "Scheduled maintenance will disrupt water supply in certain areas." },
                new EventActivity { Title = "Sanitation Drive", Date = DateTime.Now.AddDays(3), Category = "Public Services", Description = "Improve sanitation services and waste management." },

                // Emergency Alerts Events
                new EventActivity { Title = "Severe Thunderstorm Alert", Date = DateTime.Now.AddDays(1), Category = "Emergency Alerts", Description = "Thunderstorms expected in Pietermaritzburg." },
                new EventActivity { Title = "Tornado Warning", Date = DateTime.Now.AddDays(1), Category = "Emergency Alerts", Description = "A tornado has been spotted in the surrounding area." },
                new EventActivity { Title = "Heatwave Advisory", Date = DateTime.Now.AddDays(4), Category = "Emergency Alerts", Description = "Extreme heat expected. Stay indoors and stay hydrated." },
                new EventActivity { Title = "Road Closures Due to Flooding", Date = DateTime.Now.AddDays(1), Category = "Emergency Alerts", Description = "Major roads in the area are closed due to heavy flooding." },
                new EventActivity { Title = "Power Outage Alert", Date = DateTime.Now.AddDays(2), Category = "Emergency Alerts", Description = "Emergency power outage scheduled for infrastructure repair." }
             };

            foreach (var activity in eventQueue)
            {
                if (!eventDictionary.ContainsKey(activity.Title))
                {
                    eventDictionary.Add(activity.Title, activity);
                    categorySet.Add(activity.Category);
                    dateSet.Add(activity.Date);
                }
            }

            // Populate the DataGrid with sorted events (by date)
            resultsListView.ItemsSource = eventQueue.ToList();
        }

        private void EventsAndAnnouncementsWindow_Loaded(object sender, RoutedEventArgs e)
        {
            isInitialized = true; // Page is now fully initialized
        }

        // Filter by Date
        private void OnDateSelectedChanged(object sender, RoutedEventArgs e)
        {
            DateTime? selectedDate = datePicker.SelectedDate;
            if (selectedDate.HasValue)
            {
                var filteredActivities = eventDictionary.Values
                    .Where(activity => activity.Date.Date == selectedDate.Value.Date)
                    .ToList();

                resultsListView.ItemsSource = filteredActivities;
            }
            else
            {
                //Make sure all is shown when no date is selected
                resultsListView.ItemsSource = eventQueue.ToList();
            }
        }

        // Filter by Category
        private void OnCategorySelectionChanged(object sender, RoutedEventArgs e)
        {
            if (!isInitialized)
            {
                return; // Skip category selection if not initialized
            }


            // Check if the selected item is not null
            ComboBoxItem selectedCategory = (ComboBoxItem)categoryComboBox.SelectedItem;

            // Default to an empty list of activities
            List<EventActivity> filteredActivities = new List<EventActivity>();

            // Ensure the selected category is not null
            if (selectedCategory != null)
            {
                string category = selectedCategory.Content.ToString();

                // Check if eventQueue is initialized and not null
                if (eventQueue != null && eventQueue.Count > 0)
                {
                    // Filter activities based on the selected category
                    filteredActivities = eventQueue
                        .Where(activity => activity.Category == category || category == "All Categories")
                        .ToList();
                }
            }
            else
            {
                // If no category is selected, show all activities
                filteredActivities = eventQueue.ToList();
            }

            // Update the DataGrid with the filtered activities or an empty list
            //Category Filter deactivated due to null refrence excpetion when page is first run.
            resultsListView.ItemsSource = filteredActivities.Count > 0 ? filteredActivities : new List<EventActivity>();
        }



        // Search by Title
        private void SearchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            string searchQuery = SearchTextBox.Text.ToLower();

            var filteredEvents = eventQueue
                .Where(activity => activity.Title.ToLower().Contains(searchQuery))
                .ToList();

            resultsListView.ItemsSource = filteredEvents.Count > 0 ? filteredEvents : null;

            // Optional: Show a message in the UI if no results are found
            if (filteredEvents.Count == 0)
            {
                MessageBox.Show("No event found with that title.");
            }
        }

        // Clear all filters
        private void Clear_Click(object sender, RoutedEventArgs e)
        {
            LoadPlaceholderData();
            SearchTextBox.Text = string.Empty;
            datePicker.SelectedDate = null;
            categoryComboBox.SelectedIndex = 0;
            categorySelectionCount = new Dictionary<string, int>
            {
                { "Public Services", 0 },
                { "Community", 0 },
                { "Emergency Alerts", 0 },
                { "Health and Wellness", 0 }
            };
        }

        //Display List based on users click history
        private void Recommended_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Showing recommended events.");
            // Reorder the List based on the recommendation system
            ReorderActivities();
        }

        //Go home
        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            this.Close();
        }

        //Capture selected Item
        private void ResultsListView_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (resultsListView.SelectedItem is EventActivity selectedActivity)
            {
                // Show description in message box
                MessageBox.Show(selectedActivity.Description, selectedActivity.Title);

                // Track the category selection
                if (categorySelectionCount.ContainsKey(selectedActivity.Category))
                {
                    categorySelectionCount[selectedActivity.Category]++;
                }
            }
        }

        //ReorderActivies based on thier category value(higher value = higher priority)
        private void ReorderActivities()
        {
            if (activities == null)
            {
                activities = eventQueue.ToList();
            }

            // Clear the priority queue
            priorityQueue = new PriorityQueue();

            // Enqueue each activity based on its category selection count
            foreach (var activity in activities)
            {
                if (categorySelectionCount.TryGetValue(activity.Category, out int priority))
                {
                    priorityQueue.Enqueue(activity, -priority);
                }
            }

            // Create a list to hold the sorted activities
            var sortedActivities = new List<EventActivity>();

            // Dequeue activities to sort by user interaction count
            while (!priorityQueue.IsEmpty())
            {
                sortedActivities.Add(priorityQueue.Dequeue());
            }

            // Sort by date within the same category
            sortedActivities.Sort((x, y) =>
            {
                int categoryComparison = categorySelectionCount[y.Category].CompareTo(categorySelectionCount[x.Category]);
                if (categoryComparison == 0)
                {
                    return x.Date.CompareTo(y.Date); // Sort by date if categories are the same
                }
                return categoryComparison;
            });

            // Update the DataGrid with the sorted data
            resultsListView.ItemsSource = sortedActivities;
        }
    }
}
