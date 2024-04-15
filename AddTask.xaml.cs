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

namespace Productivity
{
    /// <summary>
    /// Interaction logic for AddTask.xaml
    /// </summary>
    public partial class AddTask : Window
    {
        public string Description { get; set; }

        public DateTime Date { get; set; }
        public TimeSpan Time { get; set; }
        public AddTask()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Description = descriptionTextBoxx.Text;
            Date = (DateTime)datePicker.SelectedDate;
            int hour = (int)hourIntUpDown.Value;
            int minute = (int)minuteIntUpDown.Value;
            Time = new(hour,minute,0);
            Close();
        }

        private void HourIntUpDown_ValueChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            int hour = (int)hourIntUpDown.Value;
            if (hour < 0)
            {
                hourIntUpDown.Value = 23;
            }
            else if (hour > 23)
            {
                hourIntUpDown.Value = 0;
            }
        }
        private void MinuteIntUpDown_ValueChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            int minute = (int)minuteIntUpDown.Value;
            if (minute < 0)
            {
                minuteIntUpDown.Value = 59;
            }
            else if (minute > 59)
            {
                minuteIntUpDown.Value = 0;
            }
        }
    }
}
