using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(TeleportableObject))]
public class Player : MonoBehaviour
{
    [SerializeField] GameObject gun;
    [SerializeField] GameObject thruster;
    [SerializeField] Bullet bullet;

    private Rigidbody2D rigid;
    private Collider2D collider2d;
    private SpriteRenderer spriteRenderer;
    private AudioSource audioSource;

    private float lastShoot = -Mathf.Infinity;
    private bool fading = false;
    private bool hasThrustedLastFrame;

    #region Unity Methods

    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        collider2d = GetComponent<Collider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        thruster.SetActive(hasThrustedLastFrame);
        hasThrustedLastFrame = false;
    }

    #endregion

    #region PrivateMethods

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!(collision.otherCollider.CompareTag(Tags.BULLET) || collision.collider.CompareTag(Tags.BULLET)))
        {
            GameManager.Instance.PlayerHit();
            Deactivate();
        }
    }

    private void Deactivate()
    {
        rigid.velocity = Vector2.zero;
        rigid.angularVelocity = 0f;
        collider2d.enabled = false;
    }

    private void RandomTeleport()
    {
        Vector2 position = new(UnityEngine.Random.Range(-5f, 5f), UnityEngine.Random.Range(-5f, 5f));
        transform.position = position;
    }
    #endregion

    #region PublicMethods
    public void Thrust(float rawValue)
    {
        float maxSpeed = rawValue * GameBalanceValues.Instance.Player.ThrustSpeed;
        float acceleration = GameBalanceValues.Instance.Player.Acceleration;

        float speed = Mathf.Lerp(rigid.velocity.magnitude, maxSpeed, acceleration);
        rigid.velocity = rigid.GetRelativeVector(Vector2.up) * speed;

        hasThrustedLastFrame = true;
    }

    public void Rotate(float rawValue)
    {
        rigid.angularVelocity = rawValue * GameBalanceValues.Instance.Player.RotateSpeed;
    }

    public void Shoot()
    {
        if (Time.time > lastShoot + GameBalanceValues.Instance.Shoot.SecodsPerShoot)
        {
            lastShoot = Time.time;

            Bullet bulletInstance = Instantiate(bullet,
                gun.transform.position,
                gun.transform.rotation
            );
            bulletInstance.Init(gameObject.tag);

            rigid.AddForce(rigid.GetRelativeVector(Vector2.down) * GameBalanceValues.Instance.Shoot.RecoilForce, ForceMode2D.Impulse);

            audioSource.Play();
        }
    }

    public void Hyperspace()
    {
        if (!fading) StartCoroutine(FadeOut(() =>
        {
            spriteRenderer.color = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, 1);
            RandomTeleport();
        }
             , 0.5f));
    }

    /// This could be done way better with DoTWEEN, but I'm avoiding using packages for this project
    public IEnumerator FadeOut(Action callback, float duration = 1f)
    {
        fading = true;
        spriteRenderer.color = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, 1);

        while (spriteRenderer.color.a != 0)
        {
            yield return null;
            spriteRenderer.color = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, Mathf.Max(spriteRenderer.color.a - (Time.deltaTime / duration), 0f));
        }

        fading = false;
        callback();
    }

    public void Reset()
    {
        spriteRenderer.color = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, 1);
        transform.SetPositionAndRotation(Vector3.zero, Quaternion.identity);
        collider2d.enabled = true;
    }
    #endregion
}
