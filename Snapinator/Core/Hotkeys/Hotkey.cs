using System;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using Overlay.Native;

namespace Overlay.Core.Hotkeys
{
    public sealed class Hotkey : IDisposable
    {
        private bool Equals(Hotkey other)
        {
            return _modifier == other._modifier && _key == other._key;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return obj is Hotkey && Equals((Hotkey) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return ((int) _modifier*397) ^ (int) _key;
            }
        }

        private readonly Keys _modifier;
        private readonly Keys _key;

        public Hotkey(Keys modifier, Keys key)
        {
            _modifier = modifier;
            _key = key;

            var mk = ModifierKeys.None;

            if (modifier.HasFlag(Keys.LWin) || modifier.HasFlag(Keys.RWin))
            {
                mk |= ModifierKeys.Windows;
            }

            if (modifier.HasFlag(Keys.Alt))
            {
                mk |= ModifierKeys.Alt;
            }

            if (modifier.HasFlag(Keys.Control))
            {
                mk |= ModifierKeys.Control;
            }


            if (modifier.HasFlag(Keys.Shift))
            {
                mk |= ModifierKeys.Shift;
            }

            if (!User32.RegisterHotKey(IntPtr.Zero, GetHashCode(), mk, (int)key))
                throw new Win32Exception(Marshal.GetLastWin32Error());
        }

        #region IDisposable Support
        private bool _disposedValue; // To detect redundant calls

        private void Dispose(bool disposing)
        {
            if (!_disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects).
                }

                User32.UnregisterHotKey(IntPtr.Zero, GetHashCode());

                _disposedValue = true;
            }
        }

        ~Hotkey()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(false);
        }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        #endregion
    }
}