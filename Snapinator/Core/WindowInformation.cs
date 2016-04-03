using System;
using System.Text;
using Snapinator.Native;

namespace Snapinator.Core
{
    public class WindowInformation
    {
        protected bool Equals(WindowInformation other)
        {
            return Handle.Equals(other.Handle);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((WindowInformation)obj);
        }

        public override int GetHashCode()
        {
            return Handle.GetHashCode();
        }

        public WindowInformation(IntPtr handle)
        {
            Handle = handle;

            var sb = new StringBuilder(50);

            if (User32.GetWindowText(Handle, sb, sb.Capacity) > 0)
            {
                Title = sb.ToString();
            }
            else
            {
                Title = $"[handle:{handle}]";
            }

        }

        public string Title { get; }

        public IntPtr Handle { get; }

        #region Conversion operators
        public static implicit operator IntPtr(WindowInformation wi)
        {
            return wi.Handle;
        }

        public static implicit operator WindowInformation(IntPtr windowHandle)
        {
            return new WindowInformation(windowHandle);
        }
        #endregion
    }
}