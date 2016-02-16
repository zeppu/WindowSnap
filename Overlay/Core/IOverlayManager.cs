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

    class OverlayManagerImpl : IOverlayManager
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
            var mousePosition = Cursor.Position;
            LogTo.Debug($"Mouse Location: [{mousePosition.X}, {mousePosition.Y}]");

            var layout = _layoutManager.GetActiveLayout(Screen.PrimaryScreen);
            var targetArea = layout.Areas.FirstOrDefault(area => PointInRectangle(mousePosition, area.Hotspot));

            LogTo.Info(targetArea != null
                ? $"Assigning window to area [{targetArea.Name}] [{targetArea.Rect.X},{targetArea.Rect.Y},{targetArea.Rect.Width},{targetArea.Rect.Height}]"
                : "No target area assigned");

            if (targetArea != null)
            {
                var rect = targetArea.Rect;
                User32.SetWindowPos(targetWindowHandle, IntPtr.Zero, rect.X, rect.Y, rect.Width + User32.Constants.WINDOW_PADDING_HEIGHT,
                    rect.Height + User32.Constants.WINDOW_PADDING_HEIGHT, SetWindowPosFlags.IgnoreZOrder);
            }

            LogTo.Debug("Closing overlay windows");
            _overlay.ForEach(w =>
            {
                w.Hide();
                w.Close();
            });

            _overlay.Clear();
        }

        private bool PointInRectangle(Point p, Rectangle r)
        {
            return (p.X <= r.Right && p.X >= r.Left) && (p.Y >= r.Top && p.Y <= r.Bottom);
        }

        private double ConvertMeasurementToScreenWidth(Measurement m)
        {
            switch (m.Unit)
            {
                case MeasurementUnit.Percentage:
                    return ((m.Value / 100) * Screen.PrimaryScreen.Bounds.Width);
                    break;
                case MeasurementUnit.Pixels:
                    return m.Value;
                    break;
            }

            throw new InvalidOperationException();
        }
    }
}