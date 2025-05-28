using SpacefinderOff.Models;
using SpacefinderOff.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using MySql.Data.MySqlClient;

namespace SpacefinderOff.Views
{
    public partial class LoginPage
    {
        

        public LoginPage()
        {
            InitializeComponent();
        }

        private void SignInButton_Click(object sender, RoutedEventArgs e)
        {
            string email = EmailTextBox.Text.Trim();
            string password = PasswordBox.Password.Trim();

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

                    string checkEmailQuery = "SELECT * FROM Users WHERE email = @Email";
                    MySqlCommand checkEmailCmd = new MySqlCommand(checkEmailQuery, conn);
                    checkEmailCmd.Parameters.AddWithValue("@Email", email);

                    MySqlDataReader emailReader = checkEmailCmd.ExecuteReader();

                    if (!emailReader.HasRows)
                    {
                        MessageBox.Show("This email is not registered.", "Account Not Found", MessageBoxButton.OK, MessageBoxImage.Information);
                        return;
                    }

                    emailReader.Close();

                    string checkPasswordQuery = "SELECT * FROM Users WHERE email = @Email AND password = @Password";
                    MySqlCommand checkPasswordCmd = new MySqlCommand(checkPasswordQuery, conn);
                    checkPasswordCmd.Parameters.AddWithValue("@Email", email);
                    checkPasswordCmd.Parameters.AddWithValue("@Password", password);

                    MySqlDataReader passwordReader = checkPasswordCmd.ExecuteReader();

                    if (passwordReader.HasRows)
                    {
                        MessageBox.Show("Login successful!");

                        var bookingPage = new BookingPage();
                        this.NavigationService?.Navigate(bookingPage);
                    }
                    else
                    {
                        MessageBox.Show("Incorrect password.", "Login Failed", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Database error: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
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
