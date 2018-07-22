namespace CresspressoDemos.FriendlyCollections.Editor
{
	using UnityEditor;
	using Cresspresso.FriendlyCollections.Editor;

	// Friendly property drawer for a FriHashSet<UnityEngine.Object> class.
	[CustomPropertyDrawer(typeof(ObjectHashSet))]
	public class ObjectHashSetPropertyDrawer : FriHashSetOfObjectsPropertyDrawer { }
}
