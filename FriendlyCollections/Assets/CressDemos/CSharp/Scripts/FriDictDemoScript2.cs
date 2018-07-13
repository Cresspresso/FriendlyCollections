using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cress;

namespace CressDemos
{
	public class FriDictDemoScript2 : MonoBehaviour
	{
		// Private field, drawn in the inspector.
		[SerializeField]
		private ObjObjDict m_objectMap;

		// Public property exposing the deserialized data.
		public Dictionary<GameObject, Transform> objectMap { get { return m_objectMap.data; } }



		private void Start()
		{
			Debug.LogFormat(this, "Object Map Count: {0}", objectMap.Count);
			foreach (var pair in objectMap)
			{
				Debug.LogFormat(this, "{0}: {1}",
					pair.Key ? pair.Key.name : "null",
					pair.Value ? pair.Value.name : "null"
				);
			}
		}
	}
	
	// Friendly dictionary with GameObject as the key type, so we must use FriDictOfObjects.
	[Serializable]
	public class ObjObjDict : FriDictOfObjects<GameObject, Transform, ObjObjPair> { }

	[Serializable]
	public class ObjObjPair : FriDictPair<GameObject, Transform> { }
}
