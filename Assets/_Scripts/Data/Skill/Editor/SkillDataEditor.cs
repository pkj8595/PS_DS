using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(SOSkillData))]
public class SkillDataEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector(); // 기본 필드 그리기

        SOSkillData skillData = (SOSkillData)target;

        EditorGUILayout.Space();
        skillData.icon = EditorGUILayout.ObjectField(skillData.icon, typeof(Sprite), false, GUILayout.Width(64), GUILayout.Height(64)) as Sprite;

        foreach (var skill in skillData.skills)
        {
            if (skill.targeting != null)
            {
                EditorGUILayout.Space();
                EditorGUILayout.LabelField("타겟팅", EditorStyles.boldLabel);

                Editor editor = CreateEditor(skill.targeting);
                editor.OnInspectorGUI();


            }

            if (skill.effects != null)
            {
                foreach (var affect in skill.effects)
                {
                    EditorGUILayout.BeginFoldoutHeaderGroup(true, "효과");
                    Editor editor = CreateEditor(affect);
                    editor.OnInspectorGUI();
                    EditorGUILayout.EndFoldoutHeaderGroup();
                }
            }
        }
    }

   
}