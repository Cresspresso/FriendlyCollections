namespace CresspressoDemos.FriendlyCollections
{
	using System;
	using System.Collections.Generic;
	using UnityEngine;
	using Cresspresso.FriendlyCollections;

	// Demo script for FriDict.
	public class FriDictDemoScript : MonoBehaviour
	{
		// Inspector field for the dictionary.
		[SerializeField]
		private StringIntDict friendlyDictionary;

		// Gets the deserialized dictionary.
		public Dictionary<string, int> dictionary { get { return friendlyDictionary.data; } }

		private void Start()
		{
			// Do something with the dictionary.
			string msg = string.Format("Friendly Dictionary Count: {0}\n", dictionary.Count);
			foreach (var pair in dictionary)
			{
				msg += pair.ToString() + "\n";
			}
			Debug.Log(msg, this);
		}
	}

	// Create serializable classes for serializing Dictionary<string, int>.
	// Make sure they are not generic.
	[Serializable]
	public class StringIntDict : FriDict<string, int, StringIntPair> { }

	[Serializable]
	public class StringIntPair : FriDictPair<string, int> { }
}
