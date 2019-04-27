using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    public GameObject AimPoint;
    public GameObject EyeGroup;
    private Walkable walk;
    void Start()
    {
        walk = GetComponent<Walkable>();
    }

    void Update()
    {
        Vector2 mousePos = Input.mousePosition;
        Vector2 targetPos = (Vector2)Camera.main.ScreenToWorldPoint(mousePos);
        Vector2 deltaPos = (targetPos - (Vector2)transform.position).normalized;
        AimPoint.transform.localPosition = deltaPos;
        EyeGroup.transform.localPosition = deltaPos * 0.15f;
        if (deltaPos.x < 0) {
            AimPoint.transform.localScale = new Vector3(1, -1, 1);
        } else {
            AimPoint.transform.localScale = new Vector3(1, 1, 1);
        }

        float angleDeg = Mathf.Atan2(deltaPos.y, deltaPos.x) * Mathf.Rad2Deg;
        Quaternion rotation = Quaternion.AngleAxis(angleDeg, Vector3.forward);
        AimPoint.transform.rotation = rotation;

        // Movement
        int move = 0;
        if (Input.GetKey(KeyCode.A)) {
            move += -1;
        }

        if (Input.GetKey(KeyCode.S)) {
            move += 1;
        }

        walk.Move = move;

        if (Input.GetKeyDown(KeyCode.Space)) {
            walk.Jump();
        }
    }


}
