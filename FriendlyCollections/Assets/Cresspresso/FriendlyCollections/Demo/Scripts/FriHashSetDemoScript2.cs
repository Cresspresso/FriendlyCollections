namespace CresspressoDemos.FriendlyCollections
{
	using System;
	using System.Collections.Generic;
	using UnityEngine;
	using Cresspresso.FriendlyCollections;

	// Demo script for FriHashSetOfObjects<T>.
	public class FriHashSetDemoScript2 : MonoBehaviour
	{
		[SerializeField]
		private ObjectHashSet objectSet;
		public HashSet<GameObject> hashSet { get { return objectSet.data; } }

		private void Start()
		{
			string msg = string.Format("Object Set Count: {0}\n", hashSet.Count);
			foreach (var element in hashSet)
			{
				msg += (element ? element.name : "null") + "\n";
			}
			Debug.Log(msg, this);
		}
	}

	// Demo custom property drawer was implemented for this class.
	[Serializable]
	public class ObjectHashSet : FriHashSet<GameObject> { }
}
