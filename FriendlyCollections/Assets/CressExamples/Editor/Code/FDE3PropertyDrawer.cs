using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Cress;
using CressEditor;
using CressExamples;

namespace CressEditorExamples
{
	public class FDE3PersistentData : PersistentDictionaryData
	{
		// Function used to compare two key properties. Used when drawing a friendly dictionary pair.
		protected override bool AreKeyProperitesEqual(SerializedProperty propA, SerializedProperty propB)
		{
			return propA.FindPropertyRelative("alpha").stringValue == propB.FindPropertyRelative("alpha").stringValue
				&& propA.FindPropertyRelative("beta").floatValue == propB.FindPropertyRelative("beta").floatValue;
		}
	}
	
	// Custom property drawer for a custom friendly dictionary using a custom persistent data class.
	[CustomPropertyDrawer(typeof(FDE3Dict))]
	public class FDE3DictPropertyDrawer : FriendlyListPropertyDrawerBase<FDE3PersistentData>
	{
		private static PersistenceTable<FDE3PersistentData> persistence = new PersistenceTable<FDE3PersistentData>();

		protected override FDE3PersistentData GetPersistentData(SerializedProperty property)
		{
			return persistence[property];
		}
	}
	
	// Custom property drawer for FDE3KeyStruct.
	[CustomPropertyDrawer(typeof(FDE3KeyStruct))]
	public class FDE3PairPropertyDrawer : PropertyDrawer
	{
		// Gets the height of this property drawer.
		public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
		{
			return EditorGUIUtility.singleLineHeight * 2;
		}

		// Draws this property.
		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
		{
			// Find relative properties.
			SerializedProperty propAlpha = property.FindPropertyRelative("alpha");
			SerializedProperty propBeta = property.FindPropertyRelative("beta");

			// Begin drawing the property.
			EditorGUI.BeginProperty(position, label, property);

			// Calculate rects.
			float lineHeight = EditorGUIUtility.singleLineHeight;
			float labelWidth = 60;

			Rect rectLabelAlpha = new Rect(position.x, position.y, labelWidth, lineHeight);
			Rect rectLabelBeta = new Rect(position.x, position.y + lineHeight, labelWidth, lineHeight);

			Rect rectAlpha = new Rect(rectLabelAlpha.x + rectLabelAlpha.width, rectLabelAlpha.y, position.width - labelWidth, lineHeight);
			Rect rectBeta = new Rect(rectLabelBeta.x + rectLabelBeta.width, rectLabelBeta.y, position.width - labelWidth, lineHeight);

			// Draw labels.
			EditorGUI.LabelField(rectLabelAlpha, propAlpha.displayName);
			EditorGUI.LabelField(rectLabelBeta, propBeta.displayName);

			// Draw value fields.
			EditorGUI.PropertyField(rectAlpha, propAlpha, GUIContent.none);
			EditorGUI.PropertyField(rectBeta, propBeta, GUIContent.none);

			// Finished drawing the property.
			EditorGUI.EndProperty();
		}
	}
}
