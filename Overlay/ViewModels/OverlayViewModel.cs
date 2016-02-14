using System.Collections.ObjectModel;
using System.Linq;
using Overlay.Core.Configuration;
using Overlay.Core.Configuration.Model;
using ReactiveUI;

namespace Overlay.ViewModels
{
    public class OverlayViewModel : ReactiveObject, IOverlayViewModel
    {


        public OverlayViewModel()
        {

        }
    }

    public interface IOverlayViewModel
    {

    }
}