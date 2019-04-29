using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hook : MonoBehaviour
{
    public float MaxLength = 11.875f;
    public float HookSpeed = 80f;
    public LayerMask HookTarget;
    public float HookForce = 500f;
    public float DragMaxSpeed = 15.0f;
    public AudioClip HookSound;
    public AudioClip HookFailedSound;
    private new Rigidbody2D rigidbody;
    private SpriteRenderer sprite;
    private int state = 0;
    private float length = 0;
    private Vector2 hookPos = Vector2.zero;
    private Vector2 hookDir = Vector2.zero;
    private Character character;
    private new AudioSource audio;
    // Start is called before the first frame update
    void Start()
    {
        rigidbody = GetComponentInParent<Rigidbody2D>();
        character = GetComponentInParent<Character>();
        sprite = GetComponent<SpriteRenderer>();
        audio = GetComponentInParent<AudioSource>();
        state = 0;
        length = 0;
        hookPos = Vector2.zero;
        hookDir = Vector2.zero;
    }

    // Update is called once per frame
    void Update()
    {
        // FIXME: prefab scale?
        transform.localScale = new Vector3(2.285f, 2.285f, 1);

        if (state == 0 && Input.GetButtonDown("Use Hook")) {
            hookDir = character.AimDir;
            state = 1;
        }

        if (state == 1) {
            length += HookSpeed * Time.deltaTime;
            if (length >= MaxLength) {
                length = MaxLength;
                state = 0;
            }
            hookPos = (Vector2)(transform.position) + hookDir * length;
            RaycastHit2D hit = Physics2D.Linecast(transform.position, hookPos, HookTarget | 1 << LayerMask.NameToLayer("Unhookable"));
            if (hit.collider != null) {
                if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Unhookable")) {
                    hookPos = hit.point;
                    state = 0;
                    audio.PlayOneShot(HookFailedSound);
                } else {
                    hookPos = hit.point;
                    state = 2;
                    audio.PlayOneShot(HookSound);
                }
            };
            if (state == 0) {
                length = 0;
            }
        }

        if (state == 2) {
            if (!Input.GetButton("Use Hook")) {
                state = 0;
                length = 0;
            } else {
                hookDir = (hookPos - (Vector2)transform.position).normalized;
                length = (hookPos - (Vector2)transform.position).magnitude;
            }
        }

        float angleDeg = Mathf.Atan2(hookDir.y, hookDir.x) * Mathf.Rad2Deg;
        Quaternion rotation = Quaternion.AngleAxis(angleDeg, Vector3.forward);
        transform.rotation = rotation;
        sprite.size = new Vector2(length / 2, sprite.size.y);
    }

    void FixedUpdate() {
        if (rigidbody == null) {
            return;
        }

        if (state == 2) {
            Vector2 force = (hookPos - (Vector2)transform.position).normalized * HookForce * Time.fixedDeltaTime;
            if (hookPos.y < transform.position.y) {
                force.y *= 0.3f;
            } else {
                force.y *= 1.5f;
            }
            rigidbody.AddForce(force);
            if (rigidbody.velocity.magnitude > DragMaxSpeed) {
                rigidbody.velocity = rigidbody.velocity.normalized * DragMaxSpeed;
            }
        }
    }
}
