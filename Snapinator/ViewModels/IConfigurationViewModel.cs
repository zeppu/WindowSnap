using System.Collections.Generic;

namespace Overlay.ViewModels
{
    public interface IConfigurationViewModel
    {
        IList<IConfigurationPartViewModel> Parts { get; }
    }
}