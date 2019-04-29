using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileBullet : MonoBehaviour
{
    public float FireAngle = 0;
    public float Speed = 10f;
    public int Damage = 1;
    public float ExplosionRadius = 1.5f;
    public float ExplodeTime = 1f;
    public bool ExplodeOnGround = true;
    public LayerMask Target;
    public Sprite ExplosionSprite;
    private ParticleSystem particle;
    private new Rigidbody2D rigidbody;
    private new Collider2D collider;
    private new AudioSource audio;
    private SpriteRenderer sprite;
    private bool exploded = false;
    void Start()
    {
        collider = GetComponent<Collider2D>();
        particle = GetComponent<ParticleSystem>();
        rigidbody = GetComponent<Rigidbody2D>();
        audio = GetComponent<AudioSource>();
        sprite = GetComponent<SpriteRenderer>();
        Target |= 1 << LayerMask.NameToLayer("Ground");
        Target |= 1 << LayerMask.NameToLayer("Unhookable");
        rigidbody.velocity = (Quaternion.Euler(0, 0, FireAngle) * Vector2.right) * Speed;
    }

    void FixedUpdate()
    {
        if (exploded) {
            return;
        }
        transform.Rotate(0, 0, 2f * Time.deltaTime);

        ExplodeTime -= Time.deltaTime;
        if (ExplodeTime < 0) {
            Explode();
            return;
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
                if (hit.gameObject.layer == LayerMask.NameToLayer("Ground") ||
                hit.gameObject.layer == LayerMask.NameToLayer("Unhookable")) {
                    if (ExplodeOnGround) {
                        forceDestroy = true;
                        break;
                    }    
                }

                Damageable damageable = hit.GetComponent<Damageable>();
                if (damageable) {
                    hitTarget = true;
                    break;
                }
            }
            if (forceDestroy || hitTarget) {
                Explode();
            }
        }
    }

    public void Explode() {
        exploded = true;
        sprite.color = new Color(1,0.7f,0.3f,0.8f);
        sprite.sprite = ExplosionSprite;
        sprite.gameObject.transform.localScale = new Vector3(ExplosionRadius, ExplosionRadius, 1);
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, ExplosionRadius, Target);
        foreach (Collider2D hit in hits) {
            Damageable damageable = hit.GetComponent<Damageable>();
            if (damageable != null) {
                damageable.TakeDamage(Damage);
            }

            Rigidbody2D rigidbody = hit.GetComponent<Rigidbody2D>();
            if (rigidbody != null) {
                rigidbody.AddForce((hit.transform.position - transform.position).normalized * 1500f);
            }
        }
        particle.Play();
        if (audio != null) {
            audio.Play();
        }
        this.gameObject.layer = LayerMask.NameToLayer("Corpse");
        this.rigidbody.simulated = false;
        Destroy(this.gameObject, 0.5f);
    }
}
