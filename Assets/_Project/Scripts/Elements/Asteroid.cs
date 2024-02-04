using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(TeleportableObject))]
public class Asteroid : MonoBehaviour
{
    [SerializeField] Asteroid[] deathSpawn;
    [SerializeField] GameObject[] spawnPositions;
    [SerializeField] float _startSpeed;
    [SerializeField] int _value;

    public float StartSpeed
    {
        get => _startSpeed;
        set => _startSpeed = value;
    }

    public int Value => _value;

    Rigidbody2D rigid;
    Collider2D collider2d;

    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        collider2d = GetComponent<Collider2D>();
    }

    void Start()
    {
        collider2d.enabled = false;
    }

    /// This could be done way better with DoTWEEN, but I'm avoiding using packages for this project
    public IEnumerator FadeIn(Action callback, float duration = 0.5f)
    {
        SpriteRenderer renderer = GetComponent<SpriteRenderer>();
        renderer.color = new Color(renderer.color.r, renderer.color.g, renderer.color.b, 0);

        while (renderer.color.a != 1)
        {
            yield return null;
            renderer.color = new Color(renderer.color.r, renderer.color.g, renderer.color.b, Mathf.Min(renderer.color.a + Time.deltaTime / duration, 1f));
        }

        callback();
    }

    public void Init()
    {
        collider2d.enabled = true;
        rigid.velocity = rigid.GetRelativeVector(Vector2.up) * StartSpeed;
    }
    void OnCollisionEnter2D(Collision2D collision)
    {
        List<GameObject> availablePositions = new(spawnPositions);
        if (collision.otherCollider.CompareTag(Tags.BULLET) || collision.collider.CompareTag(Tags.BULLET))
        {
            foreach (Asteroid toSpawn in deathSpawn)
            {
                int spawnIndex = UnityEngine.Random.Range(0, availablePositions.Count - 1);
                GameObject spawnPosition = availablePositions[spawnIndex];
                availablePositions.RemoveAt(spawnIndex);

                Asteroid instance = Instantiate(toSpawn);

                instance.transform.position = spawnPosition.transform.position;

                Vector2 distance = spawnPosition.transform.position - transform.position;
                Vector2 rotation = distance * collision.relativeVelocity;

                instance.transform.Rotate(0, 0, Mathf.Atan2(rotation.y, rotation.x) * Mathf.Rad2Deg);
                instance.StartSpeed = StartSpeed * Mathf.Cos(Mathf.Atan2(rotation.y, rotation.x));

                GameManager.Instance.AsteroidCreated(instance);
            }

            GameManager.Instance.AsteroidDestroyed(this);
            Destroy(gameObject);
        }
    }
}
