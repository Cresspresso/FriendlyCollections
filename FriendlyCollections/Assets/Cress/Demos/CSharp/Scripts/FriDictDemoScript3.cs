using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cress;

namespace CressDemos
{
	public class FriDictDemoScript3 : MonoBehaviour
	{
		[SerializeField]
		private FDDemo3Dict m_dictionary;

		public Dictionary<FDDemo3KeyStruct, float> dictionary { get { return m_dictionary.data; } }

		private void Start()
		{
			Debug.LogFormat(this, "Dictionary Count: {0}", dictionary.Count);
			foreach (var pair in dictionary)
			{
				Debug.LogFormat(this, "{0}: {1}", pair.Key, pair.Value);
			}
		}
	}

	[Serializable]
	public class FDDemo3Dict : FriDict<FDDemo3KeyStruct, float, FDDemo3Pair> { }

	[Serializable]
	public class FDDemo3Pair : FriDictPair<FDDemo3KeyStruct, float> { }

	// Complex type for a dictionary key.
	[Serializable]
	public struct FDDemo3KeyStruct
	{
		public string alpha;
		public float beta;

		public override string ToString()
		{
			return string.Format("({0}, {1})", alpha, beta);
		}

		// Required for equality check in FriDict.
		// Returns true if fields are equal (alpha == alpha and beta == beta).
		public override bool Equals(object obj)
		{
			if (obj is FDDemo3KeyStruct)
			{
				return Equals((FDDemo3KeyStruct)obj);
			}
			return false;
		}
		public bool Equals(FDDemo3KeyStruct obj)
		{
			return alpha == obj.alpha && beta == obj.beta;
		}

		// (Since we override Equals, we should also override GetHashCode.)
		public override int GetHashCode()
		{
			unchecked
			{
				int hash = 7;
				hash = hash * 11 + alpha.GetHashCode();
				hash = hash * 41 + beta.GetHashCode();
				return hash;
			}
		}
	}
}
