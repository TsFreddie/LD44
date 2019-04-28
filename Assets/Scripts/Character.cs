﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Character : SingletonMonoBehaviour<Character>
{
    public GameObject AimAnchor;
    public GameObject PerkAnchor;
    public GameObject EyeGroup;
    public LayerMask Target;
    public AudioClip SoundGainHeart;
    public AudioClip SoundTakeDamage;
    public AudioClip SoundJump;
    public AudioClip SoundAirJump;
    public AudioClip SoundSwitchWeapon;
    private List<Weapon> weapons;
    public int activeWeapon = -1;
    private Walkable walk;
    private Damageable health;
    private float aimAngle;
    private Vector2 aimDir;
    private bool facingLeft;
    private float nextHeart;
    private new AudioSource audio;

    public Vector2 AimDir {
        get {
            return aimDir;
        }
    }
    public float AimAngle {
        get {
            return aimAngle;
        }
    }

    public bool FacingLeft {
        get {
            return facingLeft;
        }
    }

    public int SpendableHealth {
        get {
            return Mathf.Clamp(health.Health - 5, 0, 195);
        }
    }

    protected override void SingletonAwake() {
        weapons = new List<Weapon>();
        activeWeapon = -1;
    }
    void Start()
    {
        Scene inGameUI = SceneManager.GetSceneByName("InGameUI");
        if (!inGameUI.IsValid()) {
            SceneManager.LoadScene("InGameUI", LoadSceneMode.Additive);
        }
        
        walk = GetComponent<Walkable>();
        health = GetComponent<Damageable>();
        audio = GetComponent<AudioSource>();
        aimAngle = 0;
        facingLeft = false;
        aimDir = Vector2.zero;
    }

    void Update()
    {
        if (health.Health < health.MaxHealth && nextHeart > 1f) {
            nextHeart = 0f;
            health.Health += 1;
            audio.PlayOneShot(SoundGainHeart);
        }

        if (HealthDisplay.I) {
            HealthDisplay.I.Max = health.MaxHealth;
            HealthDisplay.I.Value = health.Health;
            HealthDisplay.I.NextHeart = nextHeart;
        }
        Vector2 mousePos = Input.mousePosition;
        Vector2 targetPos = (Vector2)Camera.main.ScreenToWorldPoint(mousePos);
        Vector2 deltaPos = (targetPos - (Vector2)transform.position).normalized;
        aimDir = deltaPos;
        EyeGroup.transform.localPosition = deltaPos * 0.15f;
        if (deltaPos.x < 0) {
            facingLeft = true;
            AimAnchor.transform.localScale = new Vector3(1, -1, 1);
        } else {
            facingLeft = false;
            AimAnchor.transform.localScale = new Vector3(1, 1, 1);
        }

        aimAngle = Mathf.Atan2(deltaPos.y, deltaPos.x) * Mathf.Rad2Deg;
        Quaternion rotation = Quaternion.AngleAxis(aimAngle, Vector3.forward);
        AimAnchor.transform.rotation = rotation;

        // Movement
        int move = 0;
        if (Input.GetButton("Move Left")) {
            move += -1;
        }

        if (Input.GetButton("Move Right")) {
            move += 1;
        }

        walk.Move = move;

        if (Input.GetButtonDown("Jump")) {
            if (walk.CanJump) {
                if (walk.Grounded) {
                    audio.PlayOneShot(SoundJump);
                } else {
                    audio.PlayOneShot(SoundAirJump);
                }
            }
            walk.Jump();
        }

        if (activeWeapon >= 0) {
            if (Input.GetButtonDown("Fire Weapon")) {
                weapons[activeWeapon].Firing(true);
            } else if (Input.GetButton("Fire Weapon")) {
                weapons[activeWeapon].Firing(false);
            }
        }
    }

    public Upgradable GiveWeapon(GameObject weaponPrefab) {
        GameObject newWeapon = Instantiate(weaponPrefab);
        newWeapon.transform.SetParent(AimAnchor.transform, false);
        newWeapon.transform.localPosition = Vector3.zero;
        newWeapon.SetActive(true);
        Weapon weaponObj = newWeapon.GetComponent<Weapon>();
        weaponObj.Target = Target;
        if (weaponObj == null) {
            Destroy(newWeapon);
            return null;
        }
        weapons.Add(weaponObj);
        if (activeWeapon < 0) {
            activeWeapon = 0;
        }
        return newWeapon.GetComponent<Upgradable>();
    }

    public Upgradable GivePerk(GameObject perkPrefab) {
        GameObject newPerk = Instantiate(perkPrefab);
        newPerk.transform.SetParent(PerkAnchor.transform, false);
        newPerk.transform.localPosition = Vector3.zero;
        newPerk.SetActive(true);
        return newPerk.GetComponent<Upgradable>();
    }

    public void TakeDamage(int amount) {
        health.TakeDamage(amount);
    }

    public void ReportKill(float growAmount) {
        // TODO: gain heart;
    }

    public void Died() {
        // TODO: Gameover
    }

    public void GainMaxHearts(int amount) {
        int target = health.MaxHealth + amount;
        if (target > 200) {
            target = 200;
        }
        health.MaxHealth = target;
        health.Health = target;
    }
}
