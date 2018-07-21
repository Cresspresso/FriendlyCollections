using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using MoreLinq;
using MTLinq = Cress.MainThreadLinq.MainThreadLinq;

namespace Cress
{
	#region FriHashSet

	/// <summary>
	/// Base class for <see cref="FriHashSet{T}"/>, used by its default property drawer.
	/// </summary>
	[Serializable]
	public abstract class FriHashSet { }

	/// <summary>
	/// Serializable hash set with a custom property drawer.
	/// </summary>
	/// <typeparam name="T">Element type.</typeparam>
	[Serializable]
	public abstract class FriHashSet<T>
		: FriHashSet,
		ISerializationCallbackReceiver,
		IEqualityComparer<T>
	{
		#region Fields

		/// <summary>
		/// Deserialized hash set.
		/// </summary>
		public HashSet<T> data { get; private set; }

		/// <summary>
		/// Serialized list of elements that form a hash set.
		/// <para>Entries might not be unique.</para>
		/// </summary>
		[SerializeField]
		private List<T> serialized;

		#endregion
		#region IEqualityComparer Methods

		/// <summary>
		/// Gets the hash code of an element value.
		/// </summary>
		/// <param name="element">Element value.</param>
		/// <returns>Hash code.</returns>
		public virtual int GetHashCode(T element)
		{
			return element.GetHashCode();
		}

		/// <summary>
		/// Compares the equality of two element values.
		/// <para>Override this method to compare complex and custom types.</para>
		/// </summary>
		/// <param name="elementA">First element value.</param>
		/// <param name="elementB">Second element value.</param>
		/// <returns><see langword="true"/> if the elements are equal.</returns>
		public virtual bool Equals(T elementA, T elementB)
		{
			return object.Equals(elementA, elementB);
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
				serialized = new List<T>();

			if (data == null)
			{
				data = new HashSet<T>(this);
			}
			else
			{
				// Update entries.
				foreach (var val in data)
				{
					int index = serialized.FindIndex(x => Equals(x, val));
					if (index >= 0)
					{
						// Modify existing entry.
						serialized[index] = val;
					}
					else
					{
						// Add new entry.
						serialized.Add(val);
					}
				}

				// Remove entries that no longer exist.
				serialized.RemoveAll(x => !data.Contains(x));
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
				serialized = new List<T>();

				if (data == null)
					data = new HashSet<T>(this);
				else
					data.Clear();
			}
			else
			{
				// Deserialize.
				try
				{
					data = serialized
						.Distinct(this)
						.ToHashSet(this);
				}
				catch (Exception e1)
				{
					try
					{
						data = MTLinq.ToHashSet(serialized, this);
					}
					catch (Exception e2)
					{
						// Failed to deserialize.

						if (data == null)
							data = new HashSet<T>(this);

						Debug.Log("Failed to deserialize hash set.");
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
