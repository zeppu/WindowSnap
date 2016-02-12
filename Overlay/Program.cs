using System;
using System.Collections.Generic;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace Overlay
{
    static class Program
    {
        public static Form1 form;

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            form = new Form1()
            {
                Top = 0,
                Left = 0,
                Height = 250,
                Width = Screen.PrimaryScreen.WorkingArea.Width
            };
            // Listen for name change changes across all processes/threads on current desktop...
            var hhookend = SetWinEventHook(EVENT_SYSTEM_MOVESIZEEND, EVENT_SYSTEM_MOVESIZEEND, IntPtr.Zero,
                endDelegate, 0, 0, WINEVENT_OUTOFCONTEXT);
            var hhookstart = SetWinEventHook(EVENT_SYSTEM_MOVESIZESTART, EVENT_SYSTEM_MOVESIZESTART, IntPtr.Zero,
                startDelegate, 0, 0, WINEVENT_OUTOFCONTEXT);


            // MessageBox provides the necessary mesage loop that SetWinEventHook requires.
            // In real-world code, use a regular message loop (GetMessage/TranslateMessage/
            // DispatchMessage etc or equivalent.)
            //            MessageBox.Show("Tracking name changes on HWNDs, close message box to exit.");
            Application.Run(form);
            UnhookWinEvent(hhookstart);
            UnhookWinEvent(hhookend);
        }

        private delegate void WinEventDelegate(IntPtr hWinEventHook, uint eventType,
            IntPtr hwnd, int idObject, int idChild, uint dwEventThread, uint dwmsEventTime);

        [DllImport("user32.dll")]
        private static extern IntPtr SetWinEventHook(uint eventMin, uint eventMax, IntPtr
            hmodWinEventProc, WinEventDelegate lpfnWinEventProc, uint idProcess,
            uint idThread, uint dwFlags);

        [DllImport("user32.dll")]
        private static extern bool UnhookWinEvent(IntPtr hWinEventHook);

        [DllImport("user32.dll", SetLastError = true)]
        private static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int X, int Y, int cx, int cy,
            SetWindowPosFlags uFlags);

        private const uint EVENT_SYSTEM_MOVESIZESTART = 0x000A;
        private const uint EVENT_SYSTEM_MOVESIZEEND = 0x000B;
        private const uint WINEVENT_OUTOFCONTEXT = 0;

        // Need to ensure delegate is not collected while we're using it,
        // storing it in a class field is simplest way to do this.
        private static readonly WinEventDelegate endDelegate = WinEventProc;
        private static readonly WinEventDelegate startDelegate = WinEventProc2;

        private static readonly WindowOptions options = new WindowOptions();


        public const int WINDOW_PADDING_HEIGHT = 0;

        private static void WinEventProc2(IntPtr hWinEventHook, uint eventType,
            IntPtr hwnd, int idObject, int idChild, uint dwEventThread, uint dwmsEventTime)
        {
            form.Opacity = 100;
        }

        private static void WinEventProc(IntPtr hWinEventHook, uint eventType,
            IntPtr hwnd, int idObject, int idChild, uint dwEventThread, uint dwmsEventTime)
        {
            var position = Cursor.Position;
            var left = form.Left;
            form.Opacity = 0;
            Rectangle option = Rectangle.Empty;

            if (Between(position.X, left + form.QuarterLeft.Left, left + form.QuarterLeft.Left + form.QuarterLeft.Width))
            {
                option = options.Options["QuarterLeft"];
            }
            if (Between(position.X, left + form.HalfCenter.Left, left + form.HalfCenter.Left + form.HalfCenter.Width))
            {
                option = options.Options["Centered"];
            }
            if (Between(position.X, left + form.QuarterRight.Left, left + form.QuarterRight.Left + form.QuarterRight.Width))
            {
                option = options.Options["QuarterRight"];
            }
            if (Between(position.X, left + form.ThirdLeft.Left, left + form.ThirdLeft.Left + form.ThirdLeft.Width))
            {
                option = options.Options["1/3Left"];
            }
            if (Between(position.X, left + form.ThirdRight.Left, left + form.ThirdRight.Left + form.ThirdRight.Width))
            {
                option = options.Options["1/3Right"];
            }
            if (Between(position.X, left + form.TwoThirdLeft.Left, left + form.TwoThirdLeft.Left + form.TwoThirdLeft.Width))
            {
                option = options.Options["2/3Left"];
            }
            if (Between(position.X, left + form.TwoThirdRight.Left, left + form.TwoThirdRight.Left + form.TwoThirdRight.Width))
            {
                option = options.Options["2/3Right"];
            }

            if (option != Rectangle.Empty)
                SetWindowPos(hwnd, IntPtr.Zero, option.X, option.Y, option.Width + WINDOW_PADDING_HEIGHT, option.Height + WINDOW_PADDING_HEIGHT, SetWindowPosFlags.IgnoreZOrder);

        }

        public class WindowOptions
        {
            public Rectangle ScreenSize { get; }

            public IDictionary<string, Rectangle> Options { get; }

            public WindowOptions()
            {
                ScreenSize = Screen.PrimaryScreen.WorkingArea;
                Options = new Dictionary<string, Rectangle>()
                {
                    { "Centered", new Rectangle(ScreenSize.Width/4,0, ScreenSize.Width/2, ScreenSize.Height)},
                    { "QuarterLeft", new Rectangle(0, 0, ScreenSize.Width/4, ScreenSize.Height) },
                    { "QuarterRight", new Rectangle(ScreenSize.Width*3/4, 0, ScreenSize.Width/4, ScreenSize.Height) },
                    { "2/3Left", new Rectangle(0, 0, ScreenSize.Width*2/3, ScreenSize.Height) },
                    { "2/3Right", new Rectangle(ScreenSize.Width*1/3, 0, ScreenSize.Width*2/3, ScreenSize.Height) },
                    { "1/3Left", new Rectangle(0, 0, ScreenSize.Width*1/3, ScreenSize.Height) },
                    { "1/3Right", new Rectangle(ScreenSize.Width*2/3, 0, ScreenSize.Width*1/3, ScreenSize.Height) },
                };
            }

            public int Center(int parentWidth, int childWidth)
            {
                return (parentWidth / 2) - (childWidth / 2);
            }
        }

        public enum WindowSizes
        {
            FullWidth,
            HalfWidth,
            ThirdWith,

        }

        private static bool Between(int val, int min, int max)
        {
            return (val > min && val < max);
        }

        private static readonly IntPtr HWND_TOPMOST = new IntPtr(-1);
        private static readonly IntPtr HWND_NOTOPMOST = new IntPtr(-2);
        private static readonly IntPtr HWND_TOP = new IntPtr(0);
        private static readonly IntPtr HWND_BOTTOM = new IntPtr(1);

        /// <summary>
        /// Window handles (HWND) used for hWndInsertAfter
        /// </summary>
        public static class HWND
        {
            public static IntPtr
                NoTopMost = new IntPtr(-2),
                TopMost = new IntPtr(-1),
                Top = new IntPtr(0),
                Bottom = new IntPtr(1);
        }

        /// <summary>
        /// SetWindowPos Flags
        /// </summary>
        public static class SWP
        {
            public static readonly int
                NOSIZE = 0x0001,
                NOMOVE = 0x0002,
                NOZORDER = 0x0004,
                NOREDRAW = 0x0008,
                NOACTIVATE = 0x0010,
                DRAWFRAME = 0x0020,
                FRAMECHANGED = 0x0020,
                SHOWWINDOW = 0x0040,
                HIDEWINDOW = 0x0080,
                NOCOPYBITS = 0x0100,
                NOOWNERZORDER = 0x0200,
                NOREPOSITION = 0x0200,
                NOSENDCHANGING = 0x0400,
                DEFERERASE = 0x2000,
                ASYNCWINDOWPOS = 0x4000;
        }

        [Flags()]
        private enum SetWindowPosFlags : uint
        {
            /// <summary>If the calling thread and the thread that owns the window are attached to different input queues, 
            /// the system posts the request to the thread that owns the window. This prevents the calling thread from 
            /// blocking its execution while other threads process the request.</summary>
            /// <remarks>SWP_ASYNCWINDOWPOS</remarks>
            AsynchronousWindowPosition = 0x4000,

            /// <summary>Prevents generation of the WM_SYNCPAINT message.</summary>
            /// <remarks>SWP_DEFERERASE</remarks>
            DeferErase = 0x2000,

            /// <summary>Draws a frame (defined in the window's class description) around the window.</summary>
            /// <remarks>SWP_DRAWFRAME</remarks>
            DrawFrame = 0x0020,

            /// <summary>Applies new frame styles set using the SetWindowLong function. Sends a WM_NCCALCSIZE message to 
            /// the window, even if the window's size is not being changed. If this flag is not specified, WM_NCCALCSIZE 
            /// is sent only when the window's size is being changed.</summary>
            /// <remarks>SWP_FRAMECHANGED</remarks>
            FrameChanged = 0x0020,

            /// <summary>Hides the window.</summary>
            /// <remarks>SWP_HIDEWINDOW</remarks>
            HideWindow = 0x0080,

            /// <summary>Does not activate the window. If this flag is not set, the window is activated and moved to the 
            /// top of either the topmost or non-topmost group (depending on the setting of the hWndInsertAfter 
            /// parameter).</summary>
            /// <remarks>SWP_NOACTIVATE</remarks>
            DoNotActivate = 0x0010,

            /// <summary>Discards the entire contents of the client area. If this flag is not specified, the valid 
            /// contents of the client area are saved and copied back into the client area after the window is sized or 
            /// repositioned.</summary>
            /// <remarks>SWP_NOCOPYBITS</remarks>
            DoNotCopyBits = 0x0100,

            /// <summary>Retains the current position (ignores X and Y parameters).</summary>
            /// <remarks>SWP_NOMOVE</remarks>
            IgnoreMove = 0x0002,

            /// <summary>Does not change the owner window's position in the Z order.</summary>
            /// <remarks>SWP_NOOWNERZORDER</remarks>
            DoNotChangeOwnerZOrder = 0x0200,

            /// <summary>Does not redraw changes. If this flag is set, no repainting of any kind occurs. This applies to 
            /// the client area, the nonclient area (including the title bar and scroll bars), and any part of the parent 
            /// window uncovered as a result of the window being moved. When this flag is set, the application must 
            /// explicitly invalidate or redraw any parts of the window and parent window that need redrawing.</summary>
            /// <remarks>SWP_NOREDRAW</remarks>
            DoNotRedraw = 0x0008,

            /// <summary>Same as the SWP_NOOWNERZORDER flag.</summary>
            /// <remarks>SWP_NOREPOSITION</remarks>
            DoNotReposition = 0x0200,

            /// <summary>Prevents the window from receiving the WM_WINDOWPOSCHANGING message.</summary>
            /// <remarks>SWP_NOSENDCHANGING</remarks>
            DoNotSendChangingEvent = 0x0400,

            /// <summary>Retains the current size (ignores the cx and cy parameters).</summary>
            /// <remarks>SWP_NOSIZE</remarks>
            IgnoreResize = 0x0001,

            /// <summary>Retains the current Z order (ignores the hWndInsertAfter parameter).</summary>
            /// <remarks>SWP_NOZORDER</remarks>
            IgnoreZOrder = 0x0004,

            /// <summary>Displays the window.</summary>
            /// <remarks>SWP_SHOWWINDOW</remarks>
            ShowWindow = 0x0040,
        }

    }
}
