using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Text;

namespace Overlay.Native
{
    internal class User32
    {
        internal delegate void WinEventDelegate(IntPtr hWinEventHook, uint eventType,
            IntPtr hwnd, int idObject, int idChild, uint dwEventThread, uint dwmsEventTime);

        [DllImport("user32.dll")]
        internal static extern bool RegisterHotKey(IntPtr hWnd, int id, ModifierKeys fsModifiers, int vlc);

        [DllImport("user32.dll")]
        internal static extern bool UnregisterHotKey(IntPtr hWnd, int id);

        [DllImport("user32.dll")]
        internal static extern int GetWindowText(IntPtr hWnd, StringBuilder lpString, int nMaxCount);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool GetWindowRect(IntPtr hWnd, out RECT lpRect);

        [StructLayout(LayoutKind.Sequential)]
        // ReSharper disable once InconsistentNaming
        public struct RECT
        {
            public int Left;        // x position of upper-left corner
            public int Top;         // y position of upper-left corner
            public int Right;       // x position of lower-right corner
            public int Bottom;      // y position of lower-right corner

            public static implicit operator Rectangle(RECT r)
            {
                var size = new Size(r.Right - r.Left, r.Bottom - r.Top);
                var topLeft = new Point(r.Left, r.Top);
                return new Rectangle(topLeft, size);
            }

            public static implicit operator RECT(Rectangle r)
            {
                return new RECT()
                {
                    Top = r.Top,
                    Bottom = r.Bottom,
                    Left = r.Left,
                    Right = r.Right
                };
            }
        }


        [DllImport("user32.dll")]
        internal static extern IntPtr GetActiveWindow();

        [DllImport("user32.dll", SetLastError = true)]
        internal static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int X, int Y, int cx, int cy, SetWindowPosFlags uFlags);

        [DllImport("user32.dll")]
        internal static extern IntPtr GetForegroundWindow();

        [DllImport("user32.dll")]
        internal static extern IntPtr SetWinEventHook(uint eventMin, uint eventMax, IntPtr
            hmodWinEventProc, WinEventDelegate lpfnWinEventProc, uint idProcess,
            uint idThread, uint dwFlags);

        [DllImport("user32.dll")]
        internal static extern bool UnhookWinEvent(IntPtr hWinEventHook);

        public static class Constants
        {
            public const uint EVENT_SYSTEM_MOVESIZESTART = 0x000A;
            public const uint EVENT_SYSTEM_MOVESIZEEND = 0x000B;
            public const uint EVENT_SYSTEM_DESKTOPSWITCH = 0x0020;
            public const uint WINEVENT_OUTOFCONTEXT = 0;
            public const int WINDOW_PADDING_HEIGHT = 0;

        }
    }
}
