using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Overlay.Core.Configuration.Model;

namespace Overlay.Core.LayoutManager
{
    class LayoutManagerImpl : ILayoutManager
    {
        private readonly Dictionary<Screen, ActiveLayout> _activeLayouts = new Dictionary<Screen, ActiveLayout>();

        public void ModifyLayout(Layout layout, Screen targetScreen)
        {
            // determine shape of layout
            ActiveLayout activeLayout = null;
            if (layout.Areas.All(area => area is Column))
            {
                activeLayout = BuildColumnLayout(layout, targetScreen);
            }
            else if (layout.Areas.All(area => area is Row))
            {
                activeLayout = BuildRowLayout(layout, targetScreen);
            }
            else
            {
                activeLayout = BuildGridLayout(layout, targetScreen);
            }

            _activeLayouts[targetScreen] = activeLayout;
        }

        public ActiveLayout GetActiveLayout(Screen targetScreen)
        {
            return _activeLayouts[targetScreen];
        }

        private ActiveLayout BuildGridLayout(Layout layout, Screen targetScreen)
        {
            throw new NotImplementedException();
        }

        private ActiveLayout BuildRowLayout(Layout layout, Screen targetScreen)
        {
            throw new NotImplementedException();
        }

        private ActiveLayout BuildColumnLayout(Layout layout, Screen targetScreen)
        {
            var format = ActiveLayoutFormat.Columns;

            var offsetX = 0;

            var areaInfos = layout.Areas.Cast<Column>().Select(column =>
            {

                // calculate area dims
                var actualColumnWidth = ConvertMeasurementToScreenWidth(column.Width);
                var rect = new Rectangle(offsetX, 0, actualColumnWidth, targetScreen.WorkingArea.Height);

                // calculate drop site dims and location
                var hotspotRect = new Rectangle(
                    (int)(rect.Left + (rect.Width / 2.0)) - 50, // hotspot X
                    rect.Top + 50,          // hotspot Y
                    100,                    // hotspot width
                    100                     // hotspot height
                    );

                offsetX += actualColumnWidth;

                return new AreaInfo(column.Name, rect, hotspotRect);
            }).ToList().AsReadOnly();

            return new ActiveLayout(format, areaInfos);
        }

        private int ConvertMeasurementToScreenWidth(Measurement m)
        {
            switch (m.Unit)
            {
                case MeasurementUnit.Percentage:
                    return (int)((m.Value / 100) * Screen.PrimaryScreen.Bounds.Width);
                    break;
                case MeasurementUnit.Pixels:
                    return (int)m.Value;
                    break;
            }

            throw new InvalidOperationException();
        }
    }
}