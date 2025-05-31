using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SpacefinderOff.Data;
using SpacefinderOff.Views;
using System.Configuration;
using System.Data;
using System.Windows;


namespace SpacefinderOff
{

    public partial class App : Application
    {

        public static void RestartToLogin()
        {
            Window loginWindow = new Window
            {
                Title = "Spacefinder",
                Content = new LoginPage(),
                WindowState = WindowState.Normal,
                WindowStartupLocation = WindowStartupLocation.CenterScreen,
                SizeToContent = SizeToContent.WidthAndHeight
            };
            loginWindow.Show();

            foreach (Window window in Current.Windows)
            {
                if (window != loginWindow) window.Close();
            }
        }
    }
}

        
