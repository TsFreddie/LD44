using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Walkable))]
[RequireComponent(typeof(BoxCollider2D))]
public class Troop : MonoBehaviour
{
    public LayerMask Avoid;
    public Weapon Weapon;
    private Walkable walk;
    private new BoxCollider2D collider;
    void Start()
    {
        walk = GetComponent<Walkable>();
        collider = GetComponent<BoxCollider2D>();
    }

    void Update()
    {
        int move = 0;
        if (GlobalTarget.I) {
            if (transform.position.x < GlobalTarget.I.transform.position.x) {
                move = 1;
            }
            if (transform.position.x > GlobalTarget.I.transform.position.x) {
                move = -1;
            }
        }


        // Global Move
        walk.Move = move;
        float wallCheckPointX = transform.position.x + collider.offset.x + (move * (collider.size.x / 2f + 0.2f));
        Vector2 wallCheckPoint = new Vector2(wallCheckPointX, transform.position.y);
        Debug.DrawLine(wallCheckPoint, wallCheckPoint + move * new Vector2(2f, 0), Color.red, Time.deltaTime);
        RaycastHit2D hit = Physics2D.Linecast(wallCheckPoint, wallCheckPoint + move * new Vector2(2f, 0), Avoid);
        if (hit.collider != null) {
            walk.Jump();
        }
    }
}
