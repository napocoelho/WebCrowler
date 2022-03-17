using System;
using System.Collections.Generic ;
using System.Collections.Concurrent;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace System.Collections.Concurrent
{
    public class ConcurrentHashSet<T> : HashSet<T>
    {
        private readonly ReaderWriterLockSlim LOCK = new ReaderWriterLockSlim(LockRecursionPolicy.SupportsRecursion);

        public ConcurrentHashSet()
            : base()
        { }

        public ConcurrentHashSet(IEnumerable<T> collection)
            : base(collection)
        { }

        public ConcurrentHashSet(IEqualityComparer<T> comparer)
            : base(comparer)
        { }

        public ConcurrentHashSet(IEnumerable<T> collection, IEqualityComparer<T> comparer)
            : base(collection, comparer)
        { }


        /// <summary>
        /// Adds the specified element to a set.
        /// </summary>
        /// <param name="item">The element to add to the set.</param>
        /// <returns></returns>
        public new bool Add(T item)
        {
            try
            {
                LOCK.EnterWriteLock();
                return base.Add(item);
            }
            finally
            {
                if (this.LOCK.IsWriteLockHeld) LOCK.ExitWriteLock();
            }
        }

        /// <summary>
        /// Adds the specified element to a set.
        /// </summary>
        /// <param name="item">The element to add to the set.</param>
        /// <returns></returns>
        public void AddRange(IEnumerable< T> itemCollection)
        {
            try
            {
                LOCK.EnterWriteLock();
                foreach(T item in itemCollection )
                {
                    base.Add(item);
                }
            }
            finally
            {
                if (this.LOCK.IsWriteLockHeld) LOCK.ExitWriteLock();
            }
        }

        public new Enumerator GetEnumerator()
        {
            try
            {
                LOCK.EnterReadLock();
                return base.GetEnumerator();
            }
            finally
            {
                if (this.LOCK.IsReadLockHeld) LOCK.ExitReadLock();
            }
        }

        /// <summary>
        /// Remove all the elements from a System.Collections.Concurrent.ConcurrentHashSet<T> object.
        /// </summary>
        public new void Clear()
        {
            try
            {
                this.LOCK.EnterWriteLock();
                base.Clear();
            }
            finally
            {
                if (this.LOCK.IsWriteLockHeld) this.LOCK.ExitWriteLock();
            }
        }

        /// <summary>
        /// Determines whether a System.Collections.Concurrent.ConcurrentHashSet<T> object contains the specified element.
        /// </summary>
        /// <param name="item">The element to locate in the System.Collections.Concurrent.ConcurrentHashSet<T> object.</param>
        /// <returns></returns>
        public new bool Contains(T item)
        {
            try
            {
                this.LOCK.EnterReadLock();
                return base.Contains(item);
            }
            finally
            {
                if (this.LOCK.IsReadLockHeld) this.LOCK.ExitReadLock();
            }
        }

        public new bool Remove(T item)
        {
            try
            {
                this.LOCK.EnterWriteLock();
                return base.Remove(item);
            }
            finally
            {
                if (this.LOCK.IsWriteLockHeld) this.LOCK.ExitWriteLock();
            }
        }

        public new int RemoveWhere(Predicate<T> match)
        {
            try
            {
                this.LOCK.EnterWriteLock();
                return base.RemoveWhere(match);
            }
            finally
            {
                if (this.LOCK.IsWriteLockHeld) this.LOCK.ExitWriteLock();
            }
        }

        public new int Count
        {
            get
            {
                try
                {
                    this.LOCK.EnterReadLock();
                    return base.Count;
                }
                finally
                {
                    if (this.LOCK.IsReadLockHeld) this.LOCK.ExitReadLock();
                }
            }
        }

        public void Dispose()
        {
            if (this.LOCK != null)
                this.LOCK.Dispose();
        }
    }
}