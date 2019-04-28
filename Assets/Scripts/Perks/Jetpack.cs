using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Jetpack : Upgradable
{
    public ParticleSystem particle;
    public Image borderUI;
    public Image fillUI;
    public float[] BurnSpeed;
    public float[] RegenSpeed;
    public float MinActivate = 0.5f;
    public float VerticalMaxSpeed = 10f;
    public float VerticalForce = 200f;
    private new Rigidbody2D rigidbody;
    private bool flying = false;
    private float fuel = 1f;
    private bool disabled = false;
    private RectTransform fill;
    private float uiAlpha = 0;
    private int level = 0;
    private Character character;

    public override void UpgradeTo(int level)
    {
        this.level = level;
    }

    void Start()
    {
        rigidbody = GetComponentInParent<Rigidbody2D>();
        character = GetComponentInParent<Character>();
        
        fill = fillUI.GetComponent<RectTransform>();
        flying = false;
        disabled = false;
        uiAlpha = 0;
    }

    void Update()
    {
        if (rigidbody == null || character == null) {
            return;
        }

        if (character.FacingLeft) {
            transform.localScale = new Vector3(-1, transform.localScale.y, transform.localScale.z);
        } else {
            transform.localScale = new Vector3(1, transform.localScale.y, transform.localScale.z);
        }

        if (!disabled && Input.GetButtonDown("Use Jetpack")) {
            particle.Play();
            flying = true;
        }
        if (Input.GetButtonUp("Use Jetpack")) {
            particle.Stop();
            flying = false;
        }

        if (flying) {
            uiAlpha += 10f * Time.deltaTime;
            fuel -= BurnSpeed[level] * Time.deltaTime;
            rigidbody.AddForce(new Vector2(0, VerticalForce * Time.deltaTime));
            if (fuel <= 0) {
                fuel = 0;
                disabled = true;
                flying = false;
                particle.Stop();
            }
            rigidbody.velocity = new Vector2(rigidbody.velocity.x, Mathf.Clamp(rigidbody.velocity.y, -VerticalMaxSpeed, VerticalMaxSpeed));
        } else {
            fuel += RegenSpeed[level] * Time.deltaTime;
            if (fuel >= MinActivate) {
                disabled = false;
            }
        }

        if (fuel > 1) {
            fuel = 1;
        }

        if (fuel >= 1 && !flying) {
            uiAlpha -= 10f * Time.deltaTime;
        }

        if (uiAlpha > 1) {
            uiAlpha = 1;
        } else if (uiAlpha < 0) {
            uiAlpha = 0;
        }
        
        if (disabled) {
            fillUI.color = new Color(1, 0, 0, uiAlpha);
        } else {
            fillUI.color = new Color(1, 0.7882353f, 0, uiAlpha);
        }

        borderUI.color = new Color(borderUI.color.r, borderUI.color.g, borderUI.color.b, uiAlpha);

        fill.localScale = new Vector3(fill.localScale.x, fuel, fill.localScale.z);
    }
}
