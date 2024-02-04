using UnityEngine;
using System;

[Serializable]
public class AsteroidObjectDefinition {
    public Asteroid prefab;
    public Vector3 position;
    public Quaternion rotation;
    public float speed;
}

[CreateAssetMenu(fileName = "Level", menuName = "Asteroids/Level Definition", order = 1)]
public class LevelScriptableObject : ScriptableObject
{
    public AsteroidObjectDefinition[] asteroids;
}