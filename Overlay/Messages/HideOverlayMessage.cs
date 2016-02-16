using System;

namespace Overlay.Messages
{
    public class HideOverlayMessage
    {
        public HideOverlayMessage(IntPtr targetWindowHandle)
        {
            TargetWindowHandle = targetWindowHandle;
        }

        public IntPtr TargetWindowHandle { get; }
    }
}