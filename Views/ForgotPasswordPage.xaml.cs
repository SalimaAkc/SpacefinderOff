using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Text.RegularExpressions;
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
            string verificationCode = GenerateCode(); 

            if (IsValidThomasMoreEmail(email))
            {
                try
                {
                    SendResetLink(email, verificationCode);
                    MessageBox.Show("Verification code sent. Please check your email.");
                   
                    NavigationService.Navigate(new ResetPasswordPage(email, verificationCode));
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error sending email: " + ex.Message);
                }
            }
            else
            {
                MessageBox.Show("Please enter a valid Thomas More student or teacher email.");
            }


        }
        private string GenerateCode()
        {
            Random rnd = new Random();
            return rnd.Next(1000, 9999).ToString();
        }
        public static bool IsValidThomasMoreEmail(string email)
        {
            if (email.EndsWith("@student.thomasmore.be"))
            {
                var username = email.Split('@')[0];
                return Regex.IsMatch(username, @"^r\d+$");
            }
            else if (email.EndsWith("@thomasmore.be"))
            {
                var username = email.Split('@')[0];
                return !Regex.IsMatch(username, @"^r\d+$");
            }
            return false;
        }

        public static void SendResetLink(string recipientEmail, string verificationCode)
        {
            var fromAddress = new MailAddress("SpacefinderOff_Staff@outlook.com", "SpacefinderOff");
            var toAddress = new MailAddress(recipientEmail);
            const string fromPassword = "spacefinder";
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
