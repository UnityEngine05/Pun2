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

        EditorGUILayout.LabelField("", GUILayout.Width(120));
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Key_1", GUILayout.Width(60));
        EditorGUILayout.LabelField("", GUILayout.Width(88));
        EditorGUILayout.LabelField("Key_2", GUILayout.Width(60));
        EditorGUILayout.EndHorizontal();
        EditorGUILayout.BeginHorizontal();
        _Player.key_1 = (KeyCode)EditorGUILayout.EnumPopup(_Player.key_1, GUILayout.Width(150));
        _Player.key_2 = (KeyCode)EditorGUILayout.EnumPopup(_Player.key_2, GUILayout.Width(150));
        EditorGUILayout.EndHorizontal();

        _Player.foldOut = EditorGUILayout.Foldout(_Player.foldOut, "플레이어 정보");
        if (_Player.foldOut)
        {
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Team", GUILayout.Width(120));
            _Player._PlayerTeam = (PlayerTeam)EditorGUILayout.EnumPopup(_Player._PlayerTeam, GUILayout.Width(90));
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("이름", GUILayout.Width(120));
            _Player._PlayerCharaters = (PlayerCharaters)EditorGUILayout.EnumPopup(_Player._PlayerCharaters, GUILayout.Width(90));
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("플레이어 기본 속도", GUILayout.Width(120));
            _Player.moveSpeed = EditorGUILayout.FloatField(_Player.moveSpeed, GUILayout.Width(90));
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("고치는 속도", GUILayout.Width(120));
            _Player.fixSpeed = EditorGUILayout.FloatField(_Player.fixSpeed, GUILayout.Width(90));
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("확인 속도", GUILayout.Width(120));
            _Player.checkSpeed = EditorGUILayout.FloatField(_Player.checkSpeed, GUILayout.Width(90));
            EditorGUILayout.EndHorizontal();    
        }


        if (EditorGUI.EndChangeCheck())
            EditorUtility.SetDirty(_Player);
    }
}
