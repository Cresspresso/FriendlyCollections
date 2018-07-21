namespace CresspressoDemos.FriendlyCollections
{
	using System;
	using System.Collections.Generic;
	using UnityEngine;
	using Cresspresso.FriendlyCollections;

	// Demo script for FriList<T>.
	public class FriListDemoScript : MonoBehaviour
	{
		// Inspector field for the reorderable list.
		[SerializeField]
		private StringList friendlyList;

		// Gets the deserialized list.
		public List<string> list { get { return friendlyList.data; } }

		private void Start()
		{
			// Do something with the list.
			string msg = string.Format("Friendly List Count: {0}\n", list.Count);
			foreach (var element in list)
			{
				msg += element + "\n";
			}
			Debug.Log(msg, this);
		}
	}

	// Create serializable class for List<string>.
	// Make sure it is not generic.
	[Serializable]
	public class StringList : FriList<string> { }
}
