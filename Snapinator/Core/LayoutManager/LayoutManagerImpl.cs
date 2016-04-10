using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Snapinator.Core.Configuration;
using Snapinator.Core.Configuration.Model;
using Snapinator.Messages;

namespace Snapinator.Core.LayoutManager
{
    class LayoutManagerImpl : ILayoutManager, IMessageHandler<SwitchLayoutMessage>
    {
        private readonly IConfigurationService _configurationService;
        private readonly Dictionary<Screen, ActiveLayout> _activeLayouts = new Dictionary<Screen, ActiveLayout>();

        public LayoutManagerImpl(IConfigurationService configurationService)
        {
            _configurationService = configurationService;
        }

        public void ModifyLayout(Layout layout, Screen targetScreen)
        {
            var activeLayoutInConf = _configurationService.GetActiveLayout();
            activeLayoutInConf.IsActive = false;
            layout.IsActive = true;

            _configurationService.SaveConfiguration();

            // determine shape of layout
            ActiveLayout activeLayout = null;
            var columnLayout = layout as ColumnLayout;
            if (columnLayout != null)
            {
                activeLayout = BuildColumnLayout(columnLayout.Columns, targetScreen);
                _activeLayouts[targetScreen] = activeLayout;
                return;
            }

            var rowLayout = layout as RowLayout;
            if (rowLayout != null)
            {
                activeLayout = BuildRowLayout(rowLayout.Rows, targetScreen);
                _activeLayouts[targetScreen] = activeLayout;
                return;
            }


            //{
            //    activeLayout = BuildGridLayout(layout, targetScreen);
            //}

        }

        public ActiveLayout GetActiveLayout(Screen targetScreen)
        {
            return _activeLayouts[targetScreen];
        }

        private ActiveLayout BuildGridLayout(IEnumerable<Area> layout, Screen targetScreen)
        {
            throw new NotImplementedException();
        }

        private ActiveLayout BuildRowLayout(IEnumerable<Row> layout, Screen targetScreen)
        {
            throw new NotImplementedException();
        }

        private ActiveLayout BuildColumnLayout(IEnumerable<Column> layout, Screen targetScreen)
        {
            var format = ActiveLayoutFormat.Columns;

            var offsetX = 0;

            var areaInfos = layout.Select(column =>
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

                return new ActiveArea(column.Name, rect, hotspotRect);
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

        public void HandleMessage(SwitchLayoutMessage message)
        {
            var layout = _configurationService.GetLayouts().FirstOrDefault(l => l.Name.Equals(message.TargetLayout));
            if (layout == null)
                return;

            ModifyLayout(layout, Screen.PrimaryScreen);
        }
    }
}