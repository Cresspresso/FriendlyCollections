namespace CresspressoEditor.FriendlyCollections
{
	using UnityEngine;
	using UnityEditor;
	using UnityEditorInternal;

	#region FriendlyCollectionPData

	/// <summary>
	/// Base class for <see cref="FriListPData"/> and <see cref="FriDictPData"/>.
	/// </summary>
	public abstract class FriendlyCollectionPData : PersistentData
	{
		#region Virtual Constants

		/// <summary>
		/// Relative property path to the serialized array/list property to display.
		/// </summary>
		protected virtual string fieldPathData { get { return "serialized"; } }

		/// <summary>
		/// Gets the display label for the list header.
		/// </summary>
		protected virtual GUIContent headerLabel { get { return new GUIContent(property.displayName); } }

		/// <summary>
		/// Gets the name of the collection type, i.e. the word "List" in the phrase "List is Empty".
		/// </summary>
		protected virtual string noneElementCollectionName { get { return "List"; } }

		#endregion
		#region Constants

		/// <summary>
		/// Padding at top of list elements.
		/// </summary>
		protected const float elementPaddingTop = 1;

		/// <summary>
		/// Sum of top and bottom padding of list elements.
		/// </summary>
		protected static float elementSpacing { get { return elementPaddingTop + EditorGUIUtility.standardVerticalSpacing; } }

		/// <summary>
		/// Gets the position rect for drawing a list element.
		/// Trims the padding values.
		/// </summary>
		/// <param name="totalPosition">Total position rect.</param>
		/// <returns>Rect where the element should be drawn.</returns>
		protected static Rect GetElementInnerRect(Rect totalPosition)
		{
			return new Rect(totalPosition.x, totalPosition.y + elementPaddingTop, totalPosition.width, totalPosition.height - elementSpacing);
		}

		/// <summary>
		/// Height of the 'none' element. Used when the list has no elements.
		/// </summary>
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
				GUIContent label = new GUIContent(headerLabel);
				if (isExpanded)
					label.text = " " + label.text;

				float h = EditorGUIUtility.standardVerticalSpacing;
				float left = isExpanded ? -6 : 0;
				position = new Rect(position.x + left, position.y + h / 2, position.width - left, position.height - h);
				isExpanded = EditorGUI.Foldout(position, isExpanded, label, true);
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
				EditorGUI.PropertyField(
					GetElementInnerRect(position),
					propData.GetArrayElementAtIndex(index),
					new GUIContent("Element " + index.ToString()),
					true);
			};

			// Draw None Element
			reorderableList.drawNoneElementCallback = (position) =>
			{
				string label = noneElementCollectionName + " is Empty";
				EditorGUI.LabelField(position, label);
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
	#region FriendlyCollectionPropertyDrawer

	/// <summary>
	/// Base class for
	/// <see cref="FriListPropertyDrawer"/>,
	/// <see cref="FriDictPropertyDrawer"/>, and
	/// <see cref="FriHashSetPropertyDrawer"/>.
	/// </summary>
	public abstract class FriendlyCollectionPropertyDrawer<TData> : PropertyDrawer where TData : FriendlyCollectionPData
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
		/// Unity script event.
		/// (<a href="https://docs.unity3d.com/ScriptReference/PropertyDrawer.GetPropertyHeight.html">Link to Docs</a>)
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
		/// Unity script event.
		/// (<a href="https://docs.unity3d.com/ScriptReference/PropertyDrawer.OnGUI.html">Link to Docs</a>)
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
		/// Unity script event.
		/// (<a href="https://docs.unity3d.com/ScriptReference/PropertyDrawer.CanCacheInspectorGUI.html">Link to Docs</a>)
		/// </summary>
		public override bool CanCacheInspectorGUI(SerializedProperty property)
		{
			// List has animations, and foldout height toggles, so disallow caching.
			return false;
		}

		#endregion
	}

	#endregion
}
