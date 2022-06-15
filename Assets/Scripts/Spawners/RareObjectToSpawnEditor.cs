using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(SpawnManager))]
[CanEditMultipleObjects]
public class SpawnManagerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        SerializedProperty _rareObjectToSpawn;
        SerializedProperty _rareObjectList;
        SerializedProperty _rareSecondsToSpawn;

        _rareObjectToSpawn = serializedObject.FindProperty("_doseSpawnerHasRareObjectToSpawn");
        _rareObjectList = serializedObject.FindProperty("_rareObjectPrefab");
        _rareSecondsToSpawn = serializedObject.FindProperty("_secondsToSpawnRareObject");

        EditorGUILayout.PropertyField(_rareObjectToSpawn);
        if (_rareObjectToSpawn.boolValue)
        {
            EditorGUILayout.PropertyField(_rareObjectList);
            EditorGUILayout.PropertyField(_rareSecondsToSpawn);
        }
        
        serializedObject.ApplyModifiedProperties();
    }
}

[CustomEditor(typeof(SpawnPowerUps))]
public class SpawnPowerUpsEditor : SpawnManagerEditor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
    }
}
