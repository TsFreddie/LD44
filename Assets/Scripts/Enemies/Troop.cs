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
    private Walkable walk;
    private ScriptableWeaponControl weaponControl;
    void Start()
    {
        walk = GetComponent<Walkable>();
        weaponControl = GetComponent<ScriptableWeaponControl>();
        weaponControl.Weapon.Target = Target;
    }
    void Update()
    {
        // Global Target
        int move = 0;
        if (GlobalTarget.I != null) {
            if (transform.position.x < GlobalTarget.I.transform.position.x) {
                move = 1;
            }
            if (transform.position.x > GlobalTarget.I.transform.position.x) {
                move = -1;
            }
        }

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

            // Attack
            weaponControl.Fire();


        }
        
        // Move
        walk.Move = move;
        
        // Jumpover wall
        //float wallCheckPointX = transform.position.x + walk.Collider.ClosestPoint() collider.offset.x + (move * (collider.size.x / 2f + 0.2f));
        Vector2 wallCheckPoint = walk.Collider.ClosestPoint((Vector2)transform.position + Vector2.right * move * 100f) + Vector2.right * move * 0.05f;
        RaycastHit2D hit = Physics2D.Linecast(wallCheckPoint, wallCheckPoint + move * new Vector2(2f, 0), Avoid);
        if (hit.collider != null) {
            walk.Jump();
        }

        // Debug
        Debug.DrawLine(wallCheckPoint, wallCheckPoint + move * new Vector2(2f, 0), Color.red, Time.deltaTime);
    }

    void OnDrawGizmosSelected() {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, AttentionRange);
    }
}
