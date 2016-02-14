using System;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Interop;
using Anotar.NLog;
using Overlay.Core.Configuration;
using Overlay.Core.Hooks;
using Overlay.Core.Hotkeys;
using Overlay.Core.SystemTray;
using Overlay.Messages;
using Overlay.ViewModels;
using Overlay.Views;
using ReactiveUI;
using SimpleInjector;
using Application = System.Windows.Application;
using MessageBox = System.Windows.MessageBox;

namespace Overlay.Core
{
    public class Bootstrapper
    {
        //private static readonly Expression<Action<Container>> ContainerRegisterAction = (c) => c.Register<object, object>();

        private readonly Application _application;
        //private Form1 _overlayForm;

        public Bootstrapper(Application application)
        {
            _application = application;
        }

        public void Start()
        {
            LogTo.Debug("Bootstrapper::Start() >>");

            var c = Build();

            // setup message router
            LogTo.Info("Setting up message router...");
            MessageBus.Current.Listen<ShowAboutBoxMessage>().Subscribe(message =>
            {
                MessageBox.Show("Received message");
            });

            MessageBus.Current.Listen<TerminateAppMessage>().Subscribe(message =>
            {
                _application.Shutdown(0);
            });

            MessageBus.Current.Listen<HideOverlayMessage>()
                .Subscribe(message =>
                {
                    c.Get<IOverlayManager>().HideOverlay();
                });
            MessageBus.Current.Listen<ShowOverlayMessage>()
                .Subscribe(message =>
                {
                    c.Get<IOverlayManager>().ShowOverlay();
                });

            // setup hooks
            LogTo.Info("Registering win event hooks ...");
            c.Get<IWinEventHookManager>().Start();

            // setup system tray
            LogTo.Info("Setting up SystemTray icon ...");
            var systemTray = c.Get<ISystemTrayService>();
            systemTray.AddMenuItem<ShowAboutBoxMessage>("About", null);
            systemTray.AddSeperator();
            systemTray.AddMenuItem<TerminateAppMessage>("Exit", null);
            systemTray.SetIcon(new Uri("pack://application:,,,/Assets/App.ico"));
            systemTray.Show();

            // activate primary window but leave hidden     
            LogTo.Info("Activating primary window ...");

            _application.MainWindow = c.Get<MainWindow>();
            _application.MainWindow.Visibility = Visibility.Hidden;

            LogTo.Debug("Bootstrapper::Start() <<");
        }

        private static Container Build()
        {

            LogTo.Info("Setting up container ...");
            var c = new Container();

            c.RegisterSingleton<IServiceProvider>(c);

            c.Register<IMainViewModel, MainViewModel>();
            c.Register<MainWindow>();

            c.Register<IAboutViewModel, AboutViewModel>();
            c.Register<AboutWindow>();

            c.Register<IOverlayViewModel, OverlayViewModel>();
            c.Register<OverlayWindow>();


            c.RegisterSingleton<ISystemTrayService, SystemTrayService>();
            c.Register<ISystemTrayNotificationService, SystemTrayNotificationService>();

            c.RegisterSingleton<IWinEventHookManager, WinEventHookManager>();

            c.RegisterSingleton<IHotkeyManger, HotkeyManager>();
            c.RegisterSingleton<IConfigurationService, ConfigurationService>();

            c.RegisterSingleton<IOverlayManager, OverlayManager>();


            return c;
        }

        public void Dispose()
        {

        }
    }
}