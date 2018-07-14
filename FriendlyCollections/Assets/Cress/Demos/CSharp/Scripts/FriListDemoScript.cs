using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cress;

namespace CressDemos
{
	// Example script demonstrating how to implement and use FriendlyList.
	public class FriListDemoScript : MonoBehaviour
	{
		// Private fields displayed in the inspector.

		[SerializeField]
		private StringList serializedList;

		[SerializeField]
		private MyListStruct group;

		[SerializeField]
		private StringList[] nested;
		

		// Public property exposing the deserialized data of the first list.
		public List<string> list { get { return serializedList.data; } }
		


		private void Start()
		{
			// Print all elements of the first list.
			string msg = string.Format("Serialized List Count: {0}\n", list.Count);
			for (int i = 0; i < list.Count; ++i)
			{
				msg += string.Format("[Element {0}: {1}]\n", i, list[i]);
			}
			Debug.Log(msg, this);
		}
	}

	// Implementing FriList for string elements.
	[Serializable]
	public class StringList : FriList<string> { }
	
	// Struct to demonstrate nesting FriendlyLists is possible.
	[Serializable]
	public struct MyListStruct
	{
		public StringList alpha;
		public StringList beta;
		public StringList gamma;
	}
}
