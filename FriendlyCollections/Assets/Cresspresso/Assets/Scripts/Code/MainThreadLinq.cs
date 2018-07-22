namespace Cresspresso.Extensions
{
	using System;
	using System.Collections.Generic;

	/// <summary>
	/// Similar to the extension methods provided by <see cref="System.Linq.Enumerable"/>,
	/// but using only the main thread and no iterator functions.
	/// </summary>
	public static class MainThreadLinq
	{
		public static bool IsUnique<T>(
			this IEnumerable<T> collection,
			T item,
			IEqualityComparer<T> comparer)
		{
			foreach (T x in collection)
			{
				if (comparer.Equals(x, item))
					return false;
			}
			return true;
		}



		public static List<T> Distinct<T>(
			this IEnumerable<T> collection,
			IEqualityComparer<T> comparer)
		{
			List<T> result = new List<T>();
			foreach (T x in collection)
			{
				if (result.IsUnique(x, comparer))
				{
					result.Add(x);
				}
			}
			return result;
		}



		public static HashSet<T> ToHashSet<T>(
			this IEnumerable<T> collection,
			IEqualityComparer<T> comparer)
		{
			return new HashSet<T>(collection.Distinct(comparer));
		}



		public static bool IsUniqueBy<T, U>(
			this IEnumerable<T> collection,
			T item,
			Func<T, U> selector,
			IEqualityComparer<U> comparer)
		{
			U value = selector(item);
			foreach (T x in collection)
			{
				if (comparer.Equals(selector(x), value))
					return false;
			}
			return true;
		}



		public static List<T> DistinctBy<T, U>(
			this IEnumerable<T> collection,
			Func<T, U> selector,
			IEqualityComparer<U> comparer)
		{
			List<T> result = new List<T>();
			foreach (T x in collection)
			{
				if (result.IsUniqueBy(x, selector, comparer))
				{
					result.Add(x);
				}
			}
			return result;
		}



		public static List<U> Select<T, U>(
			this IEnumerable<T> collection,
			Func<T, U> selector)
		{
			List<U> result = new List<U>();
			foreach (T x in collection)
			{
				result.Add(selector(x));
			}
			return result;
		}



		public static Dictionary<TKey, TValue> ToDictionary<TKey, TValue>(
			this IEnumerable<KeyValuePair<TKey, TValue>> collection,
			IEqualityComparer<TKey> comparer)
		{
			Dictionary<TKey, TValue> result = new Dictionary<TKey, TValue>();
			foreach (var p in collection.DistinctBy(p => p.Key, comparer))
			{
				((IDictionary<TKey, TValue>)result).Add(p);
			}
			return result;
		}



		public static Dictionary<TKey, TValue> ToDictionary<T, TKey, TValue>(
			this IEnumerable<T> collection,
			Func<T, TKey> keySelector,
			Func<T, TValue> valueSelector,
			IEqualityComparer<TKey> comparer)
		{
			Dictionary<TKey, TValue> result = new Dictionary<TKey, TValue>();
			foreach (var p in collection.DistinctBy(keySelector, comparer))
			{
				result.Add(keySelector(p), valueSelector(p));
			}
			return result;
		}
	}
}
