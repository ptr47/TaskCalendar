using System.Globalization;
using System.Windows;

namespace Productivity
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public DateTime FirstDayOfWeek { get; set; }
        public DateTime LastDayOfWeek { get; set; }

        public MainWindow()
        {
            InitializeComponent();

        }


        // Method to retrieve tasks for a specific date from the dictionary
        static List<Task> GetTasksForDate(Dictionary<DateTime, List<Task>> tasksByDate, DateTime date)
        {
            if (tasksByDate.ContainsKey(date))
            {
                return tasksByDate[date];
            }
            else
            {
                return [];
            }
        }

        private void NewTask_btn_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ViewCalendar_btn_Click(object sender, RoutedEventArgs e)
        {
            new Calendar().ShowDialog();
        }

        private void ViewNextWeek_btn_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Settings_btn_Click(object sender, RoutedEventArgs e)
        {

        }
    }


    // Task class representing a task with a name
    class Task
    {
        public string Name { get; set; }
        bool IsCompleted { get; set; }

        public Task(string name)
        {
            Name = name;
            IsCompleted = false;
        }
    }
}
