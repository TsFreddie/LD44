using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Walkable))]
[RequireComponent(typeof(ScriptableWeaponControl))]
public class Troop : MonoBehaviour
{
    public LayerMask Avoid;
    public LayerMask Target;
    public float AttentionRange;
    public float FireRange;
    private Walkable walk;
    private ScriptableWeaponControl weaponControl;
    private GameObject lockedTarget;
    private float lockOnTime;
    private float nextMove;
    private float moveDuration;
    private bool moveLeft;
    void Start()
    {
        walk = GetComponent<Walkable>();
        weaponControl = GetComponent<ScriptableWeaponControl>();
        if (weaponControl.Weapon)
            weaponControl.Weapon.Target = Target;
        nextMove = Random.Range(0.3f, 1.5f);
    }
    void Update()
    {
        // Global Target
        int move = 0;
        /*
        if (GlobalTarget.I != null) {
            if (transform.position.x < GlobalTarget.I.transform.position.x) {
                move = 1;
            }
            if (transform.position.x > GlobalTarget.I.transform.position.x) {
                move = -1;
            }
        }
        */

        // Find Target
        Collider2D[] targetsInRange = Physics2D.OverlapCircleAll(transform.position, AttentionRange, Target);
        Collider2D finalTarget = null;
        float minDistance = float.PositiveInfinity;
        foreach (Collider2D target in targetsInRange) {
            float distance = (target.ClosestPoint(transform.position) - (Vector2)transform.position).magnitude;
            if (distance < minDistance) {
                minDistance = distance;
                finalTarget = target;
            }
        }

        if (finalTarget != null) {
            lockedTarget = finalTarget.gameObject;
            lockOnTime = 5f;
            if (transform.position.x < finalTarget.transform.position.x) {
                move = 1;
            }
            if (transform.position.x > finalTarget.transform.position.x) {
                move = -1;
            }

            // Target range
            Vector2 deltaPos = finalTarget.transform.position - transform.position;
            float angleDeg = Mathf.Atan2(deltaPos.y, deltaPos.x) * Mathf.Rad2Deg;
            Quaternion rotation = Quaternion.AngleAxis(angleDeg, Vector3.forward);
            weaponControl.AimAngle = rotation.eulerAngles.z;
        } else {
            lockOnTime -= Time.deltaTime;
            if (lockedTarget != null && lockOnTime > 0f) {
                if (transform.position.x < lockedTarget.transform.position.x) {
                    move = 1;
                }
                if (transform.position.x > lockedTarget.transform.position.x) {
                    move = -1;
                }
            } else {
                if (moveDuration > 0) {
                    moveDuration -= Time.deltaTime;
                    if (moveLeft) {
                        move = -1;
                    } else {
                        move = 1;
                    }
                } else {
                    nextMove -= Time.deltaTime;
                    if (nextMove < 0) {
                        nextMove = Random.Range(0.3f, 1.5f);
                        moveDuration = Random.Range(0.8f, 2f);
                        moveLeft = Random.Range(0, 1f) < 0.5f;
                    }
                }
            }
        }

        if (lockedTarget != null) {
            if (Mathf.Abs(((Vector2)lockedTarget.transform.position - (Vector2)transform.position).magnitude) < FireRange) {
                // Attack
                weaponControl.Fire();
            }
        }
        
        // Move
        walk.Move = move;
        
        // Jumpover wall
        //float wallCheckPointX = transform.position.x + walk.Collider.ClosestPoint() collider.offset.x + (move * (collider.size.x / 2f + 0.2f));
        if (move != 0) {
            Vector2 wallCheckPoint = walk.Collider.ClosestPoint((Vector2)transform.position + Vector2.right * move * 100f) + Vector2.right * move * 0.05f;
            RaycastHit2D hit = Physics2D.Linecast(wallCheckPoint, wallCheckPoint + move * new Vector2(1.5f, 0), Avoid);
            if (hit.collider != null) {
                walk.Jump();
            }

            // Debug
            Debug.DrawLine(wallCheckPoint, wallCheckPoint + move * new Vector2(1.5f, 0), Color.red, Time.deltaTime);
        }
    }

    void OnDrawGizmosSelected() {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, AttentionRange);
    }
}
