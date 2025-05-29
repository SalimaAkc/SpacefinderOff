using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace SpacefinderOff.Views
{
    public partial class HomePage
    {
        public HomePage()
        {
            InitializeComponent();

            if (App.Current.Properties.Contains("IsLoggedIn") &&
            (bool)App.Current.Properties["IsLoggedIn"] == true)
            {
                AuthenticatedButtonsPanel.Visibility = Visibility.Visible;
            }
        }

        private void BookingButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService?.Navigate(new BookingPage());
        }

        private void ProfileButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService?.Navigate(new ProfilePage());
        }
    }
}

  