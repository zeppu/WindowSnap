using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace Snapinator.Core
{
    public class ConditionalDataTemplateSelector : DataTemplateSelector, IList<ConditionalDataTemplate>, ICollection, IList
    {
        private readonly List<ConditionalDataTemplate> _conditionalTemplates = new List<ConditionalDataTemplate>();

        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            if (item == null)
                return base.SelectTemplate(item, container);

            if (string.IsNullOrEmpty(ComperandName))
                return base.SelectTemplate(item, container);


            var prop = item.GetType().GetProperties().FirstOrDefault(p => p.Name.Equals(ComperandName));
            if (prop == null)
                return base.SelectTemplate(item, container);

            var comperandValue = prop.GetValue(item);
            var selectedTemplate = _conditionalTemplates.FirstOrDefault(template => template.Comperand.Equals(comperandValue));
            if (selectedTemplate != null)
                return selectedTemplate;

            return base.SelectTemplate(item, container);
        }

        public string ComperandName { get; set; }

        #region List Implementation
        public IEnumerator<ConditionalDataTemplate> GetEnumerator()
        {
            return _conditionalTemplates.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable)_conditionalTemplates).GetEnumerator();
        }

        public void Add(ConditionalDataTemplate item)
        {
            _conditionalTemplates.Add(item);
        }

        public int Add(object value)
        {
            return ((IList)_conditionalTemplates).Add(value);
        }

        public bool Contains(object value)
        {
            return ((IList)_conditionalTemplates).Contains(value);
        }

        void IList.Clear()
        {
            ((IList)_conditionalTemplates).Clear();
        }

        public int IndexOf(object value)
        {
            return ((IList)_conditionalTemplates).IndexOf(value);
        }

        public void Insert(int index, object value)
        {
            ((IList)_conditionalTemplates).Insert(index, value);
        }

        public void Remove(object value)
        {
            ((IList)_conditionalTemplates).Remove(value);
        }

        void IList.RemoveAt(int index)
        {
            ((IList)_conditionalTemplates).RemoveAt(index);
        }

        object IList.this[int index]
        {
            get { return this[index]; }
            set { this[index] = (ConditionalDataTemplate)value; }
        }

        public bool IsFixedSize => ((IList)_conditionalTemplates).IsFixedSize;

        void ICollection<ConditionalDataTemplate>.Clear()
        {
            _conditionalTemplates.Clear();
        }

        public bool Contains(ConditionalDataTemplate item)
        {
            return _conditionalTemplates.Contains(item);
        }

        public void CopyTo(ConditionalDataTemplate[] array, int arrayIndex)
        {
            _conditionalTemplates.CopyTo(array, arrayIndex);
        }

        public bool Remove(ConditionalDataTemplate item)
        {
            return _conditionalTemplates.Remove(item);
        }

        public void CopyTo(Array array, int index)
        {
            ((ICollection)_conditionalTemplates).CopyTo(array, index);
        }

        public int Count => _conditionalTemplates.Count;

        public object SyncRoot => ((ICollection)_conditionalTemplates).SyncRoot;

        public bool IsSynchronized => ((ICollection)_conditionalTemplates).IsSynchronized;

        public bool IsReadOnly => ((ICollection<ConditionalDataTemplate>)_conditionalTemplates).IsReadOnly;

        public int IndexOf(ConditionalDataTemplate item)
        {
            return _conditionalTemplates.IndexOf(item);
        }

        public void Insert(int index, ConditionalDataTemplate item)
        {
            _conditionalTemplates.Insert(index, item);
        }

        void IList<ConditionalDataTemplate>.RemoveAt(int index)
        {
            _conditionalTemplates.RemoveAt(index);
        }

        public ConditionalDataTemplate this[int index]
        {
            get { return _conditionalTemplates[index]; }
            set { _conditionalTemplates[index] = value; }
        }
        #endregion
    }

    public class ConditionalDataTemplate : DataTemplate
    {
        public object Comperand { get; set; }

    }
}