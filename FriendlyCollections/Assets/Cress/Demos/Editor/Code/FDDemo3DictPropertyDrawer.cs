using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Cress;
using CressEditor;
using CressDemos;

namespace CressDemosEditor
{
	// Persistent data with overridden key equality method.
	public class FDDemo3DictPData : FriDictPData
	{
		// Function which returns a label to use in the header of the reorderable list.
		protected override GUIContent headerLabel { get { return new GUIContent(property.displayName, "Custom Header Tooltip"); } }

		// Function which compares the equality of two key properties.
		// Used when drawing a friendly dictionary pair.
		protected override bool AreKeyProperitesEqual(SerializedProperty propA, SerializedProperty propB)
		{
			return propA.FindPropertyRelative("alpha").stringValue == propB.FindPropertyRelative("alpha").stringValue
				&& propA.FindPropertyRelative("beta").floatValue == propB.FindPropertyRelative("beta").floatValue;
		}
	}
	
	// Custom property drawer for a custom friendly dictionary using a custom persistent data class.
	[CustomPropertyDrawer(typeof(FDDemo3Dict))]
	public class FDDemo3DictPropertyDrawer : ReorderableListPropertyDrawer<FDDemo3DictPData>
	{
		private static PersistenceTable<FDDemo3DictPData> persistence = new PersistenceTable<FDDemo3DictPData>();

		protected override FDDemo3DictPData GetPersistentData(SerializedProperty property)
		{
			return persistence[property];
		}
	}
	
	// Custom property drawer for FDE3KeyStruct.
	[CustomPropertyDrawer(typeof(FDDemo3KeyStruct))]
	public class FDDemo3KeyStructPropertyDrawer : PropertyDrawer
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

			Rect rectLabelAlpha = new Rect(
				position.x,
				position.y,
				labelWidth,
				lineHeight);

			Rect rectLabelBeta = new Rect(
				position.x,
				position.y + lineHeight,
				labelWidth,
				lineHeight);

			Rect rectAlpha = new Rect(
				rectLabelAlpha.x + rectLabelAlpha.width,
				rectLabelAlpha.y,
				position.width - labelWidth,
				lineHeight);

			Rect rectBeta = new Rect(
				rectLabelBeta.x + rectLabelBeta.width,
				rectLabelBeta.y,
				position.width - labelWidth,
				lineHeight);

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
