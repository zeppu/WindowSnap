using ReactiveUI;
using ReactiveUI.Fody.Helpers;

namespace Overlay.ViewModels
{
    public class InterfaceConfigurationViewModel : ReactiveObject, IConfigurationPartViewModel
    {
        public string Title { get; } = "Interface";

        [Reactive]
        public bool Autostart { get; set; }

        [Reactive]
        public bool CheckForUpdates { get; set; }
    }
}