namespace CresspressoDemos.FriendlyCollections.Editors
{
	using UnityEditor;
	using Cresspresso.FriendlyCollections.Editors;

	// Friendly property drawer for a Dictionary<UnityEngine.Object, TValue> class.
	[CustomPropertyDrawer(typeof(ObjObjDict))]
	public class ObjObjDictPropertyDrawer : FriDictOfObjectsPropertyDrawer { }
}
