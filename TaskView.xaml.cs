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
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Productivity
{
    /// <summary>
    /// Interaction logic for TaskView.xaml
    /// </summary>
    public partial class TaskView : Window
    {
        public TaskView()
        {
            InitializeComponent();
        }
        public TaskView(List<Task> tasks, DateTime date)
        {
            InitializeComponent();
            ViewTasks(tasks, date);
        }
        private void ViewTasks(List<Task> tasks, DateTime date)
        {
            taskViewTextBlock.Text = $"{date:dddd} - {date:dd.MM.yyyy}";
            foreach (var task in tasks)
            {
                AddTaskIndicator(task, date);
            }
        }
        private void AddTaskIndicator(Task task, DateTime date)
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
                };
                contextMenu.Items.Add(deleteMenuItem);

                // Show the ContextMenu at the mouse position
                contextMenu.IsOpen = true;
            };
            taskViewStackPanel.Children.Add(taskLabel);
        }

    }
}
