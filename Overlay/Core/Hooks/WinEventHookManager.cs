using System;
using Overlay.Messages;
using Overlay.Native;
using ReactiveUI;

namespace Overlay.Core.Hooks
{
    class WinEventHookManager : IWinEventHookManager
    {
        private IntPtr _hhookstart;
        private IntPtr _hhookend;
        private readonly User32.WinEventDelegate _endMoveDelegate;
        private readonly User32.WinEventDelegate _startMoveDelegate;

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
            MessageBus.Current.SendMessage(new ShowOverlayMessage());
        }

        private void WindowDragEnd(IntPtr hWinEventHook, uint eventType,
            IntPtr hwnd, int idObject, int idChild, uint dwEventThread, uint dwmsEventTime)
        {
            MessageBus.Current.SendMessage(new HideOverlayMessage(hwnd));
        }
    }
}