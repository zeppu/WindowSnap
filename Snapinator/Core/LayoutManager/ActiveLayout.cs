using System.Collections.Generic;

namespace Snapinator.Core.LayoutManager
{
    public class ActiveLayout
    {
        public ActiveLayoutFormat Format { get; private set; }

        public IReadOnlyList<ActiveArea> Areas { get; private set; }

        public ActiveLayout(ActiveLayoutFormat format, IReadOnlyList<ActiveArea> areas)
        {
            Format = format;
            Areas = areas;
        }
    }
}