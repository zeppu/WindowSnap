using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Forms;
using Overlay.Core.Configuration;
using Overlay.Core.Configuration.Model;
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
        private readonly IConfigurationService _configurationService;
        private readonly List<Window> _overlay = new List<Window>();

        public OverlayManager(IServiceProvider serviceProvider, IConfigurationService configurationService)
        {
            _serviceProvider = serviceProvider;
            _configurationService = configurationService;
        }

        public void ShowOverlay()
        {
            if (_overlay.Count != 0)
            {

                // close current overlay window, we will recreate it eitherway
                _overlay.ForEach(w => w.Close());
                _overlay.Clear();
            }


            var offsetX = 0.0;
            var layout = _configurationService.GetActiveLayout();
            foreach (var area in layout.Areas)
            {
                var w = _serviceProvider.Get<OverlayWindow>();
                _overlay.Add(w);

                w.Top = 50;
                var column = area as Column;
                if (column != null)
                {
                    var screenWidth = ConverMeasurementToScreenWidth(column.Width);
                    w.Left = (offsetX + (screenWidth / 2.0)) - 50;
                    offsetX += screenWidth;
                }

                w.Show();
            }

        }

        private double ConverMeasurementToScreenWidth(Measurement m)
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

        public void HideOverlay()
        {
            _overlay.ForEach(w =>
            {
                w.Hide();
                w.Close();
            });

            _overlay.Clear();
        }
    }
}