using Microsoft.Win32;
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
using SpacefinderOff.Services;
using SpacefinderOff.Models;
using MySql.Data.MySqlClient;


namespace SpacefinderOff.Views
{
    public partial class ProfilePage : Page 
    {
        public ProfilePage()
        {
            InitializeComponent();
            LoadUserProfile();
        }
        private void LoadUserProfile()
        {
            string email = AppState.CurrentUser.Email;
            string connectionString = "server=localhost;user=root;password=;database=SpaceFinderAppDB;";

            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                try
                {
                    conn.Open();

                    string query = "SELECT fullName, email, created_at FROM Users WHERE email = @Email";
                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@Email", email);

                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            string fullName = reader.GetString("fullName");
                            string userEmail = reader.GetString("email");
                            DateTime createdAt = reader.GetDateTime("created_at");

                            FullNameLabel.Text = fullName;
                            EmailTextBox.Text = userEmail;
                            RegistrationDateTextBlock.Text = createdAt.ToString("dd MMM yyyy");
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Failed to load profile: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
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
            if (openDialog.ShowDialog()== true)
            {
                ProfileImage.Source = new BitmapImage(new Uri(openDialog.FileName));
            }


        }
        private void LogoutButton_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show(
                "Are you sure you want to log out?",
                "Confirm Logout",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                AppState.CurrentUser = null;
                NavigationService?.Navigate(new LoginPage());
            }
        }


        private void UpdateInfoButton_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Update Info clicked.");
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

                MessageBox.Show("Your account has been deleted.", "Account Deleted", MessageBoxButton.OK, MessageBoxImage.Information);
                NavigationService.Navigate(new LoginPage());
            }
            else
            {
                // User clicks on no
            }
        }
        private void ChangePasswordButton_Click(object sender, RoutedEventArgs e)
        {

        }
        private void BackToBooking_Click(object sender, RoutedEventArgs e)
        {
            NavigationService?.Navigate(new BookingPage());
        }

    }
}
