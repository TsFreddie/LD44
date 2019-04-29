using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenericProjectileFirer : Weapon
{
    public Transform FirePoint;
    public GameObject BulletPrefab;
    public float BulletSpeed = 15f;
    public float ExplodeTime = 1f;
    public float ExplosionRadius = 1.5f;
    public Sprite ExplosionSprite;
    public bool ExplodeOnGround = true;

    protected override bool OnFire() {
        GameObject newBullet = Instantiate(BulletPrefab);
        newBullet.transform.position = FirePoint.position;
        ProjectileBullet bulletData = newBullet.AddComponent<ProjectileBullet>();
        bulletData.Damage = Damage;
        bulletData.FireAngle = transform.rotation.eulerAngles.z;
        bulletData.Speed = BulletSpeed;
        bulletData.ExplodeTime = ExplodeTime;
        bulletData.Target = Target;
        bulletData.ExplodeOnGround = ExplodeOnGround;
        bulletData.ExplosionRadius = ExplosionRadius;
        bulletData.ExplosionSprite = ExplosionSprite;
        return true;
    }
}
