namespace CresspressoDemos.FriendlyCollections
{
	using System;
	using System.Collections.Generic;
	using UnityEngine;
	using Cresspresso.FriendlyCollections;

	public class FriDictDemoScript2 : MonoBehaviour
	{
		// Private field, drawn in the inspector.
		[SerializeField]
		private ObjObjDict m_objectMap;

		// Public property exposing the deserialized data.
		public Dictionary<GameObject, Transform> objectMap { get { return m_objectMap.data; } }



		private void Start()
		{
			// Print all entries.
			string msg = string.Format("Object Map Count: {0}\n", objectMap.Count);
			foreach (var pair in objectMap)
			{
				msg += string.Format("{0}: {1}\n",
					pair.Key ? pair.Key.name : "null",
					pair.Value ? pair.Value.name : "null"
				);
			}
			Debug.Log(msg, this);
		}
	}
	
	[Serializable]
	public class ObjObjDict : FriDict<GameObject, Transform, ObjObjPair> { }

	[Serializable]
	public class ObjObjPair : FriDictPair<GameObject, Transform> { }
}
