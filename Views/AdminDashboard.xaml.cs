using MySql.Data.MySqlClient;
using SpacefinderOff.Models;
using SpacefinderOff.Services;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace SpacefinderOff.Views
{
    public partial class AdminDashboard : Page
    {
        private string connectionString = "server=localhost;user=root;password=;database=SpaceFinderAppDB;";

        public AdminDashboard()
        {
            InitializeComponent();
            Loaded += AdminDashboard_Loaded;
            Unloaded += AdminDashboard_Unloaded;

            AppState.RefreshRequested += OnRefreshRequested;
        }

        private void AdminDashboard_Unloaded(object sender, RoutedEventArgs e)
        {
            AppState.RefreshRequested -= OnRefreshRequested;
        }

        private async void OnRefreshRequested()
        {
            if (IsLoaded)
            {
                Dispatcher.Invoke(() =>
                {
                    RefreshDataButton.Content = "🔄 Refreshing...";
                });

                await Task.Delay(300);

                Dispatcher.Invoke(() =>
                {
                    LoadUsers();
                    LoadStatistics();
                    RefreshDataButton.Content = "🔄 Refresh Data";
                });
            }
        }

        private void AdminDashboard_Loaded(object sender, RoutedEventArgs e)
        {
            LoadUsers();
            LoadStatistics();
        }

        private void LoadUsers()
        {
            List<AdminUser> users = new List<AdminUser>();

            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                try
                {
                    conn.Open();

                    string query = @"
                        SELECT user_id, fullname, email, phone_number, role_id, created_at 
                        FROM Users 
                        ORDER BY created_at DESC";

                    MySqlCommand cmd = new MySqlCommand(query, conn);

                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var user = new AdminUser
                            {
                                Id = reader.GetInt32("user_id"),
                                FullName = reader.IsDBNull("fullname") ? "N/A" : reader.GetString("fullname"),
                                Email = reader.IsDBNull("email") ? "N/A" : reader.GetString("email"),
                                Phone = reader.IsDBNull("phone_number") ? "" : reader.GetString("phone_number"),
                                Role = reader.IsDBNull("role_id") ? "user" : reader.GetString("role_id"),
                                RegistrationDate = reader.IsDBNull("created_at") ? DateTime.Now : reader.GetDateTime("created_at")
                            };

                            users.Add(user);
                        }
                    }

                    Dispatcher.Invoke(() =>
                    {
                        UsersDataGrid.ItemsSource = users;
                    });
                }
                catch (Exception ex)
                {
                    Dispatcher.Invoke(() =>
                    {
                        MessageBox.Show("Error loading users: " + ex.Message, "Database Error",
                                      MessageBoxButton.OK, MessageBoxImage.Error);
                    });
                }
            }
        }

        private void LoadStatistics()
        {
            try
            {
                using (var conn = new MySqlConnection(connectionString))
                {
                    conn.Open();
                    var cmd = new MySqlCommand("SELECT COUNT(*) FROM Users", conn);
                    var totalUsers = cmd.ExecuteScalar()?.ToString() ?? "0";
                    TotalUsersText.Text = totalUsers;
                }
                using (var conn = new MySqlConnection(connectionString))
                {
                    conn.Open();
                    var cmd = new MySqlCommand(
                        "SELECT COUNT(*) FROM Users WHERE created_at >= DATE_SUB(NOW(), INTERVAL 7 DAY)",
                        conn);
                    var newUsers = cmd.ExecuteScalar()?.ToString() ?? "0";
                    NewUsersText.Text = newUsers;
                }

                ActiveBookingsText.Text = "0";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading statistics: " + ex.Message, "Database Error",
                              MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void RefreshDataButton_Click(object sender, RoutedEventArgs e)
        {
            RefreshDataButton.Content = "🔄 Refreshing...";
            LoadUsers();
            LoadStatistics();
            RefreshDataButton.Content = "🔄 Refresh Data";
        }

        private void UsersDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (UsersDataGrid.SelectedItem is AdminUser selectedUser)
            {
                UserDetailsPanel.Visibility = Visibility.Visible;
                SelectedUserName.Text = "Name: " + selectedUser.FullName;
                SelectedUserEmail.Text = "Email: " + selectedUser.Email;
                SelectedUserPhone.Text = "Phone: " + selectedUser.Phone;
                SelectedUserRole.Text = "Role: " + selectedUser.Role;
                SelectedUserRegistration.Text = "Registered: " + selectedUser.RegistrationDate.ToString("dd/MM/yyyy");

                SelectedUserBookings.Text = "Total Bookings: 0";
                SelectedUserLastLogin.Text = "Last Login: N/A";
            }
        }

        private void SearchBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            string searchText = SearchBox.Text.ToLower();

            if (string.IsNullOrEmpty(searchText))
            {
                LoadUsers();
                return;
            }

            if (UsersDataGrid.ItemsSource is List<AdminUser> users)
            {
                var filteredUsers = users.FindAll(u =>
                    u.FullName.ToLower().Contains(searchText) ||
                    u.Email.ToLower().Contains(searchText) ||
                    u.Phone.Contains(searchText));

                UsersDataGrid.ItemsSource = filteredUsers;
            }
        }

        private void CreateUserButton_Click(object sender, RoutedEventArgs e)
        {
            CreateUserWindow createWindow = new CreateUserWindow();
            createWindow.Owner = Window.GetWindow(this); 
            createWindow.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            createWindow.ShowDialog();

            LoadUsers();
        }
        private void EditUserButton_Click(object sender, RoutedEventArgs e)
          { 
            MessageBox.Show("Edit user functionality not implemented");
           }
        private void ResetPasswordButton_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Password reset functionality not implemented");
        }
        private void DeleteUserButton_Click(object sender, RoutedEventArgs e)
        {
            if (UsersDataGrid.SelectedItem is AdminUser selectedUser)
            {
                var result = MessageBox.Show(
                    $"Are you sure you want to delete the user \"{selectedUser.FullName}\" (ID: {selectedUser.Id})?",
                    "Confirm Delete",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Warning);

                if (result == MessageBoxResult.Yes)
                {
                    try
                    {
                        using (MySqlConnection conn = new MySqlConnection(connectionString))
                        {
                            conn.Open();

                            string deleteQuery = "DELETE FROM Users WHERE user_id = @UserId";
                            MySqlCommand cmd = new MySqlCommand(deleteQuery, conn);
                            cmd.Parameters.AddWithValue("@UserId", selectedUser.Id);
                            int rowsAffected = cmd.ExecuteNonQuery();


                            if (rowsAffected > 0)
                            {
                                MessageBox.Show($"User with ID {selectedUser.Id} deleted successfully.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                                LoadUsers();  
                                UserDetailsPanel.Visibility = Visibility.Collapsed;
                            }
                            else
                            {
                                MessageBox.Show($"No user found with ID {selectedUser.Id}. Deletion failed.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Exception during deletion: " + ex.Message, "Database Error",
                                        MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
            else
            {
                MessageBox.Show("Please select a user to delete.", "No User Selected", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void BackToProfileButton_Click(object sender, RoutedEventArgs e)
         {
            
            NavigationService?.Navigate(new ProfilePage());

            }

        private void LogoutButton_Click(object sender, RoutedEventArgs e)
        {
            var result = MessageBox.Show(
                "Are you sure you want to logout?",
                "Confirm Logout",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                App.Current.Properties["IsLoggedIn"] = false;
                App.Current.Properties["IsAdmin"] = false;
                AppState.CurrentUser = null;

                NavigationService?.Navigate(new LoginPage());
            }
        }

    }
}