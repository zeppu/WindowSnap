using System.Windows.Input;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;

namespace Snapinator.ViewModels.ConfigurationViewModels
{
    public interface IIntegrationConfigurationViewModel : IConfigurationPartViewModel
    {

    }

    public class IntegrationConfigurationViewModel : ReactiveObject, IIntegrationConfigurationViewModel
    {
        public string Title => "Integration";
        public void CommitChanges()
        {

        }

        public IReadOnlyReactiveList<KeyboardShortcutViewModel> KeyboardShortcuts { get; }

        [Reactive]
        public KeyboardShortcutViewModel SelectedKeyboardShortcut { get; set; }

        public IntegrationConfigurationViewModel()
        {
            KeyboardShortcuts = new ReactiveList<KeyboardShortcutViewModel>()
            {
                new KeyboardShortcutViewModel("Bring docked windows in front"),
                new KeyboardShortcutViewModel("Dock top most windows"),
                new KeyboardShortcutViewModel("Dock top most window to primary docking zone"),
                new KeyboardShortcutViewModel("Switch window to next docking zone"),
                new KeyboardShortcutViewModel("Switch window to previous docking zone"),
            };
        }
    }

    public class KeyboardShortcutViewModel : ReactiveObject
    {
        public KeyboardShortcutViewModel(string description)
        {
            Description = description;
        }

        public string Description { get; }

        [Reactive]
        public Key Key { get; set; }

        [ObservableAsProperty]
        public string KeyAsString { get; set; }

        [ObservableAsProperty]
        public string FullHotkeyCombo { get; set; }

        [Reactive]
        public Key Modifier1 { get; set; }

        [Reactive]
        public Key Modifier2 { get; set; }

        [Reactive]
        public Key Modifier3 { get; set; }
    }
}