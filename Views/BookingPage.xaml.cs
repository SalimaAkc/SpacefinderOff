using MySql.Data.MySqlClient; 
using SpacefinderOff.Models;
using SpacefinderOff.Services;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace SpacefinderOff.Views
{
    public partial class BookingPage : Page
    {
        public class RoomAvailability
        {
            public Classroom Room { get; set; }
            public string Status { get; set; } 
            public string CourseNames { get; set; }
        }

        private const string ConnectionString = "server=localhost;database=SpacefinderAppDB;user=root;password=;";
        private DateTime selectedDate;
        private TimeSpan selectedStartTime;
        private TimeSpan selectedEndTime;
        private string selectedCampusName;
        private int selectedPeopleAmount;

        public BookingPage()
        {
            InitializeComponent();

            if (AppState.CurrentUser == null)
            {
                MessageBox.Show("Please log in to access this page.", "Authentication Required",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                this.NavigationService?.Navigate(new LoginPage());
                return;
            }
        }

        private void ConfirmButton_Click(object sender, RoutedEventArgs e)
        {
            if (PeopleAmountComboBox.SelectedIndex > 0)
            {
                selectedPeopleAmount = int.Parse(((ComboBoxItem)PeopleAmountComboBox.SelectedItem).Content.ToString());
                Console.WriteLine("Number of people: " + selectedPeopleAmount);
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

                var roomAvailabilities = GetRoomAvailability(
                    selectedCampusName,
                    selectedDate,
                    selectedStartTime,
                    selectedEndTime
                );

                if (roomAvailabilities.Count == 0)
                {
                    AvailableRoomsListBox.Items.Add("No rooms found for the selected campus.");
                }
                else
                {
                    foreach (var roomAvail in roomAvailabilities)
                    {
                        var listBoxItem = new ListBoxItem();
                        listBoxItem.Tag = roomAvail; 

                        switch (roomAvail.Status)
                        {
                            case "Available":
                                listBoxItem.Content = $"{roomAvail.Room.RoomNumber} (Capacity: {roomAvail.Room.Capacity})";
                                listBoxItem.Background = new SolidColorBrush(
                                    Color.FromArgb(0x7F, 0x00, 0xFF, 0x00)); 
                                break;

                            case "Scheduled":
                                listBoxItem.Content = $"{roomAvail.Room.RoomNumber} (Capacity: {roomAvail.Room.Capacity}) - Class: {roomAvail.CourseNames}";
                                listBoxItem.Background = new SolidColorBrush(
                                    Color.FromArgb(0x7F, 0xFF, 0x00, 0x00)); 
                                break;

                            case "Booked":
                                listBoxItem.Content = $"{roomAvail.Room.RoomNumber} (Capacity: {roomAvail.Room.Capacity}) - Already Booked";
                                listBoxItem.Background = new SolidColorBrush(Colors.LightGray);
                                break;
                        }

                        AvailableRoomsListBox.Items.Add(listBoxItem);
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

        private List<RoomAvailability> GetRoomAvailability(string campusName, DateTime bookingDate, TimeSpan startTime, TimeSpan endTime)
        {
            var rooms = new List<RoomAvailability>();

            using (var connection = new MySqlConnection(ConnectionString))
            {
                connection.Open();
                int campusId = GetCampusIdByName(campusName, connection);

                string query = @"
            SELECT 
                c.classroom_id, 
                c.room_number, 
                c.capacity,
                GROUP_CONCAT(DISTINCT s.course_name SEPARATOR ', ') AS course_names,
                CASE 
                    WHEN b.classroom_id IS NOT NULL THEN 'Booked'
                    WHEN s.classroom_id IS NOT NULL THEN 'Scheduled'
                    ELSE 'Available'
                END AS status
            FROM Classrooms c
            LEFT JOIN (
                SELECT classroom_id 
                FROM Bookings 
                WHERE DATE(booking_date) = DATE(@bookingDate)
                AND status = 'Confirmed'
                AND NOT (end_time <= @startTime OR start_time >= @endTime)
            ) b ON c.classroom_id = b.classroom_id
            LEFT JOIN (
                SELECT classroom_id, course_name 
                FROM Schedules 
                WHERE @bookingDate BETWEEN start_date AND end_date
                AND day_of_week = @dayOfWeek
                AND NOT (end_time <= @startTime OR start_time >= @endTime)
            ) s ON c.classroom_id = s.classroom_id
            WHERE c.campus_id = @campusId
            GROUP BY c.classroom_id, c.room_number, c.capacity
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
                            rooms.Add(new RoomAvailability
                            {
                                Room = new Classroom
                                {
                                    ClassroomID = reader.GetInt32("classroom_id"),
                                    RoomNumber = reader.GetString("room_number"),
                                    Capacity = reader.GetInt32("capacity")
                                },
                                Status = reader.GetString("status"),
                                CourseNames = reader.IsDBNull(reader.GetOrdinal("course_names"))
                                            ? ""
                                            : reader.GetString("course_names")
                            });
                        }
                    }
                }
            }
            return rooms;
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
            if (AvailableRoomsListBox.SelectedItem is ListBoxItem selectedItem)
            {
                if (selectedItem.Tag is RoomAvailability roomAvail)
                {
                    if (roomAvail.Status != "Available")
                    {
                        string message = roomAvail.Status == "Scheduled"
                            ? $"Room has scheduled class: {roomAvail.CourseNames}"
                            : "Room is already booked";

                        MessageBox.Show(message, "Not Available",
                            MessageBoxButton.OK, MessageBoxImage.Information);

                        AvailableRoomsListBox.SelectedItem = null;
                        return;
                    }
                    var roomNumber = roomAvail.Room.RoomNumber;
                    var result = MessageBox.Show(
                        $"Book room {roomNumber} on {selectedDate:dd MMMM yyyy} " +
                        $"from {selectedStartTime:hh\\:mm} to {selectedEndTime:hh\\:mm}?",
                        "Confirm Booking",
                        MessageBoxButton.YesNo,
                        MessageBoxImage.Question);

                    if (result == MessageBoxResult.Yes)
                    {
                        BookRoom(roomNumber);
                    }
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

                    int campusId = GetCampusIdByName(selectedCampusName, connection);
                    int classroomId = GetClassroomIdByRoomNumber(roomNumber, campusId, connection);

                    var fullStart = selectedDate.Date + selectedStartTime;
                    var fullEnd = selectedDate.Date + selectedEndTime;

                    var cmd = new MySqlCommand(
                         @"INSERT INTO Bookings 
                        (user_id, classroom_id, start_time, end_time, booking_date, people_amount, status, created_at)
                        VALUES
                        (@userId, @classroomId, @start, @end, @date, @people, @status, @createdAt)", connection);

                    cmd.Parameters.AddWithValue("@userId", AppState.CurrentUser.UserID); 
                    cmd.Parameters.AddWithValue("@classroomId", classroomId);
                    cmd.Parameters.AddWithValue("@start", fullStart);
                    cmd.Parameters.AddWithValue("@end", fullEnd);
                    cmd.Parameters.AddWithValue("@date", selectedDate.Date);
                    cmd.Parameters.AddWithValue("@people", selectedPeopleAmount);
                    cmd.Parameters.AddWithValue("@status", "Confirmed");
                    cmd.Parameters.AddWithValue("@createdAt", DateTime.Now);

                    cmd.ExecuteNonQuery();
                }

                MessageBox.Show($"Room {roomNumber} booked successfully!\n\n" +
                       $"Date: {selectedDate:dd MMMM yyyy}\n" +
                       $"Time: {selectedStartTime:hh\\:mm} - {selectedEndTime:hh\\:mm}\n" +
                       $"People: {selectedPeopleAmount}\n" +
                       $"Status: Confirmed",
                       "Success", MessageBoxButton.OK, MessageBoxImage.Information);

                ConfirmButton_Click(null, null);
            }
            catch (MySqlException ex)
            {
                if (ex.Number == 1062)
                {
                    MessageBox.Show("This room is no longer available for the selected time. Please choose another room.",
                                  "Booking Conflict", MessageBoxButton.OK, MessageBoxImage.Warning);

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
