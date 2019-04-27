using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalBullet : MonoBehaviour, IBullet
{
    public float FireAngle = 0;
    public float Speed = 10f;
    public int Damage = 1;
    public LayerMask Target;
    public float Lifespan = 1.5f;

    public int GetDamage()
    {
        return Damage;
    }

    public void ReportHit()
    {
        Destroy(this.gameObject);
    }

    private float colliderLength;
    private new BoxCollider2D collider;
    void Start() {
        collider = GetComponent<BoxCollider2D>();
        if (!collider) {
            Destroy(this.gameObject);
            return;
        }
        colliderLength = collider.size.x;
        Target |= 1 << LayerMask.NameToLayer("Ground");
    }
    void Update()
    {
        transform.rotation = Quaternion.AngleAxis(FireAngle, Vector3.forward);
        transform.Translate(Vector3.right * Time.deltaTime * Speed, Space.Self);
        Lifespan -= Time.deltaTime;
        if (Lifespan < 0) {
            Destroy(this.gameObject);
        }

        Collider2D[] hits = Physics2D.OverlapBoxAll(transform.position, collider.size, transform.eulerAngles.z, Target);
        if (hits.Length > 0) {
            foreach (Collider2D hit in hits) {
                Damageable damageable = hit.GetComponent<Damageable>();
                if (damageable) {
                    damageable.TakeDamage(Damage);
                    break;
                }
            }
            Destroy(this.gameObject);
        }
    }
}
