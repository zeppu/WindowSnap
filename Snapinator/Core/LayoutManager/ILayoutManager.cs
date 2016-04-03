using System.Windows.Forms;
using Overlay.Core.Configuration.Model;

namespace Overlay.Core.LayoutManager
{
    public interface ILayoutManager
    {
        void ModifyLayout(Layout layout, Screen targetScreen);
        ActiveLayout GetActiveLayout(Screen targetScreen);
    }
}