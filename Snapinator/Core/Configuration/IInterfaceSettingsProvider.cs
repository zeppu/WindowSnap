using Snapinator.Core.Configuration.Model;

namespace Snapinator.Core.Configuration
{
    public interface IInterfaceSettingsProvider
    {
        T GetSetting<T>(InterfaceSettings setting);
        void SetSetting<T>(InterfaceSettings setting, T value);
    }
}