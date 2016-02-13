using System;
using System.Drawing;
using System.Windows.Forms;
using Overlay.Messages;
using Overlay.Native;
using ReactiveUI;

namespace Overlay.Core.Hooks
{
    public interface IWinEventHookManager : IDisposable
    {
        void Start();
    }

    class WinEventHookManager : IWinEventHookManager
    {
        private readonly Form1 _form;
        private IntPtr _hhookstart;
        private IntPtr _hhookend;
        private readonly User32.WinEventDelegate _endMoveDelegate;
        private readonly User32.WinEventDelegate _startMoveDelegate;

        public WinEventHookManager(Form1 form)
        {
            _form = form;

            _endMoveDelegate = WinEventProc;
            _startMoveDelegate = WinEventProc2;
        }

        public void Dispose()
        {
            User32.UnhookWinEvent(_hhookstart);
            User32.UnhookWinEvent(_hhookend);
        }

        public void Start()
        {
            // Listen for name change changes across all processes/threads on current desktop...
            _hhookend = User32.SetWinEventHook(User32.Constants.EVENT_SYSTEM_MOVESIZEEND, User32.Constants.EVENT_SYSTEM_MOVESIZEEND, IntPtr.Zero,
                _endMoveDelegate, 0, 0, User32.Constants.WINEVENT_OUTOFCONTEXT);
            _hhookstart = User32.SetWinEventHook(User32.Constants.EVENT_SYSTEM_MOVESIZESTART, User32.Constants.EVENT_SYSTEM_MOVESIZESTART, IntPtr.Zero,
                _startMoveDelegate, 0, 0, User32.Constants.WINEVENT_OUTOFCONTEXT);
        }

        private void WinEventProc2(IntPtr hWinEventHook, uint eventType, IntPtr hwnd, int idObject, int idChild, uint dwEventThread, uint dwmsEventTime)
        {
            MessageBus.Current.SendMessage(new ShowOverlayMessage());
        }

        private void WinEventProc(IntPtr hWinEventHook, uint eventType,
            IntPtr hwnd, int idObject, int idChild, uint dwEventThread, uint dwmsEventTime)
        {
            var position = Cursor.Position;
            var left = _form.Left;
            MessageBus.Current.SendMessage(new HideOverlayMessage());
            Rectangle option = Rectangle.Empty;

            if (Between(position.X, left + _form.QuarterLeft.Left, left + _form.QuarterLeft.Left + _form.QuarterLeft.Width))
            {
                option = WindowOptions.Instance.Options["QuarterLeft"];
            }
            if (Between(position.X, left + _form.HalfCenter.Left, left + _form.HalfCenter.Left + _form.HalfCenter.Width))
            {
                option = WindowOptions.Instance.Options["Centered"];
            }
            if (Between(position.X, left + _form.QuarterRight.Left,
                left + _form.QuarterRight.Left + _form.QuarterRight.Width))
            {
                option = WindowOptions.Instance.Options["QuarterRight"];
            }
            if (Between(position.X, left + _form.ThirdLeft.Left, left + _form.ThirdLeft.Left + _form.ThirdLeft.Width))
            {
                option = WindowOptions.Instance.Options["1/3Left"];
            }
            if (Between(position.X, left + _form.ThirdRight.Left, left + _form.ThirdRight.Left + _form.ThirdRight.Width))
            {
                option = WindowOptions.Instance.Options["1/3Right"];
            }
            if (Between(position.X, left + _form.TwoThirdLeft.Left,
                left + _form.TwoThirdLeft.Left + _form.TwoThirdLeft.Width))
            {
                option = WindowOptions.Instance.Options["2/3Left"];
            }
            if (Between(position.X, left + _form.TwoThirdRight.Left,
                left + _form.TwoThirdRight.Left + _form.TwoThirdRight.Width))
            {
                option = WindowOptions.Instance.Options["2/3Right"];
            }

            if (option != Rectangle.Empty)
            {
                NLog.LogManager.GetCurrentClassLogger()
                    .Info(
                        $"Setting window position - x: {option.X}; y: {option.Y}; width: {option.Width}; height: {option.Height}");
                User32.SetWindowPos(hwnd, IntPtr.Zero, option.X, option.Y, option.Width + User32.Constants.WINDOW_PADDING_HEIGHT,
                    option.Height + User32.Constants.WINDOW_PADDING_HEIGHT, SetWindowPosFlags.IgnoreZOrder);
            }

        }

        private static bool Between(int val, int min, int max)
        {
            return (val > min && val < max);
        }
    }
}