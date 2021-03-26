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
            var builder = new ContainerBuilder();
            builder.RegisterType<MainWindow>().AsSelf().SingleInstance();
            builder.RegisterModule(new ClassicGamesDBModule());

            _container = builder.Build();

            AppDomain.CurrentDomain.UnhandledException += (sender, args) =>
            {
                Exception exc = (Exception)args.ExceptionObject;
                Log.Error(exc.Message);
            };
        }
        private void OnStartup(object sender, StartupEventArgs e)
        {
            var mainWindow = _container.Resolve<MainWindow>();
            mainWindow.Show();
        }
    }
}
