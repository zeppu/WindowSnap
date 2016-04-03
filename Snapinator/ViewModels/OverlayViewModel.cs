using System.Windows.Media;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using Snapinator.Core.Configuration;

namespace Snapinator.ViewModels
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

        [Reactive]
        public int Left { get; set; }

        [Reactive]
        public int Top { get; set; }

        [Reactive]
        public string Name { get; set; }
    }

    public interface IOverlayViewModel
    {
        SolidColorBrush Stroke { get; set; }
        SolidColorBrush Fill { get; set; }
        int Left { get; set; }
        int Top { get; set; }
        string Name { get; set; }
    }
}