using System.Collections.Generic;
using System.Configuration;

namespace Meerkat.Security.Activities.Configuration
{
    /// <summary>
    /// Provides a generic collection of <see cref="ConfigurationElement" />, implementing most necessary behaviour for client types.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class ConfigElementCollection<T> : ConfigurationElementCollection, IList<T>
        where T : ConfigurationElement, new()
    {
        public override ConfigurationElementCollectionType CollectionType
        {
            get { return ConfigurationElementCollectionType.BasicMap; }
        }

        public T this[int index]
        {
            get
            {
                return (T)BaseGet(index);
            }

            set
            {
                if (BaseGet(index) != null)
                {
                    BaseRemoveAt(index);
                }

                BaseAdd(index, value);
            }
        }

        public new T this[string key]
        {
            get { return (T)BaseGet(key); }
        }

        public int IndexOf(T element)
        {
            return BaseIndexOf(element);
        }

        public void Add(T element)
        {
            BaseAdd(element);
        }

        public void Remove(T element)
        {
            if (BaseIndexOf(element) >= 0)
            {
                BaseRemove(GetElementKey(element));
            }
        }

        public void RemoveAt(int index)
        {
            BaseRemoveAt(index);
        }

        public void Remove(string name)
        {
            BaseRemove(name);
        }

        public void Clear()
        {
            BaseClear();
        }

        IEnumerator<T> IEnumerable<T>.GetEnumerator()
        {
            for (var i = 0; i < Count; i++)
            {
                yield return this[i];
            }
        }

        public void Insert(int index, T item)
        {
            throw new System.NotImplementedException();
        }

        public bool Contains(T item)
        {
            throw new System.NotImplementedException();
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            throw new System.NotImplementedException();
        }

        public new bool IsReadOnly
        {
            get { throw new System.NotImplementedException(); }
        }

        bool ICollection<T>.Remove(T item)
        {
            throw new System.NotImplementedException();
        }

        protected override void BaseAdd(ConfigurationElement element)
        {
            BaseAdd(element, false);
        }

        protected override ConfigurationElement CreateNewElement()
        {
            return new T();
        }
    }
}