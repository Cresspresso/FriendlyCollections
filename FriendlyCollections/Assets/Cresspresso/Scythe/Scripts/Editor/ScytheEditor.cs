using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Cresspresso.Scythe.Editor
{
	[CustomEditor(typeof(ScytheScript)), CanEditMultipleObjects]
	public class ScytheEditor : UnityEditor.Editor
	{
		public override void OnInspectorGUI()
		{
			DrawDefaultInspector();
			if (GUILayout.Button("Generate Documentation"))
			{
				foreach (ScytheScript script in targets)
				{
					script.GenerateDocumentation();
				}
			}
		}

		[MenuItem("Assets/Create/ScytheScript")]
		public static void CreateScytheAsset()
		{
			ScytheScript scythe = ScriptableObject.CreateInstance<ScytheScript>();
			AssetDatabase.CreateAsset(scythe, "Assets/scythe.asset");
		}
	}
}
