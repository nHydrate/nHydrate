using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace nHydrate.Generator.Common.Util
{
	/// <summary>
	/// This is a thread-safe hashtable class
	/// </summary>
	/// <typeparam name="K">The object type of the key</typeparam>
	/// <typeparam name="T">The object type of the value</typeparam>
	public class HashTable<K, T>
	{
		protected Hashtable _h = new Hashtable();

		public HashTable()
		{
		}

		public virtual void Add(K key, T value)
		{
			lock (_h)
			{
				_h[key] = value;
			}
		}

		public virtual void Clear()
		{
			lock (_h)
			{
				_h.Clear();
			}
		}

		public virtual T this[K key]
		{
			get
			{
				lock (_h)
				{
					return (T)_h[key];
				}
			}
			set
			{
				lock (_h)
				{
					_h[key] = value;
				}
			}
		}

		public virtual bool ContainsKey(K key)
		{
			lock (_h)
			{
				return _h.ContainsKey(key);
			}
		}

		public int Count
		{
			get
			{
				lock (_h)
				{
					return _h.Keys.Count;
				}
			}
		}

		public virtual bool ContainsValue(T value)
		{
			lock (_h)
			{
				return _h.ContainsValue(value);
			}
		}

		public virtual ICollection<K> Keys
		{
			get
			{
				lock (_h)
				{
					return new List<K>(_h.Keys.Cast<K>());
				}
			}
		}

		public virtual void Remove(K key)
		{
			lock (_h)
			{
				_h.Remove(key);
			}
		}

		ICollection<T> Values
		{
			get
			{
				lock (_h)
				{
					return new List<T>(_h.Values.Cast<T>());
				}
			}
		}

	}
}

