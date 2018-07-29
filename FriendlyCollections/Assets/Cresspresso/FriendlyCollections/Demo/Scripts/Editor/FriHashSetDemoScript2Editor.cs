namespace CresspressoDemosEditor.FriendlyCollections
{
	using UnityEditor;
	using CresspressoEditor.FriendlyCollections;
	using CresspressoDemos.FriendlyCollections;

	// Friendly property drawer for a FriHashSet<UnityEngine.Object> class.
	[CustomPropertyDrawer(typeof(ObjectHashSet))]
	public class ObjectHashSetPropertyDrawer : FriHashSetOfObjectsPropertyDrawer { }
}
