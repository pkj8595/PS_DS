using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(ShowIfEnumAttribute))]
public class ShowIfEnumDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        ShowIfEnumAttribute showIf = (ShowIfEnumAttribute)attribute;

        // Enum 값을 기반으로 필드 숨김/표시 처리
        SerializedProperty enumProp = property.serializedObject.FindProperty(showIf.enumFieldName);
        if (enumProp != null && enumProp.enumValueIndex == showIf.enumValue)
        {
            EditorGUI.PropertyField(position, property, label);
        }
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        ShowIfEnumAttribute showIf = (ShowIfEnumAttribute)attribute;
        SerializedProperty enumProp = property.serializedObject.FindProperty(showIf.enumFieldName);

        // Enum 값에 따라 높이를 0으로 설정해 숨김
        if (enumProp != null && enumProp.enumValueIndex != showIf.enumValue)
            return 0;

        return EditorGUI.GetPropertyHeight(property, label, true);
    }
}
