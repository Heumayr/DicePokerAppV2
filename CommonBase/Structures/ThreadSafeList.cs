using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonBase.Structures
{
    public class ThreadSafeList<T, TKey> : IEnumerable<T>
        where T : IAccessableByKey<TKey>, new()
    {
        private readonly List<T> list = new();
        private readonly object lockObject = new();

        public void Add(T t)
        {
            lock (lockObject)
            {
                list.Add(t);
            }
        }

        public void Remove(T t)
        {
            lock (lockObject)
            {
                list.Remove(t);
            }
        }

        public bool Contains(T t)
        {
            var result = false;

            lock (lockObject)
            {
                result = list.Contains(t);
            }

            return result;
        }

        public void AddRange(T[] range)
        {
            lock (lockObject)
            {
                list.AddRange(range);
            }
        }

        public IEnumerator<T> GetEnumerator()
        {
            return list.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return list.GetEnumerator();
        }

        public T? this[TKey key]
        {
            get
            {
                return list.FirstOrDefault(e => e.Key != null && e.Key.Equals(key));
            }
            set
            {
                lock (lockObject)
                {
                    var e = list.FirstOrDefault(e => e.Key != null && e.Key.Equals(key));
                } 
            }
        }

        public T? this[Predicate<T> predicate]
        {
            get
            {
                return list.FirstOrDefault(i => predicate(i));
            }
            set 
            {
                var e = list.FirstOrDefault(i => predicate(i));
            }
        }
    }
}
