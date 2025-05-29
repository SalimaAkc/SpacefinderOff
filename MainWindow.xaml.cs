using SpacefinderOff.Services;
using SpacefinderOff.Views;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SpacefinderOff
{
    public partial class MainWindow : Window
    {
        private bool isLoggedIn = false;
        private string loggedInUserEmail = "";

        public MainWindow()
        {
            InitializeComponent();
        }
        private void HomeButton_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new Views.HomePage());

        }

        private void AuthButton_Click(object sender, RoutedEventArgs e)
        {
            if (!isLoggedIn)
            {
                
                var loginPage = new LoginPage(); 
                loginPage.LoginSuccessful += OnLoginSuccessful;
                MainFrame.Content = loginPage;
            }
            else
            {
                isLoggedIn = false;
                loggedInUserEmail = "";
                AuthButton.Content = "Login";

                App.Current.Properties["IsLoggedIn"] = false;
                AppState.CurrentUser = null;

                MessageBox.Show("Logged out successfully.");

                var loginPage = new LoginPage();
                loginPage.LoginSuccessful += OnLoginSuccessful;
                MainFrame.Content = loginPage;
            }
        }

        private void OnLoginSuccessful(string email)
        {
            isLoggedIn = true;
            loggedInUserEmail = email;
            AuthButton.Content = "Logout";
            MessageBox.Show($"Welcome, {email}!");
        }


    }
}