using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Cress;
using CressEditor.Extensions;

namespace CressEditor
{
	#region FriHashSetPData

	/// <summary>
	/// <see cref="PersistentData"/> for <see cref="FriHashSetPropertyDrawer"/>.
	/// </summary>
	public class FriHashSetPData : FriendlyCollectionPData
	{
		#region Constants

		/// <summary>
		/// Property name of <see cref="FriHashSet{T}.serialized"/>.
		/// </summary>
		protected override string fieldPathData { get { return "serialized"; } }

		/// <summary>
		/// Type name of collection.
		/// </summary>
		protected override string noneElementCollectionName { get { return "Hash Set"; } }

		#endregion
		#region Methods

		/// <summary>
		/// Overridden method <see cref="FriendlyCollectionPData{TDataArgs}.OnInitReorderableList"/>.
		/// </summary>
		protected override void InitReorderableList()
		{
			base.InitReorderableList();

			// Draw Element
			reorderableList.drawElementCallback = (position, index, a, b) =>
			{
				position = GetElementInnerRect(position);

				SerializedProperty propElement = propData.GetArrayElementAtIndex(index);

				GUIContent label = new GUIContent("Element " + index.ToString());

				bool duplicate = CountElement(index, 0, index - 1) > 0;
				if (duplicate && !serializedObject.isEditingMultipleObjects)
				{
					GUIStyle style = new GUIStyle(EditorStyles.label);
					style.normal.textColor = Color.red;

					label.tooltip = "Duplicate element.";
					position = EditorGUI.PrefixLabel(position, label, style);
				}
				else
				{
					position = EditorGUI.PrefixLabel(position, label);
				}

				EditorGUI.PropertyField(position, propElement, GUIContent.none, true);
			};
		}

		/// <summary>
		/// Compares the equality of two element properties.
		/// <para>Override this method to compare complex and custom types.</para>
		/// </summary>
		/// <param name="propA">First element property.</param>
		/// <param name="propB">Second element property.</param>
		/// <returns><see langword="true"/> if the elements are equal.</returns>
		protected virtual bool Equals(SerializedProperty propA, SerializedProperty propB)
		{
			try
			{
				return object.Equals(propA.GetPropertyValue(), propB.GetPropertyValue());
			}
			catch (Exception e)
			{
				Debug.LogError("Friendly hash set element type is not a primitive type. Try overriding property drawer Equals.");
				Debug.LogException(e);
				return false;
			}
		}

		/// <summary>
		/// Counts the number of times a certain element occurs in the serialized list.
		/// </summary>
		/// <param name="index">Index of the pair whose element we are comparing with.</param>
		/// <param name="startIndex">Starting index to search from.</param>
		/// <param name="endIndex">Index after which searching will stop.</param>
		/// <returns>The number of times the element occurs.</returns>
		private int CountElement(int index, int startIndex = 0, int endIndex = int.MaxValue)
		{
			if (reorderableList.count <= 0)
				return 0;

			SerializedProperty propElement = propData.GetArrayElementAtIndex(index);
			return CountElement(propElement, startIndex, endIndex);
		}

		/// <summary>
		/// Counts the number of times a certain key occurs in the serialized list.
		/// </summary>
		/// <param name="propKey">Key property to compare with.</param>
		/// <param name="startIndex">Starting index to search from.</param>
		/// <param name="endIndex">Index after which searching will stop.</param>
		/// <returns>The number of times the key occurs.</returns>
		private int CountElement(SerializedProperty propElement, int startIndex = 0, int endIndex = int.MaxValue)
		{
			int count = 0;
			int arraySize = propData.arraySize;
			for (int i = startIndex; i < arraySize && i <= endIndex; ++i)
			{
				SerializedProperty otherElement = propData.GetArrayElementAtIndex(i);
				if (Equals(propElement, otherElement))
				{
					++count;
				}
			}
			return count;
		}

		#endregion
	}

	#endregion
	#region FriHashSetPropertyDrawer

	/// <summary>
	/// <see cref="CustomPropertyDrawer"/> for <see cref="FriHashSet"/>.
	/// </summary>
	[CustomPropertyDrawer(typeof(FriHashSet), true)]
	public class FriHashSetPropertyDrawer : FriendlyCollectionPropertyDrawer<FriHashSetPData>
	{
		#region Persistence

		/// <summary>
		/// Table of persistent data associated with properties.
		/// </summary>
		private static PersistenceTable<FriHashSetPData> persistence = new PersistenceTable<FriHashSetPData>();

		/// <summary>
		/// Gets persistent data associated with a property.
		/// </summary>
		/// <param name="property">The current property being drawn.</param>
		protected override FriHashSetPData GetPersistentData(SerializedProperty property)
		{
			return persistence[property];
		}

		#endregion
	}

	#endregion
	#region FriHashSetOfObjectsPData

	/// <summary>
	/// <see cref="PersistentData"/> for <see cref="FriHashSetOfObjectsPropertyDrawer"/>.
	/// </summary>
	public class FriHashSetOfObjectsPData : FriHashSetPData
	{
		/// <summary>
		/// Compares the equality of two element properties (where element type inherits <see cref="UnityEngine.Object"/>).
		/// <para>Override this method to compare complex and custom types.</para>
		/// </summary>
		/// <param name="propA">First element property.</param>
		/// <param name="propB">Second element property.</param>
		/// <returns><see langword="true"/> if the elements are equal.</returns>
		protected override bool Equals(SerializedProperty propA, SerializedProperty propB)
		{
			return propA.objectReferenceInstanceIDValue == propB.objectReferenceInstanceIDValue;
		}
	}

	#endregion
	#region FriHashSetOfObjectsPropertyDrawer

	/// <summary>
	/// Friendly property drawer for <see cref="FriHashSet{T}"/> of <see cref="UnityEngine.Object"/>.
	/// <para>Must be instantiated with the <see cref="CustomPropertyDrawer"/> attribute.</para>
	/// </summary>
	public class FriHashSetOfObjectsPropertyDrawer : FriendlyCollectionPropertyDrawer<FriHashSetOfObjectsPData>
	{
		#region Persistence

		/// <summary>
		/// Table of persistent data associated with properties.
		/// </summary>
		private static PersistenceTable<FriHashSetOfObjectsPData> persistence = new PersistenceTable<FriHashSetOfObjectsPData>();

		/// <summary>
		/// Gets persistent data associated with a property.
		/// </summary>
		/// <param name="property">The current property being drawn.</param>
		protected override FriHashSetOfObjectsPData GetPersistentData(SerializedProperty property)
		{
			return persistence[property];
		}

		#endregion
	}

	#endregion
}
