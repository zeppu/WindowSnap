using System.Drawing;
using System.Windows.Media;
using ReactiveUI;

namespace Overlay.ViewModels
{
    public class AreaViewModel : ReactiveObject
    {
        public string Name { get; set; }

        public double Width { get; set; }

        public AreaViewModel()
        {
            Width = 250;
        }
    }
}