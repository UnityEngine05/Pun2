using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.TextCore.Text;

[CustomEditor(typeof(Player))]
public class PlayerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        Player _Player = (Player)target;


        _Player.foldOut = EditorGUILayout.Foldout(_Player.foldOut, "플레이어 정보");
        if (_Player.foldOut)
        {
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Team", GUILayout.Width(120));
            _Player._PlayerTeam = (PlayerTeam)EditorGUILayout.EnumPopup(_Player._PlayerTeam, GUILayout.Width(90));
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Team", GUILayout.Width(120));
            _Player._PlayerCharaters = (PlayerCharaters)EditorGUILayout.EnumPopup(_Player._PlayerCharaters, GUILayout.Width(90));
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("플레이어 기본 속도", GUILayout.Width(120));
            _Player.moveSpeed = EditorGUILayout.FloatField(_Player.moveSpeed, GUILayout.Width(90));
            EditorGUILayout.EndHorizontal();





            if (EditorGUI.EndChangeCheck())
                EditorUtility.SetDirty(_Player);
        }
    }
}
