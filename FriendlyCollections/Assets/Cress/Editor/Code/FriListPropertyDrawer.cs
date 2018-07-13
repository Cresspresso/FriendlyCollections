using UnityEngine;
using UnityEditor;
using UnityEditorInternal;
using Cress;

namespace CressEditor
{
	#region ReorderableListPData

	/// <summary>
	/// Base class for <see cref="FriListPData"/> and <see cref="FriDictPData"/>.
	/// </summary>
	public abstract class ReorderableListPData : PersistentData
	{
		#region Virtual Constants

		/// <summary>
		/// Relative property path to the serialized array/list property to display.
		/// </summary>
		protected virtual string fieldPathData { get { return "data"; } }

		#endregion
		#region Constants

		protected const float elementPaddingTop = 1;
		protected static float elementSpacing { get { return elementPaddingTop + EditorGUIUtility.standardVerticalSpacing; } }

		protected static Rect GetElementInnerRect(Rect totalPosition)
		{
			return new Rect(totalPosition.x, totalPosition.y + elementPaddingTop, totalPosition.width, totalPosition.height - elementSpacing);
		}

		protected static float noneElementHeight { get { return EditorGUIUtility.singleLineHeight; } }

		#endregion
		#region Fields

		/// <summary>
		/// Property for the serialized list of elements to display.
		/// </summary>
		public SerializedProperty propData;

		/// <summary>
		/// Graphical list for displaying the serialized list.
		/// </summary>
		public ReorderableList reorderableList;

		/// <summary>
		/// If true, the list will be drawn. If false, only the display label will be drawn.
		/// </summary>
		public bool isExpanded;

		#endregion
		#region Methods

		/// <summary>
		/// Overridden method <see cref="PersistentData.Reset(SerializedProperty)"/>.
		/// </summary>
		public override void Reset(SerializedProperty property)
		{
			base.Reset(property);

			propData = property.FindPropertyRelative(fieldPathData);
			
			property.isExpanded = isExpanded;

			InitReorderableList();
		}

		/// <summary>
		/// Creates a friendly <see cref="ReorderableList"/> based on a property.
		/// </summary>
		protected virtual void InitReorderableList()
		{
			reorderableList = new ReorderableList(propData.serializedObject, propData, true, true, true, true);

			// Draw Header
			reorderableList.headerHeight = EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
			reorderableList.drawHeaderCallback = (position) =>
			{
				string displayName = property.displayName;
				float h = EditorGUIUtility.standardVerticalSpacing;
				float left = isExpanded ? -6 : 0;
				position = new Rect(position.x + left, position.y + h / 2, position.width - left, position.height - h);
				isExpanded = EditorGUI.Foldout(position, isExpanded, new GUIContent(isExpanded ? " " + displayName : displayName), true);
			};

			// Element Height
			reorderableList.elementHeightCallback = (index) =>
			{
				if (reorderableList.count == 0)
					return noneElementHeight;

				SerializedProperty propElement = propData.GetArrayElementAtIndex(index);

				return EditorGUI.GetPropertyHeight(propElement, GUIContent.none, true) + elementSpacing;
			};

			// Draw Element
			reorderableList.drawElementCallback = (position, index, a, b) =>
			{
				position = GetElementInnerRect(position);
				EditorGUI.PropertyField(position, propData.GetArrayElementAtIndex(index), new GUIContent("Element " + index.ToString()), true);
			};

			// On Add
			reorderableList.onAddCallback = (list) =>
			{
				if (list.index >= 0 && list.index < propData.arraySize - 1)
				{
					propData.InsertArrayElementAtIndex(list.index);
				}
				else
				{
					++propData.arraySize;
					list.index = propData.arraySize - 1;
				}
			};

			// On Remove
			reorderableList.onCanRemoveCallback = (list) => propData.arraySize > 0;
			reorderableList.onRemoveCallback = (list) =>
			{
				int index = Mathf.Clamp(list.index, 0, propData.arraySize - 1);
				propData.DeleteArrayElementAtIndex(index);

				if (list.index == propData.arraySize)
				{
					list.index = propData.arraySize - 1;
				}
			};
		}

		#endregion
	}

	#endregion
	#region ReorderableListPropertyDrawer

	/// <summary>
	/// Base class for <see cref="FriListPropertyDrawer"/> and <see cref="FriDictPropertyDrawer"/>.
	/// </summary>
	public abstract class ReorderableListPropertyDrawer<TData> : PropertyDrawer where TData : ReorderableListPData
	{
		#region Persistence

		/// <summary>
		/// Persistent data for the current property.
		/// </summary>
		protected TData data;

		/// <summary>
		/// Gets persistent data associated with a property.
		/// </summary>
		/// <param name="property">The current property being drawn.</param>
		protected abstract TData GetPersistentData(SerializedProperty property);

		/// <summary>
		/// Prepares this class' <see cref="data"/>.
		/// </summary>
		/// <param name="property">The current property being drawn.</param>
		private void Prepare(SerializedProperty property)
		{
			data = GetPersistentData(property);
		}

		#endregion
		#region PropertyDrawer Methods

		/// <summary>
		/// Unity script event. (<a href="https://docs.unity3d.com/ScriptReference/PropertyDrawer.GetPropertyHeight.html">Link to Docs</a>)
		/// </summary>
		public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
		{
			Prepare(property);
			
			if (data.isExpanded)
			{
				return data.reorderableList.GetHeight();
			}
			else
			{
				return EditorGUIUtility.singleLineHeight;
			}
		}

		/// <summary>
		/// Unity script event. (<a href="https://docs.unity3d.com/ScriptReference/PropertyDrawer.OnGUI.html">Link to Docs</a>)
		/// </summary>
		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
		{
			Prepare(property);

			EditorGUI.BeginProperty(position, GUIContent.none, property);

			position = EditorGUI.IndentedRect(position);

			int oldIndent = EditorGUI.indentLevel;
			EditorGUI.indentLevel = 0;
			
			if (data.isExpanded)
			{
				data.reorderableList.DoList(position);
			}
			else
			{
				data.reorderableList.drawHeaderCallback.Invoke(position);
			}

			EditorGUI.indentLevel = oldIndent;

			EditorGUI.EndProperty();
		}

		/// <summary>
		/// Unity script event. (<a href="https://docs.unity3d.com/ScriptReference/PropertyDrawer.CanCacheInspectorGUI.html">Link to Docs</a>)
		/// </summary>
		public override bool CanCacheInspectorGUI(SerializedProperty property)
		{
			// List has animations, and foldout height toggles, so don't cache.
			return false;
		}

		#endregion
	}

	#endregion
	#region FriListPData

	/// <summary>
	/// <see cref="PersistentData"/> for <see cref="FriListPropertyDrawer"/>.
	/// </summary>
	public class FriListPData : ReorderableListPData
	{
		/// <summary>
		/// Name of field <see cref="FriList{T}.data"/>.
		/// </summary>
		protected override string fieldPathData { get { return "data"; } }
	}

	#endregion
	#region FriListPropertyDrawer

	/// <summary>
	/// <see cref="CustomPropertyDrawer"/> for <see cref="FriList"/>.
	/// </summary>
	[CustomPropertyDrawer(typeof(FriList), true)]
	public class FriListPropertyDrawer : ReorderableListPropertyDrawer<FriListPData>
	{
		#region Persistence

		/// <summary>
		/// Table of persistent data associated with properties.
		/// </summary>
		private static PersistenceTable<FriListPData> persistence = new PersistenceTable<FriListPData>();

		/// <summary>
		/// Gets persistent data associated with a property.
		/// </summary>
		/// <param name="property">The current property being drawn.</param>
		protected override FriListPData GetPersistentData(SerializedProperty property)
		{
			return persistence[property];
		}

		#endregion
	}

	#endregion
}
