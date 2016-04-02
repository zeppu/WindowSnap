using System;
using System.Linq;
using Overlay.Core.Configuration;
using Overlay.Core.Configuration.Model;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;

namespace Overlay.ViewModels
{
    public class LayoutConfigurationViewModel : ReactiveObject, ILayoutConfigurationViewModel
    {
        public string Title { get; } = "Layouts";

        public IReactiveList<ILayoutEditorViewModel> Layouts { get; }

        public ReactiveCommand<object> AddLayout { get; }

        [Reactive]
        public ILayoutEditorViewModel SelectedLayout { get; set; }

        public LayoutConfigurationViewModel(IConfigurationService configurationService)
        {
            Layouts = new ReactiveList<ILayoutEditorViewModel>(configurationService.GetLayouts().Select(c => new ColumnLayoutEditorViewModel(c)));



            AddLayout = ReactiveCommand.Create();
            AddLayout.Subscribe(x => Layouts.Add(new ColumnLayoutEditorViewModel()
            {
                Name = "NewLayout" + (Layouts.Count + 1),
                DisplayName = "New Column Layout" + (Layouts.Count + 1),
            }));
        }
    }

    public interface ILayoutConfigurationViewModel : IConfigurationPartViewModel
    {

    }
}