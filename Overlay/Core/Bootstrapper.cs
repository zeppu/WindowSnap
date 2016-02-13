using System;
using System.Windows.Forms;
using Overlay.Core.Hooks;
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
        private Form1 _overlayForm;

        public Bootstrapper(Application application)
        {
            _application = application;
        }

        public void Start()
        {
            var c = Build();

            // setup message router
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
                    _overlayForm.Opacity = 0;
                });
            MessageBus.Current.Listen<ShowOverlayMessage>()
                .Subscribe(message =>
                {
                    _overlayForm.Opacity = 100;
                });

            // setup hooks
            _overlayForm = c.Get<Form1>();
            _overlayForm.Top = 0;
            _overlayForm.Left = 0;
            _overlayForm.Height = 250;
            _overlayForm.Width = Screen.PrimaryScreen.WorkingArea.Width;
            _overlayForm.Visible = true;
            c.Get<IWinEventHookManager>().Start();

            // setup system tray
            var systemTray = c.Get<ISystemTrayService>();
            systemTray.AddMenuItem<ShowAboutBoxMessage>("About", null);
            systemTray.AddSeperator();
            systemTray.AddMenuItem<TerminateAppMessage>("Exit", null);
            systemTray.SetIcon(new Uri("pack://application:,,,/Assets/App.ico"));
            systemTray.Show();
        }

        private static Container Build()
        {
            var c = new Container();

            c.RegisterSingleton<IServiceProvider>(c);

            c.Register<IMainViewModel, MainViewModel>();
            c.Register<MainWindow>();

            c.Register<IAboutViewModel, AboutViewModel>();
            c.Register<AboutWindow>();

            c.RegisterSingleton<ISystemTrayService, SystemTrayService>();
            c.Register<ISystemTrayNotificationService, SystemTrayNotificationService>();

            c.RegisterSingleton<Form1>();

            c.RegisterSingleton<IWinEventHookManager, WinEventHookManager>();
            return c;
        }

        public void Dispose()
        {

        }
    }
}