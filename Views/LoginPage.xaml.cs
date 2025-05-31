using MySql.Data.MySqlClient;
using SpacefinderOff.Models;
using SpacefinderOff.Services;
using SpacefinderOff.Views;
using System;
using System.Data;
using System.Windows;
using System.Windows.Controls;

namespace SpacefinderOff.Views
{
    public partial class LoginPage : Page
    {
        public event Action<string> LoginSuccessful;

        public LoginPage()
        {
            InitializeComponent();
        }

        private void SignInButton_Click(object sender, RoutedEventArgs e)
        {
            string email = EmailTextBox.Text.Trim();
            string password = PasswordHidden.Password.Trim();

            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Please enter both email and password.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (!EmailValidator.IsValidThomasMoreEmail(email) && email.ToLower() != "admin@spacefinder.be")
            {
                MessageBox.Show("Only @student.thomasmore.be or @thomasmore.be emails, or admin are allowed.",
                                "Invalid Email", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            string connectionString = "server=localhost;user=root;password=;database=SpaceFinderAppDB;";

            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                try
                {
                    conn.Open();

                    string getUserQuery = @"
                SELECT user_id, fullname, email, phone_number, role_id, created_at
                FROM Users
                WHERE email = @Email AND password = @Password";

                    MySqlCommand getUserCmd = new MySqlCommand(getUserQuery, conn);
                    getUserCmd.Parameters.AddWithValue("@Email", email);
                    getUserCmd.Parameters.AddWithValue("@Password", password);

                    using (MySqlDataReader reader = getUserCmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            string roleFromDb = reader.IsDBNull(reader.GetOrdinal("role_id"))
                                              ? "user" 
                                              : reader.GetString("role_id");

                            bool isAdmin = IsUserAdmin(email, roleFromDb);
                            string userRole = isAdmin ? "admin" : "user";

                            AppState.CurrentUser = new Models.User
                            {
                                UserID = reader.GetInt32("user_id"),
                                FullName = reader.GetString("fullname"),
                                Email = reader.GetString("email"),
                                PhoneNumber = reader.IsDBNull("phone_number") ? "" : reader.GetString("phone_number"),
                                Role = userRole
                            };

                            MessageBox.Show("Login successful!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                            App.Current.Properties["IsLoggedIn"] = true;
                            App.Current.Properties["IsAdmin"] = isAdmin;
                            LoginSuccessful?.Invoke(email);

                            if (isAdmin)
                            {
                                this.NavigationService?.Navigate(new AdminDashboard());
                            }
                            else
                            {
                                this.NavigationService?.Navigate(new ProfilePage());
                            }
                        }
                        else
                        {
                            MessageBox.Show("Invalid email or password.", "Login Failed", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Database error: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private bool IsUserAdmin(string email, string roleFromDb)
        {
            bool result = false;

            if (email.ToLower() == "admin@spacefinder.be")
            {
                result = true;
            }

            if (!result && !string.IsNullOrEmpty(roleFromDb))
            {
                string role = roleFromDb.ToLower().Trim();
                result = role == "admin" || role == "administrator" || role == "1" || role == "superuser" || role == "super_user";
            }

            MessageBox.Show($"IsUserAdmin check for email={email}, role={roleFromDb} => {result}", "Debug",
                          MessageBoxButton.OK, MessageBoxImage.Information);
            return result;
        }

        private void TogglePasswordVisibilityButton_Checked(object sender, RoutedEventArgs e)
        {
            PasswordVisible.Visibility = Visibility.Visible;
            PasswordHidden.Visibility = Visibility.Collapsed;
            PasswordVisible.Text = PasswordHidden.Password;

            EyeOutline.Visibility = Visibility.Collapsed;
            EyeFilled.Visibility = Visibility.Visible;
        }

        private void TogglePasswordVisibilityButton_Unchecked(object sender, RoutedEventArgs e)
        {
            PasswordVisible.Visibility = Visibility.Collapsed;
            PasswordHidden.Visibility = Visibility.Visible;
            PasswordHidden.Password = PasswordVisible.Text;

            EyeOutline.Visibility = Visibility.Visible;
            EyeFilled.Visibility = Visibility.Collapsed;
        }

        private void PasswordHidden_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (TogglePasswordVisibilityButton.IsChecked == true)
            {
                PasswordVisible.Text = PasswordHidden.Password;
            }
        }

        private void PasswordVisible_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (TogglePasswordVisibilityButton.IsChecked == false)
            {
                PasswordHidden.Password = PasswordVisible.Text;
            }
        }

        private void SignUpButton_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService?.Navigate(new SignUpPage());
        }

        private void ForgotPasswordButton_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService?.Navigate(new ForgotPasswordPage());
        }
    }
}