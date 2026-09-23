using System;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(BulletDataBase))]
public class BulletDataBaseEditor : Editor
{
    SerializedProperty _bulletList;

    void OnEnable()
    {
        _bulletList = serializedObject.FindProperty("_bulletList");

        var database = target as BulletDataBase;
        database.SetListCount((BulletType[])Enum.GetValues(typeof(BulletType)));
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        for(int i = 0; i < _bulletList.arraySize; i++)
        {
            var element = _bulletList.GetArrayElementAtIndex(i);
            EditorGUILayout.LabelField(new GUIContent(((BulletType)i).ToString()));
            EditorGUILayout.PropertyField(element, new GUIContent(""));
        }

        serializedObject.ApplyModifiedProperties();
    }
}
