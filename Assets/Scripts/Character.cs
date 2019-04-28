using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Character : MonoBehaviour
{
    public GameObject AimAnchor;
    public GameObject PerkAnchor;
    public GameObject EyeGroup;
    public LayerMask Target;
    private List<Weapon> weapons;
    public int activeWeapon = -1;
    private Walkable walk;
    private Damageable health;

    void Awake() {
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
    }

    void Update()
    {
        if (HealthDisplay.I) {
            HealthDisplay.I.Max = health.MaxHealth;
            HealthDisplay.I.Value = health.Health;
        }
        Vector2 mousePos = Input.mousePosition;
        Vector2 targetPos = (Vector2)Camera.main.ScreenToWorldPoint(mousePos);
        Vector2 deltaPos = (targetPos - (Vector2)transform.position).normalized;
        EyeGroup.transform.localPosition = deltaPos * 0.15f;
        if (deltaPos.x < 0) {
            AimAnchor.transform.localScale = new Vector3(1, -1, 1);
        } else {
            AimAnchor.transform.localScale = new Vector3(1, 1, 1);
        }

        float angleDeg = Mathf.Atan2(deltaPos.y, deltaPos.x) * Mathf.Rad2Deg;
        Quaternion rotation = Quaternion.AngleAxis(angleDeg, Vector3.forward);
        AimAnchor.transform.rotation = rotation;

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

        if (activeWeapon >= 0) {
            if (Input.GetMouseButtonDown(0)) {
                weapons[activeWeapon].Firing(true);
            } else if (Input.GetMouseButton(0)) {
                weapons[activeWeapon].Firing(false);
            }
        }
    }

    public void GiveWeapon(GameObject weaponPrefab) {
        GameObject newWeapon = Instantiate(weaponPrefab);
        newWeapon.transform.SetParent(AimAnchor.transform);
        newWeapon.transform.localPosition = Vector3.zero;
        newWeapon.SetActive(true);
        Weapon weaponObj = newWeapon.GetComponent<Weapon>();
        weaponObj.Target = Target;
        if (weaponObj == null) {
            Destroy(newWeapon);
            return;
        }
        weapons.Add(weaponObj);
        if (activeWeapon < 0) {
            activeWeapon = 0;
        }
    }

    public void GivePerk(GameObject perkPrefab) {
        GameObject newPerk = Instantiate(perkPrefab);
        newPerk.transform.SetParent(PerkAnchor.transform);
        newPerk.transform.localPosition = Vector3.zero;
        newPerk.SetActive(true);
    }


}
