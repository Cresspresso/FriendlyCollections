using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cress;

namespace CressDemos
{
	public class FriDictDemoScript1 : MonoBehaviour
	{
		// Fields

		[SerializeField]
		private FloatStringDictStruct group;

		[SerializeField]
		private FloatStringDict[] array;

		// Classes

		[Serializable]
		public class FloatStringDict : FriDict<float, string, FloatStringPair> { }

		[Serializable]
		public class FloatStringPair : FriDictPair<float, string> { }

		// Struct to demonstrate nesting.
		[Serializable]
		public struct FloatStringDictStruct
		{
			public FloatStringDict alpha;
			public FloatStringDict beta;
			public FloatStringDict gamma;
		}
	}
}
