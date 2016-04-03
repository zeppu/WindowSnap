using System;

namespace Snapinator.Messages
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