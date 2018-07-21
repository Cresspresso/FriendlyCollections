using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cress;

namespace CressDemos
{
	public class FriListDemoScript2 : MonoBehaviour
	{
		// Fields

		[SerializeField]
		private ObjectListStruct group;

		[SerializeField]
		private ObjectList[] array;

		// Classes
		
		[Serializable]
		public class ObjectList : FriList<GameObject> { }

		// Struct to demonstrate nesting friendly lists is possible.
		[Serializable]
		public struct ObjectListStruct
		{
			public ObjectList alpha;
			public ObjectList beta;
			public ObjectList gamma;
		}
	}
}
