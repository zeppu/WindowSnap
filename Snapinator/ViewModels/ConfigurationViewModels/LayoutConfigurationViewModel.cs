using System;
using System.Linq;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using Snapinator.Core.Configuration;

namespace Snapinator.ViewModels.ConfigurationViewModels
{
    public class LayoutConfigurationViewModel : ReactiveObject, ILayoutConfigurationViewModel
    {
        private readonly IConfigurationService _configurationService;
        public string Title { get; } = "Layouts";

        public void CommitChanges()
        {
            foreach (var layoutEditorViewModel in Layouts)
            {
                if (layoutEditorViewModel.IsNew)
                {
                    // add new layout
                    var newLayout = layoutEditorViewModel.CreateLayout();
                    _configurationService.AddLayout(newLayout);
                    continue;
                }

                var originalName = layoutEditorViewModel.OriginalName;
                if (string.IsNullOrEmpty(originalName))
                    continue;

                var layout = _configurationService.GetLayoutByName(originalName);
                layoutEditorViewModel.CommitChangesToLayout(layout);
            }
        }

        public IReactiveList<ILayoutEditorViewModel> Layouts { get; }

        public ReactiveCommand<object> AddLayout { get; }

        [Reactive]
        public ILayoutEditorViewModel SelectedLayout { get; set; }

        public LayoutConfigurationViewModel(IConfigurationService configurationService)
        {
            _configurationService = configurationService;
            Layouts = new ReactiveList<ILayoutEditorViewModel>(configurationService.GetLayouts().Select(c => new ColumnLayoutEditorViewModel(c)));

            SelectedLayout = Layouts.FirstOrDefault();

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
        IReactiveList<ILayoutEditorViewModel> Layouts { get; }
        ReactiveCommand<object> AddLayout { get; }
        ILayoutEditorViewModel SelectedLayout { get; set; }
    }
}