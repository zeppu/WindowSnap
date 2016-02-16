using System.Drawing;

namespace Overlay.Core.LayoutManager
{
    public class AreaInfo
    {
        public AreaInfo(string name, Rectangle rect, Rectangle hotspot)
        {
            Name = name;
            Rect = rect;
            Hotspot = hotspot;
        }

        public string Name { get; private set; }

        public Rectangle Rect { get; private set; }

        public Rectangle Hotspot { get; private set; }
    }
}