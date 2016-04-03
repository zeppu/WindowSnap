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

        [Reactive]
        public ColumnViewModel SelectedColumn { get; set; }

        public ReactiveCommand<object> AddItemCommand { get; set; }


    }

    public class ColumnViewModel : ReactiveObject
    {
        [Reactive]
        public string Name { get; set; }

        [Reactive]
        public MeasurementEditModel Width { get; set; }
    }

    public class MeasurementEditModel : ReactiveObject
    {
        public MeasurementEditModel() : this(new Measurement())
        {

        }

        public MeasurementEditModel(Measurement measurement)
        {
            Value = measurement.Value;
            Unit = measurement.Unit;
        }

        #region Conversion Ops
        public static implicit operator Measurement(MeasurementEditModel em)
        {
            return new Measurement(em.Value, em.Unit);
        }

        public static implicit operator MeasurementEditModel(Measurement m)
        {
            return new MeasurementEditModel(m);
        }
        #endregion

        [Reactive]
        public double Value { get; set; }

        [Reactive]
        public MeasurementUnit Unit { get; set; }

        public override string ToString()
        {
            switch (Unit)
            {
                case MeasurementUnit.Percentage:
                    return $"{Value:F}%";
                case MeasurementUnit.Pixels:
                    return $"{Value:N}px";
            }

            return base.ToString();

        }
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