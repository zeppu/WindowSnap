using System;
using System.Drawing;
using ReactiveUI;
using Snapinator.Messages;
using Snapinator.Native;

namespace Snapinator.Core.Hooks
{
    class WinEventHookManager : IWinEventHookManager
    {
        private IntPtr _hhookstart;
        private IntPtr _hhookend;
        private readonly User32.WinEventDelegate _endMoveDelegate;
        private readonly User32.WinEventDelegate _startMoveDelegate;
        private User32.RECT _currentWindowRect;

        public WinEventHookManager()
        {
            _endMoveDelegate = WindowDragEnd;
            _startMoveDelegate = WindowDragBegin;
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

        private void WindowDragBegin(IntPtr hWinEventHook, uint eventType, IntPtr hwnd, int idObject, int idChild, uint dwEventThread, uint dwmsEventTime)
        {
            User32.GetWindowRect(hwnd, out _currentWindowRect);

            MessageBus.Current.SendMessage(new StartingWindowDrag(hwnd));
        }

        private void WindowDragEnd(IntPtr hWinEventHook, uint eventType,
            IntPtr hwnd, int idObject, int idChild, uint dwEventThread, uint dwmsEventTime)
        {
            User32.RECT newRect;
            User32.GetWindowRect(hwnd, out newRect);

            var prevRectangle = (Rectangle)_currentWindowRect;
            var newRectangle = (Rectangle)newRect;

            var prevArea = prevRectangle.Width * prevRectangle.Height;
            var newArea = newRectangle.Width * newRectangle.Height;

            if (prevArea == newArea)
            {
                MessageBus.Current.SendMessage(new EndingWindowDrag(hwnd, false));
            }
            else
            {
                MessageBus.Current.SendMessage(new EndingWindowDrag(hwnd, true));
            }
        }
    }
}