using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damageable : MonoBehaviour
{
    public int MaxHealth = 10;
    public int Health = 10;
    public float CorpseLifespan = 5f;
    public float KillReward = 0.1f;
    public AudioClip DamageSound;
    public AudioClip DieSound;
    private bool dead;
    private Animator animator;
    private new AudioSource audio;
    void Start() {
        animator = GetComponent<Animator>();
        if (animator != null) animator.logWarnings = false;
        audio = GetComponent<AudioSource>();
    }
    void Update() {
        if (dead) {
            CorpseLifespan -= Time.deltaTime;
            if (CorpseLifespan < 0) {
                if (gameObject.layer == LayerMask.NameToLayer("Player")) {
                    Character.I.Died();
                } else {
                    Destroy(this.gameObject);
                }
            }
        }
    }
    public void TakeDamage(int amount) {
        if (amount <= 0) {
            return;
        }
        if (animator) {
            animator.SetTrigger("Hurt");
        }
        Health -= amount;
        if (Health <= 0) {
            Health = 0;
            Die();
            return;
        }
        if (audio != null && DamageSound != null) {
            audio.PlayOneShot(DamageSound);
        }
    }

    public void Die() {
        if (animator) {
            animator.SetTrigger("Dead");
        }
        dead = true;
        if (audio != null && DieSound != null) {
            audio.PlayOneShot(DieSound);
        }
        if (gameObject.layer == LayerMask.NameToLayer("Enemy")) {
            Character.I.ReportKill(KillReward);
        }
        gameObject.layer = LayerMask.NameToLayer("Corpse");
    }

}