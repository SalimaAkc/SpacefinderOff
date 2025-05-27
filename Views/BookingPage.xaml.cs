using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace SpacefinderOff.Views
{
    public partial class BookingPage : Page
    {
        public BookingPage()
        {
            InitializeComponent();
        }
        private void ConfirmButton_Click(object sender, RoutedEventArgs e)
        {
            AvailableRoomsListBox.Items.Clear();

            if (CampusComboBox.SelectedIndex <= 0 ||
                DayComboBox.SelectedIndex <= 0 ||
                MonthComboBox.SelectedIndex <= 0 ||
                YearComboBox.SelectedIndex <= 0)
            {
                MessageBox.Show("Please select all options before confirming.", "Incomplete Selection", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            string? campus = (CampusComboBox.SelectedItem as ComboBoxItem)?.Content.ToString();
            string? day = (DayComboBox.SelectedItem as ComboBoxItem)?.Content.ToString();
            string? month = (MonthComboBox.SelectedItem as ComboBoxItem)?.Content.ToString();
            string? year = (YearComboBox.SelectedItem as ComboBoxItem)?.Content.ToString();
            string? startTime = (StartTimeComboBox.SelectedItem as ComboBoxItem)?.Content.ToString();
            string? endTime = (EndTimeComboBox.SelectedItem as ComboBoxItem)?.Content.ToString();

            AvailableRoomsListBox.Items.Add($"Room A - {campus} - {day} {month} {year} - {startTime} to {endTime}");
            AvailableRoomsListBox.Items.Add($"Room B - {campus} - {day} {month} {year} - {startTime} to {endTime}");
            AvailableRoomsListBox.Items.Add($"Room C - {campus} - {day} {month} {year} - {startTime} to {endTime}");

            if (string.Compare(startTime, endTime) >= 0)
            {
                MessageBox.Show("End time must be after start time.", "Invalid Time Range", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

        }

        private void ProfileButton_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService?.Navigate(new ProfilePage());
        }
    }
}
