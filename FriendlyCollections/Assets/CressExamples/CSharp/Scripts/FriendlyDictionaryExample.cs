using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cress;

namespace CressExamples
{
	public class FriendlyDictionaryExample : MonoBehaviour
	{
		public StringIntDict friendlyDictionary;
		public Dictionary<string, int> dictionary { get { return friendlyDictionary.data; } }

		public MyDictStruct group;

		private void Start()
		{
			Debug.Log("Dictionary Count: " + dictionary.Count);
			foreach (var pair in dictionary)
			{
				Debug.LogFormat("{0}: {1}", pair.Key, pair.Value);
			}
		}
	}

	[Serializable]
	public class StringIntPair : FriendlyDictionaryPair<string, int> { }

	[Serializable]
	public class StringIntDict : FriendlyDictionary<string, int, StringIntPair> { }

	[Serializable]
	public struct MyDictStruct
	{
		public StringIntDict alpha;
		public StringIntDict beta;
		public StringIntDict gamma;
	}
}
