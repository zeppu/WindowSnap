using System;

namespace Overlay.Core.Hooks
{
    public interface IWinEventHookManager : IDisposable
    {
        void Start();
    }
}