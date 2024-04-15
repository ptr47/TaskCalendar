using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Productivity
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private static Dictionary<DateTime, List<Task>> weekTasks = [];

        private static readonly TaskManager taskManager = new("C:\\Users\\vboxuser\\Documents\\TasksPath");

        public MainWindow()
        {
            InitializeComponent();
        }

        private void ShowWeekTasks()
        {
            DateTime today = DateTime.Today;
            DateTime startDate = today.AddDays(-(today.DayOfWeek - DayOfWeek.Monday + 7) % 7);
            DateTime endDate = startDate.AddDays(6);

            // Get tasks for each day of current week
            for (DateTime date = startDate; date <= endDate; date = date.AddDays(1))
            {
                List<Task> tasks = taskManager.GetTasks(date);
                weekTasks[date] = tasks;
            }

            // Display tasks on the calendar
            foreach (var date in weekTasks.Keys)
            {
                foreach (var task in weekTasks[date])
                {

                    AddTaskIndicator(date.DayOfWeek, task);
                }
            }
        }
        private void AddTaskIndicator(DayOfWeek dayOfWeek, Task task)
        {
            StackPanel stackPanel = GetStackPanel(dayOfWeek);
            if (stackPanel != null)
            {
                Label label = new()
                {
                    Content = task.Description

                };
                stackPanel.Children.Add(label);
            }
        }
        private StackPanel GetStackPanel(DayOfWeek dayOfWeek)
        {
#pragma warning disable CS8603 // Possible null reference return.
            return dayOfWeek switch
            {
                DayOfWeek.Monday => Monday_StackPanel,
                DayOfWeek.Tuesday => Tuesday_StackPanel,
                DayOfWeek.Wednesday => Wednesday_StackPanel,
                DayOfWeek.Thursday => Thursday_StackPanel,
                DayOfWeek.Friday => Friday_StackPanel,
                DayOfWeek.Saturday => Saturday_StackPanel,
                DayOfWeek.Sunday => Sunday_StackPanel,
                _ => null,
            };
#pragma warning restore CS8603 // Possible null reference return.
        }

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

        private void Settings_btn_Click(object sender, RoutedEventArgs e)
        {
            ShowWeekTasks();
        }
    }


}
