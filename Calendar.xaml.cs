using System.Globalization;
using System.Runtime.InteropServices;
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
            displayedDate = displayedDate.AddMonths(-1); // Move to the next month
            UpdateCalendar();
        }

        private void ShowNextMonth(object sender, RoutedEventArgs e)
        {
            displayedDate = displayedDate.AddMonths(1); // Move to the next month
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
                if (yearIntUpDown.Value < 1900 || yearIntUpDown.Value > 2100)
                {
                    // Display error message or take appropriate action
                    MessageBox.Show("Please enter a valid year between 1900 and 2100");
                    // Reset year to current year
                    yearIntUpDown.Value = displayedDate.Year;
                }
                else
                {
                int yearDiff = (int)yearIntUpDown.Value - displayedDate.Year; // Value will not be null
                    displayedDate.AddYears(yearDiff);
                    UpdateCalendar();
                }
        }

        //private void MonthComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        //{

        //}
    }
}
