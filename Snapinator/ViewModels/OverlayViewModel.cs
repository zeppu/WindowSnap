using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Ink;
using System.Windows.Media;
using Overlay.Core.Configuration;
using Overlay.Core.Configuration.Model;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;

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