using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cress;

namespace CressExamples
{
	public class FriendlyDictionaryExample2 : MonoBehaviour
	{
		[SerializeField]
		private ObjObjDict m_objectMap;

		public Dictionary<GameObject, Transform> objectMap { get { return m_objectMap.data; } }

		private void Start()
		{
			Debug.Log("Dictionary Count: " + objectMap.Count);
			foreach (var pair in objectMap)
			{
				Debug.LogFormat("{0}: {1}",
					pair.Key ? pair.Key.name : "null",
					pair.Value ? pair.Value.name : "null"
				);
			}
		}
	}

	[Serializable]
	public class ObjObjPair : FriendlyDictionaryPair<GameObject, Transform> { }

	[Serializable]
	public class ObjObjDict : FriendlyDictionary<GameObject, Transform, ObjObjPair> { }
}
