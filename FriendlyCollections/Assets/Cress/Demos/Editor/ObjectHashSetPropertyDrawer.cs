using UnityEditor;
using CressEditor;
using CressDemos;

namespace CressDemosEditor
{
	// Friendly property drawer for a FriHashSetOfObjects class.
	[CustomPropertyDrawer(typeof(ObjectHashSet))]
	public class ObjectHashSetPropertyDrawer : FriHashSetOfObjectsPropertyDrawer { }
}
