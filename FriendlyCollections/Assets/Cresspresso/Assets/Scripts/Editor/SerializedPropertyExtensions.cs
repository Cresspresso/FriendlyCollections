using System;
using UnityEngine;
using UnityEditor;

namespace Cresspresso.Editor.Extensions
{
	/// <summary>
	/// Extension methods relating to <see cref="SerializedProperty"/>.
	/// </summary>
	public static class SerializedPropertyExtensions
	{
		/// <summary>
		/// Gets the value of a <see cref="SerializedProperty"/>. Does not work for complex types, e.g. arrays.
		/// </summary>
		/// <param name="property">Property to get the value of.</param>
		/// <returns>Value stored in the property.</returns>
		/// <exception cref="InvalidOperationException">Property type is not primitive.</exception>
		/// <exception cref="NotImplementedException">This function has not been implemented for that possibly primitive property type.</exception>
		public static object GetPropertyValue(this SerializedProperty property)
		{
			switch (property.propertyType)
			{
				default:
					throw new InvalidOperationException("Cannot extract data from serialized property due to complex type.");

				case SerializedPropertyType.ArraySize:
					throw new NotImplementedException("Cannot extract data from ArraySize serialized property.");

				case SerializedPropertyType.Boolean:
					return property.boolValue;

				case SerializedPropertyType.Bounds:
					return property.boundsValue;

				case SerializedPropertyType.BoundsInt:
					return property.boundsIntValue;

				//case SerializedPropertyType.Character

				case SerializedPropertyType.Color:
					return property.colorValue;

				case SerializedPropertyType.Enum:
					return property.enumValueIndex;

				case SerializedPropertyType.ExposedReference:
					return property.exposedReferenceValue;

				case SerializedPropertyType.FixedBufferSize:
					throw new NotImplementedException("Cannot extract data from FixedBufferSize serialized property.");

				case SerializedPropertyType.Float:
					return property.floatValue;

				//case SerializedPropertyType.Generic

				//case SerializedPropertyType.Gradient

				case SerializedPropertyType.Integer:
					return property.intValue;

				case SerializedPropertyType.LayerMask:
					throw new NotImplementedException("Cannot extract data from LayerMask serialized property.");

				case SerializedPropertyType.ObjectReference:
					return property.objectReferenceValue;

				case SerializedPropertyType.Quaternion:
					return property.quaternionValue;

				case SerializedPropertyType.Rect:
					return property.rectValue;

				case SerializedPropertyType.RectInt:
					return property.rectIntValue;

				case SerializedPropertyType.String:
					return property.stringValue;

				case SerializedPropertyType.Vector2:
					return property.vector2Value;

				case SerializedPropertyType.Vector2Int:
					return property.vector2IntValue;

				case SerializedPropertyType.Vector3:
					return property.vector3Value;

				case SerializedPropertyType.Vector3Int:
					return property.vector3IntValue;

				case SerializedPropertyType.Vector4:
					return property.vector4Value;
			}
		}
	}
}
