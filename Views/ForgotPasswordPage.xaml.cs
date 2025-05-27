using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace SpacefinderOff.Views
{
    public partial class ForgotPasswordPage
    {
        public ForgotPasswordPage()
        {
            InitializeComponent();
        }
        private void SendLinkButton_Click(object sender, RoutedEventArgs e)
        {
            string userEmail = EmailTextBox.Text;

            if (string.IsNullOrWhiteSpace(userEmail))
            {
                MessageBox.Show("Please enter your school email address.", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                SendResetLink(userEmail);
                MessageBox.Show("Password reset link has been sent to your email.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to send email: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public static void SendResetLink(string recipientEmail)
        {
            string fromEmail = "r1059518@student.thomasmore.be";
            string fromPassword = "My-password";  
            string subject = "Password Reset Link";
            string body = "Click here to reset your password: https: https://github.com/SalimaAkc/SpacefinderOff.git.com/reset-password?token=UNIQUE_TOKEN_HERE";

            SmtpClient client = new("smtp.gmail.com", 587)
            {
                Credentials = new NetworkCredential(fromEmail, fromPassword),
                EnableSsl = true
            };

            MailMessage message = new (fromEmail, recipientEmail, subject, body);
            client.Send(message);
        }

        private void BackToLoginButton_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService?.Navigate(new LoginPage());
        }
    }
}
