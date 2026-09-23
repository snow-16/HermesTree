using System;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(CellDataBase))]
public class CellDataBaseEditor : Editor
{
    SerializedProperty _cellList;

    void OnEnable()
    {
        _cellList = serializedObject.FindProperty("_cellList");

        var database = target as CellDataBase;
        database.SetListCount((CellType[])Enum.GetValues(typeof(CellType)));
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        for(int i = 0; i < _cellList.arraySize; i++)
        {
            var element = _cellList.GetArrayElementAtIndex(i);
            EditorGUILayout.LabelField(new GUIContent(((CellType)i).ToString()));
            EditorGUILayout.PropertyField(element, new GUIContent(""));
        }

        serializedObject.ApplyModifiedProperties();
    }
}
