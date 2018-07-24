namespace Cresspresso.Extensions
{
	using System;
	using System.Collections.Generic;
	using System.Linq;

	/// <summary>
	/// Extension methods for <see cref="Dictionary{TKey, TValue}"/>.
	/// </summary>
	public static class DictionaryExtensions
	{
		/// <summary>
		/// Removes dictionary entries where the key satisfies the predicate.
		/// </summary>
		/// <param name="dictionary">Dictionary to remove entries from.</param>
		/// <param name="predicate">Function that returns true if a given key should be removed.</param>
		/// <returns>Number of keys that were removed.</returns>
		public static int RemoveAll<TKey, TValue>(
			this Dictionary<TKey, TValue> dictionary,
			Func<TKey, bool> predicate)
		{
			return RemoveAllEntries(dictionary, p => predicate(p.Key));
		}

		/// <summary>
		/// Removes dictionary entries that satisfy the predicate.
		/// </summary>
		/// <param name="dictionary">Dictionary to remove entries from.</param>
		/// <param name="predicate">Function that returns true if a given entry should be removed.</param>
		/// <returns>Number of keys that were removed.</returns>
		public static int RemoveAllEntries<TKey, TValue>(
			this Dictionary<TKey, TValue> dictionary,
			Func<KeyValuePair<TKey, TValue>, bool> predicate
			)
		{
			TKey[] keysToDelete = dictionary.Where(predicate).Select(p => p.Key).ToArray();
			foreach (TKey key in keysToDelete)
			{
				dictionary.Remove(key);
			}
			return keysToDelete.Length;
		}
	}
}
