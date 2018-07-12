using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cress;

namespace CressExamples
{
	public class FriendlyDictionaryExample : MonoBehaviour
	{
		// Friendly dictionary drawn in the inspector.
		public StringIntDict friendlyDictionary;

		public MyDictStruct group;



		private void Start()
		{
			// Alias the deserialized dictionary.
			Dictionary<string, int> dictionary = friendlyDictionary.data;

			// Print all entries.
			Debug.Log("Dictionary Count: " + dictionary.Count);
			foreach (var pair in dictionary)
			{
				Debug.LogFormat("{0}: {1}", pair.Key, pair.Value);
			}
		}
	}

	// Remove generic nature from the FriendlyDictionaryPair class.
	[Serializable]
	public class StringIntPair : FriendlyDictionaryPair<string, int> { }

	// Remove generic nature from the FriendlyDictionary class.
	[Serializable]
	public class StringIntDict : FriendlyDictionary<string, int, StringIntPair> { }

	// Struct to demonstrate that nested dictionaries are possible.
	[Serializable]
	public struct MyDictStruct
	{
		public StringIntDict alpha;
		public StringIntDict beta;
		public StringIntDict gamma;
	}
}
