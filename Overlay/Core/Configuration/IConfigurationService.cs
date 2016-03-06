using Overlay.Core.Configuration.Model;

namespace Overlay.Core.Configuration
{
    public interface IConfigurationService
    {
        Layout GetActiveLayout();

        OverlayVisuals GetOverlayVisuals();

        void LoadConfiguration();
    }
}