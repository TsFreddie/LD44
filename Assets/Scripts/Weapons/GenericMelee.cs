using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenericMelee : Weapon
{
    private new BoxCollider2D collider;

    void Start()
    {
        collider = GetComponentInChildren<BoxCollider2D>();
        if (collider == null) {
            Debug.LogError("A Melee weapon is missing collider");
        }
    }

    protected override bool OnFire() {
        Collider2D[] hits = Physics2D.OverlapBoxAll(transform.position, collider.size, transform.eulerAngles.z, Target);
        if (hits.Length > 0) {
            foreach (Collider2D hit in hits) {
                Damageable damageable = hit.GetComponent<Damageable>();
                if (damageable) {
                    damageable.TakeDamage(Damage);
                    break;
                }
            }
        }
        return true;
    }
}
