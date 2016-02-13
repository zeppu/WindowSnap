using System;
using System.Windows;
using Overlay.Core.SystemTray;
using Overlay.Messages;
using Overlay.ViewModels;
using Overlay.Views;
using ReactiveUI;
using SimpleInjector;

namespace Overlay.Core
{
    public class Bootstrapper
    {
        //private static readonly Expression<Action<Container>> ContainerRegisterAction = (c) => c.Register<object, object>();

        private readonly Application _application;

        public Bootstrapper(Application application)
        {
            _application = application;
        }

        public void Start()
        {
            var c = new Container();


            c.RegisterSingleton<IServiceProvider>(c);

            c.Register<IMainViewModel, MainViewModel>();
            c.Register<MainWindow>();

            c.Register<IAboutViewModel, AboutViewModel>();
            c.Register<AboutWindow>();

            c.RegisterSingleton<ISystemTrayService, SystemTrayService>();
            c.Register<ISystemTrayNotificationService, SystemTrayNotificationService>();


            // setup message router
            MessageBus.Current.Listen<ShowAboutBoxMessage>().Subscribe(message =>
            {
                MessageBox.Show("Received message");
            });

            MessageBus.Current.Listen<TerminateAppMessage>().Subscribe(message =>
            {
                _application.Shutdown(0);
            });



            // setup system tray
            var systemTray = c.Get<ISystemTrayService>();
            systemTray.AddMenuItem<ShowAboutBoxMessage>("About", null);
            systemTray.AddSeperator();
            systemTray.AddMenuItem<TerminateAppMessage>("Exit", null);
            systemTray.SetIcon(new Uri("pack://application:,,,/Assets/App.ico"));
            systemTray.Show();
        }

        public void Dispose()
        {

        }
    }
}