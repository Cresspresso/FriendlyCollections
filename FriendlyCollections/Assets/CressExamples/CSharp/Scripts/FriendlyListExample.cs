using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cress;

namespace CressExamples
{
	/// <summary>
	/// Example script demonstrating how to implement and use <see cref="FriendlyList{T}"/>.
	/// </summary>
	public class FriendlyListExample : MonoBehaviour
	{
		// Inspector Properties

		[SerializeField]
		private StringList serializedList;

		[SerializeField]
		private MyListStruct group;

		[SerializeField]
		private StringList[] nested;
		

		// Script Properties

		public List<string> list { get { return serializedList.data; } }


		// Methods

		private void Start()
		{
			// Print out all items of the first list.
			Debug.Log("Count: " + list.Count);
			for (int i = 0; i < list.Count; ++i)
			{
				Debug.LogFormat("[{0}]: {1}", i, list[i]);
			}
		}
	}

	/// <summary>
	/// Example of implementing <see cref="FriendlyList{T]"/> for <see cref="string"/>.
	/// </summary>
	[Serializable]
	public class StringList : FriendlyList<string> { }

	/// <summary>
	/// A struct of several <see cref="StringList"/>s, for layout demonstration.
	/// </summary>
	[Serializable]
	public struct MyListStruct
	{
		public StringList alpha;
		public StringList beta;
		public StringList gamma;
	}
}
