namespace Cresspresso.FriendlyCollections.Editors
{
	using UnityEditor;

	#region FriListPData

	/// <summary>
	/// <see cref="PersistentData"/> for <see cref="FriListPropertyDrawer"/>.
	/// </summary>
	public class FriListPData : FriendlyCollectionPData { }

	#endregion
	#region FriListPropertyDrawer

	/// <summary>
	/// <see cref="CustomPropertyDrawer"/> for <see cref="FriList"/>.
	/// </summary>
	[CustomPropertyDrawer(typeof(FriList), true)]
	public class FriListPropertyDrawer : FriendlyCollectionPropertyDrawer<FriListPData>
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
