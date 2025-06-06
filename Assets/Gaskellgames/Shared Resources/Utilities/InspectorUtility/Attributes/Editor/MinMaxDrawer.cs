#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

/// <summary>
/// Code created by Gaskellgames
/// </summary>

namespace Gaskellgames
{
    [CustomPropertyDrawer(typeof(MinMaxAttribute))]
    public class MinMaxDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            // open property and get reference to attribute instance
            EditorGUI.BeginProperty(position, GUIContent.none, property);
            MinMaxAttribute minMax = attribute as MinMaxAttribute;

            if (property.propertyType == SerializedPropertyType.Float)
            {
                float limitedValue = property.floatValue;
                if (minMax.max < property.floatValue)
                {
                    limitedValue = minMax.max;
                }
                else if (property.floatValue < minMax.min)
                {
                    limitedValue = minMax.min;
                }

                property.floatValue = EditorGUI.FloatField(position, label, limitedValue);
            }
            else if (property.propertyType == SerializedPropertyType.Integer)
            {
                int limitedValue = property.intValue;
                if ((int)minMax.max < property.intValue)
                {
                    limitedValue = (int)minMax.max;
                }
                else if (property.intValue < (int)minMax.min)
                {
                    limitedValue = (int)minMax.min;
                }

                property.intValue = EditorGUI.IntField(position, label, limitedValue);
            }
            else
            {
                EditorGUI.PropertyField(position, property, label);
            }

            // close property
            EditorGUI.EndProperty();
        }

    } // class end
}

#endif