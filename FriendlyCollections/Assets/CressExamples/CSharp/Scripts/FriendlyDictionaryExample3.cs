using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cress;

namespace CressExamples
{
	public class FriendlyDictionaryExample3 : MonoBehaviour
	{
		[SerializeField]
		private FDE3Dict m_dictionary;

		public Dictionary<FDE3KeyStruct, float> dictionary { get { return m_dictionary.data; } }

		private void Start()
		{
			Debug.Log("Dictionary Count: " + dictionary.Count);
			foreach (var pair in dictionary)
			{
				Debug.LogFormat("{0}: {1}", pair.Key, pair.Value);
			}
		}
	}

	// Complex type for a dictionary key.
	[Serializable]
	public struct FDE3KeyStruct
	{
		public string alpha;
		public float beta;

		public override string ToString()
		{
			return string.Format("({0}, {1})", alpha, beta);
		}
	}

	[Serializable]
	public class FDE3Pair : FriendlyDictionaryPair<FDE3KeyStruct, float> { }

	[Serializable]
	public class FDE3Dict : FriendlyDictionary<FDE3KeyStruct, float, FDE3Pair> { }
}
