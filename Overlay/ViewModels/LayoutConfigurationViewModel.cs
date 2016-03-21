using System;
using Overlay.Core.Configuration;
using Overlay.Core.Configuration.Model;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;

namespace Overlay.ViewModels
{
    public class LayoutConfigurationViewModel : ReactiveObject, ILayoutConfigurationViewModel
    {
        public string Title { get; } = "Layouts";

        public IReactiveList<Layout> Layouts { get; }

        public ReactiveCommand<object> AddLayout { get; }

        [Reactive]
        public Layout SelectedLayout { get; set; }

        public LayoutConfigurationViewModel(IConfigurationService configurationService)
        {
            Layouts = new ReactiveList<Layout>(configurationService.GetLayouts());



            AddLayout = ReactiveCommand.Create();
            AddLayout.Subscribe(x => Layouts.Add(new ColumnLayout()));
        }
    }

    public interface ILayoutConfigurationViewModel : IConfigurationPartViewModel
    {

    }
}