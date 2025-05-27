using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace SpacefinderOff.Views
{
    public partial class HomePage : Page
    {
        public HomePage()
        {
            InitializeComponent();

        }

        private void FindYourSpace_Click(object sender, RoutedEventArgs e)
        {
            NavigationService?.Navigate(new BookingPage());
        }


    }
}
