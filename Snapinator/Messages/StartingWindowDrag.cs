using System;

namespace Overlay.Messages
{
    public class StartingWindowDrag
    {
        public StartingWindowDrag(IntPtr targetWindowHandle)
        {
            TargetWindowHandle = targetWindowHandle;
        }

        public IntPtr TargetWindowHandle { get; }

    }
}