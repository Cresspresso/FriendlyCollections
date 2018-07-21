using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Cress;
using CressEditor;
using CressDemos;

namespace CressDemosEditor
{
	// Friendly property drawer for a FriDictOfObjects class.
	[CustomPropertyDrawer(typeof(ObjObjDict))]
	public class ObjObjDictPropertyDrawer : FriDictOfObjectsPropertyDrawer { }
}
