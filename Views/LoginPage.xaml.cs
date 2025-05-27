using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

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

            var bookingPage = new BookingPage();

            if (this.NavigationService != null)
            {
                this.NavigationService.Navigate(bookingPage);
            }
            else
            {
                MessageBox.Show("NavigationService not found. Please ensure your LoginPage is hosted in a Frame or NavigationWindow.");
            }


        }
        private void SignUpButton_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService?.Navigate(new SignUpPage());
        }

        private void ForgotPasswordButton_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new ForgotPasswordPage());

        }
    }
}
