using System.Collections.Generic;

namespace Overlay.Core.LayoutManager
{
    public class ActiveLayout
    {
        public ActiveLayoutFormat Format { get; private set; }

        public IReadOnlyList<AreaInfo> Areas { get; private set; }

        public ActiveLayout(ActiveLayoutFormat format, IReadOnlyList<AreaInfo> areas)
        {
            Format = format;
            Areas = areas;
        }
    }
}