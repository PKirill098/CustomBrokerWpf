
using System.Collections.Generic;

namespace KirillPolyanskiy.CustomBrokerWpf
{
    public class ListNotifyChanged<T> : System.Collections.Generic.List<T>, System.Collections.Specialized.INotifyCollectionChanged
    {
        public event System.Collections.Specialized.NotifyCollectionChangedEventHandler CollectionChanged;
        public void LaunchCollectionChanged(System.Collections.Specialized.NotifyCollectionChangedEventArgs changedArgs)
        {
            if (CollectionChanged != null) CollectionChanged(this, changedArgs);
        }

        public void OnResetCollectionChanged()
        {
            LaunchCollectionChanged(new System.Collections.Specialized.NotifyCollectionChangedEventArgs(System.Collections.Specialized.NotifyCollectionChangedAction.Reset));
        }
        private void OnAddCollectionChanged(T addItem)
        {
            LaunchCollectionChanged(new System.Collections.Specialized.NotifyCollectionChangedEventArgs(System.Collections.Specialized.NotifyCollectionChangedAction.Add, addItem));
        }
        private void OnAddRangeCollectionChanged(System.Collections.IList changedItems)
        {
            LaunchCollectionChanged(new System.Collections.Specialized.NotifyCollectionChangedEventArgs(System.Collections.Specialized.NotifyCollectionChangedAction.Add, changedItems));
        }
        private void OnRemoveCollectionChanged(T removeItem)
        {
            LaunchCollectionChanged(new System.Collections.Specialized.NotifyCollectionChangedEventArgs(System.Collections.Specialized.NotifyCollectionChangedAction.Remove, removeItem));
        }

        public new void Add(T item)
        {
            base.Add(item);
            OnAddCollectionChanged(item);
        }
        public new void AddRange(System.Collections.Generic.IEnumerable<T> range)
        {
            base.AddRange(range);
            List<T> list = new List<T>(range);
            OnAddRangeCollectionChanged(list);
        }

        public new void Insert(int index,T item)
        {
            base.Insert(index, item);
            OnAddCollectionChanged(item);
        }
        public new void InsertRange(int index,IEnumerable<T> range)
        {
            base.InsertRange(index, range);
            List<T> list = new List<T>(range);
            OnAddRangeCollectionChanged(list);
        }


        public new bool Remove(T item)
        {
            bool result;
            result = base.Remove(item);
            if (result) OnRemoveCollectionChanged(item);
            return result;
        }


    }
}
