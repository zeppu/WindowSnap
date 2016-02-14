using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Ink;
using System.Windows.Media;
using Overlay.Core.Configuration;
using Overlay.Core.Configuration.Model;
using ReactiveUI;

namespace Overlay.ViewModels
{
    public class OverlayViewModel : ReactiveObject, IOverlayViewModel
    {
        public OverlayViewModel(IConfigurationService configurationService)
        {
            var overlayVisuals = configurationService.GetOverlayVisuals();

            Fill = overlayVisuals.Fill;
            Stroke = overlayVisuals.Border;
        }

        public SolidColorBrush Stroke { get; set; }

        public SolidColorBrush Fill { get; set; }
    }

    public interface IOverlayViewModel
    {

    }
}