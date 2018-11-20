using UnityEngine;
using UnityEditor;

[CustomPropertyDrawer(typeof(TriCoordinates))]
public class TriCoordinatesDrawer : PropertyDrawer {
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
        TriCoordinates coordinates = new TriCoordinates(
            property.FindPropertyRelative("x").intValue,
            property.FindPropertyRelative("z").intValue
        );
        position = EditorGUI.PrefixLabel(position, label);
        GUI.Label(position, coordinates.ToString());
    }
}