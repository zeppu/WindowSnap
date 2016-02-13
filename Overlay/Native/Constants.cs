using System;

namespace Overlay.Native
{
    public class Constants
    {
        public static class HWND
        {
            public static IntPtr
                NoTopMost = new IntPtr(-2),
                TopMost = new IntPtr(-1),
                Top = new IntPtr(0),
                Bottom = new IntPtr(1);
        }
    }
}