using System;
using System.Collections.ObjectModel;
using System.Linq;
using Overlay.Core.Configuration.Model;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;

namespace Overlay.ViewModels.ConfigurationViewModels
{
    public class ColumnLayoutEditorViewModel : ReactiveObject, IColumnLayoutEditorViewModel
    {
        public ColumnLayoutEditorViewModel()
        {
            AddItemCommand = ReactiveCommand.Create();
            AddItemCommand.Subscribe(x => Columns.Add(new ColumnViewModel()
            {
                Name = $"Column {Columns.Count}"
            }));

            Columns = new ObservableCollection<ColumnViewModel>();
        }

        public ColumnLayoutEditorViewModel(Layout c) : this()
        {
            Name = c.Name;
            DisplayName = c.DisplayName;

            var subitems = ((ColumnLayout)c).Columns.Select(x => new ColumnViewModel()
            {
                Name = x.Name,
                Width = x.Width
            });

            foreach (var item in subitems)
                Columns.Add(item);
        }

        [Reactive]
        public string Name { get; set; }

        [Reactive]
        public string DisplayName { get; set; }

        [Reactive]
        public ObservableCollection<ColumnViewModel> Columns { get; set; }

        public ReactiveCommand<object> AddItemCommand { get; set; }


    }

    public class ColumnViewModel : ReactiveObject
    {
        public string Name { get; set; }

        public Measurement Width { get; set; }
    }

    public interface ILayoutEditorViewModel
    {
        string Name { get; set; }
        string DisplayName { get; set; }
    }

    public interface IColumnLayoutEditorViewModel : ILayoutEditorViewModel
    {

    }
}