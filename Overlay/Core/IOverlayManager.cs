using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Input;
using Overlay.Core.Configuration;
using Overlay.Core.Configuration.Model;
using Overlay.ViewModels;
using Overlay.Views;
using Cursor = System.Windows.Forms.Cursor;

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
                var vm = ((IOverlayViewModel)w.DataContext);

                var column = area as Column;
                if (column != null)
                {
                    vm.Top = 50;
                    var screenWidth = ConvertMeasurementToScreenWidth(column.Width);
                    vm.Left = (int)(offsetX + (screenWidth / 2.0)) - 50;
                    offsetX += screenWidth;
                }

                _overlay.Add(w);

                w.Show();
            }

        }

        public void HideOverlay()
        {
            var mousePosition = Cursor.Position;

            _overlay.ForEach(w =>
            {

            });

            _overlay.ForEach(w =>
            {
                w.Hide();
                w.Close();
            });

            _overlay.Clear();
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