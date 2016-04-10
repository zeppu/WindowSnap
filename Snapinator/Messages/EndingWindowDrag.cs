using System;

namespace Snapinator.Messages
{
    public class EndingWindowDrag
    {
        public EndingWindowDrag(IntPtr targetWindowHandle, bool isWindowResize)
        {
            TargetWindowHandle = targetWindowHandle;
            IsWindowResize = isWindowResize;
        }

        public IntPtr TargetWindowHandle { get; }
        public bool IsWindowResize { get; set; }
    }
}