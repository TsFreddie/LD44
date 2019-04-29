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
    public bool RotateSprite = true;
    public int PiercingCount = 1;
    private HashSet<Collider2D> hitSet; 

    public int GetDamage()
    {
        return Damage;
    }

    public void ReportHit()
    {
        Destroy(this.gameObject);
    }

    private new Collider2D collider;
    void Start() {
        collider = GetComponent<Collider2D>();
        hitSet = new HashSet<Collider2D>();
        if (!collider) {
            Destroy(this.gameObject);
            return;
        }
        Target |= 1 << LayerMask.NameToLayer("Ground");
        Target |= 1 << LayerMask.NameToLayer("Unhookable");
    }
    void Update()
    {
        transform.rotation = Quaternion.AngleAxis(FireAngle, Vector3.forward);
        transform.Translate(Vector3.right * Time.deltaTime * Speed, Space.Self);
        Lifespan -= Time.deltaTime;
        if (Lifespan < 0) {
            Destroy(this.gameObject);
        }
        Collider2D[] hits = new Collider2D[3];
        ContactFilter2D contactFilter = new ContactFilter2D();
        contactFilter.SetLayerMask(Target);
        Physics2D.OverlapCollider(collider, contactFilter, hits);
        if (hits.Length > 0) {
            bool forceDestroy = false;
            bool hitTarget = false;
            foreach (Collider2D hit in hits) {
                if (hit == null) {
                    break;
                }
                if (hitSet.Contains(hit)) {
                    continue;
                }
                
                if (hit.gameObject.layer == LayerMask.NameToLayer("Ground") ||
                hit.gameObject.layer == LayerMask.NameToLayer("Unhookable")) {
                    forceDestroy = true;
                    break;
                } else {
                    hitSet.Add(hit);
                }

                Damageable damageable = hit.GetComponent<Damageable>();
                if (damageable) {
                    hitTarget = true;
                    damageable.TakeDamage(Damage);
                    break;
                }
            }
            if (forceDestroy) {
                Destroy(this.gameObject);
            } else {
                if (hitTarget) PiercingCount--;
                if (PiercingCount <= 0) {
                    Destroy(this.gameObject);
                }
            }
        }
    }
}
