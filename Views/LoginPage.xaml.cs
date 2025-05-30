using MySql.Data.MySqlClient;
using SpacefinderOff.Models;
using SpacefinderOff.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace SpacefinderOff.Views
{
    public partial class LoginPage
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


            if (email.EndsWith("@student.thomasmore.be") || email.EndsWith("@teacher.thomasmore.be"))
            {
                LoginSuccessful?.Invoke(email); 
            }
            else
            {
                MessageBox.Show("Invalid email or password.");
            }

            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Please enter both email and password.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            if (!EmailValidator.IsValidThomasMoreEmail(email))
            {
                MessageBox.Show("Only @student.thomasmore.be or @thomasmore.be emails are allowed.",
                                "Invalid Email", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            string connectionString = "server=localhost;user=root;password=;database=SpaceFinderAppDB;";

            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                try
                {
                    conn.Open();

                    string getUserQuery = "SELECT user_id, fullname, email, created_at FROM Users WHERE email = @Email AND password = @Password";
                    MySqlCommand getUserCmd = new MySqlCommand(getUserQuery, conn);
                    getUserCmd.Parameters.AddWithValue("@Email", email);
                    getUserCmd.Parameters.AddWithValue("@Password", password);
                    
                    using (MySqlDataReader reader = getUserCmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            AppState.CurrentUser = new User
                            {
                                UserID = reader.GetInt32("user_id"),
                                FullName = reader.GetString("fullname"),
                                Email = reader.GetString("email")
                            };

                            MessageBox.Show("Login successful!");

                            App.Current.Properties["IsLoggedIn"] = true;

                            var bookingPage = new BookingPage();
                            this.NavigationService?.Navigate(bookingPage);
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
