namespace Overlay.Core.SystemTray
{
    public class SystemTrayNotificationService : ISystemTrayNotificationService
    {
        private readonly ISystemTrayService _systemTray;

        public SystemTrayNotificationService(ISystemTrayService systemTray)
        {
            _systemTray = systemTray;
        }

        public void ShowInformation(string message, string caption)
        {
            _systemTray.ShowInformation(message, caption);
        }

        public void ShowWarning(string message, string caption)
        {
            _systemTray.ShowWarning(message, caption);
        }

        public void ShowError(string message, string caption)
        {
            _systemTray.ShowError(message, caption);
        }

        public bool AskConfirmation(string message, string caption)
        {
            return _systemTray.AskConfirmation(message, caption);
        }
    }
}