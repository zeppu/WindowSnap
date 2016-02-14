using System.Collections.ObjectModel;
using System.Linq;
using Overlay.Core.Configuration;
using Overlay.Core.Configuration.Model;
using ReactiveUI;

namespace Overlay.ViewModels
{
    public class OverlayViewModel : ReactiveObject, IOverlayViewModel
    {
        public ObservableCollection<AreaViewModel> Areas { get; set; }

        public OverlayViewModel(IConfigurationService configurationService)
        {
            Areas = new ObservableCollection<AreaViewModel>(
                configurationService.GetActiveLayout().Areas.Cast<Column>().Select(area => new AreaViewModel()
                {
                    Name = area.Name,
                    Width = area.Width
                }));
        }
    }

    public interface IOverlayViewModel
    {
        ObservableCollection<AreaViewModel> Areas { get; }
    }
}