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
        private string verificationCode;

        public ForgotPasswordPage()
        {
            InitializeComponent();
        }
        private void SendLinkButton_Click(object sender, RoutedEventArgs e)
        {


            string email = EmailTextBox.Text.Trim();

            if (!email.EndsWith("@student.thomasmore.be") && !email.EndsWith("@thomasmore.be"))
            {
                MessageBox.Show("Only Thomas More emails are allowed.");
                return;
            }

            if (string.IsNullOrWhiteSpace(email))
            {
                MessageBox.Show("Please enter your school email.");
                return;
            }

            Random random = new Random();
            verificationCode = random.Next(1000, 9999).ToString();

            try
            {
                SendResetLink(email, verificationCode);

                CodeVerificationPage verificationPage = new CodeVerificationPage(email, verificationCode);
                this.NavigationService.Navigate(verificationPage);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to send email: " + ex.Message);
            }


        }

        public static void SendResetLink(string recipientEmail, string verificationCode)
        {
            var fromAddress = new MailAddress("spacefinder@office365.com", "Spacefinder");
            var toAddress = new MailAddress(recipientEmail);
            const string fromPassword = "your_email_password";
            const string subject = "Your Spacefinder verification code";
            string body = $"Your verification code is: {verificationCode}";

            var smtp = new SmtpClient
            {
                Host = "smtp.office365.com", 
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                Credentials = new NetworkCredential(fromAddress.Address, fromPassword),
                Timeout = 20000
            };
            using (var message = new MailMessage(fromAddress, toAddress)
            {
                Subject = subject,
                Body = body
            })
            {
                smtp.Send(message);
            }
        }

        private void BackToLoginButton_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService?.Navigate(new LoginPage());
        }
    }
}
