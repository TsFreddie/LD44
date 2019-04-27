using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damageable : MonoBehaviour
{
    public int MaxHealth = 10;
    public int Health = 10;
    public float CorpseLifespan = 5f;
    private bool dead;
    private Animator animator;
    void Start() {
        animator = GetComponent<Animator>();
    }
    void Update() {
        if (dead) {
            CorpseLifespan -= Time.deltaTime;
            if (CorpseLifespan < 0) {
                Destroy(this.gameObject);
            }
        }
    }
    public void TakeDamage(int amount) {
        Health -= amount;
        if (Health <= 0) {
            Health = 0;
            Die();
        }
    }

    public void Die() {
        if (animator) {
            animator.SetTrigger("Dead");
        }
        dead = true;
        gameObject.layer = LayerMask.NameToLayer("Corpse");
    }

}