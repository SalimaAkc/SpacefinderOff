using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace SpacefinderOff.Views
{
    public partial class CodeVerificationPage : Page
    {
        private string userEmail;
        private string correctCode;

        public CodeVerificationPage(string email, string verificationCode)
        {
            InitializeComponent();
            userEmail = email;
            correctCode = verificationCode;
        }

        private void VerifyCode_Click(object sender, RoutedEventArgs e)
        {
            string enteredCode = CodeTextBox.Text.Trim();

            if (enteredCode == correctCode)
            {
                MessageBox.Show("Code verified successfully!");
                NavigationService.Navigate(new ResetPasswordPage(userEmail, correctCode)); 
            }
            else
            {
                MessageBox.Show("Incorrect code. Please try again.");
            }
        }
    }

}
