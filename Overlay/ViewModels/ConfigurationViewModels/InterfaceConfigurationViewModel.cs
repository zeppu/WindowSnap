using ReactiveUI;
using ReactiveUI.Fody.Helpers;

namespace Overlay.ViewModels.ConfigurationViewModels
{
    public class InterfaceConfigurationViewModel : ReactiveObject, IConfigurationPartViewModel
    {
        public string Title { get; } = "Interface";

        [Reactive]
        public bool AutostartOnLogon { get; set; }

        [Reactive]
        public bool CheckForUpdates { get; set; }
    }
}