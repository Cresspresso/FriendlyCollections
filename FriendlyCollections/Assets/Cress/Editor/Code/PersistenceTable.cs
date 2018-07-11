using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEditor;
using UnityEditorInternal;
using Cress;

namespace CressEditor
{
	#region PersistentData

	/// <summary>
	/// Data associated with a <see cref="SerializedProperty"/> which should persist between multiple editor frames.
	/// </summary>
	/// <typeparam name="TArgs">Type of extra arguments suplied when resetting.</typeparam>
	public class PersistentData<TArgs>
	{
		#region Fields

		/// <summary>
		/// Serialized object of the property this data relates to.
		/// </summary>
		public SerializedObject serializedObject { get; private set; }

		/// <summary>
		/// Property this data relates to.
		/// </summary>
		public SerializedProperty property { get; private set; }

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
		/// <param name="args">Arguments passed on to the <see cref="OnReset(TArgs)"/> method.</param>
		public void Reset(SerializedProperty property, TArgs args)
		{
			this.property = property;
			serializedObject = property.serializedObject;

			OnReset(args);

			OnChange();
		}

		/// <summary>
		/// Custom setup for this data.
		/// </summary>
		/// <param name="args">Arguments passed down from the containing <see cref="PersistenceTable{TData, TDataArgs}"/>.</param>
		protected virtual void OnReset(TArgs args) { }

		#endregion
	}

	#endregion
	#region PersistenceTable

	/// <summary>
	/// Collection of <typeparamref name="TData"/> objects which are associated with <see cref="SerializedProperty"/>s.
	/// </summary>
	/// <typeparam name="TData">Type of <see cref="PersistentData{TArgs}"/> stored for each <see cref="SerializedProperty"/>.</typeparam>
	/// <typeparam name="TDataArgs">Type of arguments supplied to the <see cref="TData.Reset(SerializedProperty, TDataArgs)"/> method.</typeparam>
	public class PersistenceTable<TData, TDataArgs> where TData : PersistentData<TDataArgs>, new()
	{
		#region Fields

		/// <summary>
		/// Dictionary of <see cref="SerializedProperty.propertyPath"/> keys and the data associated with the properties.
		/// </summary>
		private Dictionary<string, TData> persistence;

		/// <summary>
		/// Function which gets extra arguments to supply to a data object when it is reset.
		/// </summary>
		private Func<SerializedProperty, TData, TDataArgs> onGetArgs;

		#endregion
		#region Constructor

		/// <summary>
		/// Creates a <see cref="PersistenceTable{TData, TDataArgs}"/>.
		/// </summary>
		public PersistenceTable(Func<SerializedProperty, TData, TDataArgs> onGetArgs = null)
		{
			persistence = new Dictionary<string, TData>();
			this.onGetArgs = onGetArgs;
		}

		#endregion
		#region Indexer Property

		/// <summary>
		/// Gets the persistent data associated with a property.
		/// </summary>
		/// <param name="property">The current property.</param>
		/// <returns>The persistent data associated with the property.</returns>
		public TData this[SerializedProperty property] {
			get
			{
				TData data;
				string key = property.propertyPath;
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
						ResetData(data, property);
					}
				}
				else
				{
					data = new TData();
					ResetData(data, property);
					persistence[key] = data;
				}

				Clean();

				return data;
			}
		}

		#endregion
		#region Other Methods

		/// <summary>
		/// Resets a data object.
		/// </summary>
		/// <param name="data">The data object to reset.</param>
		/// <param name="property">The property it is associated with.</param>
		private void ResetData(TData data, SerializedProperty property)
		{
			TDataArgs args = onGetArgs != null ? onGetArgs(property, data) : default(TDataArgs);
			data.Reset(property, args);
		}

		/// <summary>
		/// Cleans up any expired data.
		/// </summary>
		private void Clean()
		{
			foreach (var k in (from p in persistence where p.Value.hasExpired select p.Key).ToArray())
			{
				persistence.Remove(k);
			}
		}

		#endregion
	}

	#endregion
}
