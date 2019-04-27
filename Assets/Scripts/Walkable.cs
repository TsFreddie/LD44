﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Walkable : MonoBehaviour
{
    public float AirMaxVelocity = 250f;
    public float AirAcceleration = 1.5f;
    public float AirFriction = 47.5f;
    public float MaxVelocity = 10f;
    public float Acceleration = 100f;
    public float GroundFriction = 25f;
    public float JumpVelocity = 13.2f;
    public float AirJumpVelocity = 12.0f;
    public Transform GroundCheckPoint;
    public int MaxJumps = 2;
    private new Rigidbody2D rigidbody;
    private int move;
    private bool grounded;
    private int jumps;
    private bool jumping;
    public int Move {
        set {
            this.move = value;
        }
    }

    public void Jump() {
        jumping = true;
    }
    void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        move = 0;
        jumps = 0;
        grounded = false;
    }

    void Update()
    {
        if (GroundCheckPoint == null) {
            return;
        }
        grounded = Physics2D.OverlapPoint(GroundCheckPoint.position, 1 << LayerMask.NameToLayer("Ground"));
        if (grounded) {
            jumps = MaxJumps;
        }

        if (jumping) {
            jumping = false;
            if (jumps > 0) {
                if (grounded) {
                    rigidbody.velocity = new Vector2(rigidbody.velocity.x, JumpVelocity);
                } else {
                    rigidbody.velocity = new Vector2(rigidbody.velocity.x, AirJumpVelocity);
                }
                jumps--;
            }
        }
    }

    void FixedUpdate() {
        float acc = grounded ? Acceleration : AirAcceleration;
        float maxV = grounded ? MaxVelocity : AirMaxVelocity;
        float fric = grounded ? GroundFriction : AirFriction;
        if (move != 0) {
            if ((move > 0 && rigidbody.velocity.x < maxV) || (move < 0 && rigidbody.velocity.x > -maxV)) {
                rigidbody.velocity += (move * acc * Time.deltaTime * new Vector2(1, 0));
                rigidbody.velocity = new Vector2(Mathf.Clamp(rigidbody.velocity.x, -maxV, maxV), rigidbody.velocity.y);
            }
        } else {
            rigidbody.velocity = new Vector2(rigidbody.velocity.x * fric * Time.deltaTime, rigidbody.velocity.y);
        }
    }
}