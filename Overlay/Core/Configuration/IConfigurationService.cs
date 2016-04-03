﻿using System.Collections.Generic;
using Overlay.Core.Configuration.Model;

namespace Overlay.Core.Configuration
{
    public interface IConfigurationService
    {
        Layout GetActiveLayout();

        OverlayVisuals GetOverlayVisuals();

        void LoadConfiguration();
        IEnumerable<Layout> GetLayouts();

        void AddLayout(Layout newLayout);
        Layout GetLayoutByName(string originalName);
        void SaveConfiguration();
    }
}