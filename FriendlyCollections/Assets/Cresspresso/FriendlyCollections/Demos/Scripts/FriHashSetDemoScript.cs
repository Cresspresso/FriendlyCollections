namespace CresspressoDemos.FriendlyCollections
{
	using System;
	using System.Collections.Generic;
	using UnityEngine;
	using Cresspresso.FriendlyCollections;
	
	// Demo script for FriHashSet<T>.
	public class FriHashSetDemoScript : MonoBehaviour
	{
		// Inspector field for the hash set.
		[SerializeField]
		private StringHashSet friendlyHashSet;

		// Gets the deserialized hash set.
		public HashSet<string> hashSet { get { return friendlyHashSet.data; } }

		private void Start()
		{
			// Do something with the hash set.
			string msg = string.Format("Friendly Hash Set Count: {0}\n", hashSet.Count);
			foreach (var element in hashSet)
			{
				msg += element + "\n";
			}
			Debug.Log(msg, this);
		}
	}

	// Create serializable class for HashSet<string>.
	// Make sure it is not generic.
	[Serializable]
	public class StringHashSet : FriHashSet<string> { }
}
