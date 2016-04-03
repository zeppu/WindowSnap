using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive.Linq;
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

            Columns = new ReactiveList<ColumnViewModel>();

            IsNew = true;
            IsModified = true;

            Columns.Changed.Subscribe(_ =>
            {
                IsModified = true;
            });

            this.WhenAnyValue(model => model.Name, model => model.DisplayName)
                .Subscribe(_ =>
                {
                    IsModified = true;
                });
        }

        public ColumnLayoutEditorViewModel(Layout c) : this()
        {
            OriginalName = Name = c.Name;
            DisplayName = c.DisplayName;

            var subitems = ((ColumnLayout)c).Columns.Select(x => new ColumnViewModel(x.Name, x.Width));

            foreach (var item in subitems)
                Columns.Add(item);

            IsNew = false;
            IsModified = false;
        }

        public string OriginalName { get; set; }

        public Layout CreateLayout()
        {
            var newLayout = new ColumnLayout
            {
                IsActive = false
            };
            CommitChangesToLayoutWorker(newLayout);

            return newLayout;
        }

        public void CommitChangesToLayout(Layout layout)
        {
            var columnLayout = layout as ColumnLayout;
            if (columnLayout == null)
                return;

            CommitChangesToLayoutWorker(columnLayout);
        }

        private void CommitChangesToLayoutWorker(ColumnLayout columnLayout)
        {
            columnLayout.Name = this.Name;
            columnLayout.DisplayName = this.DisplayName;
            columnLayout.Columns = this.Columns
                .Select(
                    (vm, index) => new Column()
                    {
                        ColumnIndex = index,
                        Name = vm.Name,
                        Width = vm.Width,
                        RowIndex = 0
                    })
                .ToList();
        }

        [Reactive]
        public bool IsModified { get; set; }

        [Reactive]
        public bool IsNew { get; set; }

        [Reactive]
        public string Name { get; set; }

        [Reactive]
        public string DisplayName { get; set; }

        [Reactive]
        public ReactiveList<ColumnViewModel> Columns { get; set; }

        [Reactive]
        public ColumnViewModel SelectedColumn { get; set; }

        public ReactiveCommand<object> AddItemCommand { get; set; }


    }

    public class ColumnViewModel : ReactiveObject
    {

        public ColumnViewModel() : this("Unnamed", new MeasurementEditModel())
        {

        }

        public ColumnViewModel(string name, MeasurementEditModel width)
        {
            Name = name;
            Width = width;
        }



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

            this.WhenAnyValue(x => x.Value, x => x.Unit)
                .Select(x =>
                {
                    switch (Unit)
                    {
                        case MeasurementUnit.Percentage:
                            return $"{Value:F}%";
                        case MeasurementUnit.Pixels:
                            return $"{Value:N}px";
                    }

                    return $"{Value}";
                })
                .ToPropertyEx(this, x => x.Display);
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

        [ObservableAsProperty]
        public extern string Display { get; }

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

    public interface IColumnLayoutEditorViewModel : ILayoutEditorViewModel
    {

    }
}