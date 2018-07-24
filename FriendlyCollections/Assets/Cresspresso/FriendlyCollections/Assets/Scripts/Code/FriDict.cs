namespace Cresspresso.FriendlyCollections
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using MoreLinq;
	using UnityEngine;
	using MTLinq = Extensions.MainThreadLinq;

	#region FriDictPair

	/// <summary>
	/// Base class for <see cref="FriDictPair{TKey, TValue}"/>, used by its default property drawer.
	/// </summary>
	[Serializable]
	public abstract class FriDictPair { }

	/// <summary>
	/// Serializable key-value pair for a <see cref="FriDict{TKey, TValue, TPair}"/>.
	/// </summary>
	/// <typeparam name="TKey">Dictionary key type.</typeparam>
	/// <typeparam name="TValue">Dictionary value type.</typeparam>
	[Serializable]
	public abstract class FriDictPair<TKey, TValue> : FriDictPair
	{
		public TKey key;
		public TValue value;
	}

	#endregion
	#region FriDict

	/// <summary>
	/// Base class for <see cref="FriDict{TKey, TValue, TPair}"/>, used by its default property drawer.
	/// </summary>
	[Serializable]
	public abstract class FriDict { }

	/// <summary>
	/// Serializable dictionary with a custom property drawer.
	/// </summary>
	/// <typeparam name="TKey">Dictionary key type.</typeparam>
	/// <typeparam name="TValue">Dictionary value type.</typeparam>
	/// <typeparam name="TPair">Serializable key/value pair class inheriting from <see cref="FriDictPair{TKey, TValue}"/>.</typeparam>
	[Serializable]
	public abstract class FriDict<TKey, TValue, TPair>
		: FriDict,
		ISerializationCallbackReceiver,
		IEqualityComparer<TKey>
		where TPair : FriDictPair<TKey, TValue>, new()
	{
		#region Fields

		/// <summary>
		/// Deserialized dictionary.
		/// </summary>
		public Dictionary<TKey, TValue> data { get; private set; }

		/// <summary>
		/// Serialized list of key/value pairs that form a dictionary.
		/// <para>Entries might not be unique.</para>
		/// </summary>
		[SerializeField]
		private List<TPair> serialized;

		#endregion
		#region IEqualityComparer Methods

		/// <summary>
		/// Gets the hash code of a key value.
		/// </summary>
		/// <param name="key">Key value.</param>
		/// <returns>Hash code.</returns>
		public virtual int GetHashCode(TKey key)
		{
			return key.GetHashCode();
		}
		
		/// <summary>
		/// Compares the equality of two key values.
		/// <para>Override this method to compare complex and custom types.</para>
		/// </summary>
		/// <param name="keyA">First key value.</param>
		/// <param name="keyB">Second key value.</param>
		/// <returns><see langword="true"/> if the keys are equal.</returns>
		public virtual bool Equals(TKey keyA, TKey keyB)
		{
			return object.Equals(keyA, keyB);
		}

		#endregion
		#region Methods

		/// <summary>
		/// Unity script event.
		/// (<a href="https://docs.unity3d.com/ScriptReference/ISerializationCallbackReceiver.OnBeforeSerialize.html">Link to Docs</a>)
		/// </summary>
		public void OnBeforeSerialize()
		{
			if (serialized == null)
				serialized = new List<TPair>();

			if (data == null)
			{
				data = new Dictionary<TKey, TValue>(this);
			}
			else
			{
				// Update entries.
				foreach (var pair in data)
				{
					int index = serialized.FindIndex(x => Equals(x.key, pair.Key));
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
				serialized.RemoveAll(p => !data.ContainsKey(p.key));
			}
		}

		/// <summary>
		/// Unity script event.
		/// (<a href="https://docs.unity3d.com/ScriptReference/ISerializationCallbackReceiver.OnAfterDeserialize.html">Link to Docs</a>)
		/// </summary>
		public void OnAfterDeserialize()
		{
			if (serialized == null)
			{
				serialized = new List<TPair>();

				if (data == null)
					data = new Dictionary<TKey, TValue>(this);
				else
					data.Clear();
			}
			else
			{
				// Deserialize.
				try
				{
					data = serialized
						.DistinctBy(p => p.key, this)
						.ToDictionary(p => p.key, p => p.value, this);
				}
				catch (Exception e1)
				{
					try
					{
						data = MTLinq.ToDictionary(serialized, p => p.key, p => p.value, this);
					}
					catch (Exception e2)
					{
						// Failed to deserialize.
						if (data == null)
							data = new Dictionary<TKey, TValue>(this);

						Debug.Log("Failed to deserialize dictionary.");
						Debug.LogException(e1);
						Debug.LogException(e2);
					}
				}
			}
		}

		#endregion
	}

	#endregion
}
