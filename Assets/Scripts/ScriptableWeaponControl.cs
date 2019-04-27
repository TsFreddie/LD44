using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScriptableWeaponControl : MonoBehaviour
{
    public GameObject AimAnchor;
    public Weapon Weapon;
    public float AimAngle;
    void Start()
    {
        
    }

    void Update()
    {
        if (Mathf.Abs(Mathf.DeltaAngle(0, AimAngle)) > 90 ) {
            AimAnchor.transform.localScale = new Vector3(1, -1, 1);
        } else {
            AimAnchor.transform.localScale = new Vector3(1, 1, 1);
        }
        Quaternion rotation = Quaternion.AngleAxis(AimAngle, Vector3.forward);
        AimAnchor.transform.rotation = rotation;
    }

    public void Fire() {
        if (Weapon != null) {
            Weapon.Firing(false);
        }
    }
}
