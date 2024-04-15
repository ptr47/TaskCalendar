using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

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
            ShowWeekTasks();
        }
        private void ClearCalendar()
        {
            foreach (var stackPanel in weekCalendarGrid.Children.OfType<StackPanel>())
            {
                stackPanel.Children.Clear();
            }
        }
        private void ShowWeekTasks()
        {
            ClearCalendar();
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
                    AddTaskIndicator(date.DayOfWeek, task, date);
                }
            }
        }
        private void AddTaskIndicator(DayOfWeek dayOfWeek, Task task, DateTime date)
        {
            Label taskLabel = new()
            {
                Content = $"{DateTime.Today.Add(task.Time):HH:mm}\n{task.Description}",
                Foreground = Brushes.White,
                Background = Brushes.DimGray,
                Padding = new(2),
                Margin = new(2),
                HorizontalContentAlignment = HorizontalAlignment.Left,
                VerticalContentAlignment = VerticalAlignment.Center
            };

            taskLabel.MouseLeftButtonUp += (sender, e) =>
            {
                if (!task.IsCompleted)
                {
                    MessageBoxResult result = MessageBox.Show($"Task: {task.Description}\nTime: {DateTime.Today.Add(task.Time).ToString("hh:mm tt")}\n\nDo you want to mark this task as completed?", "Task Information", MessageBoxButton.YesNo, MessageBoxImage.Question);

                    if (result == MessageBoxResult.Yes)
                    {
                        task.IsCompleted = true; // Mark the task as completed
                                                 // Update the task in the task manager or wherever it's stored
                                                 // Assuming you have a method to update the task status
                        taskManager.UpdateTask(date, task);

                        // Refresh the UI
                        ShowWeekTasks();
                    }
                }
                else
                {
                    MessageBoxResult result = MessageBox.Show($"Task: {task.Description}\nTime: {DateTime.Today.Add(task.Time).ToString("hh:mm tt")}\n\nDo you want to delete this task?", "Task Information", MessageBoxButton.YesNo, MessageBoxImage.Question);

                    if (result == MessageBoxResult.Yes)
                    {
                        // Delete the task
                        taskManager.DeleteTask(date, task);
                        // Refresh the UI
                        ShowWeekTasks();
                    }
                }
            };


            GetStackPanel(dayOfWeek).Children.Add(taskLabel);
        }

        private StackPanel GetStackPanel(DayOfWeek dayOfWeek) => dayOfWeek switch
        {
            DayOfWeek.Monday => Monday_StackPanel,
            DayOfWeek.Tuesday => Tuesday_StackPanel,
            DayOfWeek.Wednesday => Wednesday_StackPanel,
            DayOfWeek.Thursday => Thursday_StackPanel,
            DayOfWeek.Friday => Friday_StackPanel,
            DayOfWeek.Saturday => Saturday_StackPanel,
            DayOfWeek.Sunday => Sunday_StackPanel,
            _ => Sunday_StackPanel,
        };

        private void NewTask_btn_Click(object sender, RoutedEventArgs e)
        {
            AddTask addTask = new();
            addTask.ShowDialog();
            DateTime date = addTask.Date;
            TimeSpan time = addTask.Time;
            Task newTask = new(addTask.Description, time);
            MessageBox.Show(date.ToString());
            taskManager.AddTask(date, newTask);
            ShowWeekTasks();
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
