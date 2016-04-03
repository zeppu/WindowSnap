using System;

namespace Snapinator.Core.Hooks
{
    public interface IWinEventHookManager : IDisposable
    {
        void Start();
    }
}