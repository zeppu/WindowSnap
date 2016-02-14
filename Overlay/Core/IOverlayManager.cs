using System;
using System.Windows;
using Overlay.Views;

namespace Overlay.Core
{
    public interface IOverlayManager
    {
        void ShowOverlay();

        void HideOverlay();
    }

    class OverlayManager : IOverlayManager
    {
        private readonly IServiceProvider _serviceProvider;
        private OverlayWindow _overlayWindow;

        public OverlayManager(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            _overlayWindow = null;
        }

        public void ShowOverlay()
        {
            if (_overlayWindow != null)
            {
                if (_overlayWindow.IsVisible)
                {
                    // possibly a double call to ShowOverlay()
                    return;
                }

                // close current overlay window, we will recreate it eitherway
                _overlayWindow.Close();
                _overlayWindow = null;
            }

            _overlayWindow = _serviceProvider.Get<OverlayWindow>();
            _overlayWindow.Visibility = Visibility.Visible;

        }

        public void HideOverlay()
        {
            if (_overlayWindow == null)
                return;

            _overlayWindow.Visibility = Visibility.Hidden;
            _overlayWindow.Close();
            _overlayWindow = null;
        }
    }
}