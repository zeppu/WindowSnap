using System;

namespace Overlay.Core.SystemTray
{
    public interface ISystemTrayService : ISystemTrayNotificationService
    {
        void Show();
        void Hide();
        void SetIcon(Uri packUri);

        void AddMenuItem<T>(string caption, Uri iconUri)
            where T : class, new();

        void AddSeperator();
    }
}