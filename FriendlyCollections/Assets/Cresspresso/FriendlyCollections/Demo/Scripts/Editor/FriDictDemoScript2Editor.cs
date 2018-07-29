namespace CresspressoDemosEditor.FriendlyCollections
{
	using UnityEditor;
	using CresspressoEditor.FriendlyCollections;
	using CresspressoDemos.FriendlyCollections;

	// Friendly property drawer for a Dictionary<UnityEngine.Object, TValue> class.
	[CustomPropertyDrawer(typeof(ObjObjDict))]
	public class ObjObjDictPropertyDrawer : FriDictOfObjectsPropertyDrawer { }
}
