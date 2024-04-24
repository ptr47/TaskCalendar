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

        private static readonly TaskManager taskManager = new();
        private static bool startOnSunday = false;

        public MainWindow()
        {
            InitializeComponent();
            UpdateMainView();
        }
        private void ClearCalendar()
        {
            foreach (var stackPanel in weekCalendarGrid.Children.OfType<StackPanel>())
            {
                stackPanel.Children.Clear();
            }
        }
        private void UpdateStats()
        {
            TotalTasksTextBlock.Text = $"Total Tasks: {TaskManager.TotalTasks}";
            CompletedTasksTextBlock.Text = $"Completed Tasks: {TaskManager.CompletedTasks}";
            IncompleteTasksTextBlock.Text = $"Incomplete Tasks: {TaskManager.IncompleteTasks}";
        }
        private void UpdateMainView()
        {
            ClearCalendar();
            UpdateStats();
            DateTime today = DateTime.Today;
            DateTime startDate = today.AddDays(-(today.DayOfWeek - DayOfWeek.Monday + 7) % 7);
            DateTime endDate = startDate.AddDays(6);

            // Get tasks for each day of current week
            for (DateTime date = startDate; date <= endDate; date = date.AddDays(1))
            {
                List<Task> tasks = TaskManager.GetTasks(date);
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
        public void AddTaskIndicator(DayOfWeek dayOfWeek, Task task, DateTime date)
        {
            Label taskLabel = new()
            {
                Content = $"{DateTime.Today.Add(task.Time):HH:mm}\n{task.Description}",
                Foreground = date < DateTime.Today ? Brushes.Red : Brushes.WhiteSmoke,
                Padding = new(2),
                Margin = new(2),
                HorizontalContentAlignment = HorizontalAlignment.Left,
                VerticalContentAlignment = VerticalAlignment.Center
            };
            if (task.IsCompleted)
            {
                taskLabel.Background = Brushes.ForestGreen;
            }
            else
            {
                taskLabel.Background = Brushes.DimGray;
            }

            taskLabel.MouseRightButtonUp += (sender, e) =>
            {
                ContextMenu contextMenu = new();

                MenuItem showMenuItem = new()
                {
                    Header = "Show"
                };
                showMenuItem.Click += (s, _) =>
                {
                    MessageBox.Show($"Task: {task.Description}\nTime: {DateTime.Today.Add(task.Time):HH:mm}", "Task Information", MessageBoxButton.OK, MessageBoxImage.Information);
                };
                contextMenu.Items.Add(showMenuItem);

                MenuItem completeMenuItem = new()
                {
                    Header = "Complete"
                };
                completeMenuItem.Click += (s, _) =>
                {
                    task.IsCompleted = true;
                    TaskManager.UpdateTask(date, task);
                    TaskManager.UpdateStatistics(UpdateMode.Complete);
                    MessageBox.Show("Task completed!");
                    UpdateMainView();
                };
                contextMenu.Items.Add(completeMenuItem);

                // Add "Delete" option
                MenuItem deleteMenuItem = new()
                {
                    Header = "Delete"
                };
                deleteMenuItem.Click += (s, _) =>
                {
                    TaskManager.DeleteTask(date, task);
                    TaskManager.UpdateStatistics(UpdateMode.Delete);
                    MessageBox.Show("Task deleted!");
                    UpdateMainView();
                };
                contextMenu.Items.Add(deleteMenuItem);

                // Show the ContextMenu at the mouse position
                contextMenu.IsOpen = true;
            };
            taskLabel.MouseLeftButtonUp += (sender, e) =>
            {
                MessageBox.Show($"Task: {task.Description}\nTime: {DateTime.Today.Add(task.Time):HH:mm}", "Task Information", MessageBoxButton.OK, MessageBoxImage.Information);
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
        private void ChangeDayOrder()
        {
            foreach (var child in weekCalendarGrid.Children)
            {
                if (child is StackPanel stackPanel)
                {
                    // Get the current column index
                    int currentColumn = Grid.GetColumn(stackPanel);

                    // Adjust the column index based on the flag
                    int newColumn = startOnSunday ? (currentColumn - 1 + 7) % 7 : (currentColumn + 1) % 7;
                    Grid.SetColumn(stackPanel, newColumn);

                    // Adjust background color
                    stackPanel.Background = (newColumn % 2 == 0) ? new SolidColorBrush((Color)ColorConverter.ConvertFromString("#4a4a4a")) : new SolidColorBrush((Color)ColorConverter.ConvertFromString("#5a5a5a"));
                }
                if (child is TextBlock textBlock)
                {
                    // Get the current column index
                    int currentColumn = Grid.GetColumn(textBlock);

                    int newColumn = startOnSunday ? (currentColumn - 1 + 7) % 7 : (currentColumn + 1) % 7;

                    // Set the new column index
                    Grid.SetColumn(textBlock, newColumn);

                }
            }
            if ((string)Settings_btn.Content == "Setting - Monday")
            {
                Settings_btn.Content = "Setting - Sunday";
            }
            else
            {
                Settings_btn.Content = "Setting - Monday";
            }

            startOnSunday = !startOnSunday;
        }
        private void NewTask_btn_Click(object sender, RoutedEventArgs e)
        {
            AddTask addTask = new();
            addTask.ShowDialog();
            DateTime date = addTask.Date;
            TimeSpan time = addTask.Time;
            Task newTask = new(addTask.Description, time);
            TaskManager.AddTask(date, newTask);
            TaskManager.UpdateStatistics(UpdateMode.Add);
            UpdateMainView();
        }

        private void ViewCalendar_btn_Click(object sender, RoutedEventArgs e)
        {
            new Calendar(startOnSunday).ShowDialog();
        }

        private void Settings_btn_Click(object sender, RoutedEventArgs e)
        {
            ChangeDayOrder();
        }
    }


}
