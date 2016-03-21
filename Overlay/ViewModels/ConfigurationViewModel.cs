using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Reactive.Linq;
using ReactiveUI;

namespace Overlay.ViewModels
{
    public class ConfigurationViewModel : ReactiveObject, IConfigurationViewModel
    {
        public IList<IConfigurationPartViewModel> Parts { get; }

        public ConfigurationViewModel(IEnumerable<IConfigurationPartViewModel> parts)
        {
            Parts = new ObservableCollection<IConfigurationPartViewModel>(parts);
        }
    }
}