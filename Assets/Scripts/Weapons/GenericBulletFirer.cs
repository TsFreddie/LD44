using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenericBulletFirer : Weapon
{
    public Transform FirePoint;
    public GameObject BulletPrefab;

    protected override bool OnFire() {
        GameObject newBullet = Instantiate(BulletPrefab);
        newBullet.transform.position = FirePoint.position;
        NormalBullet bulletData = newBullet.AddComponent<NormalBullet>();
        bulletData.Damage = Damage;
        bulletData.FireAngle = transform.rotation.eulerAngles.z;
        bulletData.Speed = 15f;
        bulletData.Lifespan = 1.5f;
        bulletData.Target = Target;
        return true;
    }
}
