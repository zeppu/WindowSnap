using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Input;
using Anotar.NLog;
using Overlay.Core.Configuration;
using Overlay.Core.Configuration.Model;
using Overlay.Core.LayoutManager;
using Overlay.Messages;
using Overlay.Native;
using Overlay.ViewModels;
using Overlay.Views;
using Cursor = System.Windows.Forms.Cursor;
using Point = System.Drawing.Point;

namespace Overlay.Core
{
    public interface IOverlayManager
    {
        void ShowOverlay();

        void HideOverlay(IntPtr targetWindowHandle);
    }

    class OverlayManagerImpl : IOverlayManager, IMessageHandler<StartingWindowDrag>, IMessageHandler<EndingWindowDrag>
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILayoutManager _layoutManager;
        private readonly List<Window> _overlay = new List<Window>();

        public OverlayManagerImpl(IServiceProvider serviceProvider, ILayoutManager layoutManager)
        {
            _serviceProvider = serviceProvider;
            _layoutManager = layoutManager;
        }

        public void ShowOverlay()
        {
            if (_overlay.Count != 0)
            {

                // close current overlay window, we will recreate it eitherway
                _overlay.ForEach(w => w.Close());
                _overlay.Clear();
            }

            var layout = _layoutManager.GetActiveLayout(Screen.PrimaryScreen);

            // build overlays and render on screen
            foreach (var area in layout.Areas)
            {
                var w = _serviceProvider.Get<OverlayWindow>();
                var vm = ((IOverlayViewModel)w.DataContext);

                vm.Top = area.Hotspot.Top;
                vm.Left = area.Hotspot.Left;
                vm.Name = area.Name;

                _overlay.Add(w);

                w.Show();
            }
        }

        public void HideOverlay(IntPtr targetWindowHandle)
        {
            LogTo.Debug("Closing overlay windows");
            _overlay.ForEach(w =>
            {
                w.Hide();
                w.Close();
            });

            _overlay.Clear();
        }

        public void HandleMessage(StartingWindowDrag message)
        {
            ShowOverlay();
        }

        public void HandleMessage(EndingWindowDrag message)
        {
            HideOverlay(message.TargetWindowHandle);
        }
    }
}