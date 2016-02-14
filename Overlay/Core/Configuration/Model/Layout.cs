using System;
using System.Collections.Generic;
using System.Windows.Documents;

namespace Overlay.Core.Configuration.Model
{
    [Serializable]
    public class Layout
    {
        public List<Area> Areas { get; private set; }

        public Layout()
        {
            Areas = new List<Area>();
        }
    }
}