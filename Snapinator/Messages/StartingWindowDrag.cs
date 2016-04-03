using System;

namespace Snapinator.Messages
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