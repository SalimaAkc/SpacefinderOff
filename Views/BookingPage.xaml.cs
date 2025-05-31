using MySql.Data.MySqlClient;
using SpacefinderOff.Models;
using SpacefinderOff.Services;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;

namespace SpacefinderOff.Views
{
    public partial class BookingPage : Page
    {
        public class ConflictInfo
        {
            public string CourseName { get; set; }
            public TimeSpan StartTime { get; set; }
            public TimeSpan EndTime { get; set; }
        }

        public class RoomViewModel
        {
            public string RoomNumber { get; set; }
            public string CampusName { get; set; }
            public int Capacity { get; set; }
            public List<string> Features { get; set; }
            public string Status { get; set; }
            public List<ConflictInfo> Conflicts { get; set; }

            public string EventDetails { get; set; }

            public string DisplayText => RoomNumber;

            public string StatusText => Status switch
            {
                "Scheduled" => $"Scheduled: {EventDetails}",
                "Booked" => "Booked",
                _ => "Available"
            };

            public Brush StatusBackground => Status switch
            {
                "Booked" => new SolidColorBrush(Color.FromArgb(15, 255, 0, 0)),     
                "Scheduled" => new SolidColorBrush(Color.FromArgb(15, 255, 0, 0)),  
                _ => new SolidColorBrush(Color.FromArgb(15, 0, 255, 0))             
            };

            public bool IsAvailable => Status == "Available";

            public string FeaturesDisplay => Features != null ? string.Join(", ", Features) : "None";
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
                MessageBox.Show(
                    "Please log in to access this page.",
                    "Authentication Required",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning
                );
                this.NavigationService?.Navigate(new LoginPage());
                return;
            }

            AvailableRoomsListView.ItemTemplate = CreateRoomTemplate();

            Style itemStyle = new Style(typeof(ListViewItem));
            itemStyle.Setters.Add(new Setter(Control.BackgroundProperty, new Binding("StatusBackground")));
            itemStyle.Setters.Add(new Setter(Control.BorderBrushProperty, Brushes.DarkGray));
            itemStyle.Setters.Add(new Setter(Control.BorderThicknessProperty, new Thickness(1)));
            itemStyle.Setters.Add(new Setter(Control.MarginProperty, new Thickness(0, 2, 0, 2)));
            itemStyle.Setters.Add(new Setter(Control.PaddingProperty, new Thickness(8)));
            AvailableRoomsListView.ItemContainerStyle = itemStyle;
        }

        private DataTemplate CreateRoomTemplate()
        {
            var template = new DataTemplate();

            var stackFactory = new FrameworkElementFactory(typeof(StackPanel));
            stackFactory.SetValue(StackPanel.OrientationProperty, Orientation.Horizontal);

            var roomText = new FrameworkElementFactory(typeof(TextBlock));
            roomText.SetBinding(TextBlock.TextProperty, new Binding("DisplayText"));
            roomText.SetValue(TextBlock.FontWeightProperty, FontWeights.SemiBold);
            roomText.SetValue(TextBlock.VerticalAlignmentProperty, VerticalAlignment.Center);
            roomText.SetValue(TextBlock.MarginProperty, new Thickness(0, 0, 10, 0));
            roomText.SetValue(TextBlock.ForegroundProperty, Brushes.Black); 

            var statusText = new FrameworkElementFactory(typeof(TextBlock));
            statusText.SetBinding(TextBlock.TextProperty, new Binding("StatusText"));
            statusText.SetValue(TextBlock.ForegroundProperty, Brushes.Black); 
            statusText.SetValue(TextBlock.FontStyleProperty, FontStyles.Italic);
            statusText.SetValue(TextBlock.VerticalAlignmentProperty, VerticalAlignment.Center);

            stackFactory.AppendChild(roomText);
            stackFactory.AppendChild(statusText);

            template.VisualTree = stackFactory;
            return template;
        }

        private void ConfirmButton_Click(object sender, RoutedEventArgs e)
        {
            AvailableRoomsListView.ItemsSource = null;

            if (CampusComboBox.SelectedIndex <= 0 ||
                DayComboBox.SelectedIndex <= 0 ||
                MonthComboBox.SelectedIndex <= 0 ||
                YearComboBox.SelectedIndex <= 0 ||
                StartTimeComboBox.SelectedIndex <= 0 ||
                EndTimeComboBox.SelectedIndex <= 0 ||
                PeopleAmountComboBox.SelectedIndex <= 0)
            {
                MessageBox.Show(
                    "Please select all options before confirming.",
                    "Incomplete Selection",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning
                );
                return;
            }

            try
            {
                var day = ((ComboBoxItem)DayComboBox.SelectedItem).Content.ToString();
                var month = ((ComboBoxItem)MonthComboBox.SelectedItem).Content.ToString();
                var year = ((ComboBoxItem)YearComboBox.SelectedItem).Content.ToString();
                var startTime = ((ComboBoxItem)StartTimeComboBox.SelectedItem).Content.ToString();
                var endTime = ((ComboBoxItem)EndTimeComboBox.SelectedItem).Content.ToString();
                selectedPeopleAmount = int.Parse(((ComboBoxItem)PeopleAmountComboBox.SelectedItem).Content.ToString());

                selectedDate = DateTime.ParseExact($"{day} {month} {year}", "d MMMM yyyy", CultureInfo.InvariantCulture);
                selectedStartTime = DateTime.ParseExact(startTime, "HH:mm", CultureInfo.InvariantCulture).TimeOfDay;
                selectedEndTime = DateTime.ParseExact(endTime, "HH:mm", CultureInfo.InvariantCulture).TimeOfDay;
                selectedCampusName = ((ComboBoxItem)CampusComboBox.SelectedItem).Content.ToString();

                if (selectedEndTime <= selectedStartTime)
                {
                    MessageBox.Show(
                        "End time must be after start time.",
                        "Invalid Time Range",
                        MessageBoxButton.OK,
                        MessageBoxImage.Warning
                    );
                    return;
                }

                var roomViewModels = GetRoomViewModels(
                    selectedCampusName,
                    selectedDate,
                    selectedStartTime,
                    selectedEndTime,
                    selectedPeopleAmount
                );

                if (roomViewModels.Count == 0)
                {
                    var noResultsMessage = new List<object> { "No available rooms found for the selected criteria." };
                    AvailableRoomsListView.ItemsSource = noResultsMessage;
                }
                else
                {
                    AvailableRoomsListView.ItemsSource = roomViewModels;
                }
            }
            catch (FormatException ex)
            {
                MessageBox.Show(
                    $"Invalid date/time format: {ex.Message}",
                    "Error",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error
                );
            }
            catch (MySqlException ex)
            {
                MessageBox.Show(
                    $"Database error: {ex.Message}",
                    "Error",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error
                );
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"Unexpected error: {ex.Message}",
                    "Error",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error
                );
            }
        }

        private List<RoomViewModel> GetRoomViewModels(
            string campusName,
            DateTime bookingDate,
            TimeSpan startTime,
            TimeSpan endTime,
            int requiredCapacity
        )
        {
            var viewModels = new List<RoomViewModel>();
            DateTime bookingStart = bookingDate.Date + startTime;
            DateTime bookingEnd = bookingDate.Date + endTime;

            using (var connection = new MySqlConnection(ConnectionString))
            {
                connection.Open();
                int campusId = GetCampusIdByName(campusName, connection);

                string query = @"
                    SELECT 
    c.classroom_id, 
    c.room_number, 
    c.capacity,
    s.course_name,
    s.start_time AS s_start,
    s.end_time AS s_end,
    GROUP_CONCAT(DISTINCT f.feature_name SEPARATOR '|') AS features,
    CASE 
        WHEN EXISTS (
            SELECT 1 FROM Bookings 
            WHERE classroom_id = c.classroom_id
            AND status = 'Confirmed'
            AND booking_date = @bookingDate
            AND NOT (end_time <= @bookingStart OR start_time >= @bookingEnd)
        )
        THEN 'Booked'
        WHEN EXISTS (
            SELECT 1 FROM Schedules s2
            WHERE s2.classroom_id = c.classroom_id
            AND @bookingDate BETWEEN s2.start_date AND s2.end_date
            AND s2.day_of_week = @dayOfWeek
            AND NOT (s2.end_time <= @startTime OR s2.start_time >= @endTime)
        )
        THEN 'Scheduled'
        ELSE 'Available'
    END AS status
FROM Classrooms c
LEFT JOIN ClassroomFeatures cf ON c.classroom_id = cf.classroom_id
LEFT JOIN Features f ON cf.feature_id = f.feature_id
LEFT JOIN Schedules s ON c.classroom_id = s.classroom_id 
    AND @bookingDate BETWEEN s.start_date AND s.end_date
    AND s.day_of_week = @dayOfWeek
    AND NOT (s.end_time <= @startTime OR s.start_time >= @endTime)
WHERE c.campus_id = @campusId
AND c.capacity >= @requiredCapacity
GROUP BY c.classroom_id, c.room_number, c.capacity, s.course_name, s.start_time, s.end_time
ORDER BY c.room_number";

                using (var cmd = new MySqlCommand(query, connection))
                {
                    cmd.Parameters.AddWithValue("@campusId", campusId);
                    cmd.Parameters.AddWithValue("@bookingDate", bookingDate.Date);
                    cmd.Parameters.AddWithValue("@startTime", startTime);
                    cmd.Parameters.AddWithValue("@endTime", endTime);
                    cmd.Parameters.AddWithValue("@dayOfWeek", bookingDate.DayOfWeek.ToString());
                    cmd.Parameters.AddWithValue("@bookingStart", bookingStart);
                    cmd.Parameters.AddWithValue("@bookingEnd", bookingEnd);
                    cmd.Parameters.AddWithValue("@requiredCapacity", requiredCapacity);

                    using (var reader = cmd.ExecuteReader())
                    {
                        var roomDict = new Dictionary<int, RoomViewModel>();

                        while (reader.Read())
                        {
                            int roomId = reader.GetInt32("classroom_id");

                            if (!roomDict.TryGetValue(roomId, out RoomViewModel roomVm))
                            {
                                roomVm = new RoomViewModel
                                {
                                    RoomNumber = reader.GetString("room_number"),
                                    CampusName = campusName,
                                    Capacity = reader.GetInt32("capacity"),
                                    Status = reader.GetString("status"),
                                    Features = new List<string>(),
                                    Conflicts = new List<ConflictInfo>()
                                };
                                roomDict.Add(roomId, roomVm);
                                viewModels.Add(roomVm);
                            }

                            if (!reader.IsDBNull(reader.GetOrdinal("features")))
                            {
                                var features = reader.GetString("features")
                                    .Split(new[] { '|' }, StringSplitOptions.RemoveEmptyEntries)
                                    .Distinct()
                                    .ToList();
                                roomVm.Features = features;
                            }

                            if (roomVm.Status == "Scheduled" &&
                                !reader.IsDBNull(reader.GetOrdinal("course_name")))
                            {
                                roomVm.Conflicts.Add(new ConflictInfo
                                {
                                    CourseName = reader.GetString("course_name"),
                                    StartTime = reader.GetTimeSpan("s_start"),
                                    EndTime = reader.GetTimeSpan("s_end")
                                });
                            }
                        }
                    }
                }
            }
            return viewModels;
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
            if (AvailableRoomsListView.SelectedItem is RoomViewModel selectedRoom)
            {
                var detailWindow = new Window
                {
                    Title = $"Room Details: {selectedRoom.RoomNumber}",
                    Width = 400,
                    Height = 400,
                    WindowStartupLocation = WindowStartupLocation.CenterOwner,
                    Owner = Application.Current.MainWindow
                };

                var stackPanel = new StackPanel { Margin = new Thickness(20) };

                stackPanel.Children.Add(new TextBlock
                {
                    Text = $"Room: {selectedRoom.RoomNumber}",
                    FontSize = 18,
                    FontWeight = FontWeights.Bold,
                    Margin = new Thickness(0, 0, 0, 10)
                });

                stackPanel.Children.Add(new TextBlock
                {
                    Text = $"Campus: {selectedRoom.CampusName}",
                    FontSize = 14,
                    Margin = new Thickness(0, 0, 0, 5)
                });

                stackPanel.Children.Add(new TextBlock
                {
                    Text = $"Capacity: {selectedRoom.Capacity} people",
                    FontSize = 14,
                    Margin = new Thickness(0, 0, 0, 5)
                });

                stackPanel.Children.Add(new TextBlock
                {
                    Text = $"Status: ",
                    FontSize = 14,
                    Margin = new Thickness(0, 0, 0, 5)
                });
                var statusText = new TextBlock
                {
                    Text = selectedRoom.Status,
                    FontSize = 14,
                    Foreground = selectedRoom.Status switch
                    {
                        "Available" => Brushes.Green,
                        "Booked" => Brushes.Gray,
                        "Scheduled" => Brushes.Red,
                        _ => Brushes.Black
                    },
                    FontWeight = FontWeights.Bold,
                    Margin = new Thickness(0, 0, 0, 15)
                };
                stackPanel.Children.Add(statusText);

                stackPanel.Children.Add(new TextBlock
                {
                    Text = "Features:",
                    FontSize = 16,
                    FontWeight = FontWeights.SemiBold,
                    Margin = new Thickness(0, 0, 0, 5)
                });
                var featuresText = new TextBlock
                {
                    Text = selectedRoom.FeaturesDisplay,
                    FontSize = 14,
                    TextWrapping = TextWrapping.Wrap,
                    Margin = new Thickness(10, 0, 0, 15)
                };
                stackPanel.Children.Add(featuresText);

                if (selectedRoom.Status == "Scheduled" && selectedRoom.Conflicts.Count > 0)
                {
                    stackPanel.Children.Add(new TextBlock
                    {
                        Text = "Scheduled During This Time:",
                        FontSize = 16,
                        FontWeight = FontWeights.SemiBold,
                        Margin = new Thickness(0, 0, 0, 5)
                    });

                    foreach (var conflict in selectedRoom.Conflicts)
                    {
                        stackPanel.Children.Add(new TextBlock
                        {
                            Text = $"• {conflict.CourseName} ({conflict.StartTime:hh\\:mm}-{conflict.EndTime:hh\\:mm})",
                            FontSize = 14,
                            Margin = new Thickness(10, 0, 0, 3)
                        });
                    }
                }

                if (selectedRoom.Status == "Available")
                {
                    var bookButton = new Button
                    {
                        Content = "Book This Room",
                        Margin = new Thickness(0, 20, 0, 0),
                        Padding = new Thickness(10, 5, 10, 5),
                        Background = Brushes.DodgerBlue,
                        Foreground = Brushes.White,
                        FontWeight = FontWeights.Bold
                    };

                    bookButton.Click += (s, args) =>
                    {
                        var result = MessageBox.Show(
                            $"Book room {selectedRoom.RoomNumber} on {selectedDate:dd MMMM yyyy} " +
                            $"from {selectedStartTime:hh\\:mm} to {selectedEndTime:hh\\:mm}?",
                            "Confirm Booking",
                            MessageBoxButton.YesNo,
                            MessageBoxImage.Question);

                        if (result == MessageBoxResult.Yes)
                        {
                            BookRoom(selectedRoom.RoomNumber);
                            detailWindow.Close();
                        }
                    };

                    stackPanel.Children.Add(bookButton);
                }

                detailWindow.Content = stackPanel;
                detailWindow.ShowDialog();

                AvailableRoomsListView.SelectedItem = null;
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
                        (@userId, @classroomId, @start, @end, @date, @people, @status, @createdAt)",
                        connection
                    );

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

                MessageBox.Show(
                    $"Room {roomNumber} booked successfully!\n\n" +
                    $"Date: {selectedDate:dd MMMM yyyy}\n" +
                    $"Time: {selectedStartTime:hh\\:mm} - {selectedEndTime:hh\\:mm}\n" +
                    $"People: {selectedPeopleAmount}\n" +
                    $"Status: Confirmed",
                    "Success",
                    MessageBoxButton.OK,
                    MessageBoxImage.Information
                );

                ConfirmButton_Click(null, null);
            }
            catch (MySqlException ex)
            {
                if (ex.Number == 1062) 
                {
                    MessageBox.Show(
                        "This room is no longer available for the selected time. Please choose another room.",
                        "Booking Conflict",
                        MessageBoxButton.OK,
                        MessageBoxImage.Warning
                    );
                    ConfirmButton_Click(null, null);
                }
                else
                {
                    MessageBox.Show(
                        $"Database error: {ex.Message}",
                        "Error",
                        MessageBoxButton.OK,
                        MessageBoxImage.Error
                    );
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"Unexpected error: {ex.Message}",
                    "Error",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error
                );
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