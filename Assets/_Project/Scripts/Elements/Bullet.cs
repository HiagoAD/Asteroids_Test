using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(TeleportableObject))]
public class Bullet : MonoBehaviour
{
    Rigidbody2D rigid;

    private bool destroyed = false;
    private string ignoreTag = "";

    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
    }

    public void Init(string ignoreTag) {
        rigid.velocity = rigid.GetRelativeVector(Vector2.up) * GameBalanceValues.Instance.Bullet.Speed;
        this.ignoreTag = ignoreTag;

        StartCoroutine(CoutdownToExtinction());
    }

    private IEnumerator CoutdownToExtinction()
    {
        yield return new WaitForSeconds(GameBalanceValues.Instance.Bullet.TimeToDie);
        if(!destroyed) Destroy(gameObject);
    }

    void OnCollisionEnter2D(Collision2D collision) {
        if(!(collision.otherCollider.CompareTag(ignoreTag) || collision.collider.CompareTag(ignoreTag))) {
            destroyed = true;
            Destroy(gameObject);
        }
    }
}
