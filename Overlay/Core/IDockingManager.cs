using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Anotar.NLog;
using Overlay.Core.Configuration.Model;
using Overlay.Core.LayoutManager;
using Overlay.Messages;

namespace Overlay.Core
{
    public interface IDockingManager
    {
        bool IsWindowOverAreaHotspot(WindowInformation window, out ActiveArea targetArea);

        void DockWindow(WindowInformation window, ActiveArea targetArea);

        bool IsWindowDocked(WindowInformation window);

        void UndockWindow(WindowInformation window);
    }

    class DockingManagerImpl : IDockingManager, IMessageHandler<StartingWindowDrag>, IMessageHandler<EndingWindowDrag>
    {
        private readonly ILayoutManager _layoutManager;

        public DockingManagerImpl(ILayoutManager layoutManager)
        {
            _layoutManager = layoutManager;
        }

        public bool IsWindowOverAreaHotspot(WindowInformation window, out ActiveArea targetArea)
        {
            var mousePosition = Cursor.Position;
            LogTo.Debug($"Mouse Location: [{mousePosition.X}, {mousePosition.Y}]");

            var screen = GetActiveScreen(mousePosition);
            var layout = _layoutManager.GetActiveLayout(screen);
            targetArea = layout.Areas.FirstOrDefault(area => PointInRectangle(mousePosition, area.Hotspot));

            return targetArea != null;
        }

        private Screen GetActiveScreen(Point mousePosition)
        {
            // TODO determine on which screen the mouse is located
            return Screen.PrimaryScreen;
        }

        private Screen GetActiveScreen()
        {
            return GetActiveScreen(Cursor.Position);
        }

        public void DockWindow(WindowInformation window, ActiveArea targetArea)
        {
            LogTo.Trace();

            if (targetArea == null)
                return;

            LogTo.Info($"Attaching window [{window.Title}] to area [{targetArea.Name}] [{targetArea.Rect.X},{targetArea.Rect.Y},{targetArea.Rect.Width},{targetArea.Rect.Height}]");
            targetArea.AttachWindow(window);
        }

        public bool IsWindowDocked(WindowInformation window)
        {
            var screen = GetActiveScreen();
            var layout = _layoutManager.GetActiveLayout(screen);
            return layout.Areas.Any(area => area.IsWindowAttached(window));
        }

        public void UndockWindow(WindowInformation window)
        {
            LogTo.Trace();

            ActiveArea attachedArea;
            if (GetAreaForAttachedWindow(window, out attachedArea))
                return;

            LogTo.Info($"Detaching window [{window.Title}] from [{attachedArea.Name}]");
            attachedArea.DetachWindow(window);
        }

        private bool GetAreaForAttachedWindow(WindowInformation window, out ActiveArea attachedArea)
        {
            var screen = GetActiveScreen();
            var layout = _layoutManager.GetActiveLayout(screen);
            attachedArea = layout.Areas.FirstOrDefault(area => area.IsWindowAttached(window));

            if (attachedArea == null)
            {
                LogTo.Info("Window not attached to an area");
                return true;
            }
            return false;
        }

        private bool PointInRectangle(Point p, Rectangle r)
        {
            return (p.X <= r.Right && p.X >= r.Left) && (p.Y >= r.Top && p.Y <= r.Bottom);
        }

        public void HandleMessage(StartingWindowDrag message)
        {
            if (IsWindowInExclusionList(message.TargetWindowHandle))
                return;
            // does nothing for now since we are doing the undock operation when user stops dragging window
            // for some reason, calling setwindowpos() while the user is dragging the window simple causes the window to jitter 
            // and remains the same size
        }

        public void HandleMessage(EndingWindowDrag message)
        {
            ActiveArea targetArea;

            var window = (WindowInformation)message.TargetWindowHandle;

            if (IsWindowInExclusionList(window))
                return;

            if (IsWindowDocked(message.TargetWindowHandle))
            {
                LogTo.Info($"Undocking window [{message.TargetWindowHandle}]");
                UndockWindow(message.TargetWindowHandle);
            }
            else
            {
                LogTo.Info($"Window [{message.TargetWindowHandle}] not docked");
            }

            if (IsWindowOverAreaHotspot(message.TargetWindowHandle, out targetArea))
            {
                DockWindow(message.TargetWindowHandle, targetArea);
            }
        }

        private bool IsWindowInExclusionList(WindowInformation window)
        {
            return false;
        }
    }
}