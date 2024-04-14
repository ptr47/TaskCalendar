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

        readonly TaskManager taskManager = new("TasksDirectory");

        public MainWindow()
        {
            InitializeComponent();
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

        private void ViewNextWeek_btn_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Settings_btn_Click(object sender, RoutedEventArgs e)
        {

        }
    }


}
