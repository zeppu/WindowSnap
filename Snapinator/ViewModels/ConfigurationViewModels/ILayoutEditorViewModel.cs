using Overlay.Core.Configuration.Model;

namespace Overlay.ViewModels.ConfigurationViewModels
{
    public interface ILayoutEditorViewModel
    {
        string Name { get; set; }
        string DisplayName { get; set; }
        bool IsModified { get; set; }
        bool IsNew { get; set; }
        string OriginalName { get; set; }
        Layout CreateLayout();
        void CommitChangesToLayout(Layout layout);
    }
}