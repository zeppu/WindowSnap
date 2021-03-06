﻿using System;
using System.Windows;
using System.Windows.Forms;
using Anotar.NLog;
using ReactiveUI;
using SimpleInjector;
using Snapinator.Core.Configuration;
using Snapinator.Core.Hooks;
using Snapinator.Core.Hotkeys;
using Snapinator.Core.LayoutManager;
using Snapinator.Core.SystemTray;
using Snapinator.Messages;
using Snapinator.ViewModels;
using Snapinator.ViewModels.ConfigurationViewModels;
using Snapinator.Views;
using Application = System.Windows.Application;

namespace Snapinator.Core
{
    public class Bootstrapper
    {
        //private static readonly Expression<Action<Container>> ContainerRegisterAction = (c) => c.Register<object, object>();

        private readonly Application _application;
        private Container _container;
        //private Form1 _overlayForm;

        public Bootstrapper(Application application)
        {
            _application = application;
        }

        public void Start()
        {
            LogTo.Debug("Bootstrapper::Start() >>");

            var c = Build();
            _container = c;

            // setup message router
            LogTo.Info("Setting up message router...");
            MessageBus.Current.Listen<ShowAboutBoxMessage>().Subscribe(message =>
            {
                var view = c.GetInstance<AboutWindow>();
                view.ShowDialog();
            });

            MessageBus.Current.Listen<ShowConfigurationMessage>()
                .Subscribe(message =>
                {
                    var view = c.GetInstance<ConfigurationWindow>();
                    view.ShowDialog();
                });

            MessageBus.Current.Listen<TerminateAppMessage>().Subscribe(message =>
            {

                _application.Shutdown(0);
            });

            MessageBus.Current.Listen<EndingWindowDrag>()
                .Subscribe(message =>
                {
                    HandleMessage(c, message);
                });
            MessageBus.Current.Listen<StartingWindowDrag>()
                .Subscribe(message =>
                {
                    HandleMessage(c, message);
                });

            MessageBus.Current.Listen<SwitchLayoutMessage>()
                .Subscribe(message =>
                {
                    HandleMessage(c, message);
                });

            // setup hooks
            LogTo.Info("Registering win event hooks ...");
            c.Get<IWinEventHookManager>().Start();

            // setup system tray
            LogTo.Info("Setting up SystemTray icon ...");
            var systemTray = c.Get<ISystemTrayService>();
            systemTray.AddMenuItem<ShowConfigurationMessage>("Options", null);
            systemTray.AddSeperator();
            systemTray.AddMenuItem<ShowAboutBoxMessage>("About", null);
            systemTray.AddMenuItem<TerminateAppMessage>("Exit", null);
            systemTray.SetIcon(new Uri("pack://application:,,,/Assets/App.ico"));
            systemTray.Show();

            // activate primary window but leave hidden     
            LogTo.Info("Activating primary window ...");

            _application.MainWindow = c.Get<MainWindow>();
            _application.MainWindow.Visibility = Visibility.Hidden;

            // preliminary handling of active layout
            var configurationService = c.Get<IConfigurationService>();
            configurationService.LoadConfiguration();
            c.Get<ILayoutManager>().ModifyLayout(configurationService.GetActiveLayout(), Screen.PrimaryScreen);


            LogTo.Debug("Bootstrapper::Start() <<");
        }

        private static void HandleMessage<TMessage>(Container c, TMessage message)
            where TMessage : class
        {
            foreach (var handler in c.GetAllInstances<IMessageHandler<TMessage>>())
            {
                handler.HandleMessage(message);
            }
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

            c.Register<IConfigurationViewModel, ConfigurationViewModel>();
            c.Register<ConfigurationWindow>();

            c.RegisterCollection<IConfigurationPartViewModel>(new[]
            {
                Lifestyle.Transient.CreateRegistration<LayoutConfigurationViewModel>(c),
                Lifestyle.Transient.CreateRegistration<InterfaceConfigurationViewModel>(c),
                Lifestyle.Transient.CreateRegistration<IntegrationConfigurationViewModel>(c),
            });

            c.RegisterSingleton<ISystemTrayService, SystemTrayService>();
            c.Register<ISystemTrayNotificationService, SystemTrayNotificationService>();

            c.RegisterSingleton<IWinEventHookManager, WinEventHookManager>();

            c.RegisterSingleton<IHotkeyManger, HotkeyManager>();

            var configurationServiceActivator = Lifestyle.Singleton.CreateRegistration<ConfigurationService>(c);
            c.AddRegistration(typeof(IConfigurationService), configurationServiceActivator);
            c.AddRegistration(typeof(IInterfaceSettingsProvider), configurationServiceActivator);

            var layoutManagerActivator = Lifestyle.Singleton.CreateRegistration<LayoutManagerImpl>(c);
            c.AddRegistration(typeof(ILayoutManager), layoutManagerActivator);


            var commonMessageHandlers = new[]
            {
                Lifestyle.Singleton.CreateRegistration<OverlayManagerImpl>(c),
                Lifestyle.Singleton.CreateRegistration<DockingManagerImpl>(c),
            };
            c.RegisterCollection<IMessageHandler<StartingWindowDrag>>(commonMessageHandlers);
            c.RegisterCollection<IMessageHandler<EndingWindowDrag>>(commonMessageHandlers);
            c.RegisterCollection<IMessageHandler<SwitchLayoutMessage>>(new[]
            {
                layoutManagerActivator
            });

            return c;
        }

        public void Dispose()
        {
            _container.Get<IConfigurationService>().SaveConfiguration();
            _container.Dispose();
        }
    }
}