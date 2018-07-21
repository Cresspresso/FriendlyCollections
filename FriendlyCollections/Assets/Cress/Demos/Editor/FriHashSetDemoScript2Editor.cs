using UnityEditor;
using CressEditor;
using CressDemos;

namespace CressDemosEditor
{
	// Friendly property drawer for a FriHashSet<UnityEngine.Object> class.
	[CustomPropertyDrawer(typeof(ObjectHashSet))]
	public class ObjectHashSetPropertyDrawer : FriHashSetOfObjectsPropertyDrawer { }
}
