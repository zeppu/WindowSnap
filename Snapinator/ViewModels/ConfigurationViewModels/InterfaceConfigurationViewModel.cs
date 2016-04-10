using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using Snapinator.Core.Configuration;
using Snapinator.Core.Configuration.Model;

namespace Snapinator.ViewModels.ConfigurationViewModels
{
    public class InterfaceConfigurationViewModel : ReactiveObject, IConfigurationPartViewModel
    {
        private readonly IInterfaceSettingsProvider _interfaceSettingsProvider;
        public string Title { get; } = "Interface";
        public void CommitChanges()
        {
            _interfaceSettingsProvider.SetSetting<bool>(InterfaceSettings.CheckForUpdates, CheckForUpdates);
            _interfaceSettingsProvider.SetSetting<bool>(InterfaceSettings.RunAtStartup, AutostartOnLogon);
            _interfaceSettingsProvider.SetSetting<bool>(InterfaceSettings.RememberWindowDimensions, RememberWindowDimensions);
            _interfaceSettingsProvider.SetSetting<bool>(InterfaceSettings.RememberWindowDockZone, RememberWindowDockingZone);
        }


        public InterfaceConfigurationViewModel(IInterfaceSettingsProvider interfaceSettingsProvider)
        {
            _interfaceSettingsProvider = interfaceSettingsProvider;

            CheckForUpdates = _interfaceSettingsProvider.GetSetting<bool>(InterfaceSettings.CheckForUpdates);
            AutostartOnLogon = _interfaceSettingsProvider.GetSetting<bool>(InterfaceSettings.RunAtStartup);
            RememberWindowDimensions = _interfaceSettingsProvider.GetSetting<bool>(InterfaceSettings.RememberWindowDimensions);
            RememberWindowDockingZone = _interfaceSettingsProvider.GetSetting<bool>(InterfaceSettings.RememberWindowDockZone);

        }

        [Reactive]
        public bool AutostartOnLogon { get; set; }

        [Reactive]
        public bool CheckForUpdates { get; set; }

        [Reactive]
        public bool RememberWindowDockingZone { get; set; }

        [Reactive]
        public bool RememberWindowDimensions { get; set; }
    }
}