using System.Configuration;
using System.Data;
using System.Windows;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SpacefinderOff.Data;


namespace SpacefinderOff
{
    
    public partial class App : Application
    {
        public static IServiceProvider? ServiceProvider { get; private set; }

        protected override void OnStartup(StartupEventArgs e)
        {
            var services = new ServiceCollection();

            services.AddDbContext<SpacefinderContext>(options =>
                options.UseMySql( // Fixed method name
                    "server=localhost;database=SpacefinderAppDB;user=root;password=;",
                    new MySqlServerVersion(new Version(8, 0, 21))
                ));

            ServiceProvider = services.BuildServiceProvider();
            base.OnStartup(e);
        }
    }

}
