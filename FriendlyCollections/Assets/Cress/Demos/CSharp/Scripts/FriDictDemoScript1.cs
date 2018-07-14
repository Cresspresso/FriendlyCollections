using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cress;

namespace CressDemos
{
	// Demo script.
	public class FriDictDemoScript1 : MonoBehaviour
	{
		// Friendly dictionary drawn in the inspector.
		public StringIntDict friendlyDictionary;

		public MyDictStruct group;



		private void Start()
		{
			// Alias the deserialized dictionary.
			Dictionary<string, int> dictionary = friendlyDictionary.data;

			// Print all entries.
			Debug.LogFormat(this, "Friendly Dictionary Count: {0}", dictionary.Count);
			foreach (var pair in dictionary)
			{
				Debug.LogFormat(this, "{0}: {1}", pair.Key, pair.Value);
			}
		}
	}

	// Serializable attribute to remove generic nature from the FriDictPair class.
	[Serializable]
	public class StringIntPair : FriDictPair<string, int> { }

	// Serializable attribut to remove generic nature from the FriDict class.
	[Serializable]
	public class StringIntDict : FriDict<string, int, StringIntPair> { }

	// Struct to demonstrate that nested dictionaries are possible.
	[Serializable]
	public struct MyDictStruct
	{
		public StringIntDict alpha;
		public StringIntDict beta;
		public StringIntDict gamma;
	}
}
