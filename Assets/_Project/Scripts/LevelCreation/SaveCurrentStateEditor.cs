///Based on: https://learn.unity.com/tutorial/editor-scripting#5c7f8528edbc2a002053b5f9

using UnityEngine;
using System.Collections;
using UnityEditor;


#if UNITY_EDITOR
[CustomEditor(typeof(SaveCurrentState))]
public class ObjectBuilderEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        SaveCurrentState myScript = (SaveCurrentState)target;
        if(GUILayout.Button("Save State Into Level"))
        {
            myScript.SaveCurrentChildStateIntoLevel();
            Debug.Log("Save Complete!");
        }

    }
}
#endif