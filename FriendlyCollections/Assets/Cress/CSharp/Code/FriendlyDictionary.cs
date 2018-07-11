using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Cress
{
	[Serializable]
	public abstract class FriendlyDictionaryPair { }

	[Serializable]
	public abstract class FriendlyDictionaryPair<TKey, TValue> : FriendlyDictionaryPair
	{
		public TKey key;
		public TValue value;
	}

	[Serializable]
	public abstract class FriendlyDictionary { }

	[Serializable]
	public abstract class FriendlyDictionary<TKey, TValue, TPair> : FriendlyDictionary, ISerializationCallbackReceiver where TPair : FriendlyDictionaryPair<TKey, TValue>, new()
	{
		public Dictionary<TKey, TValue> data = new Dictionary<TKey, TValue>();

		[SerializeField]
		private List<TPair> serialized = new List<TPair>();

		/// <summary>
		/// Ensures fields are not <see langword="null"/>.
		/// </summary>
		private void Prepare()
		{
			if (data == null)
				data = new Dictionary<TKey, TValue>();

			if (serialized == null)
				serialized = new List<TPair>();
		}

		/// <summary>
		/// Unity script event. (<a href="https://docs.unity3d.com/ScriptReference/ISerializationCallbackReceiver.OnBeforeSerialize.html">Link to Docs</a>)
		/// </summary>
		public void OnBeforeSerialize()
		{
			Prepare();

			// Update entries.
			foreach (var pair in data)
			{
				int index = serialized.FindIndex(x => object.Equals(x.key, pair.Key));
				if (index >= 0)
				{
					// Modify existing entry.
					serialized[index].value = pair.Value;
				}
				else
				{
					// Add new entry.
					var p = new TPair();
					p.key = pair.Key;
					p.value = pair.Value;
					serialized.Add(p);
				}
			}

			// Remove entries that no longer exist.
			int i = 0;
			while (i < serialized.Count)
			{
				if (data.ContainsKey(serialized[i].key))
				{
					++i;
				}
				else
				{
					serialized.RemoveAt(i);
				}
			}
		}

		/// <summary>
		/// Unity script event. (<a href="https://docs.unity3d.com/ScriptReference/ISerializationCallbackReceiver.OnAfterDeserialize.html">Link to Docs</a>)
		/// </summary>
		public void OnAfterDeserialize()
		{
			Prepare();

			// Remove entries that no longer exist.
			List<TKey> keysToDelete = new List<TKey>();
			foreach (var pair in data)
			{
				if (!serialized.Exists(x => object.Equals(x.key, pair.Key)))
				{
					keysToDelete.Add(pair.Key);
				}
			}
			foreach (var key in keysToDelete)
			{
				data.Remove(key);
			}

			// Update entries by adding or modifying them.
			foreach (TPair pair in serialized)
			{
				data[pair.key] = pair.value;
			}
		}
	}
}
