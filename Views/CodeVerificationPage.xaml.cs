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
        private readonly string correctCode;
        private readonly string userEmail;

        public CodeVerificationPage(string email, string code)
        {
            InitializeComponent();
            correctCode = code;
            userEmail = email;
        }

        private void VerifyButton_Click(object sender, RoutedEventArgs e)
        {
            string enteredCode = CodeTextBox.Text.Trim();

            if (enteredCode == correctCode)
            {
                MessageBox.Show("Code verified! Now you can reset your password.");
               
            }
            else
            {
                MessageBox.Show("Incorrect code. Please try again.");
            }
        }
    }

}
