using System;

namespace Overlay.Native
{
    [Flags]
    public enum ModifierKeys : int
    {
        Alt = 0x1,
        Control = 0x2,
        Shift = 0x4,
        Windows = 0x8,
        None = 0x0,
    }
}