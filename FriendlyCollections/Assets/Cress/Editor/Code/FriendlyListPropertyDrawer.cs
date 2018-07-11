using UnityEngine;
using UnityEditor;
using UnityEditorInternal;
using Cress;

namespace CressEditor
{
	#region PersistentReorderableListData

	/// <summary>
	/// Base class for <see cref="PersistentListData"/> and <see cref="PersistentDictionaryData"/>.
	/// </summary>
	public abstract class PersistentReorderableListData<TDataArgs> : PersistentData<TDataArgs>
	{
		#region Overload Constants

		/// <summary>
		/// Relative property path to the serialized array/list property to display.
		/// </summary>
		protected virtual string fieldPathData { get { return "data"; } }

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

		#endregion
		#region Methods

		/// <summary>
		/// Overloaded method <see cref="PersistentData{TArgs}.OnReset(TArgs)"/>.
		/// </summary>
		protected override void OnReset(TDataArgs args)
		{
			propData = property.FindPropertyRelative(fieldPathData);
			InitReorderableList();
		}

		/// <summary>
		/// Creates a friendly <see cref="ReorderableList"/> based on a property.
		/// </summary>
		protected void InitReorderableList()
		{
			reorderableList = new ReorderableList(propData.serializedObject, propData, true, true, true, true);

			// Footer
			reorderableList.footerHeight = EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;

			// Header
			reorderableList.headerHeight = EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
			reorderableList.drawHeaderCallback = (rect) =>
			{
				float h = EditorGUIUtility.standardVerticalSpacing;
				float left = property.isExpanded ? -6 : 0;
				rect = new Rect(rect.x + left, rect.y + h / 2, rect.width - left, rect.height - h);
				property.isExpanded = EditorGUI.Foldout(rect, property.isExpanded, new GUIContent(property.isExpanded ? " " + property.displayName : property.displayName), true);
			};

			// Element
			reorderableList.elementHeightCallback = (index) => EditorGUI.GetPropertyHeight(propData.GetArrayElementAtIndex(index), true) + 1 + EditorGUIUtility.standardVerticalSpacing;
			reorderableList.drawElementCallback = (rect, index, a, b) =>
			{
				rect = new Rect(rect.x, rect.y + 1, rect.width, rect.height - EditorGUIUtility.standardVerticalSpacing - 1);
				EditorGUI.PropertyField(rect, propData.GetArrayElementAtIndex(index), new GUIContent("Element " + index.ToString()), true);
			};

			// Add
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

			// Remove
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


			OnInitReorderableList();
		}

		/// <summary>
		/// Custom setup after reorderable list initialization.
		/// </summary>
		protected virtual void OnInitReorderableList() { }

		#endregion
	}

	#endregion
	#region FriendlyListPropertyDrawerBase

	/// <summary>
	/// Base class for <see cref="FriendlyListPropertyDrawer"/> and <see cref="FriendlyDictionaryPropertyDrawer"/>.
	/// </summary>
	public abstract class FriendlyListPropertyDrawerBase<TData, TDataArgs> : PropertyDrawer where TData : PersistentReorderableListData<TDataArgs>
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

			if (property.isExpanded)
			{
				float height = data.reorderableList.headerHeight + data.reorderableList.footerHeight;
				int size = data.propData.arraySize;
				if (size > 0)
				{
					for (int i = 0; i < size; ++i)
					{
						height += data.reorderableList.elementHeightCallback.Invoke(i);
					}
				}
				else
				{
					height += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing * 2.5f;
				}
				return height;
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

			Rect rect = EditorGUI.IndentedRect(position);

			int oldIndent = EditorGUI.indentLevel;
			EditorGUI.indentLevel = 0;

			if (property.isExpanded)
			{
				data.reorderableList.DoList(rect);
			}
			else
			{
				data.reorderableList.drawHeaderCallback.Invoke(rect);
			}

			EditorGUI.indentLevel = oldIndent;

			EditorGUI.EndProperty();
		}

		#endregion
	}

	#endregion
	#region PersistentListData

	/// <summary>
	/// <see cref="PersistentData{TArgs}"/> for <see cref="FriendlyListPropertyDrawerBase"/>.
	/// </summary>
	public class PersistentListData : PersistentReorderableListData<Void>
	{
		/// <summary>
		/// Name of field <see cref="FriendlyList{T}.data"/>.
		/// </summary>
		protected override string fieldPathData { get { return "data"; } }
	}

	#endregion
	#region FriendlyListPropertyDrawer

	/// <summary>
	/// <see cref="CustomPropertyDrawer"/> for <see cref="FriendlyList"/>.
	/// </summary>
	[CustomPropertyDrawer(typeof(FriendlyList), true)]
	public class FriendlyListPropertyDrawer : FriendlyListPropertyDrawerBase<PersistentListData, Void>
	{
		#region Persistence

		/// <summary>
		/// Table of persistent data associated with properties.
		/// </summary>
		private static PersistenceTable<PersistentListData, Void> persistence = new PersistenceTable<PersistentListData, Void>();

		/// <summary>
		/// Gets persistent data associated with a property.
		/// </summary>
		/// <param name="property">The current property being drawn.</param>
		protected override PersistentListData GetPersistentData(SerializedProperty property)
		{
			return persistence[property];
		}

		#endregion
	}

	#endregion
}
