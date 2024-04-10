using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Productivity
{
    /// <summary>
    /// Interaction logic for Calendar.xaml
    /// </summary>
    public partial class Calendar : Window
    {
        private DateTime displayedDate;
        public Calendar()
        {
            InitializeComponent();
            displayedDate = DateTime.Today;
            GenerateCalendar();
            UpdateCalendar();
        }
        private void GenerateCalendar()
        {
            // Add column and row definitions to the Grid
            for (int i = 0; i < 7; i++)
            {
                CalendarGrid.ColumnDefinitions.Add(new ColumnDefinition());
            }

            CalendarGrid.RowDefinitions.Add(new RowDefinition() { Height = GridLength.Auto }); ;
            for (int i = 0; i < 5; i++)
            {
                CalendarGrid.RowDefinitions.Add(new RowDefinition());
            }

        }
        private void FillCalendar(DateTime targetDate)
        {
            // Clear any existing content in the calendar Grid
            CalendarGrid.Children.Clear();

            // Create a DateTime object for the first day of the month
            DateTime firstDayOfMonth = new(targetDate.Year, targetDate.Month, 1);
            int startDayOfWeek = ((int)firstDayOfMonth.DayOfWeek + 6) % 7;
            int daysInMonth = DateTime.DaysInMonth(targetDate.Year, targetDate.Month);

            // Add the day labels to the calendar
            for (int i = 0; i < 7; i++)
            {
                TextBlock dayLabel = new()
                {
                    Text = CultureInfo.CurrentCulture.DateTimeFormat.AbbreviatedDayNames[(i + 1) % 7], // Shifted by one to start with Monday
                    FontWeight = FontWeights.Bold,
                    HorizontalAlignment = HorizontalAlignment.Center,
                    VerticalAlignment = VerticalAlignment.Center,
                    Margin = new Thickness(5),
                    Foreground = Brushes.WhiteSmoke
                };
                Grid.SetColumn(dayLabel, i);
                Grid.SetRow(dayLabel, 0);
                CalendarGrid.Children.Add(dayLabel);
            }

            int row = 1;
            int col = startDayOfWeek;

            for (int day = 1; day <= daysInMonth; day++)
            {
                TextBlock dayNumber = new()
                {
                    Text = day.ToString(),
                    FontWeight = FontWeights.Bold,
                    HorizontalAlignment = HorizontalAlignment.Center,
                    VerticalAlignment = VerticalAlignment.Top,
                    Margin = new Thickness(2),
                    Foreground = Brushes.WhiteSmoke
                };

                Grid.SetColumn(dayNumber, col);
                Grid.SetRow(dayNumber, row);
                CalendarGrid.Children.Add(dayNumber);

                col++;
                if (col > 6)
                {
                    col = 0;
                    row++;
                }
            }
        }
        private void ShowPreviousMonth(object sender, RoutedEventArgs e)
        {
            if (!(displayedDate.Year == 1 && displayedDate.Month == 1))
            {
                displayedDate = displayedDate.AddMonths(-1); // Move to previous month only if it's not January year 1
            }
            UpdateCalendar();
        }

        private void ShowNextMonth(object sender, RoutedEventArgs e)
        {
            if (!(displayedDate.Year == 9999 && displayedDate.Month == 12))
            {
                displayedDate = displayedDate.AddMonths(1); // Move to the next month only if it's not December year 9999
            }
            UpdateCalendar();
        }

        private void UpdateCalendar()
        {
            FillCalendar(displayedDate); // Generate the calendar for the new month
            currentMonth_lbl.Content = displayedDate.ToString("MMMM");
            yearIntUpDown.Value = displayedDate.Year;
        }

        private void YearIntUpDown_ValueChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            int year = (int)yearIntUpDown.Value;
            if (year >= 1 && year <= 9999)
            {
                displayedDate = displayedDate.AddYears(year - displayedDate.Year);
                UpdateCalendar();
            }
            else
            {
                yearIntUpDown.Value = displayedDate.Year;
            }    
        }

        //private void MonthComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        //{

        //}
    }
}
