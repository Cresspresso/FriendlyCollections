namespace CresspressoDemos.FriendlyCollections.Editor
{
	using UnityEditor;
	using Cresspresso.FriendlyCollections.Editor;

	// Friendly property drawer for a Dictionary<UnityEngine.Object, TValue> class.
	[CustomPropertyDrawer(typeof(ObjObjDict))]
	public class ObjObjDictPropertyDrawer : FriDictOfObjectsPropertyDrawer { }
}
