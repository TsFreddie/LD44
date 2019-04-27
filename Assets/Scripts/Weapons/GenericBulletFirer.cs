using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenericBulletFirer : Weapon
{
    public Transform FirePoint;
    public GameObject BulletPrefab;
    public float BulletSpeed = 15f;
    public float BulletLifespan = 1.5f;

    public float BulletRange {
        get {
            return BulletSpeed * BulletLifespan;
        }
    }

    protected override bool OnFire() {
        GameObject newBullet = Instantiate(BulletPrefab);
        newBullet.transform.position = FirePoint.position;
        NormalBullet bulletData = newBullet.AddComponent<NormalBullet>();
        bulletData.Damage = Damage;
        bulletData.FireAngle = transform.rotation.eulerAngles.z;
        bulletData.Speed = BulletSpeed;
        bulletData.Lifespan = BulletLifespan;
        bulletData.Target = Target;
        return true;
    }

    void OnDrawGizmosSelected() {
        Vector2 line = Quaternion.Euler(0, 0, transform.rotation.eulerAngles.z) * Vector2.right;
        Gizmos.DrawLine((Vector2)FirePoint.position, (Vector2)FirePoint.position + line * BulletRange);
    }
}
