using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;

namespace TKF
{
    /// <summary>
    /// Weak reference List.
    /// ValueはWeakReferenceで保持される
    /// </summary>
    public class WeakReferenceList<TValue> : IEnumerable<TValue> where TValue : class
    {
        private List<WeakReference> _list = new List<WeakReference>();

        public int Count
        {
            get { return _list.Count; }
        }

        public void Add(TValue value)
        {
            if (value == null)
            {
                return;
            }

            _list.Add(CreateWeakReference(value));
        }

        public TValue Find(Predicate<TValue> match)
        {
            TValue rval = null;
            var weakRef =
                _list.Find((val) => match(val.Target as TValue));

            if (weakRef != null)
            {
                rval = weakRef.Target as TValue;
            }

            return rval;
        }

        public int RemoveNullValue()
        {
            return _list.RemoveAll((val) => val.Target == null);
        }

        public bool Remove(TValue value)
        {
            WeakReference removeTarget = null;

            foreach (var weakRef in _list)
            {
                if (weakRef.Target == value)
                {
                    removeTarget = weakRef;
                    break;
                }
            }

            return _list.Remove(removeTarget);
        }

        public void RemoveAt(int index)
        {
            _list.RemoveAt(index);
        }

        public int RemoveAll(Predicate<TValue> match)
        {
            if (match == null)
            {
                return 0;
            }

            return _list.RemoveAll((val) => match(val.Target as TValue));
        }

        public void Clear()
        {
            _list.Clear();
        }

        public IEnumerator<TValue> GetEnumerator()
        {
            return _list.Select(_ => _.Target).Cast<TValue>().GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        private WeakReference CreateWeakReference(TValue value)
        {
            return new WeakReference(value);
        }
    }
}