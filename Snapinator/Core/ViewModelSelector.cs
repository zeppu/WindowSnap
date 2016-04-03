using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace Snapinator.Core
{
    public class ViewModelSelector : DataTemplateSelector, IList<DataTemplateMapping>, ICollection, IList
    {
        private readonly List<DataTemplateMapping> _mappings = new List<DataTemplateMapping>();
        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            if (item == null)
                return base.SelectTemplate(item, container);

            var template = _mappings.Where(m => m.ViewModelType == item.GetType()).Select(m => m.Template).FirstOrDefault();
            if (template == null)
                return base.SelectTemplate(item, container);

            return template;
        }

        public IEnumerator<DataTemplateMapping> GetEnumerator()
        {
            return _mappings.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable)_mappings).GetEnumerator();
        }

        public void Add(DataTemplateMapping item)
        {
            _mappings.Add(item);
        }

        public int Add(object value)
        {
            return ((IList)_mappings).Add(value);
        }

        public bool Contains(object value)
        {
            return ((IList)_mappings).Contains(value);
        }

        public void Clear()
        {
            _mappings.Clear();
        }

        public int IndexOf(object value)
        {
            return ((IList)_mappings).IndexOf(value);
        }

        public void Insert(int index, object value)
        {
            ((IList)_mappings).Insert(index, value);
        }

        public void Remove(object value)
        {
            ((IList)_mappings).Remove(value);
        }

        public bool Contains(DataTemplateMapping item)
        {
            return _mappings.Contains(item);
        }

        public void CopyTo(DataTemplateMapping[] array, int arrayIndex)
        {
            _mappings.CopyTo(array, arrayIndex);
        }

        public bool Remove(DataTemplateMapping item)
        {
            return _mappings.Remove(item);
        }

        public void CopyTo(Array array, int index)
        {
            ((ICollection)_mappings).CopyTo(array, index);
        }

        public int Count => _mappings.Count;

        public object SyncRoot => ((ICollection)_mappings).SyncRoot;

        public bool IsSynchronized => ((ICollection)_mappings).IsSynchronized;

        public bool IsReadOnly => false;

        public bool IsFixedSize => ((IList)_mappings).IsFixedSize;

        public int IndexOf(DataTemplateMapping item)
        {
            return _mappings.IndexOf(item);
        }

        public void Insert(int index, DataTemplateMapping item)
        {
            _mappings.Insert(index, item);
        }

        public void RemoveAt(int index)
        {
            _mappings.RemoveAt(index);
        }

        object IList.this[int index]
        {
            get { return _mappings[index]; }
            set { ((IList)_mappings)[index] = value; }
        }

        public DataTemplateMapping this[int index]
        {
            get { return _mappings[index]; }
            set { _mappings[index] = value; }
        }
    }

    public class DataTemplateMapping
    {
        public Type ViewModelType { get; set; }

        public DataTemplate Template { get; set; }
    }
}