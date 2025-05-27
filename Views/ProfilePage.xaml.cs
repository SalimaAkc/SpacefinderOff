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

namespace SpacefinderOff.Views
{
    public partial class ProfilePage : Page 
    {
        //ResourceManager rm = new ("SpacefinderOff.Resources", typeof(ProfilePage).Assembly);
        public ProfilePage()
        {
            InitializeComponent();
            //SetLanguageTexts();
        }
        /*
        public void SetLanguageTexts()
        {
            FullNameLabel.Text = rm.GetString("FullNameLabel");
            UsernameLabel.Text = rm.GetString("UsernameLabel");
            EmailTextBox.PlaceholderText = rm.GetString("EmailTextBox");
            UploadPhotoButton.Content = rm.GetString("UploadPhotoButton");
            DeleteAccountButton.Content = rm.GetString("DeleteAccountButton");
            UpdateInfoButton.Content = rm.GetString("UpdateInfoButton");
            ChangePasswordButton.Content = rm.GetString("ChangePasswordButton");
            CurrentPasswordBox.PlaceholderText = rm.GetString("CurrentPasswordBox");
            NewPasswordBox.PlaceholderText = rm.GetString("NewPasswordBox");
            ConfirmNewPasswordBox.PlaceholderText = rm.GetString("ConfirmNewPasswordBox");
            LanguageCombobox.Header = rm.GetString("LanguageCombobox");
            StudyProgramTextBox.PlaceholderText = rm.GetString("StudyProgramTextBox");
            RecentBookings.Text = rm.GetString("RecentBookings");
            AccountStatus.Text = rm.GetString("AccountStatus");
        }


        private void LanguageComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (LanguageCombobox.SelectedItem is ComboBoxItem selectedItem)
            {
                string? selectedLanguage = selectedItem.Content.ToString();

                string cultureCode = selectedLanguage switch
                {
                    "English" => "en",
                    "Dutch" => "nl",
                    "French" => "fr",
                    "German" => "de",
                    _ => "en"
                };

                Thread.CurrentThread.CurrentUICulture = new CultureInfo(cultureCode);
                Thread.CurrentThread.CurrentCulture = new CultureInfo(cultureCode);

                SetLanguageTexts(); // Reload all UI strings
            }
        }*/


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

    }
}
