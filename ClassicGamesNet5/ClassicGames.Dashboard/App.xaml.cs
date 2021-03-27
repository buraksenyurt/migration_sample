using ClassicGames.DAL;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using System;
using System.IO;
using System.Windows;

namespace ClassicGames.Dashboard
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public IServiceProvider ServiceProvider { get; private set; }
        public IConfiguration Configuration { get; private set; }

        private void ConfigureServices(IServiceCollection services)
        {
            services.AddTransient<GamesWindow>();
            // DAL içerisinde IServiceCollection'a LoadDb isimli yeni bir extension metot dahil etmiştik.
            // Böylece Context nesnesini burada çalışma zamanına entegre edebiliriz.
            // Parametre olarak connection string bilgisini de veriyoruz. Bu bilgiyi appsettings içerisindeki CommodoreDb isimli section'dan almakta.
            services.LoadDb(Configuration.GetConnectionString("CommodoreDB"));
        }
        public App()
        {
            AppDomain.CurrentDomain.UnhandledException += (sender, args) =>
            {
                Exception exc = (Exception)args.ExceptionObject;
                Log.Error(exc.Message);
            };
        }

        private void Initialize(object sender, StartupEventArgs e)
        {
            // Configuration nesnesini builder üzerinden alırken appsettings.json dosyasını kaynak olarak kullanacağımızı belirtiyoruz
            var builder = new ConfigurationBuilder()
             .SetBasePath(Directory.GetCurrentDirectory())
             .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

            Configuration = builder.Build();

            var serviceCollection = new ServiceCollection();
            ConfigureServices(serviceCollection);

            ServiceProvider = serviceCollection.BuildServiceProvider();

            var dbOptions = ServiceProvider.GetService<DbContextOptions<CommodoreDBContext>>();
            using (var context = new CommodoreDBContext(dbOptions))
            {
                context.Database.EnsureCreated(); // Veritabanını yoksa oluşturulduğundan emin olunmasını da sağlıyoruz
            }

            var mainWindow = ServiceProvider.GetRequiredService<GamesWindow>();
            mainWindow.Show();
        }
    }
}
