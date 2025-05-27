using SpacefinderOff.Models;
using SpacefinderOff.Services;
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
            if (!EmailValidator.IsValidThomasMoreEmail(email))
            {
                MessageBox.Show("Only @student.thomasmore.be or @teacher.thomasmore.be emails are allowed.",
                                "Invalid Email", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (!UserService.IsEmailRegistered(email))
            {
                MessageBox.Show("This email is not registered. Please sign up first.", "Login Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (!UserService.ValidateUser(email, password))
            {
                MessageBox.Show("Incorrect email or password.", "Login Failed", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            var bookingPage = new BookingPage();
            this.NavigationService?.Navigate(bookingPage);


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
