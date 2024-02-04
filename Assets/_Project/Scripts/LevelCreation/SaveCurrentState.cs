using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

#if UNITY_EDITOR
[ExecuteInEditMode]
public class SaveCurrentState : MonoBehaviour
{
    [SerializeField] LevelScriptableObject editingLevel;
    [SerializeField] bool clearWhenSave;

    LevelScriptableObject _editingLevel;

    void Update()
    {
        if (editingLevel != _editingLevel)
        {
            _editingLevel = editingLevel;
            if (_editingLevel != null)
            {
                Utils.RemoveAllChild(transform);

                foreach (AsteroidObjectDefinition def in _editingLevel.asteroids)
                {
                    Asteroid instance = PrefabUtility.InstantiatePrefab(def.prefab, transform) as Asteroid;
                    instance.transform.position = def.position;
                    instance.transform.localRotation = def.rotation;
                    instance.StartSpeed = def.speed;
                }
            }
        }
    }

    public void SaveCurrentChildStateIntoLevel()
    {
        List<AsteroidObjectDefinition> defs = new();
        foreach (Asteroid child in GetComponentsInChildren<Asteroid>())
        {
            AsteroidObjectDefinition def = new()
            {
                prefab = PrefabUtility.GetCorrespondingObjectFromOriginalSource<Asteroid>(child),
                position = child.transform.position,
                rotation = child.transform.localRotation,
                speed = child.StartSpeed
            };

            defs.Add(def);
        }

        editingLevel.asteroids = defs.ToArray();
        EditorUtility.SetDirty(editingLevel);

        if(clearWhenSave) {
            editingLevel = null;
            Utils.RemoveAllChild(transform);
            clearWhenSave = false;
        }
    }
}
#endif