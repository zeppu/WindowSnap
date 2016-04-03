using System.Collections.Generic;

namespace Snapinator.ViewModels
{
    public interface IConfigurationViewModel
    {
        IList<IConfigurationPartViewModel> Parts { get; }
    }
}