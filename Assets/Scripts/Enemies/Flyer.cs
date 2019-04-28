using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Flyable))]
[RequireComponent(typeof(ScriptableWeaponControl))]
public class Flyer : MonoBehaviour
{
    public float FireRange;
    private float nextMove;
    private float moveDuration;
    private Vector2 moveDir;
    private Flyable fly;
    private GameObject target;
    private ScriptableWeaponControl weaponControl;
    void Start()
    {
        fly = GetComponent<Flyable>();
        weaponControl = GetComponent<ScriptableWeaponControl>();
        target = Character.I.gameObject;
        nextMove = Random.Range(0.3f, 1.5f);
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 move = Vector2.zero;
        if (target != null) {
            Vector2 deltaPos = (Vector2)target.transform.position - (Vector2)transform.position;
            if (moveDuration > 0) {
                moveDuration -= Time.deltaTime;
                move = moveDir;
            } else {
                nextMove -= Time.deltaTime;
                if (nextMove < 0) {
                    nextMove = Random.Range(0.3f, 1.5f);
                    moveDuration = Random.Range(0.8f, 2f);
                    moveDir = deltaPos;
                }
            }

            if (Mathf.Abs(deltaPos.magnitude) < FireRange) {
                // Attack
                weaponControl.Fire();
            }
        }

        fly.Move = move;
    }
}
