using ReactiveUI;

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

        public IntegrationConfigurationViewModel()
        {
            KeyboardShortcuts = new ReactiveList<KeyboardShortcutViewModel>()
            {
                new KeyboardShortcutViewModel("Bring docked windows in front"),
                new KeyboardShortcutViewModel("Auto dock top windows")
            };
        }
    }

    public class KeyboardShortcutViewModel
    {
        public KeyboardShortcutViewModel(string description)
        {
            Description = description;
        }

        public string Description { get; }
    }
}