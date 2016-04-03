using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Overlay.Core.Hotkeys
{
    public class HotkeyManager : IHotkeyManger
    {
        private readonly List<Hotkey> _hotkeyList;

        public HotkeyManager()
        {
            _hotkeyList = new List<Hotkey>();
        }

        public void AddHotkey(Keys modifier, Keys key)
        {
            _hotkeyList.Add(new Hotkey(modifier, key));
        }

        public void Dispose()
        {
            _hotkeyList.ForEach(key => key.Dispose());
        }
    }

    public interface IHotkeyManger : IDisposable
    {
        void AddHotkey(Keys modifier, Keys key);
    }
}