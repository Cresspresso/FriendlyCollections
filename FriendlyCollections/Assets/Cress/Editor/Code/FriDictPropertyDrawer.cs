using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Cress;

namespace CressEditor
{
	#region FriDictPData

	/// <summary>
	/// <see cref="PersistentData"/> for <see cref="FriDictPropertyDrawer"/>.
	/// </summary>
	public class FriDictPData : ReorderableListPData
	{
		#region Constants

		/// <summary>
		/// Property name of <see cref="FriDict{TKey, TValue, TPair}.serialized"/>.
		/// </summary>
		protected override string fieldPathData { get { return "serialized"; } }

		/// <summary>
		/// Property name of <see cref="FriDictPair{TKey, TValue}.key"/>.
		/// </summary>
		private const string fieldPathPairKey = "key";

		/// <summary>
		/// Property name of <see cref="FriDictPair{TKey, TValue}.value"/>.
		/// </summary>
		private const string fieldPathPairValue = "value";

		#endregion
		#region Methods

		/// <summary>
		/// Overridden method <see cref="ReorderableListPData{TDataArgs}.OnInitReorderableList"/>.
		/// </summary>
		protected override void InitReorderableList()
		{
			base.InitReorderableList();

			// Element Height
			reorderableList.elementHeightCallback = (index) =>
			{
				if (reorderableList.count == 0)
					return noneElementHeight;

				SerializedProperty propPair = propData.GetArrayElementAtIndex(index);
				SerializedProperty propKey = propPair.FindPropertyRelative(fieldPathPairKey);
				SerializedProperty propValue = propPair.FindPropertyRelative(fieldPathPairValue);

				return Mathf.Max(
					EditorGUI.GetPropertyHeight(propKey, GUIContent.none, true),
					EditorGUI.GetPropertyHeight(propValue, GUIContent.none, true)
				) + elementSpacing;
			};

			// Draw Element
			reorderableList.drawElementCallback = (position, index, a, b) =>
			{
				position = GetElementInnerRect(position);

				SerializedProperty propPair = propData.GetArrayElementAtIndex(index);
				SerializedProperty propKey = propPair.FindPropertyRelative(fieldPathPairKey);
				SerializedProperty propValue = propPair.FindPropertyRelative(fieldPathPairValue);

				float half = position.width / 2;
				float x;

				Rect rectKeyLabel = new Rect(position.x, position.y, 26, position.height);
				x = rectKeyLabel.width + 2;
				Rect rectKeyField = new Rect(rectKeyLabel.x + x, position.y, half - x - 2, position.height);
				
				Rect rectValueLabel = new Rect(position.x + half, position.y, 38, position.height);
				x = rectValueLabel.width + 2;
				Rect rectValueField = new Rect(rectValueLabel.x + x, position.y, half - x, position.height);
				
				bool duplicate = CountKey(index, 0, index - 1) > 0;
				if (duplicate)
				{
					GUIStyle style = new GUIStyle(EditorStyles.label);
					style.normal.textColor = Color.red;

					EditorGUI.LabelField(rectKeyLabel, new GUIContent(propKey.displayName, "Duplicate key."), style);
					EditorGUI.LabelField(rectValueLabel, new GUIContent(propValue.displayName, "Duplicate key."), style);
				}
				else
				{
					EditorGUI.LabelField(rectKeyLabel, propKey.displayName);
					EditorGUI.LabelField(rectValueLabel, propValue.displayName);
				}
				
				EditorGUI.PropertyField(rectKeyField, propKey, GUIContent.none, true);
				EditorGUI.PropertyField(rectValueField, propValue, GUIContent.none, true);
			};

			// Draw None Element
			reorderableList.drawNoneElementCallback = (position) =>
			{
				EditorGUI.LabelField(position, "Dictionary is Empty");
			};
		}

		/// <summary>
		/// Compares the equality of two key properties.
		/// <para>Override this method to compare complex and custom types.</para>
		/// </summary>
		/// <param name="propA">First key property.</param>
		/// <param name="propB">Second key property.</param>
		/// <returns><see langword="true"/> if the keys are equal.</returns>
		protected virtual bool AreKeyProperitesEqual(SerializedProperty propA, SerializedProperty propB)
		{
			try
			{
				return object.Equals(propA.GetPropertyValue(), propB.GetPropertyValue());
			}
			catch (Exception e)
			{
				Debug.LogError("Friendly dictionary key type is not a primitive type. Try overriding AreKeyPropertiesEqual.");
				Debug.LogException(e);
				return false;
			}
		}

		/// <summary>
		/// Counts the number of times a certain key occurs in the serialized list.
		/// </summary>
		/// <param name="index">Index of the pair whose key we are comparing with.</param>
		/// <param name="startIndex">Starting index to search from.</param>
		/// <param name="endIndex">Index after which searching will stop.</param>
		/// <returns>The number of times the key occurs.</returns>
		private int CountKey(int index, int startIndex = 0, int endIndex = int.MaxValue)
		{
			if (reorderableList.count <= 0)
				return 0;

			SerializedProperty propKey = propData.GetArrayElementAtIndex(index).FindPropertyRelative(fieldPathPairKey);
			return CountKey(propKey, startIndex, endIndex);
		}

		/// <summary>
		/// Counts the number of times a certain key occurs in the serialized list.
		/// </summary>
		/// <param name="propKey">Key property to compare with.</param>
		/// <param name="startIndex">Starting index to search from.</param>
		/// <param name="endIndex">Index after which searching will stop.</param>
		/// <returns>The number of times the key occurs.</returns>
		private int CountKey(SerializedProperty propKey, int startIndex = 0, int endIndex = int.MaxValue)
		{
			int count = 0;
			int arraySize = propData.arraySize;
			for (int i = startIndex; i < arraySize && i <= endIndex; ++i)
			{
				SerializedProperty otherKey = propData.GetArrayElementAtIndex(i).FindPropertyRelative(fieldPathPairKey);
				if (AreKeyProperitesEqual(propKey, otherKey))
				{
					++count;
				}
			}
			return count;
		}

		#endregion
	}

	#endregion
	#region FriDictPropertyDrawer

	/// <summary>
	/// <see cref="CustomPropertyDrawer"/> for <see cref="FriDict"/>.
	/// </summary>
	[CustomPropertyDrawer(typeof(FriDict), true)]
	public class FriDictPropertyDrawer : ReorderableListPropertyDrawer<FriDictPData>
	{
		#region Persistence

		/// <summary>
		/// Table of persistent data associated with properties.
		/// </summary>
		private static PersistenceTable<FriDictPData> persistence = new PersistenceTable<FriDictPData>();

		/// <summary>
		/// Gets persistent data associated with a property.
		/// </summary>
		/// <param name="property">The current property being drawn.</param>
		protected override FriDictPData GetPersistentData(SerializedProperty property)
		{
			return persistence[property];
		}

		#endregion
	}

	#endregion
	#region FriDictOfObjectsPData

	/// <summary>
	/// <see cref="PersistentData"/> for <see cref="FriDictOfObjectsPropertyDrawer"/>.
	/// </summary>
	public class FriDictOfObjectsPData : FriDictPData
	{
		protected override bool AreKeyProperitesEqual(SerializedProperty propA, SerializedProperty propB)
		{
			return propA.objectReferenceInstanceIDValue == propB.objectReferenceInstanceIDValue;
		}
	}

	#endregion
	#region FriDictOfObjectsPropertyDrawer
	
	/// <summary>
	/// Friendly property drawer for <see cref="FriDictOfObjects{TKey, TValue, TPair}"/>.
	/// <para>Must be instantiated with the <see cref="CustomPropertyDrawer"/> attribute.</para>
	/// </summary>
	public class FriDictOfObjectsPropertyDrawer : ReorderableListPropertyDrawer<FriDictOfObjectsPData>
	{
		#region Persistence

		/// <summary>
		/// Table of persistent data associated with properties.
		/// </summary>
		private static PersistenceTable<FriDictOfObjectsPData> persistence = new PersistenceTable<FriDictOfObjectsPData>();

		/// <summary>
		/// Gets persistent data associated with a property.
		/// </summary>
		/// <param name="property">The current property being drawn.</param>
		protected override FriDictOfObjectsPData GetPersistentData(SerializedProperty property)
		{
			return persistence[property];
		}

		#endregion
	}

	#endregion
}
