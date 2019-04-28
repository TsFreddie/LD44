using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hook : MonoBehaviour
{
    public float MaxLength = 11.875f;
    public float HookSpeed = 80f;
    public LayerMask HookTarget;
    private new Rigidbody2D rigidbody;
    private SpriteRenderer sprite;
    private int state = 0;
    private float length = 0;
    private Vector2 hookPos = Vector2.zero;
    // Start is called before the first frame update
    void Start()
    {
        rigidbody = GetComponentInParent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
        state = 0;
        length = 0;
        hookPos = Vector2.zero;
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 mousePos = Input.mousePosition;
        Vector2 targetPos = (Vector2)Camera.main.ScreenToWorldPoint(mousePos);
        Vector2 deltaPos = (targetPos - (Vector2)transform.position).normalized;

        float angleDeg = Mathf.Atan2(deltaPos.y, deltaPos.x) * Mathf.Rad2Deg;
        Quaternion rotation = Quaternion.AngleAxis(angleDeg, Vector3.forward);
        transform.rotation = rotation;

        if (state == 0 && Input.GetMouseButtonDown(1)) {
            state = 1;
        }

        if (state == 1) {
            length += HookSpeed * Time.deltaTime;
            if (length >= MaxLength) {
                length = MaxLength;
                state = 0;
            }
            Vector2 hookPos = (Vector2)(transform.position) + deltaPos * length;
            if (Physics2D.OverlapPoint(hookPos, HookTarget)) {
                state = 2;
            };
        }

        sprite.size = new Vector2(length / 2, sprite.size.y);
    }
}
