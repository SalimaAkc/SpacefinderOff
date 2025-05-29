using Microsoft.Win32;
using MySql.Data.MySqlClient;
using SpacefinderOff.Models;
using SpacefinderOff.Services;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Resources;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;


namespace SpacefinderOff.Views
{
    public partial class ProfilePage : Page 
    {
        public ProfilePage()
        {
            InitializeComponent();

            if (AppState.CurrentUser == null)
            {
                MessageBox.Show("Please log in to access this page.", "Authentication Required",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                this.NavigationService?.Navigate(new LoginPage());
                return;
            }

            LoadUserInfo();
            LoadUserBookings();
        }

        private void LoadUserInfo()
        {
            if (AppState.CurrentUser == null || string.IsNullOrEmpty(AppState.CurrentUser.Email))
                return;

            string connectionString = "server=localhost;user=root;password=;database=SpaceFinderAppDB;";
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                try
                {
                    conn.Open();

                    string query = "SELECT user_id, fullName, email, created_at FROM Users WHERE email = @Email";
                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@Email", AppState.CurrentUser.Email);

                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            string fullName = reader["fullName"].ToString();
                            string email = reader["email"].ToString();
                            DateTime createdAt = Convert.ToDateTime(reader["created_at"]);

                           
                            AppState.CurrentUser.UserID = reader.GetInt32("user_id");
                            AppState.CurrentUser.FullName = fullName;
                            AppState.CurrentUser.Email = email;

                            string userType = "Student"; 
                            if (email.EndsWith("@thomasmore.be"))
                            {
                                userType = "Teacher";
                            }
                            else if (email.EndsWith("@student.thomasmore.be"))
                            {
                                userType = "Student";
                            }

                            UserNameLabel.Text = fullName;         
                            FullNameLabel.Text = fullName;         
                            EmailTextBox.Text = email;
                            StatusName.Text = userType;
                            RegistrationDateTextBlock.Text = $"Member Since: {createdAt:dd MMMM yyyy}";
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error loading profile: " + ex.Message, "Database Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void LoadUserBookings()
        {
            if (AppState.CurrentUser == null)
                return;

            string connectionString = "server=localhost;user=root;password=;database=SpaceFinderAppDB;";
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                try
                {
                    conn.Open();

                    string query = @"
                        SELECT 
                            b.booking_date,
                            b.start_time,
                            b.end_time,
                            b.people_amount,
                            b.status,
                            c.room_number,
                            ca.campus_name,
                            b.created_at
                        FROM Bookings b
                        INNER JOIN Classrooms c ON b.classroom_id = c.classroom_id
                        INNER JOIN Campuses ca ON c.campus_id = ca.campus_id
                        WHERE b.user_id = @UserId
                        ORDER BY b.booking_date DESC, b.start_time DESC";

                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@UserId", AppState.CurrentUser.UserID);

                    if (BookingsListBox != null)
                        BookingsListBox.Items.Clear();

                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        bool hasBookings = false;

                        while (reader.Read())
                        {
                            hasBookings = true;

                            DateTime bookingDate = Convert.ToDateTime(reader["booking_date"]);
                            DateTime startTime = Convert.ToDateTime(reader["start_time"]);
                            DateTime endTime = Convert.ToDateTime(reader["end_time"]);
                            int peopleAmount = Convert.ToInt32(reader["people_amount"]);
                            string status = reader["status"].ToString();
                            string roomNumber = reader["room_number"].ToString();
                            string campusName = reader["campus_name"].ToString();
                            DateTime createdAt = Convert.ToDateTime(reader["created_at"]);

                            string bookingInfo = $"Room {roomNumber} - {campusName}\n" +
                                               $"Date: {bookingDate:dd MMMM yyyy}\n" +
                                               $"Time: {startTime:HH:mm} - {endTime:HH:mm}\n" +
                                               $"People: {peopleAmount}\n" +
                                               $"Status: {status}\n" +
                                               $"Booked on: {createdAt:dd MMMM yyyy HH:mm}";

                            if (BookingsListBox != null)
                            {
                                BookingsListBox.Items.Add(bookingInfo);
                            }
                        }

                        if (!hasBookings && BookingsListBox != null)
                        {
                            BookingsListBox.Items.Add("No bookings found.");
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error loading bookings: " + ex.Message, "Database Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        public void RefreshBookings()
        {
            LoadUserBookings();
        }

        private void CancelBookingButton_Click(object sender, RoutedEventArgs e)
        {
            if (BookingsListBox.SelectedItem == null)
            {
                MessageBox.Show("Please select a booking to cancel.", "No Selection", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            MessageBoxResult result = MessageBox.Show(
                "Are you sure you want to cancel this booking?",
                "Confirm Cancellation",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                MessageBox.Show("Booking cancellation feature needs to be implemented with booking IDs.",
                              "Feature Notice", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }



        private void BackToBookingsButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService?.Navigate(new BookingPage());
        }

        private void UploadPhotoButton_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openDialog = new OpenFileDialog();
            openDialog.Filter = "Image files|*.bmp;*.jpg;*.png";
            openDialog.FilterIndex = 1;
            if (openDialog.ShowDialog() == true)
            {
                ProfileImage.Source = new BitmapImage(new Uri(openDialog.FileName));
            }
        }

       
        private void UpdateInfoButton_Click(object sender, RoutedEventArgs e)
        {
            string newFullName = FullNameLabel.Text.Trim();
            string newEmail = EmailTextBox.Text.Trim();


            if (string.IsNullOrWhiteSpace(newFullName) || string.IsNullOrWhiteSpace(newEmail))
            {
                MessageBox.Show("Fullname or Email cannot be empty.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (!newEmail.EndsWith("@student.thomasmore.be") && !newEmail.EndsWith("@thomasmore.be"))
            {
                MessageBox.Show("Only school email addresses are allowed.", "Invalid Email", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            string connectionString = "server=localhost;user=root;password=;database=SpaceFinderAppDB;";
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                try
                {
                    conn.Open();

                    string updateQuery = "UPDATE Users SET fullName = @FullName, email = @Email WHERE user_id = @UserId";
                    MySqlCommand cmd = new MySqlCommand(updateQuery, conn);
                    cmd.Parameters.AddWithValue("@FullName", newFullName);
                    cmd.Parameters.AddWithValue("@Email", newEmail);
                    cmd.Parameters.AddWithValue("@UserId", AppState.CurrentUser.UserID);

                    int rowsAffected = cmd.ExecuteNonQuery();


                    if (rowsAffected > 0)
                    {
                        AppState.CurrentUser.FullName = newFullName;
                        AppState.CurrentUser.Email = newEmail;

                        UserNameLabel.Text = newFullName;
                        EmailTextBox.Text = newEmail;

                        MessageBox.Show("Your name and email has been updated successfully.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    else
                    {
                        MessageBox.Show("No changes were made.", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error updating name: " + ex.Message, "Database Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void DeleteAccountButton_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show(
                "Are you sure you want to delete your account? This action cannot be undone.",
                "Confirm Account Deletion",
                MessageBoxButton.YesNo,
                MessageBoxImage.Warning);

            if (result == MessageBoxResult.Yes)
            {
                try
                {
                    string connectionString = "server=localhost;user=root;password=;database=SpaceFinderAppDB;";
                    using (MySqlConnection conn = new MySqlConnection(connectionString))
                    {
                        conn.Open();
 
                        string deleteBookingsQuery = "DELETE FROM Bookings WHERE user_id = @UserId";
                        MySqlCommand deleteBookingsCmd = new MySqlCommand(deleteBookingsQuery, conn);
                        deleteBookingsCmd.Parameters.AddWithValue("@UserId", AppState.CurrentUser.UserID);
                        deleteBookingsCmd.ExecuteNonQuery();

                        string deleteUserQuery = "DELETE FROM Users WHERE user_id = @UserId";
                        MySqlCommand deleteUserCmd = new MySqlCommand(deleteUserQuery, conn);
                        deleteUserCmd.Parameters.AddWithValue("@UserId", AppState.CurrentUser.UserID);
                        deleteUserCmd.ExecuteNonQuery();

                        MessageBox.Show("Your account has been deleted.", "Account Deleted", MessageBoxButton.OK, MessageBoxImage.Information);

                        AppState.CurrentUser = null;
                        NavigationService?.Navigate(new LoginPage());
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error deleting account: " + ex.Message, "Database Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
        private void ChangePasswordButton_Click(object sender, RoutedEventArgs e)
        {
            string currentPassword = CurrentPasswordBox.Password;
            string newPassword = NewPasswordBox.Password;
            string confirmPassword = ConfirmNewPasswordBox.Password;
            string email = EmailTextBox.Text;

            if (string.IsNullOrWhiteSpace(currentPassword) ||
                string.IsNullOrWhiteSpace(newPassword) ||
                string.IsNullOrWhiteSpace(confirmPassword))
            {
                MessageBox.Show("Please fill in all password fields.", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (newPassword != confirmPassword)
            {
                MessageBox.Show("New passwords do not match.", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            string connectionString = "server=localhost;user=root;password=;database=SpaceFinderAppDB;"; 

            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    string query = "SELECT password FROM users WHERE email = @Email";
                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@Email", email);
                    object result = cmd.ExecuteScalar();

                    if (result == null)
                    {
                        MessageBox.Show("User not found.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }

                    string storedPassword = result.ToString();

                    if (storedPassword != currentPassword) 
                    {
                        MessageBox.Show("Current password is incorrect.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }

                    string updateQuery = "UPDATE users SET password = @NewPassword WHERE email = @Email";
                    MySqlCommand updateCmd = new MySqlCommand(updateQuery, conn);
                    updateCmd.Parameters.AddWithValue("@NewPassword", newPassword); 
                    updateCmd.Parameters.AddWithValue("@Email", email);
                    int rowsAffected = updateCmd.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("Password successfully changed.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                        CurrentPasswordBox.Clear();
                        NewPasswordBox.Clear();
                        ConfirmNewPasswordBox.Clear();
                    }

                    else
                    {
                        MessageBox.Show("Password update failed. Please try again.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error: {ex.Message}", "Exception", MessageBoxButton.OK, MessageBoxImage.Error);
                }

            }

        }

        private void BackToBooking_Click(object sender, RoutedEventArgs e)
        {
            NavigationService?.Navigate(new BookingPage());
        }
        private void RefreshBookingsButton_Click(object sender, RoutedEventArgs e)
        {
            LoadUserBookings();
        }
    }
}
