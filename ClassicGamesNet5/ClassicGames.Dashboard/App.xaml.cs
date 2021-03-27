using Autofac;
using ClassicGames.DAL;
using Serilog;
using System;
using System.Windows;

namespace ClassicGames.Dashboard
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private readonly IContainer _container;

        public App()
        {
            // Gerekli register işlemleri
            var builder = new ContainerBuilder();
            builder.RegisterType<GamesWindow>().AsSelf().SingleInstance(); // Tek bir GamesWindows örneği olacak şekilde kaydettirdik
            builder.RegisterModule(new ClassicGamesDBModule()); // DbContenxt ve Repository nesnelerini yükleyen modülü çalıştırdık

            _container = builder.Build();

            // Uygulama bazında bir hata olması durumunda onu yakalayıp Serilog üstünden logladığımız event metodu
            AppDomain.CurrentDomain.UnhandledException += (sender, args) =>
            {
                Exception exc = (Exception)args.ExceptionObject;
                Log.Error(exc.Message);
            };
        }
        /*
            App.xaml içindeki Startup niteliğine atanan fonksiyon.
            Uygulama ayağa kalkarken GamesWindow'u register edip yükler
            Varsayılan olarak App.xaml'e gelen Startupuri niteliği unutulur ve Startup niteliği ile değiştirilmezse bu register operasyonu gerçekleşmeyeceğinden,
            çalışma zamanında Null Reference hatası alınabilir.
        */
        private void Initialize(object sender, StartupEventArgs e)
        {
            var gamesWindow = _container.Resolve<GamesWindow>();
            gamesWindow.Show();
        }
    }
}
