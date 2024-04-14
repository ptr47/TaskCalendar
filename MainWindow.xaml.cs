using System.Globalization;
using System.Windows;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Productivity
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public DateTime FirstDayOfWeek { get; set; }
        public DateTime LastDayOfWeek { get; set; }

        private static readonly TaskManager taskManager = new("dir/path");

        public MainWindow()
        {
            InitializeComponent();
        }

        //private static void ShowWeekTasks()
        //{
        //    DateTime today = DateTime.Today;
        //    DateTime startDate = today.AddDays(-(today.DayOfWeek - DayOfWeek.Monday + 7) % 7);
        //    DateTime endDate = startDate.AddDays(6);

        //    for (DateTime date = startDate; date <= endDate; date = date.AddDays(1))
        //    {
        //        List<Task> tasks = taskManager.GetTasks(date);
        //        tasksByDate[date] = tasks;
        //    }

        //    // Display tasks on the calendar
        //    foreach (var date in tasksByDate.Keys)
        //    {
        //        List<Task> tasks = tasksByDate[date];
        //        foreach (var task in tasks)
        //        {
        //            // Add visual indication for each task on the calendar
        //            AddTaskIndicator(date, task);
        //        }
        //    }
        //}

        private void NewTask_btn_Click(object sender, RoutedEventArgs e)
        {
            DateTime date = DateTime.Today;
            Task newTask = new("Description", TimeSpan.Zero);
            taskManager.AddTask(date, newTask);
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


}
