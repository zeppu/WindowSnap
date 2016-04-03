using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Anotar.NLog;
using Snapinator.Core.LayoutManager;
using Snapinator.Messages;
using Snapinator.Native;

namespace Snapinator.Core
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
        private readonly Dictionary<WindowInformation, ActiveArea> _windowAttachments = new Dictionary<WindowInformation, ActiveArea>();
        private readonly Dictionary<WindowInformation, Rectangle> _preDockWindowDimensions = new Dictionary<WindowInformation, Rectangle>();

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

            _windowAttachments.Add(window, targetArea);

            // store previous dimensions
            User32.RECT rect;
            if (User32.GetWindowRect(window, out rect))
            {
                _preDockWindowDimensions.Add(window, rect);
            }

            // resize window to match our area dimensions
            User32.SetWindowPos(window, IntPtr.Zero,
                targetArea.Rect.X, targetArea.Rect.Y,
                targetArea.Rect.Width + User32.Constants.WINDOW_PADDING_HEIGHT,
                targetArea.Rect.Height + User32.Constants.WINDOW_PADDING_HEIGHT,
                SetWindowPosFlags.IgnoreZOrder);
        }

        public bool IsWindowDocked(WindowInformation window)
        {
            return _windowAttachments.ContainsKey(window);
        }

        public void UndockWindow(WindowInformation window)
        {
            LogTo.Trace();

            ActiveArea attachedArea;
            if (!GetAreaForAttachedWindow(window, out attachedArea))
                return;

            _windowAttachments.Remove(window);

            LogTo.Info($"Detaching window [{window.Title}] from [{attachedArea.Name}]");
            attachedArea.DetachWindow(window);

            // restore original window dimensions
            if (!_preDockWindowDimensions.ContainsKey(window))
                return;

            var preDockRectangle = _preDockWindowDimensions[window];
            _preDockWindowDimensions.Remove(window);

            User32.SetWindowPos(window, IntPtr.Zero,
                0, 0,
                preDockRectangle.Width,
                preDockRectangle.Height,
                SetWindowPosFlags.IgnoreZOrder | SetWindowPosFlags.IgnoreMove | SetWindowPosFlags.DoNotCopyBits | SetWindowPosFlags.AsynchronousWindowPosition | SetWindowPosFlags.FrameChanged | SetWindowPosFlags.DoNotActivate);
        }

        private bool GetAreaForAttachedWindow(WindowInformation window, out ActiveArea attachedArea)
        {
            return _windowAttachments.TryGetValue(window, out attachedArea);
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