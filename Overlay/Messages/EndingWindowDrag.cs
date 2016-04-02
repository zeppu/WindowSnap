using System;

namespace Overlay.Messages
{
    public class EndingWindowDrag
    {
        public EndingWindowDrag(IntPtr targetWindowHandle)
        {
            TargetWindowHandle = targetWindowHandle;
        }

        public IntPtr TargetWindowHandle { get; }
    }
}