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
            LoadUserInfo();
        }
        private void LoadUserInfo()
        {
            var user = AppState.CurrentUser;

            if (user != null)
            {
                FullNameLabel.Text = user.FullName;
                UserNameLabel.Text = user.UserName;
                EmailTextBox.Text = user.Email;
            }
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
                
                // Example: DeleteAccountFromDatabase();

                MessageBox.Show("Your account has been deleted.", "Account Deleted", MessageBoxButton.OK, MessageBoxImage.Information);
                NavigationService.Navigate(new LoginPage());
            }
            else
            {
                // User cancelled the deletion
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
