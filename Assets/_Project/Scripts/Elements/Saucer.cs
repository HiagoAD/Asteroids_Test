using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(TeleportableObject))]
public class Saucer : MonoBehaviour
{
    [SerializeField] int value;
    [SerializeField] GameObject gun;
    [SerializeField] Bullet bullet;

    private Rigidbody2D rigid;
    private AudioSource audioSource;

    private Player player;
    private float lastShoot = -Mathf.Infinity;

    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        audioSource = GetComponent<AudioSource>();
    }


    void Start()
    {
        Deactivate();
    }

    public void Init(Player player)
    {
        Activate();

        this.player = player;

        bool startRight = Random.value > 0.5f;
        Vector3 startPosition = new(
            startRight ? 5 : -5,
            Random.Range(-GameBalanceValues.Instance.Saucer.MaximumHeight, GameBalanceValues.Instance.Saucer.MaximumHeight),
            0
        );

        transform.position = startPosition;

        float direction = startRight ? -1 : 1;
        rigid.velocity = direction * GameBalanceValues.Instance.Saucer.Speed * Vector2.right;
    }

    void Update()
    {
        if (Time.time > lastShoot + GameBalanceValues.Instance.Saucer.SecodsPerShoot)
        {
            lastShoot = Time.time;
            Shoot();
        }
    }

    private void Activate()
    {
        gameObject.SetActive(true);
    }

    private void Deactivate()
    {
        gameObject.SetActive(false);
    }

    private void Shoot()
    {

        Vector3 distance = player.transform.position - gun.transform.position;

        Bullet bulletInstance = Instantiate(bullet,
            gun.transform.position,
            Quaternion.Euler(0, 0, (Mathf.Atan2(distance.y, distance.x) * Mathf.Rad2Deg) - 90)
        );

        bulletInstance.Init(gameObject.tag);

        audioSource.Play();

        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!(collision.otherCollider.CompareTag(Tags.SAUCER_BULLET) || collision.collider.CompareTag(Tags.SAUCER_BULLET)))
        {
            GameManager.Instance.SaucerDestroyed(value);
            Deactivate();
        }
    }
}
