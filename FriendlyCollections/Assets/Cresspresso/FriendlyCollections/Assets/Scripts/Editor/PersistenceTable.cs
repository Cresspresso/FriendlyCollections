namespace Cresspresso.FriendlyCollections.Editor
{
	using System;
	using System.Collections.Generic;
	using UnityEditor;
	using Cresspresso.Extensions;

	#region PersistentData

	/// <summary>
	/// Data associated with a <see cref="SerializedProperty"/> which should persist between multiple editor frames.
	/// </summary>
	public class PersistentData
	{
		#region Fields

		/// <summary>
		/// Property this data relates to.
		/// </summary>
		public SerializedProperty property { get; private set; }

		/// <summary>
		/// Serialized object of the property this data relates to.
		/// </summary>
		public SerializedObject serializedObject { get { return property.serializedObject; } }

		/// <summary>
		/// Moment in time when the <see cref="OnChange"/> method was last called.
		/// </summary>
		private DateTime timeOfLastChange;

		#endregion
		#region Expiry Methods

		/// <summary>
		/// Called when the property is visible, to tell it not to expire.
		/// </summary>
		public void OnChange()
		{
			timeOfLastChange = DateTime.Now;
		}

		/// <summary>
		/// Max time before data expiry.
		/// </summary>
		private static readonly TimeSpan expiryTime = new TimeSpan(0, 5, 0);

		/// <summary>
		/// True if this data has not been updated recently, and can be disposed.
		/// </summary>
		public bool hasExpired { get { return DateTime.Now - timeOfLastChange > expiryTime; } }

		#endregion
		#region Reset Methods

		/// <summary>
		/// Initializes this persistent data.
		/// </summary>
		/// <param name="property">The property this data is associated with.</param>
		public virtual void Reset(SerializedProperty property)
		{
			this.property = property;

			OnChange();
		}

		#endregion
	}

	#endregion
	#region PersistenceTable

	/// <summary>
	/// Collection of <typeparamref name="TData"/> objects which are associated with <see cref="SerializedProperty"/>s.
	/// </summary>
	/// <typeparam name="TData">Type of <see cref="PersistentData"/> stored for each <see cref="SerializedProperty"/>.</typeparam>
	public class PersistenceTable<TData> where TData : PersistentData, new()
	{
		#region Fields

		/// <summary>
		/// Dictionary of <see cref="SerializedProperty.propertyPath"/> keys and the data associated with the properties.
		/// </summary>
		private Dictionary<string, TData> persistence = new Dictionary<string, TData>();

		#endregion
		#region GetData Method

		/// <summary>
		/// Gets the persistent data associated with a property.
		/// </summary>
		/// <param name="property">The current property.</param>
		/// <returns>The persistent data associated with the property.</returns>
		public TData this[SerializedProperty property]
		{
			get
			{
				string key = string.Format("{0}::{1}",
					property.serializedObject.targetObject.GetType().FullName,
					property.propertyPath);
				// (Using just propertyPath causes changes to other serializedObjects with similar property paths.)

				TData data;
				if (persistence.ContainsKey(key))
				{
					data = persistence[key];
					data.OnChange();


					// if old serializedObject has been disposed, reset data.
					try
					{
						// Something to test if the property's serializedObject has been disposed.
						data.property.FindPropertyRelative("");
					}
					catch (NullReferenceException)
					{
						data.Reset(property);
					}
				}
				else
				{
					data = new TData();
					data.Reset(property);
					persistence[key] = data;
				}

				Clean();

				return data;
			}
		}

		#endregion
		#region Other Methods

		/// <summary>
		/// Cleans up any expired data.
		/// </summary>
		private void Clean()
		{
			persistence.RemoveAllEntries(p => p.Value.hasExpired);
		}

		#endregion
	}

	#endregion
}
