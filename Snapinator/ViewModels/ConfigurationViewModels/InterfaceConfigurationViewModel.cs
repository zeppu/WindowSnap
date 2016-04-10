using ReactiveUI;
using ReactiveUI.Fody.Helpers;

namespace Snapinator.ViewModels.ConfigurationViewModels
{
    public class InterfaceConfigurationViewModel : ReactiveObject, IConfigurationPartViewModel
    {
        public string Title { get; } = "Interface";
        public void CommitChanges()
        {

        }

        [Reactive]
        public bool AutostartOnLogon { get; set; }

        [Reactive]
        public bool CheckForUpdates { get; set; }

        [Reactive]
        public bool RememberLastWindowDockingZone { get; set; }
    }
}