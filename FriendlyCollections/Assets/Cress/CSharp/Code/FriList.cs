using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Cress
{
	/// <summary>
	/// Base class for <see cref="FriList{T}"/>, used by its custom property drawer.
	/// </summary>
	[Serializable]
	public abstract class FriList { }

	/// <summary>
	/// <see cref="List{T}"/> wrapper class styled as a reorderable list in the inspector.
	/// </summary>
	/// <typeparam name="T">List element type.</typeparam>
	[Serializable]
	public abstract class FriList<T> : FriList
	{
		public List<T> data;
	}
}
