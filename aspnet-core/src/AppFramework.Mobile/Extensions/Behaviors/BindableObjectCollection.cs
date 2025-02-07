using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using Xamarin.Forms;
using Xamarin.Forms.Internals;

namespace AppFramework.Shared.Behaviors
{
    [Preserve(AllMembers = true)]
    public class BindableObjectCollection : BindableObject, IList<BindableObject>, INotifyCollectionChanged
    {
        private readonly List<BindableObject> _items = new List<BindableObject>();

        public event NotifyCollectionChangedEventHandler CollectionChanged;

        public int IndexOf(BindableObject item)
        {
            return _items.IndexOf(item);
        }

        public void Insert(int index, BindableObject item)
        {
            _items.Insert(index, item);
            CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, item, index));
        }

        public void RemoveAt(int index)
        {
            var oldItem = this[index];
            _items.RemoveAt(index);
            CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, oldItem, index));
        }

        public BindableObject this[int index]
        {
            get
            {
                return this[index];
            }
            set
            {
                var oldItem = this[index];
                this[index] = (BindableObject)value;
                CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Replace, value, oldItem));
            }
        }

        public void Add(BindableObject item)
        {
            _items.Add(item);
            CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, item, Count - 1));
        }

        public void Clear()
        {
            _items.Clear();
            CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
        }

        public bool Contains(BindableObject item)
        {
            return _items.Contains(item);
        }

        public void CopyTo(BindableObject[] array, int arrayIndex)
        {
            _items.CopyTo(array, arrayIndex);
        }

        public bool Remove(BindableObject item)
        {
            var oldIndex = IndexOf(item);
            if (_items.Remove(item))
            {
                CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, item, oldIndex));
                return true;
            }
            return false;
        }

        public int Count => _items.Count;

        public bool IsReadOnly => false;

        public IEnumerator<BindableObject> GetEnumerator()
        {
            return _items.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}