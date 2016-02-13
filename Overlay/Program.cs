using System;
using System.Collections.Generic;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using Overlay.Native;

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
            var app = new App();


            form = new Form1()
            {
                Top = 0,
                Left = 0,
                Height = 250,
                Width = Screen.PrimaryScreen.WorkingArea.Width
            };
            // Listen for name change changes across all processes/threads on current desktop...
            var hhookend = User32.SetWinEventHook(EVENT_SYSTEM_MOVESIZEEND, EVENT_SYSTEM_MOVESIZEEND, IntPtr.Zero,
                endDelegate, 0, 0, WINEVENT_OUTOFCONTEXT);
            var hhookstart = User32.SetWinEventHook(EVENT_SYSTEM_MOVESIZESTART, EVENT_SYSTEM_MOVESIZESTART, IntPtr.Zero,
                startDelegate, 0, 0, WINEVENT_OUTOFCONTEXT);


            // MessageBox provides the necessary mesage loop that SetWinEventHook requires.
            // In real-world code, use a regular message loop (GetMessage/TranslateMessage/
            // DispatchMessage etc or equivalent.)
            //            MessageBox.Show("Tracking name changes on HWNDs, close message box to exit.");
            form.Visible = true;
            app.Run();

            User32.UnhookWinEvent(hhookstart);
            User32.UnhookWinEvent(hhookend);
        }





        private const uint EVENT_SYSTEM_MOVESIZESTART = 0x000A;
        private const uint EVENT_SYSTEM_MOVESIZEEND = 0x000B;
        private const uint WINEVENT_OUTOFCONTEXT = 0;

        // Need to ensure delegate is not collected while we're using it,
        // storing it in a class field is simplest way to do this.
        private static readonly User32.WinEventDelegate endDelegate = WinEventProc;
        private static readonly User32.WinEventDelegate startDelegate = WinEventProc2;

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
            if (Between(position.X, left + form.QuarterRight.Left,
                left + form.QuarterRight.Left + form.QuarterRight.Width))
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
            if (Between(position.X, left + form.TwoThirdLeft.Left,
                left + form.TwoThirdLeft.Left + form.TwoThirdLeft.Width))
            {
                option = options.Options["2/3Left"];
            }
            if (Between(position.X, left + form.TwoThirdRight.Left,
                left + form.TwoThirdRight.Left + form.TwoThirdRight.Width))
            {
                option = options.Options["2/3Right"];
            }

            if (option != Rectangle.Empty)
            {
                NLog.LogManager.GetCurrentClassLogger()
                    .Info(
                        $"Setting window position - x: {option.X}; y: {option.Y}; width: {option.Width}; height: {option.Height}");
                User32.SetWindowPos(hwnd, IntPtr.Zero, option.X, option.Y, option.Width + WINDOW_PADDING_HEIGHT,
                    option.Height + WINDOW_PADDING_HEIGHT, SetWindowPosFlags.IgnoreZOrder);
            }

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
                    {"Centered", new Rectangle(ScreenSize.Width/4, 0, ScreenSize.Width/2, ScreenSize.Height)},
                    {"QuarterLeft", new Rectangle(0, 0, ScreenSize.Width/4, ScreenSize.Height)},
                    {"QuarterRight", new Rectangle(ScreenSize.Width*3/4, 0, ScreenSize.Width/4, ScreenSize.Height)},
                    {"2/3Left", new Rectangle(0, 0, ScreenSize.Width*2/3, ScreenSize.Height)},
                    {"2/3Right", new Rectangle(ScreenSize.Width*1/3, 0, ScreenSize.Width*2/3, ScreenSize.Height)},
                    {"1/3Left", new Rectangle(0, 0, ScreenSize.Width*1/3, ScreenSize.Height)},
                    {"1/3Right", new Rectangle(ScreenSize.Width*2/3, 0, ScreenSize.Width*1/3, ScreenSize.Height)},
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
    }
}
