using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flyable : MonoBehaviour
{
    public float AirMaxVelocity = 5f;
    public float AirAcceleration = 75f;
    public float AirFriction = 47.5f;
    private new Collider2D collider;
    private new Rigidbody2D rigidbody;
    private Vector2 move;
    private Animator animator;
    public Vector2 Move {
        set {
            this.move = value.normalized;
        }
    }

    public float MoveAngle {
        set {
            this.move = (Vector2)(Quaternion.Euler(0,0,value) * Vector2.right);
        }
    }

    public Collider2D Collider {
        get {
            return collider;
        }
    }

    void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        collider = GetComponent<Collider2D>();
        animator = GetComponent<Animator>();
        if (animator != null) animator.logWarnings = false;
        move = Vector2.zero;
    }

    void Update() {

    }

    void FixedUpdate() {
        float acc = AirAcceleration;
        float maxV = AirMaxVelocity;
        float fric = AirFriction;
        int moveLeftRight = 0;
        if (move.magnitude > 0) {
            if (move.x >= 0) {
                moveLeftRight = 1;
            } else {
                moveLeftRight = -1;
            }
            rigidbody.velocity += (move * acc * Time.deltaTime);
            if (rigidbody.velocity.magnitude > AirMaxVelocity) {
                rigidbody.velocity = rigidbody.velocity.normalized * AirMaxVelocity;
            }
            
        } else {
            rigidbody.velocity = rigidbody.velocity * fric * Time.deltaTime;
        }

        if (animator != null) 
            animator.SetInteger("Move", moveLeftRight);
    }
}
