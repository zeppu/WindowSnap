using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace Overlay.Core
{
    public class WindowOptions
    {
        public static WindowOptions Instance = new WindowOptions();

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
}