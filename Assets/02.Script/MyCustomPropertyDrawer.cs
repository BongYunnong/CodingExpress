using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


[CustomPropertyDrawer(typeof(ArrayLayout))]
public class MyCustomPropertyDrawer  : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        //base.OnGUI(position, property, label);
        EditorGUI.PrefixLabel(position, label);
        Rect newPosition = position;
        newPosition.y += 18f*2;
        SerializedProperty data = property.FindPropertyRelative("rows");

        for (int j = 0; j < 4; j++)
        {
            newPosition.x += 50f;
            SerializedProperty row = data.GetArrayElementAtIndex(j).FindPropertyRelative("row");
            newPosition.height = 18f;
            if (row.arraySize != 4)
                row.arraySize = 4;
            newPosition.width = (position.width- 50) / 4;
            for (int i = 0; i < 4; i++)
            {
                EditorGUI.PropertyField(newPosition, row.GetArrayElementAtIndex(i),GUIContent.none);
                newPosition.x += newPosition.width;
            }

            newPosition.x = position.x;
            newPosition.y += 18f;
        }
    }
    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        return 18f*6f;
    }
}
