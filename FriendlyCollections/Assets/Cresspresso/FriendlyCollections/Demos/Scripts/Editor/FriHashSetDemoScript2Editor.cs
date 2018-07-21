namespace CresspressoDemos.FriendlyCollections.Editors
{
	using UnityEditor;
	using Cresspresso.FriendlyCollections.Editors;

	// Friendly property drawer for a FriHashSet<UnityEngine.Object> class.
	[CustomPropertyDrawer(typeof(ObjectHashSet))]
	public class ObjectHashSetPropertyDrawer : FriHashSetOfObjectsPropertyDrawer { }
}
