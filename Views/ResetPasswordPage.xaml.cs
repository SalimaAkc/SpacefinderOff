using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using MySql.Data.MySqlClient;

namespace SpacefinderOff.Views
{
    public partial class ResetPasswordPage : Page
    {
        private string userEmail;
        private string correctCode;

        public ResetPasswordPage(string email, string verificationCode)
        {
            InitializeComponent();
            userEmail = email;
            correctCode = verificationCode;
        }

        private void ResetPasswordButton_Click(object sender, RoutedEventArgs e)
        {
            string newPassword = NewPasswordBox.Password.Trim();
            string confirmPassword = ConfirmPasswordBox.Password.Trim();

            if (string.IsNullOrEmpty(newPassword) || string.IsNullOrEmpty(confirmPassword))
            {
                MessageBox.Show("Please fill in both password fields.");
                return;
            }

            if (newPassword != confirmPassword)
            {
                MessageBox.Show("Passwords do not match.");
                return;
            }

            try
            {
                UpdatePasswordInDatabase(userEmail, newPassword);
                MessageBox.Show("Your password has been reset successfully!");
                this.NavigationService.Navigate(new LoginPage());
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to reset password: " + ex.Message);
            }
        }

        private void UpdatePasswordInDatabase(string email, string newPassword)
        {
            string connectionString = "server=localhost;user=root;password=;database=SpaceFinderAppDB;";
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();

                string query = "UPDATE users SET password = @password WHERE email = @mail";
                MySqlCommand command = new MySqlCommand(query, connection);
                command.Parameters.AddWithValue("@password", newPassword); 
                command.Parameters.AddWithValue("@mail", email);

                int rowsAffected = command.ExecuteNonQuery();
                if (rowsAffected == 0)
                    throw new Exception("Email not found or password not updated.");
            }
        }

        private void BackToLoginButton_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService?.Navigate(new LoginPage());
        }
    }
}
