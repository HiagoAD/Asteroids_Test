using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Asteriods/Game Balance")]
public class GameBalanceValues : ScriptableObject
{
    [Serializable]
    public class PlayerValues
    {
        [SerializeField] float thrustSpeed;
        [SerializeField] float acceleration;
        [SerializeField] float rotateSpeed;

        public float ThrustSpeed => thrustSpeed;
        public float Acceleration => acceleration;
        public float RotateSpeed => rotateSpeed;
    }

    [Serializable]
    public class ShootValues
    {
        [SerializeField] float shootsPerSecond;
        [SerializeField] float recoilForce;

        public float SecodsPerShoot => 1 / shootsPerSecond;
        public float RecoilForce => recoilForce;
    }

    [Serializable]
    public class BulletValues
    {
        [SerializeField] float speed;
        [SerializeField] float timeToDie;

        public float Speed => speed;
        public float TimeToDie => timeToDie;
    }

    [Serializable]
    public class LifeValues
    {
        [SerializeField] int maxAmount;

        public int MaxAmount => maxAmount;
    }

    [Serializable]
    public class SaucerValues
    {
        [SerializeField] float shootsPerSecond;
        [SerializeField] float maximumHeight;
        [SerializeField] float speed;
        [SerializeField] float timeToSpawn;

        public float SecodsPerShoot { get { return 1 / shootsPerSecond; } }
        public float MaximumHeight => maximumHeight;
        public float Speed => speed;
        public float TimeToSpawn => timeToSpawn;
    }

    [SerializeField] PlayerValues player = new();
    [SerializeField] ShootValues shoot = new();
    [SerializeField] BulletValues bullet = new();
    [SerializeField] LifeValues life = new();
    [SerializeField] SaucerValues saucer = new();

    public PlayerValues Player => player;
    public ShootValues Shoot => shoot;
    public BulletValues Bullet => bullet;
    public LifeValues Life => life;
    public SaucerValues Saucer => saucer;

    public static GameBalanceValues Instance
    { get; private set; }
    public void SetActive()
    {
        Instance = this;
    }
}
