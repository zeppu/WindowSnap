using System.Windows.Forms;
using Snapinator.Core.Configuration.Model;

namespace Snapinator.Core.LayoutManager
{
    public interface ILayoutManager
    {
        void ModifyLayout(Layout layout, Screen targetScreen);
        ActiveLayout GetActiveLayout(Screen targetScreen);
    }
}