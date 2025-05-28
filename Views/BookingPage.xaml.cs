using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using MySql.Data.MySqlClient; 
using System.Globalization;
using SpacefinderOff.Models;

namespace SpacefinderOff.Views
{
    public partial class BookingPage : Page
    {

        private readonly int currentUserId = 1;
        private const string ConnectionString = "server=localhost;database=SpacefinderAppDB;user=root;password=;"; 
        private DateTime selectedDate;
        private TimeSpan selectedStartTime;
        private TimeSpan selectedEndTime;
        private string selectedCampusName;

        public BookingPage()
        {
            InitializeComponent();
        }

        private void ConfirmButton_Click(object sender, RoutedEventArgs e)
        {
            if (PeopleAmountComboBox.SelectedIndex > 0)
            {
                int numberOfPeople = int.Parse(((ComboBoxItem)PeopleAmountComboBox.SelectedItem).Content.ToString());

                Console.WriteLine("Number of people: " + numberOfPeople);

            }
            else
            {
                MessageBox.Show("Please select the number of people.", "Missing Info", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            AvailableRoomsListBox.Items.Clear();

            if (CampusComboBox.SelectedIndex <= 0 ||
                DayComboBox.SelectedIndex <= 0 ||
                MonthComboBox.SelectedIndex <= 0 ||
                YearComboBox.SelectedIndex <= 0 ||
                StartTimeComboBox.SelectedIndex <= 0 ||
                EndTimeComboBox.SelectedIndex <= 0)
            {
                MessageBox.Show("Please select all options before confirming.", "Incomplete Selection",
                             MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                var day = ((ComboBoxItem)DayComboBox.SelectedItem).Content.ToString();
                var month = ((ComboBoxItem)MonthComboBox.SelectedItem).Content.ToString();
                var year = ((ComboBoxItem)YearComboBox.SelectedItem).Content.ToString();
                var startTime = ((ComboBoxItem)StartTimeComboBox.SelectedItem).Content.ToString();
                var endTime = ((ComboBoxItem)EndTimeComboBox.SelectedItem).Content.ToString();

                selectedDate = DateTime.ParseExact($"{day} {month} {year}", "d MMMM yyyy", CultureInfo.InvariantCulture);
                selectedStartTime = DateTime.ParseExact(startTime, "HH:mm", CultureInfo.InvariantCulture).TimeOfDay;
                selectedEndTime = DateTime.ParseExact(endTime, "HH:mm", CultureInfo.InvariantCulture).TimeOfDay;
                selectedCampusName = ((ComboBoxItem)CampusComboBox.SelectedItem).Content.ToString();

                if (selectedEndTime <= selectedStartTime)
                {
                    MessageBox.Show("End time must be after start time.", "Invalid Time Range",
                                 MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }


                    var availableRooms = GetAvailableRooms(selectedCampusName, selectedDate, selectedStartTime, selectedEndTime);

                    if (availableRooms.Count == 0)
                    {
                        AvailableRoomsListBox.Items.Add("No rooms available for the selected time.");
                    }
                    else
                    {
                        foreach (var room in availableRooms)
                        {
                            AvailableRoomsListBox.Items.Add($"{room.RoomNumber} (Capacity: {room.Capacity})");
                        }
                    }
                }

                catch (FormatException ex)
                {
                    MessageBox.Show($"Invalid date/time format: {ex.Message}", "Error",
                                  MessageBoxButton.OK, MessageBoxImage.Error);
                }
                catch (MySqlException ex)
                {
                    MessageBox.Show($"Database error: {ex.Message}", "Error",
                                  MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }

            private List<Classroom> GetAvailableRooms(string campusName, DateTime bookingDate, TimeSpan startTime, TimeSpan endTime)
        {
            var availableRooms = new List<Classroom>();

            using (var connection = new MySqlConnection(ConnectionString))
            {
                connection.Open();

                // Get campus ID
                int campusId = GetCampusIdByName(campusName, connection);

                var query = @"
                    SELECT c.classroom_id, c.room_number, c.capacity
                    FROM Classrooms c
                    WHERE c.campus_id = @campusId
                    AND c.classroom_id NOT IN (
                        SELECT b.classroom_id 
                        FROM Bookings b 
                        WHERE DATE(b.booking_date) = DATE(@bookingDate)
                        AND b.status = 'Confirmed'
                        AND NOT (
                            TIME(b.end_time) <= @startTime OR 
                            TIME(b.start_time) >= @endTime
                        )
                    )
                    AND c.classroom_id NOT IN (
                        SELECT s.classroom_id 
                        FROM Schedules s 
                        WHERE @bookingDate BETWEEN s.start_date AND s.end_date
                        AND s.day_of_week = @dayOfWeek
                        AND NOT (
                            s.end_time <= @startTime OR 
                            s.start_time >= @endTime
                        )
                    )
                    ORDER BY c.room_number";

                using (var cmd = new MySqlCommand(query, connection))
                {
                    cmd.Parameters.AddWithValue("@campusId", campusId);
                    cmd.Parameters.AddWithValue("@bookingDate", bookingDate.Date);
                    cmd.Parameters.AddWithValue("@startTime", startTime);
                    cmd.Parameters.AddWithValue("@endTime", endTime);
                    cmd.Parameters.AddWithValue("@dayOfWeek", bookingDate.DayOfWeek.ToString());

                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            availableRooms.Add(new Classroom
                            {
                                ClassroomID = reader.GetInt32("classroom_id"),
                                RoomNumber = reader.GetString("room_number"),
                                Capacity = reader.GetInt32("capacity")
                            });
                        }
                    }
                }
            }

            return availableRooms;
        }

        private int GetCampusIdByName(string campusName, MySqlConnection connection)
        {
            var query = "SELECT campus_id FROM Campuses WHERE campus_name = @campusName";
            using (var cmd = new MySqlCommand(query, connection))
            {
                cmd.Parameters.AddWithValue("@campusName", campusName);
                var result = cmd.ExecuteScalar();
                if (result == null)
                    throw new ArgumentException($"Campus '{campusName}' not found");
                return Convert.ToInt32(result);
            }
        }

        private void AvailableRoomsListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (AvailableRoomsListBox.SelectedItem != null)
            {
                var selectedRoomText = AvailableRoomsListBox.SelectedItem.ToString();

                var roomNumber = selectedRoomText.Split(' ')[0];

                var result = MessageBox.Show(
                    $"Do you want to book room {roomNumber} on {selectedDate:dd MMMM yyyy} from {selectedStartTime:hh\\:mm} to {selectedEndTime:hh\\:mm}?",
                    "Confirm Booking",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Question);

                if (result == MessageBoxResult.Yes)
                {
                    BookRoom(roomNumber);
                }
            }
        }

        private void BookRoom(string roomNumber)
        {
            try
            {
                using (var connection = new MySqlConnection(ConnectionString))
                {
                    connection.Open();

                    // Get classroom ID by room number and campus
                    int campusId = GetCampusIdByName(selectedCampusName, connection);
                    int classroomId = GetClassroomIdByRoomNumber(roomNumber, campusId, connection);

                    var fullStart = selectedDate.Date + selectedStartTime;
                    var fullEnd = selectedDate.Date + selectedEndTime;

                    var cmd = new MySqlCommand(
                        @"INSERT INTO Bookings 
                        (user_id, classroom_id, start_time, end_time, booking_date, people_amount, status)
                        VALUES
                        (@userId, @classroomId, @start, @end, @date, @people, @status)", connection);

                    cmd.Parameters.AddWithValue("@userId", currentUserId);
                    cmd.Parameters.AddWithValue("@classroomId", classroomId);
                    cmd.Parameters.AddWithValue("@start", fullStart);
                    cmd.Parameters.AddWithValue("@end", fullEnd);
                    cmd.Parameters.AddWithValue("@date", selectedDate.Date);
                    cmd.Parameters.AddWithValue("@people", 1);
                    cmd.Parameters.AddWithValue("@status", "Confirmed");

                    cmd.ExecuteNonQuery();
                }

                MessageBox.Show($"Room {roomNumber} booked successfully!", "Success",
                              MessageBoxButton.OK, MessageBoxImage.Information);

                // Refresh the available rooms list
                ConfirmButton_Click(null, null);
            }
            catch (MySqlException ex)
            {
                if (ex.Number == 1062) // Duplicate entry error
                {
                    MessageBox.Show("This room is no longer available for the selected time. Please choose another room.",
                                  "Booking Conflict", MessageBoxButton.OK, MessageBoxImage.Warning);
                    // Refresh the list
                    ConfirmButton_Click(null, null);
                }
                else
                {
                    MessageBox.Show($"Database error: {ex.Message}", "Error",
                                  MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }


        private int GetClassroomIdByRoomNumber(string roomNumber, int campusId, MySqlConnection connection)
        {
            var query = "SELECT classroom_id FROM Classrooms WHERE room_number = @roomNumber AND campus_id = @campusId";
            using (var cmd = new MySqlCommand(query, connection))
            {
                cmd.Parameters.AddWithValue("@roomNumber", roomNumber);
                cmd.Parameters.AddWithValue("@campusId", campusId);
                var result = cmd.ExecuteScalar();
                if (result == null)
                    throw new ArgumentException($"Room '{roomNumber}' not found");
                return Convert.ToInt32(result);
            }
        }

        private void ProfileButton_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService?.Navigate(new ProfilePage());
        }
    }

}
