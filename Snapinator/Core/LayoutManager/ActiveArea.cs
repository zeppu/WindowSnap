using System.Collections.Generic;
using System.Drawing;

namespace Snapinator.Core.LayoutManager
{
    public class ActiveArea
    {
        private readonly List<WindowInformation> _windows;

        public ActiveArea(string name, Rectangle rect, Rectangle hotspot)
        {
            Name = name;
            Rect = rect;
            Hotspot = hotspot;

            _windows = new List<WindowInformation>();
        }

        public string Name { get; }

        public Rectangle Rect { get; }

        public Rectangle Hotspot { get; }

        public IReadOnlyList<WindowInformation> Windows => _windows;

        public void AttachWindow(WindowInformation window)
        {
            // add window to our assignee list
            _windows.Add(window);
        }

        public bool IsWindowAttached(WindowInformation window)
        {
            return _windows.Contains(window);
        }

        public void DetachWindow(WindowInformation window)
        {
            if (!_windows.Contains(window))
                return;

            _windows.Remove(window);
        }
    }
}