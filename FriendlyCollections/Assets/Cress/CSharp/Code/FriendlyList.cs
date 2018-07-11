using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Cress
{
	[Serializable]
	public abstract class FriendlyList { }

	[Serializable]
	public abstract class FriendlyList<T> : FriendlyList
	{
		public List<T> data;
	}
}
